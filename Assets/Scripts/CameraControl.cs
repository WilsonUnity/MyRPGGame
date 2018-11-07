using System.Collections;
using System.Collections.Generic;
using System.Net.Mime;
using UnityEditor.Experimental.Animations;
using UnityEngine;
using UnityEngine.UI;

public class CameraControl : MonoBehaviour
{		
	private GameObject cameraController;

	private ActionControl _action;
	
	private GameObject playerController;
	
	private GameObject Model;
	
	private GameObject camera; //Main Camera
	
	public LockTarget lockTarget = null;
	
	private float tempEuler; //保存限制过后的欧拉角

	public bool isDie = false;
	
	[Header("===== camera move speed ======")]
	public float AxixSpeed;
	public float AxiySpeed;
	
	[Header("===== camera delay move =====")]
	private Vector3 cameraDampVelocity;
	public float DampTime;
	
	[Header("===== OnLock =====")]
	public Image LockIco; //锁定时显示的图标
	public bool LockState; //当前是否锁定

	public bool IsAi = false;

	private void Awake()
	{
		LockIco.enabled = false;
		LockState = false;
	}

	private void Start()
	{
		camera = Camera.main.gameObject;
		cameraController = transform.parent.gameObject;
		
		playerController = cameraController.transform.parent.gameObject;
		Model = playerController.GetComponent<ActionControl>().playerModel;
		_action = playerController.GetComponent<ActionControl>();
		
		if (!IsAi)
		{
			//游戏开始时让鼠标游标消失
			Cursor.lockState = CursorLockMode.Locked; 
		}
	}
	
	
	private void Update()
	{
		//锁定敌人的时候
		if (lockTarget != null)
		{
			
			if (!IsAi)
			{
				//调整显示点的位置
				LockIco.rectTransform.position = Camera.main.WorldToScreenPoint(lockTarget.obj.transform.position +
				                                                                new Vector3(0, lockTarget.halfHeight,
					                                                                0));
			}

			//与敌人的距离超过15时解锁
			if (Vector3.Distance(playerController.transform.position, lockTarget.obj.transform.position) >= 10)
			{
				OnLock(null, false, false, IsAi);
			}

			if (lockTarget != null && lockTarget.am != null && lockTarget.am.sm.isDie)
			{
				OnLock(null, false, false, IsAi);
			}
		}
		
	
	}
 
	private void FixedUpdate()
	{


		if (lockTarget == null)
		{
			//将人物model的欧拉角保存在临时变量
			Vector3 tempModelEuler = Model.transform.eulerAngles;

			if (!isDie)
			{
				//旋转人物控制柄
				playerController.transform.Rotate(Vector3.up, _action.InputState.Jright * AxixSpeed * Time.fixedDeltaTime);

				//绕x轴旋转度数进行限制
				tempEuler -= _action.InputState.Jup * AxiySpeed * Time.fixedDeltaTime;
				tempEuler = Mathf.Clamp(tempEuler, -10f, 60f);
				cameraController.transform.localEulerAngles = new Vector3(tempEuler, 0, 0);
			}

			//将人物的欧拉角归位
			Model.transform.eulerAngles = tempModelEuler;
			
		}

		else
		{
			//将人物控制柄的z轴对准locktarget
			Vector3 tempPos = lockTarget.obj.transform.position - Model.transform.position;
			tempPos.y = 0;
			playerController.transform.forward = tempPos;
			
			//让相机控制柄盯着locktarget的脚底
			cameraController.transform.LookAt(lockTarget.obj.transform);
			
		}

		if (!IsAi)
		{
			//相机延迟跟随
			camera.transform.position = Vector3.SmoothDamp(camera.transform.position, transform.position,
				ref cameraDampVelocity, DampTime);

			//相机指向相机控制柄
			camera.transform.LookAt(cameraController.transform);

			//camera.transform.eulerAngles = cameraController.transform.eulerAngles;
		}


	}

	

	/// <summary>
	/// 锁定敌人的功能
	/// </summary>
	public void LockUnLock()
	{

		//箱型不可见框的参数
		Vector3 modelOrigin1 = Model.transform.position;
		Vector3 modelOrigin2 = Model.transform.position + new Vector3(0, 1, 0);
		Vector3 center = modelOrigin2 + Model.transform.forward * 5;
		
		//创建一个箱型不可见框，通过输出与框接触的任何碰撞器来测试碰撞
		Collider[] cols = Physics.OverlapBox(center, new Vector3(0.5f, 0.5f, 5), Model.transform.rotation,
			LayerMask.GetMask((IsAi ? "Player" : "Enemy")));

		//没有锁定任何敌人
		if (cols.Length == 0)
		{
			OnLock(null, false, false, IsAi);
		}

		foreach (var col in cols)
		{
			if (lockTarget != null && lockTarget.obj == col.gameObject)
			{
				OnLock(null, false, false, IsAi);
				break;
			}
			OnLock(new LockTarget(col.gameObject, col.bounds.extents.y), true, true, IsAi);
			break;
		}


	}

	//解锁
	void OnLock(LockTarget _lockTarget, bool _lockico, bool _lockstate, bool _isAi)
	{
		lockTarget = _lockTarget;
		
		if (!_isAi)
		{
			LockIco.enabled = _lockico;
		}

		LockState = _lockstate;
	}


}
