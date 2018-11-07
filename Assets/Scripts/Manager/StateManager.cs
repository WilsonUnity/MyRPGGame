using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateManager : IActionInterface
{

	private float hp = 100;
	public float HpMax = 50;

	[Header("1st order state flags")]
	public bool isGround;
	public bool isJump;
	public bool isAttack;
	public bool isRoll;
	public bool isHit;
	public bool isDie;
	public bool isBlock;
	public bool isDefense;
	public bool isJab;
	public bool isCounterBack;
	public bool isCounterBackEnable;

	[Header("2nd order state flags")]
	public bool isimmortal; //是否无敌

	public bool isCountBackSuccess;
	public bool isCountBackFailure;

	private void Update()
	{
		isGround = am.ac.CheckState("Ground");
		isJump = am.ac.CheckState("Jump");
		isAttack = am.ac.CheckStateTag("attackL") || am.ac.CheckStateTag("attackR");
		isRoll = am.ac.CheckState("Roll");
		isHit = am.ac.CheckState("hit");
		isDie = am.ac.CheckState("death");
		isBlock = am.ac.CheckState("block");
		isDefense = am.ac.CheckState("Defense", "Defense");
		isJab = am.ac.CheckState("Jab");
		isimmortal = isRoll || isJab; //翻滚或者后跃的状态则无敌
		
		isCounterBack = am.ac.CheckState("CounterBack");
		isCountBackSuccess = isCounterBackEnable;
		isCountBackFailure = isCounterBack && !isCounterBackEnable;
		//print(isCountBackSuccess);
    }

	public float HP
	{
		set
		{
			hp += value;
			hp = Mathf.Clamp(value, 0, HpMax);
			if (hp > 0)
			{
				am.Hit();
			}
			else
			{
				am.Die();
			}
		}
		get { return hp; }
	}

	public void setCounterBackEnable(bool value)
	{
		isCounterBackEnable = value;
	}

	public void Block()
	{
		am.ac.IssueTrigger("block");
	}

	private void Start()
	{
		hp = HpMax;
	}

	public void DisplayText()
	{
		//print(HP);
	}
}
