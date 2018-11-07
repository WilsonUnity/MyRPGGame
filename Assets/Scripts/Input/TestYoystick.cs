using UnityEngine;
using System.Collections;
using System;
using UnityEngine.Animations;


/// <summary>
/// 手柄按键测试类，测试用，与项目功能没有关系
/// </summary>
public class TestYoystick : MonoBehaviour
{
//    private Animator anim;
//
//    private void Start()
//    {
//        anim = GetComponentInChildren<Animator>();
//    }
//
//    private void Update()
//    {
//        if (Input.GetKeyDown(KeyCode.J))
//        {
//            anim.SetTrigger("hit");
//        }
//    }
}

//	private string currentButton;//当前按下的按键
//	private float LTRT;
//	 
//	
//	// Use this for initialization 
//	void Start()
//	{
// 
//	}
//	// Update is called once per frame 
//	void Update()
//	{
//		var values = Enum.GetValues(typeof(KeyCode));//存储所有的按键
//	 
//		
//		for (int x = 0; x < values.Length; x++)
//		{
//			if (Input.GetKeyDown((KeyCode)values.GetValue(x)))
//			{
//				currentButton = values.GetValue(x).ToString();//遍历并获取当前按下的按键
//			}
//
//			LTRT = -Input.GetAxis("LT/RT");
//			
//		}
//		 
//
//	}
//	// Show some data 
//	void OnGUI()
//	{
//		GUI.TextArea(new Rect(0, 0, 250, 40), "Current Button : " + currentButton); 
//		GUI.TextArea(new Rect(0, 50, 250, 40), "LTRT : " + LTRT); 
//	 
//	}