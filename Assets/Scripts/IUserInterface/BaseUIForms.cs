/*
 * IUserInterface : 用户界面系统
 * BaseUIForms ： 这个类是窗体类的基类，定义窗体的生命周期以及封装一些常用的方法
 * 这个类不作为接口，因为不强制要求子类要实现所有方法
 * 日期 ：2018
 * 程序员 : 林逸群
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IUserInterface
{
    public class BaseUIForms : MonoBehaviour {

        //设置返回窗体类型
        private UIType _type = new UIType();
        public UIType UiType
        {
            get { return _type; }
            set { _type = value; }
        }

        //显示窗体
        public virtual void DisPlay()
        {
            this.gameObject.SetActive(true);
            //只有模态窗体类型能进行遮罩操作
            if (UiType.m_UIFormType == UIFormType.PopUp)
            {
                UIMeskMgr.GetInstance().ShowMesk(gameObject, UiType.m_UIFormLucency);
            }
        }

        //隐藏窗体
        public virtual void Hide()
        {
            this.gameObject.SetActive(false);
            //只有模态窗体类型能进行遮罩操作
            if (UiType.m_UIFormType == UIFormType.PopUp)
            {
                UIMeskMgr.GetInstance().CancelMesk();
            }
        }

        //重新显示
        public virtual void RedisPlay()
        {
            this.gameObject.SetActive(true);
            //只有模态窗体类型能进行遮罩操作
            if (UiType.m_UIFormType == UIFormType.PopUp)
            {
                UIMeskMgr.GetInstance().ShowMesk(gameObject, UiType.m_UIFormLucency);
            }
        }

        //冻结
        public virtual void Free()
        {
            this.gameObject.SetActive(true);
        }

        public void RigisterButtonEvent(EventTriggerListener.EventDelegate delHandle, string btnName)
        {
            GameObject go = UnityHelper.DeepFind(gameObject.transform, btnName).gameObject;
            
            if (go != null)
            {
                EventTriggerListener.Get(go).onClick = delHandle;
            }
        }
        
        public void RigisterButtonEvent(EventTriggerListener.EventDelegate delHandle, GameObject btnObject)
        {
            EventTriggerListener.Get(btnObject).onClick = delHandle;
        }

    }

}
