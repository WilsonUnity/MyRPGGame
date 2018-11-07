using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : IActionInterface
{

	private GameObject weaponHandleL;
	private GameObject weaponHandleR;

	private Collider colL; //left weapon collider
	private Collider colR; //right weapon collider

	public WeaponController wcL; 
	public WeaponController wcR;


	private void Start()
	{
		weaponHandleL = UnityHelper.DeepFind(transform, "weaponHandleL").gameObject;
		weaponHandleR = UnityHelper.DeepFind(transform, "weaponHandleR").gameObject;

		colL = weaponHandleL.GetComponentInChildren<Collider>();
		colR = weaponHandleR.GetComponentInChildren<Collider>();
		
		wcL = BindWeaponController(weaponHandleL);
		wcR = BindWeaponController(weaponHandleR);
	}

	//自动为武器添加控制组件，目标物体可以获取
	public WeaponController BindWeaponController(GameObject tarOnject)
	{
		WeaponController temp;
		temp = tarOnject.GetComponent<WeaponController>();
		if (temp == null)
		{
			temp = tarOnject.AddComponent<WeaponController>();
		}

		temp.wm = this;
		return temp;
	}

	//动画事件响应，启用武器触发功能
	public void WeaponEnable()
	{
		if (am.ac.CheckStateTag("attackL"))
		{
			colL.enabled = true;
		}
		else
		{
			colR.enabled = true;
		}
	}

	//动画事件响应，禁用武器触发功能
	public void WeaponDisenable()
	{
		colL.enabled = false;
		colR.enabled = false;
	}

	public void CounterBackDisEnable()
	{
		am.sm.setCounterBackEnable(false);
	}

	public void CounterBackEnable()
	{
		am.sm.setCounterBackEnable(true);
	}
}
