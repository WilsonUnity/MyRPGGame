/*
 * UnityHelper：封装一些unity常用的方法
 * 作者：林逸群
 * 日期：2018
 * 修改记录：11/1 - DeepFind方法参数修改，this Transform parent改为Transform parent
 */
using System.Collections;
using System.Collections.Generic;
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
	
	
}
