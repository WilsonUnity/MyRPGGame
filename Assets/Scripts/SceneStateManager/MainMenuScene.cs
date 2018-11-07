/*
 * SceneStateManager：场景状态管理模块
 * MainMenuScene：主场景状态
 * 作者：林逸群
 * 日期：2018
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SceneStateManager
{
	
	public class MainMenuScene : ISceneState {
	
		public MainMenuScene(SceneStateController theController) : base(theController)
		{
			//设置本场景状态名
			this.MSceneName = "MainMenuScene";
		}

		//开始
		public override void SceneStart()
		{
			//获取开始菜单并进行事件绑定
			Button btnStart = GameObject.Find("Start").GetComponent<Button>();
			if (btnStart != null)
			{
				btnStart.onClick.AddListener(() => OnStartGameBtnClick(btnStart));
			}
		 
		}

		//将场景状态设置为BattleScene
		private void OnStartGameBtnClick(Button btn)
		{
			m_Controller.SetState(new BattleScene(m_Controller), "BattleScene");
		}

	
	}
}

