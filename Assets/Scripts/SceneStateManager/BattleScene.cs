/*
 * SceneStateManager：场景状态管理模块
 * BattleScene：战斗场景状态
 * 作者：林逸群
 * 日期：2018
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SceneStateManager
{
	public class BattleScene : ISceneState {

		public BattleScene(SceneStateController theController) : base(theController)
		{
			this.MSceneName = "BattleScene";
		}

		public override void SceneUpdate()
		{
		    
		}
	}
}

