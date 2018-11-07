/*
 * SceneStateManager：场景状态管理模块
 * ISceneState：场景状态类接口
 * 简要说明：提供场景状态的模板，不一定要求子类完全实现，故不定义为抽象类
 * 作者：林逸群
 * 日期：2018
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SceneStateManager
{
    public class ISceneState
    {
        //状态名称
        private string m_SceneName = "SceneName";
        public string MSceneName
        {
            get { return m_SceneName; }
            set { m_SceneName = value; }
        }

        //控制者
        protected SceneStateController m_Controller = null;
    
        //建造者
        public ISceneState(SceneStateController theController)
        {
            m_Controller = theController;
        }

        //开始
        public virtual void SceneStart()
        {
            //场景转换成功后通知此方法，一般进行参数设置等
        }

        //结束
        public virtual void SceneEnd()
        {
            //场景要结束时通知此方法，可以释放游戏不再需要的资源等
        }

        //更新
        public virtual void SceneUpdate()
        {
            //更新功能
        }
    }//Class_End
}

