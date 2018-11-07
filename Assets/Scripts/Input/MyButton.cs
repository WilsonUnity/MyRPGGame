using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

/// <summary>
/// 按钮功能实现类
/// </summary>
public class MyButton{
	
#region Variable

	//按钮按住（不能松开）
	[HideInInspector]public bool isPressing = false; 
	
	//按钮按下（一次）
	[HideInInspector]public bool OnPressed = false; 
	
	//按钮抬起
	[HideInInspector]public bool OnReleased = false; 

	//按钮信号的当前状态
	private bool curState = false;
	//按钮信号的最后状态
	private bool lastState = false;
	
	public bool IsExtentTime = false;
	public bool IsDelaying = false;

#endregion
	
//---------------------------------------------------------

#region Instantiation
	
	public MyTime extTime = new MyTime();
	public MyTime delayTime = new MyTime();
	
#endregion
	

//---------------------------------------------------------	
	
#region Function

	/// <summary>
	/// 获取信号判断按钮状态
	/// </summary>
	/// <param name="state">接收按钮信号（bool）</param>
	/// <param name="number">此参数的功能是将轴类型转换为按钮类型，比如LT or RT,可选填的参数，默认为1.0.</param>
	public void GetSignal(bool state,float number = 1.0f)
	{
		extTime.Tick();
		delayTime.Tick();
		
		//按住状态，直接赋予state
		isPressing = state;

		//默认状态下都为false
		OnPressed = false;
		OnReleased = false;
		IsExtentTime = false;
		IsDelaying = false;
		
	//====================按下功能==========================
		//curState为二阶的bool，由state以及number决定最终值
		curState = state && (number != 0? true : false);
		if (curState != lastState)
		{
			if (curState == true)
			{
				OnPressed = true;
				StartTime(delayTime, 1);
			}
			else
			{
				OnReleased = true;
				StartTime(extTime, 0.2f);
			}
		}
		
		lastState = curState;
	//=======================================================
		
		//处于Run状态下设为true
		if (extTime.state == MyTime.State.RUN)
		{
			IsExtentTime = true;
		}
		 
		//处于Run状态下设为true
		if (delayTime.state == MyTime.State.RUN)
		{
			IsDelaying = true;
		}
	}
	

	//该函数来让不同计时器启动
    public void StartTime(MyTime time,float duration)
	{
		time.durationTime = duration;
		time.Go();
	}

#endregion
	

}
