using System.Collections;
using System.Collections.Generic;
using System.Security.Permissions;
using UnityEditor;
using UnityEngine;
using UnityEngine.Experimental.UIElements;

/// <summary>
/// 手柄类
/// </summary>
public class JoyStrick : IUserInput {

	[Header("===== Key Setting =====")]
	public string AxisX = "axisX"; //左摇杆X
	public string AxisY = "axisY"; //左摇杆Y

	public string LT = "LT"; //LT键
	public string RT = "RT"; //RT键
		
    public string Axis4 = "axis4"; //右摇杆X
    public string Axis5 = "axis5"; //右摇杆Y
	
    public string btn0 = "btn0"; //A
	public string btn1 = "btn1"; //B
	
	public string btn4 = "btn4"; //LB
	public string btn5 = "btn5"; //RB
	
	public string btn8 = "btn8"; //左摇杆按压(L3)
	public string btn9 = "btn9"; //右摇杆按压(R3)

	public string Btn_Select = "Select";
	public string Btn_Start = "Start";

//---------------------------------------------------

//想继续定制某按钮的功能在这里实例化
#region MyButton实例化
	
	public MyButton buttonA = new MyButton();
	public MyButton buttonB = new MyButton();
	 
	public MyButton buttonRB = new MyButton();
	public MyButton buttonRT = new MyButton();
	public MyButton buttonLB = new MyButton();
	public MyButton buttonLT = new MyButton();
	
	public MyButton buttonL3 = new MyButton();
	public MyButton buttonR3 = new MyButton();
	
	public MyButton buttonSelect = new MyButton();
	public MyButton buttonStart = new MyButton();
	

#endregion
											
//---------------------------------------------------

	private void Update()
	{
    //接收按钮信号，true or false
	#region buttonX.GetSignal() 

		buttonA.GetSignal(Input.GetButton(btn0));
		buttonB.GetSignal(Input.GetButton(btn1));

		buttonL3.GetSignal(Input.GetButton(btn8));
		buttonR3.GetSignal(Input.GetButton(btn9));

		buttonLB.GetSignal(Input.GetButton(btn4));
		buttonRB.GetSignal(Input.GetButton(btn5));

		buttonRT.GetSignal(true, Input.GetAxis(RT));
		buttonLT.GetSignal(true, Input.GetAxis(LT));
		
		buttonSelect.GetSignal(Input.GetButton(Btn_Select));
		buttonStart.GetSignal(Input.GetButton(Btn_Start));

	#endregion

    //---------------------------------------------------

		
		
		//左摇杆Y轴分量
		Target_Vertical = Input.GetAxis(AxisY);
		//左摇杆X轴分量
		Target_Horizontal = Input.GetAxis(AxisX);
		//右摇杆Y轴分量
		Jup = Input.GetAxis(Axis5);
		//右摇杆X轴分量
		Jright = -Input.GetAxis(Axis4);

		//是否禁用移动功能，移动分量归0
		if (!IsInputEnable)
		{
			Target_Horizontal = 0;
			Target_Vertical = 0;
		}

		//将控制移动的分量X,Y开方得到DMag
		DMag = Mathf.Sqrt((Target_Horizontal * Target_Horizontal) + (Target_Vertical * Target_Vertical));

		//计算转向,注意该脚本是挂载在角色控制柄上而非角色模型本身，旋转的是角色控制柄（角色模型是控制柄的子对象）
		Dvec = (Target_Horizontal * transform.right) + (Target_Vertical * transform.forward);

	
	
	//按钮信号的进一步判断，比如是按下，还是按住。
	#region 

		isrun = (buttonL3.isPressing);
		islock = buttonR3.OnPressed;
		isdefense = buttonLB.isPressing;
		isjump = buttonA.OnPressed;
		isFontStab = buttonB.OnPressed;
		isStart = buttonStart.OnPressed;

		rb = buttonRB.OnPressed;
		lb = buttonLB.OnPressed;
		rt = buttonRT.OnPressed;
		lt = buttonLT.OnPressed;
		
    #endregion
	
		
		
//按钮连击实现示例，再加入一个计时器可以实现类似战神那样子的连按击杀功能
//		if (buttonB.OnPressed && buttonB.IsExtentTime)
//		{
//			Debug.Log("按钮连击成功");
//		}


	}
}
