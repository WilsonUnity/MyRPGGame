using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyBoardInput : IUserInput {
 
	[Header("===== Key Setting =====")]
	public string KeyUp = "w";
	public string KeyDown = "s";
	public string KeyLeft = "a";
	public string KeyRight = "d";

	public string KeyA = "left shift";
	public string KeyB = "space";
	public string KeyC;
	public string KeyD;
    
	public string KeyUpArrow = "up";
	public string KeyDownArrow = "down";
	public string KeyLeftArrow = "left";
	public string KeyRightArrow = "right";
	
	public bool isOpenMouse;
	public float mouseSpeedX;
	public float mouseSpeedY;
	
	private void Update()
	{
		Target_Vertical = (Input.GetKey(KeyUp) ? 1.0f : 0.0f) - (Input.GetKey(KeyDown) ? 1.0f : 0.0f);
		Target_Horizontal = (Input.GetKey(KeyRight) ? 1.0f : 0.0f) - (Input.GetKey(KeyLeft) ? 1.0f : 0.0f);
		if (!isOpenMouse)
		{
			Jup = (Input.GetKey(KeyUpArrow) ? 1.0f : 0.0f) - (Input.GetKey(KeyDownArrow) ? 1.0f : 0.0f);
			Jright = (Input.GetKey(KeyRightArrow) ? 1.0f : 0.0f) - (Input.GetKey(KeyLeftArrow) ? 1.0f : 0.0f);
		}
		else
		{
			Jup = Input.GetAxis("Mouse Y") * mouseSpeedX;
			Jright = Input.GetAxis("Mouse X") * mouseSpeedY;
		}

		if (!IsInputEnable)
		{
			Target_Horizontal = 0;
			Target_Vertical = 0;
		}

		//对键值的增长进行平滑化处理
		Current_Vertical = Mathf.SmoothDamp(Current_Vertical, Target_Vertical, ref Velocity_Vertical, 0.1f);
		Current_Horizontal = Mathf.SmoothDamp(Current_Horizontal, Target_Horizontal, ref Velocity_Horizontal, 0.1f);

		Vector2 tempDAxis = SquareToCircle(new Vector2(Current_Horizontal, Current_Vertical));

		//将平移值开方简化程序
		DMag = Mathf.Sqrt((tempDAxis.x * tempDAxis.x) + (tempDAxis.y * tempDAxis.y));
        
		//计算转向
		Dvec = (tempDAxis.x * transform.right) + (tempDAxis.y * transform.forward);

		
		isrun = Input.GetKey(KeyA);
		isdefense = Input.GetKey(KeyD);
        
		//是否按下跳跃键
		bool newjump = Input.GetKey(KeyB);
		if (newjump != isjumplast && newjump == true)
		{
			isjump = true;
		}
		else
		{
			isjump = false;
		}
		isjumplast = newjump;
		
        
		bool newattack = Input.GetKey(KeyC);
		if (newattack != isattacklast && newattack == true)
		{
			rb = true;
		}
		else
		{
			rb = false;
		}
		isattacklast = newattack;
	}
	
	
}
