/*
 * IUserInput：用户输入模块的接口基类
 * 脚本挂载对象：Player(角色控制柄)
 * 作者：林逸群
 * 日期：2018
 * 修改记录：10/xx -使用事件驱动设计,优化与StateMachine脚本的通信
 *          11/1 -代码可读性优化
 * 主题：
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class IUserInput : MonoBehaviour {

#region variable

	[Header("===== 目标速度 =====")]
	protected float Target_Vertical;
	protected float Target_Horizontal;

	[Header("===== 当前速度 =====")]
	protected float Current_Vertical;
	protected float Current_Horizontal;
    
	protected float Velocity_Vertical;
	protected float Velocity_Horizontal;

	//视角旋转量Y
	public float Jup;
	//视角旋转量Y
	public float Jright;
	//位移量(x,y开方后的值)
	public float DMag;
	//角色控制柄的旋转量
	public Vector3 Dvec;
	
	//是否奔跑
	public bool isrun;
	//是否防御
	public bool isdefense; 
	//是否锁定敌人
	public bool islock; 
    
	//是否跳跃
	public bool isjump; 
	public  bool isjumplast;
	
	//是否攻击
	public  bool isattacklast;
	
	//是否能够移动
	public bool IsInputEnable = true; 
	
	//是否前刺
	public bool isFontStab = false;
    
	//手柄LB键
	public bool lb;
	//手柄LT键
	public bool lt;
	//手柄RB键
	public bool rb;
	//手柄RT键
	public bool rt;

	public bool isStart;
	
    //public bool isattack;

#endregion
	
//-----------------------------------------------
	
	/// <summary>
	/// 进行球形映射用于解决斜边加速问题,手柄对斜边加速的问题进行了优化，所以手柄类无需调用此函数
	/// </summary>
	/// <param name="target"></param>
	/// <returns></returns>
	public Vector2 SquareToCircle(Vector2 target)
	{
		Vector2 output = Vector2.zero;
		output.x = target.x * Mathf.Sqrt(1 - (target.y * target.y) / 2.0f);
		output.y = target.y * Mathf.Sqrt(1 - (target.x * target.x) / 2.0f);
		return output;
	}

	#region 旧代码
//废弃的代码，记录用
//	protected void UpdateDmagDvec(float Horizontal,float Vertical)
//	{
//		//将平移值开方简化程序
//		DMag = Mathf.Sqrt((Horizontal * Horizontal) + (Vertical * Vertical));
//        
//		//计算转向
//		Dvec = (Horizontal * transform.right) + (Vertical * transform.forward);
//
//	}
	#endregion
		
}
