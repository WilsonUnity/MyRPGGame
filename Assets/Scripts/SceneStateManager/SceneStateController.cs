/*
 * SceneStateManager：场景状态管理模块
 * SceneStateController：场景状态控制类
 * 主题：调用子状态类的开始，结束，更新逻辑。需要与游戏主循环搭配使用
 * 作者：林逸群
 * 日期：2018
 */
using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SceneStateManager
{
	public class SceneStateController
	{
		//状态
		private ISceneState m_State = null;
		private bool m_bRunBegin = false;
		private AsyncOperation async;

		//设置状态
		public void SetState(ISceneState State,string SceneName)
		{
			m_bRunBegin = false;
		
			OnLoadScene(SceneName);
		
			//如果存在上一个场景状态，就通知其结束
			if (m_State != null)
			{
				m_State.SceneEnd();
			}
		    m_State = State;
		}

		/// <summary>
		/// 场景加载
		/// </summary>
		/// <param name="LoadSceneName">场景名称</param>
		public void OnLoadScene(string LoadSceneName)
		{
			if (String.IsNullOrEmpty(LoadSceneName))
			{
				return;
			}

			async = SceneManager.LoadSceneAsync(LoadSceneName);
		}

		//更新，执行状态子类的方法
		public void StateUpdate()
		{
			//如果场景还在加载中就返回
			if (async != null && !async.isDone)
			{
				return;
			}

			//通知新的state开始
			if (m_State != null && m_bRunBegin == false)
			{
				m_State.SceneStart();
				m_bRunBegin = true;
			}

		
			if (m_State != null)
			{
				m_State.SceneUpdate();
			}
		}
	

	}

}

