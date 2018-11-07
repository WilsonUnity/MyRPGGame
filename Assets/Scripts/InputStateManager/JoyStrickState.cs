using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JoyStrickState : IInputState {

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

	private float value = 0;
	
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
	

	#endregion

	public JoyStrickState(InputStateController mController, Transform trans) : base(mController, trans)
	{
		this.MStateName = "JoyStrickState";
	}

	public override void StateUpdate()
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

		/* 以下注释的为原来的代码。手柄模式下无需进行斜边值的映射，代码修改。
		Vector2 tempDAxis = SquareToCircle(new Vector2(Target_Horizontal, Target_Vertical));
		将平移值开方简化程序
		DMag = Mathf.Sqrt((tempDAxis.x * tempDAxis.x) + (tempDAxis.y * tempDAxis.y));
		*/

		//将控制移动的分量X,Y开方得到DMag
		DMag = Mathf.Sqrt((Target_Horizontal * Target_Horizontal) + (Target_Vertical * Target_Vertical));

		//计算转向,注意该脚本是挂载在角色控制柄上而非角色模型本身，旋转的是角色控制柄（角色模型是控制柄的子对象）
		Dvec = (Target_Horizontal * m_transform.right) + (Target_Vertical * m_transform.forward);

		//---------------------------------------------------
	
		//按钮信号的进一步判断，比如是按下，还是按住。
		#region 
        isrun = (buttonL3.isPressing);
		islock = buttonR3.OnPressed;
		isdefense = buttonLB.isPressing;
		isjump = buttonA.OnPressed;
		isFontStab = buttonB.OnPressed;

		rb = buttonRB.OnPressed;
		lb = buttonLB.OnPressed;
		rt = buttonRT.OnPressed;
		lt = buttonLT.OnPressed;
        #endregion

		value = CheckController.GetCurController();
		if (value == 1)
		{
			m_Controller.SetState(new KeyBoardState(m_Controller, m_transform));
		}

	}
}
