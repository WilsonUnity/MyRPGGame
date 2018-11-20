/*
 * IUserInterface : 用户界面系统
 * UISysDefined ： 这个类是UI窗体的系统定义类
 * 日期 ：2018
 * 程序员 : 林逸群
 */
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IUserInterface
{
    //窗体类型
    //根据不同的窗体类型将窗体放在对应的父物件上，用来进行层级管理
    //层级关系为Normal -> Fixed -> PopUp,这意味着PopUp型的窗体是处于画布顶层
    public enum UIFormType
    {
        //正常窗体
        Normal,
        //固定窗体
        Fixed,
        //弹出窗体
        PopUp,
        //世界空间窗体
        Space
    }
	
    //窗体显示类型
    //模态窗体 ： 父窗体下生成子窗体时，如果此时让父窗体不能进行操作，就称其为模态窗体
    public enum UIFormShowType
    {
        //正常显示
        Normal,
        //出现时隐藏其它窗体
        HideOther,
        //模态窗体
        ReverseChange,
        //世界空间窗体，另外处理
        Space
    }
	
    //透明度类型
    //其实就是定义遮罩的类型
    public enum UIFormLucency
    {
        //完全透明，射线不能穿透
        Lucency,
        //半透明，射线不能穿透
        Translucence,
        //低透明度，射线不能穿透
        ImPenetrable,
        //射线可以穿透
        Pentrate
    }
    
    public class UISysDefined
    {
        [Header("===路径常量===")]
        public const string SYS_PATH_CANVAS         = "Canvas";
        public const string SYS_PATH_UIFORMS_CONFIG_INFO = "UIFormsConfigInfo";
        
        [Header("===便签常量===")]
        public const string SyS_TAG_CANVAS          = "_TagCanvas";

        [Header("===节点常量===")]
        public const string SYS_SCRIPTMANAGER_NODE  = "_ScriptMgr";
    }

}
