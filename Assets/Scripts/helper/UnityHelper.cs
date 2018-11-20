/*
 * UnityHelper：封装一些unity常用的方法
 * 作者：林逸群
 * 日期：2018
 * 修改记录：11/1 - DeepFind方法参数修改，this Transform parent改为Transform parent
 */
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;

public static class UnityHelper{

	/// <summary>
	/// 深度查找子物体
	/// </summary>
	/// <param name="parent">父对象</param>
	/// <param name="name">子物体名称</param>
	/// <returns></returns>
	public static Transform DeepFind(Transform parent,string name)
	{
		Transform tempTransform = null;
		foreach (Transform child in parent)
		{
			if (child.name == name)
			{
				return child;
			}
			tempTransform = DeepFind(child, name);
			if (tempTransform != null)
			{
				return tempTransform;
			}
		}

		return null;
	}
	
	
//--------------------------------------------------------------------------------------
	

	/// <summary>
	/// 为子物件添加组件
	/// </summary>
	/// <param name="parent">父结点</param>
	/// <param name="name">子物件名称</param>
	/// <typeparam name="T"></typeparam>
	/// <returns></returns>
	public static T AddChildCompont<T>(Transform parent, string name) where T : Component
	{
		Transform child = DeepFind(parent, name);
		T TCompont;
		
		if (child == null)
		{
			return null;
		}
		
		TCompont = child.GetComponent<T>();
		if (TCompont == null)
		{
			return child.gameObject.AddComponent<T>();
		}

		return TCompont;
	}

	
//--------------------------------------------------------------------------------------	

	
	/// <summary>
	/// 为物件设置父结点
	/// </summary>
	/// <param name="parent">父结点</param>
	/// <param name="child">子物件</param>
	public static void SetParent(Transform parent, Transform child)
	{
		child.SetParent(parent);
		child.localPosition = Vector3.zero;
		child.localEulerAngles = Vector3.zero;
		child.localScale = Vector3.one;
		
	}


}
