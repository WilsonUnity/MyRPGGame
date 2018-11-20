/*
 * IUserInterface : 用户界面系统
 * UIFormsMgr ： 这个类是UI系统的管理类，核心类
 * 该类对窗体进行加载缓存，同时定义窗户的行为
 * 日期 ：2018
 * 程序员 : 林逸群
 */
using System.Collections;
using System.Collections.Generic;
using UnityEditor.VersionControl;
using UnityEngine;

namespace IUserInterface
{
    public class UIFormsMgr : MonoBehaviour {
        
        #region 私有变量
        
        //本脚本的实例
        private static UIFormsMgr _uiFormsMgr = null;

        //UI窗体的根结点
        private Transform _transCanvas = null;

        //本脚本对象的父结点
        private Transform _transScript = null;

        //Normal结点
        private Transform _transNormal = null;

        //Fixed结点
        private Transform _transFixed = null;

        //Popup结点
        private Transform _transPopUp = null;

        //保存UI路径的字典
        private Dictionary<string, string> _dicPath = null;
        
        //保存‘当前显示窗体’的字典
        private Dictionary<string, BaseUIForms> _dictCurUIForms = null;
        
        //保存‘所有窗体资源’的字典
        private Dictionary<string, BaseUIForms> _dicAllUIForms = null;
        
        //保存‘模态窗体’的栈结构
        private Stack<BaseUIForms> _UIFormsStack = null;
        
        #endregion
        
        private void Awake()
        {
            #region 初始化

            _dicPath = new Dictionary<string, string>();
            _dicAllUIForms = new Dictionary<string, BaseUIForms>();
            _dictCurUIForms = new Dictionary<string, BaseUIForms>();
            _UIFormsStack = new Stack<BaseUIForms>();
            
            #endregion
            
            //加载UI根结点(Canvas)
            _transCanvas = InitLoadCanvas();
            
            //找到本脚本对象的父结点
            _transScript = _transCanvas.Find("_ScriptMgr").transform;
            //将本脚本对象作为_transScript的子结点
            gameObject.transform.SetParent(_transScript);
            
            //在根结点下找Normal,Fixed,PopUp结点
            _transNormal = _transCanvas.Find("Normal").transform;
            _transFixed = _transCanvas.Find("Fixed").transform;
            _transPopUp = _transCanvas.Find("PopUp").transform;

            //切换场景不销毁本物件
            DontDestroyOnLoad(_transCanvas);

            //路径导入
            if (_dicPath != null)
            {
                _dicPath.Add("Main_UI", "Main_UI");
                _dicPath.Add("BG_UI","BG");
            }

        }

        #region 公有方法

        //返回本脚本的实例
        public static UIFormsMgr GetInstance()
        {
            if (_uiFormsMgr == null)
            {
                _uiFormsMgr = new GameObject("_UIFormsMgr").AddComponent<UIFormsMgr>();
            }

            return _uiFormsMgr;
        }
        
         public void ShowUIForm(string strUIFormName)
        {
            BaseUIForms baseUiForms = null;
            
            //空名称结束程序
            if (string.IsNullOrEmpty(strUIFormName))
            {
                return;
            }

            //获取到指定的窗体基础类
            baseUiForms = LoadUIFormFromAllForms(strUIFormName);
            if (baseUiForms == null) return;
           
            //此窗体的显示方式
            switch (baseUiForms.UiType.m_UIFormShowType)
            {
                //正常窗体
                case UIFormShowType.Normal:
                    EnterUIForm(strUIFormName);
                    break;
                //隐藏其他窗体
                case UIFormShowType.HideOther:
                    EnterUIFormAndHideOther(strUIFormName);
                    break;
                //模态窗体
                case UIFormShowType.ReverseChange:
                    PushUIForm(strUIFormName);
                    break;
                default:
                    break;
            }
        }

        //-----------------------------------------------------------------
        
        /// <summary>
        /// 关闭窗体
        /// </summary>
        /// <param name="strUIFormName">UI窗体名称</param>
        public void CloseUIForm(string strUIFormName)
        {
            BaseUIForms baseUI = null;
            if (string.IsNullOrEmpty(strUIFormName)) return;

            //如果无法从所有UI资源中找到指定窗体就结束程序
            _dicAllUIForms.TryGetValue(strUIFormName, out baseUI);
            if (baseUI == null) return;

            //根据不同窗体类型进行不同的关闭操作
            switch (baseUI.UiType.m_UIFormShowType)
            {
                case UIFormShowType.Normal:
                    ExitUIForm(strUIFormName);
                    break;
                case UIFormShowType.HideOther:
                    ExitUIFormAndShowOther(strUIFormName);
                    break;
                case UIFormShowType.ReverseChange:
                    PopUIForm();
                    break;
                default:
                    break;
            }

        }

