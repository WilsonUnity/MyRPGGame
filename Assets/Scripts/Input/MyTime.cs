using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEditor;
using UnityEngine;
using Debug = UnityEngine.Debug;

/// <summary>
/// 按钮的计时器功能
/// </summary>
public class MyTime{

	public enum State
	{
		IDLE,
		RUN,
		FINISHED
	}

	//默认状态设置为IDLE，就是什么也不做
	public State state = State.IDLE;
	
	//时间总长
	public float durationTime = 1.0f; 
	//流逝的时间
	public float elapseTime = 0.0f;

	//该函数实现时间记录功能，必须处于循环状态下
	public void Tick()
	{
		switch (state)
		{
		   case State.IDLE:  //Nothing
			   break;
				
		   case State.RUN:
					elapseTime += Time.deltaTime;//开始计时
					if (elapseTime >= durationTime) //超过规定时间就结束计时
					{
						state = State.FINISHED;
					}
			   break;
		   case State.FINISHED:
			   break;
		   default:
			   Debug.Log("MyTime Error");
			   break;
		}
	}

	//开始计时
	public void Go()
	{
		state = State.RUN;
		elapseTime = 0.0f;
	}

}
