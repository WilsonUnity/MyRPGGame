/*
 * SceneStateManager：场景状态管理模块
 * StartScene：开始场景状态
 * 作者：林逸群
 * 日期：2018
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SceneStateManager
{
	
	public class StartScene : ISceneState{

		public StartScene(SceneStateController theController) : base(theController)
		{
			//设置本场景状态名
			this.MSceneName = "StartScene";
		}

		//开始
		public override void SceneStart()
		{
			//获取开始菜单并进行事件绑定
			Button btn_Start = GameObject.Find("Start").GetComponent<Button>();
			if (btn_Start != null)
			{
				btn_Start.onClick.AddListener(() => OnEnterGameBtnClick());
			}
 	
		}

		//将场景状态设置为MainMenuScene
		public void OnEnterGameBtnClick()
		{
			m_Controller.SetState(new MainMenuScene(m_Controller), "MainMenuScene");
		}

	
	}                                  

}