        #endregion

        #region 私人方法
        
        private Transform InitLoadCanvas()
        {
            return ResouerMgr.GetInstance().LoadAsset("Canvas", false).transform;
        }
        
        
//--------------------------------------------------------------------------------------
        
        
        /// <summary>
        /// 从‘所有窗体集合’中获取到BaseUIForms组件
        /// 如果‘所有窗体集合’中并不存在指定BaseUIForms组件，则进行一次加载操作
        /// </summary>
        /// <param name="UIFormName">窗体名称</param>
        /// <returns></returns>
        private BaseUIForms LoadUIFormFromAllForms(string UIFormName)
        {
            BaseUIForms baseUiForms = null;
            
            //根据窗体名称匹配BaseUIForms组件
            _dicAllUIForms.TryGetValue(UIFormName, out baseUiForms);
            
            if (baseUiForms == null)
            {
                baseUiForms = LoadUIForm(UIFormName);
            }

            return baseUiForms;
        }
        
        
//--------------------------------------------------------------------------------------
        

        /// <summary>
        /// 根据窗体名称加载窗体，并将其保存到‘所有窗体集合’的字典中。
        /// 实例化到场景的窗体，会根据窗体类型自动挑选父结点
        /// </summary>
        /// <param name="UIFormName">窗体名称</param>
        /// <returns></returns>
        private BaseUIForms LoadUIForm(string UIFormName)
        {
            //路径名称
            string UIFormPath = null;
            
            //实例化到场景的窗体
            GameObject goClone = null;
            
            //窗体上挂载的BaseUiForms组件
            BaseUIForms baseUiForms = null;

            //检查有没有与窗体名称匹配的路径
            _dicPath.TryGetValue(UIFormName, out UIFormPath);

            goClone = ResouerMgr.GetInstance().LoadAsset(UIFormPath, true);
            baseUiForms = goClone.GetComponent<BaseUIForms>();

            //根据窗体类型选择父结点
            if (_transCanvas != null && baseUiForms != null)
            {
                switch (baseUiForms.UiType.m_UIFormType)
                {
                    case UIFormType.Normal:
                        UnityHelper.SetParent(_transNormal,goClone.transform);
                       // goClone.transform.SetParent(_transNormal);
                        break;
                    case UIFormType.Fixed:
                        UnityHelper.SetParent(_transFixed, goClone.transform);
                        //goClone.transform.SetParent(_transFixed);
                        break;
                    case UIFormType.PopUp:
                        UnityHelper.SetParent(_transPopUp, goClone.transform);
                       // goClone.transform.SetParent(_transPopUp);
                        break;
                    default:
                        break;
                }
            }

            //先设置为不可见,世界空间型窗体除外
            if (baseUiForms.UiType.m_UIFormType != UIFormType.Space)
                goClone.SetActive(false);
            
            //保存到‘所有窗体集合’
            _dicAllUIForms.Add(UIFormName, baseUiForms);

            return baseUiForms;
            
        }//LoadUIForm_End

        
//--------------------------------------------------------------------------------------


        /// <summary>
        /// 显示方式为Normal型窗体的显示操作
        /// 将窗体加入’当前显示集合‘中
        /// </summary>
        /// <param name="UIFormName"></param>
        private void EnterUIForm(string UIFormName)
        {
            BaseUIForms curBaseUiForms;
            BaseUIForms allBaseUiForms;

            //检测‘当前显示集合’中是否存在该窗体
            //如果存在，不进行后续操作
            _dictCurUIForms.TryGetValue(UIFormName, out curBaseUiForms);
            if (curBaseUiForms != null)
            {
                return;
            }

            //检测’所有窗体集合‘是否存在该窗体
            //如果存在，加入’当前显示集合‘，并设置为显示
            _dicAllUIForms.TryGetValue(UIFormName, out allBaseUiForms);
            if (allBaseUiForms != null)
            {
                _dictCurUIForms.Add(UIFormName, allBaseUiForms);
                allBaseUiForms.DisPlay();
            }
            else
            {
                Debug.Log("无法提取到窗体资源：" + UIFormName);
            }
        }
        
        
//--------------------------------------------------------------------------------------
        

