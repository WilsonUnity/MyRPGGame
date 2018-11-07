using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerControl : MonoBehaviour
{

	private Animator anim;
	private ActionControl ac;

	private void Awake()
	{
		ac = GetComponentInParent<ActionControl>();
		anim = GetComponent<Animator>();
	}

	private void ResetTrigger(string triggerName)
	{
		if (ac.leftIsShield)
		{
			anim.ResetTrigger(triggerName);
		}
	}
}
