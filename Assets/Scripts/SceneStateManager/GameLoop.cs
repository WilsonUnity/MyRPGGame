using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SceneStateManager
{
    /// <summary>
    /// 游戏主循环
    /// </summary>
    public class GameLoop : MonoBehaviour {
	
        SceneStateController m_sceneStateController = new SceneStateController();
	
        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
        }

        // Use this for initialization
        void Start ()
        {
            m_sceneStateController.SetState(new StartScene(m_sceneStateController), "");
        }
	
        // Update is called once per frame
        void Update () {
            m_sceneStateController.StateUpdate();
        }
    }

}

