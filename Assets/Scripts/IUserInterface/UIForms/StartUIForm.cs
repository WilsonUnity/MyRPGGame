/*
 * StartUIForm:开始界面
 */
using System.Collections;
using System.Collections.Generic;
using IUserInterface;
using UnityEngine;
using UnityEngine.UI;

namespace IUserInterface
{
    public class StartUIForm : BaseUIForms
    {
        //设置按钮
        public Button btn_Setting;
        //返回按钮
        public Button btn_Back;
        //相机
        public Camera m_Camera;
        //状态机
        private Animator _animator;
        
        private void Awake()
        {
            //获取相机的动画状态机
            _animator = m_Camera.GetComponent<Animator>();
            
            UiType.m_UIFormType = UIFormType.Space;
            UiType.m_UIFormShowType = UIFormShowType.Space;
            UiType.m_UIFormLucency = UIFormLucency.Lucency;

            //给按钮绑定点击事件
            RigisterButtonEvent(p => { _animator.SetTrigger("go"); }, btn_Setting.gameObject);
            RigisterButtonEvent(p => { _animator.SetTrigger("return"); }, btn_Back.gameObject);
        }
        
       
    }

}
