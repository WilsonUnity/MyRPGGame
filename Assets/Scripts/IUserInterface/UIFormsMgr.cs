/*
 * IUserInterface : 用户界面系统
 * UIFormsMgr ： 这个类是UI系统的管理类，核心类
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
        
        
        //返回本脚本的实例
        public UIFormsMgr GetInstance()
        {
            if (_uiFormsMgr == null)
            {
                _uiFormsMgr = new GameObject("_UIFormsMgr").AddComponent<UIFormsMgr>();
            }

            return _uiFormsMgr;
        }

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


        #region 私人方法
        
        private Transform InitLoadCanvas()
        {
            return ResouerMgr.GetInstance().LoadAsset("Canvas", false).transform;
        }

        private BaseUIForms LoadUIFormFromAllForms(string UIFormName)
        {
            BaseUIForms baseUiForms = null;
            _dicAllUIForms.TryGetValue(UIFormName, out baseUiForms);
            if (baseUiForms == null)
            {
                baseUiForms = LoadUIForm(UIFormName);
            }

            return baseUiForms;
        }

        private BaseUIForms LoadUIForm(string UIFormName)
        {
            string UIFormPath = null;
            GameObject goClone = null;
            BaseUIForms baseUiForms = null;

            _dicPath.TryGetValue(UIFormName, out UIFormPath);

            goClone = ResouerMgr.GetInstance().LoadAsset(UIFormPath, true);
            baseUiForms = goClone.GetComponent<BaseUIForms>();

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

            goClone.SetActive(false);
            _dicAllUIForms.Add(UIFormName, baseUiForms);

            return baseUiForms;
        }

        #endregion
       
    }
}

