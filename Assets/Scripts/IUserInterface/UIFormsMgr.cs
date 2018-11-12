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
            _transScript = _transCanvas.Find("_UIMgrScript").transform;
            //将本脚本对象作为_transScript的子结点
            gameObject.transform.SetParent(_transScript);
            
            //在根结点下找Normal,Fixed,PopUp结点
            _transNormal = _transCanvas.Find("Normal").transform;
            _transFixed = _transCanvas.Find("Fixed").transform;
            _transPopUp = _transCanvas.Find("PopUp").transform;

            //切换场景不销毁本物件
            DontDestroyOnLoad(this.gameObject);

            //路径导入

        }

        #region 公有方法

        //返回本脚本的实例
        public UIFormsMgr GetInstance()
        {
            if (_uiFormsMgr == null)
            {
                _uiFormsMgr = new GameObject("_UIFormsMgr").AddComponent<UIFormsMgr>();
            }

            return _uiFormsMgr;
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
                        goClone.transform.SetParent(_transNormal);
                        break;
                    case UIFormType.Fixed:
                        goClone.transform.SetParent(_transFixed);
                        break;
                    case UIFormType.PopUp:
                        goClone.transform.SetParent(_transPopUp);
                        break;
                    default:
                        break;
                }
            }

            //先设置为不可见
            goClone.SetActive(false);
            
            //保存到‘所有窗体集合’
            _dicAllUIForms.Add(UIFormName, baseUiForms);

            return baseUiForms;
            
        }//LoadUIForm_End

        #endregion
       
    }//Class_End
}