        /// <summary>
        /// 显示方式为Normal型窗体的隐藏操作
        /// 将窗体移除’当前显示集合‘中
        /// </summary>
        /// <param name="UIFormName"></param>
        private void ExitUIForm(string UIFormName)
        {
            BaseUIForms baseUiForms;

            //如果该窗体已经隐藏，就无需后续操作
            _dictCurUIForms.TryGetValue(UIFormName, out baseUiForms);
            if (baseUiForms == null)
            {
                return;
            }
            
            baseUiForms.Hide();
            _dictCurUIForms.Remove(UIFormName);
        }
        
        
//--------------------------------------------------------------------------------------


        /// <summary>
        /// 显示方式为HideOther型窗体的显示操作
        /// 当该窗体显示时，会隐藏场景中其他窗体
        /// </summary>
        private void EnterUIFormAndHideOther(string UIFormName)
        {
            BaseUIForms baseUiForms;
            BaseUIForms allBaseUiForms;

            _dictCurUIForms.TryGetValue(UIFormName, out baseUiForms);
            if (baseUiForms != null)
            {
                return;
            }

            //将'当前显示集合'中窗体隐藏
            foreach (BaseUIForms baseUI in _dictCurUIForms.Values)
            {
                baseUI.Hide();
            }

            //将'窗体栈'中的窗体隐藏
            foreach (BaseUIForms baseUI in _UIFormsStack)
            {
                baseUI.Hide();
            }

            _dicAllUIForms.TryGetValue(UIFormName, out allBaseUiForms);
            if (allBaseUiForms != null)
            {
                _dictCurUIForms.Add(UIFormName, allBaseUiForms);
                allBaseUiForms.DisPlay();
            }
            else
            {
                Debug.Log("无法提取到窗体资源：" + UIFormName);
            }
        }
        
        
//--------------------------------------------------------------------------------------


        /// <summary>
        /// 显示方式为HideOther型窗体的隐藏操作
        /// 当该窗体隐藏时，会重新显示场景中其他窗体
        /// </summary>
        private void ExitUIFormAndShowOther(string UIFormName)
        {
            BaseUIForms baseUiForms;

            _dictCurUIForms.TryGetValue(UIFormName, out baseUiForms);
            if (baseUiForms == null)
            {
                return;
            }

            //重新显示
            foreach (BaseUIForms baseUI in _dictCurUIForms.Values)
            {
                baseUI.RedisPlay();
            }

            foreach (BaseUIForms baseUI in _UIFormsStack)
            {
                baseUI.RedisPlay();
            }
            
            baseUiForms.Hide();
            _dictCurUIForms.Remove(UIFormName);
        }
        
        
        
//--------------------------------------------------------------------------------------


        /// <summary>
        /// 显示方式为ReverseChange型窗体的显示操作
        /// 用栈结构来处理模态窗体的层级关系
        /// </summary>
        private void PushUIForm(string UIFormName)
        {
            BaseUIForms baseUiForms;
            
            //检查是否存在栈顶元素
            if (_UIFormsStack.Count > 0)
            {
                BaseUIForms baseUI = _UIFormsStack.Peek();
                baseUI.Free();
            }

            _dicAllUIForms.TryGetValue(UIFormName, out baseUiForms);

            if (baseUiForms != null)
            {
                //将窗体推入栈
                _UIFormsStack.Push(baseUiForms);
                baseUiForms.DisPlay();
            }
            else
            {
                Debug.Log("无法提取到窗体资源：" + UIFormName);
            }
        }

        
//--------------------------------------------------------------------------------------


        /// <summary>
        /// 出栈，清除栈顶的窗体
        /// </summary>
        private void PopUIForm()
        {
            //如果栈中存在2个以上窗体
            if (_UIFormsStack.Count >= 2)
            {
                //移除顶上窗体
                BaseUIForms topBaseUI = _UIFormsStack.Pop();
                topBaseUI.Hide();
                //返回当前顶层窗体
                BaseUIForms curBaseUI = _UIFormsStack.Peek();
                curBaseUI.RedisPlay();
            }
            
            else//只存在一个窗体，直接隐藏
            if(_UIFormsStack.Count == 1)
            {
                BaseUIForms baseUI = _UIFormsStack.Pop();
                baseUI.Hide();
            }
        }
        

//--------------------------------------------------------------------------------------
        
        
        #endregion
       
    }//Class_End
}

