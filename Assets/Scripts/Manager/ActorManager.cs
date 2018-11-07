using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActorManager : MonoBehaviour
{

	public ActionControl ac;
	public BattleManager bm;
	public WeaponManager wm;
	public StateManager sm;
	public DirctorManager dm;
	public InteractionManager im;
	
	void Awake ()
	{
		ac = GetComponent<ActionControl>();
	}

	private void Start()
	{
		GameObject sensor = this.transform.Find("Sensor").gameObject;
		bm = Bind<BattleManager>(sensor);
		wm = Bind<WeaponManager>(this.transform.Find("ybot").gameObject);
		sm = Bind<StateManager>(sensor);
		dm = Bind<DirctorManager>(gameObject);
		im = Bind<InteractionManager>(sensor);
		ac.OnACtion += DoAction;
	}

	//演出特殊击杀
	public void DoAction()
	{
		if (im.overlapEvent.Count != 0)
		{
			if (im.overlapEvent[0].eventName == "FrontStab")
			{
				dm.PlayTimeLine(im.overlapEvent[0].eventName, this, im.overlapEvent[0].am);
			}
		}
	}

	public T Bind<T>(GameObject go) where T : IActionInterface
	{
		T temp;
		temp = go.GetComponent<T>();
		if (temp == null)
		{
			temp = go.AddComponent<T>();
		}

		temp.am = this;
        return temp;
	}


	public void DoDamage(WeaponController targetWc, bool attackVaild, bool counterbackVaild)
	{
		if (sm.HP > 0)
		{
			if (sm.isCountBackSuccess)
			{
				if (counterbackVaild)
					targetWc.wm.am.Stunned();
			}
			else if (sm.isCountBackFailure)
			{
				if (attackVaild)
					sm.HP -= 5;
			}
			else if (sm.isimmortal)
			{
				//处于无敌状态，不做任何事
			}
			else if (sm.isDefense)
			{
				sm.Block();
			}
			else
			{
				if (attackVaild)
					sm.HP -= 5;
				print(sm.HP);
			}
		}
	}

	public void Stunned()
	{
		ac.IssueTrigger("stunned");
	}

	public void Hit()
	{
		ac.IssueTrigger("hit");
	}

	public void Die()
	{
		ac.IssueTrigger("die");
		ac.InputState.IsInputEnable = false;
		if (ac.camcon.LockState == true)
		{
			ac.camcon.LockUnLock();
		}

		ac.camcon.isDie = true;
	}

	public void lockUnlock(bool value)
	{
		ac.SetBool("lock", value);
	}

}
