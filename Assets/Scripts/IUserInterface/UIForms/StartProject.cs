using System.Collections;
using System.Collections.Generic;
using IUserInterface;
using UnityEngine;

public class StartProject : MonoBehaviour {
	
	private void Awake()
	{
		UIFormsMgr.GetInstance().ShowUIForm("Main_UI");
	}
}
