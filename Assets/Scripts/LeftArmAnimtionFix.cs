using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeftArmAnimtionFix : MonoBehaviour
{

	private Animator anim;
	public Vector3 LeftLowerArmEular;
	public Vector3 LeftUpperArmEular;
	private bool isfix = false;
	private Transform LeftArm;
	private ActionControl ac;
	 
	private void Awake()
	{
		anim = GetComponent<Animator>();
		ac = this.GetComponentInParent<ActionControl>();
	}

	private void Start()
	{
		LeftArm = anim.GetBoneTransform(HumanBodyBones.LeftUpperArm);
	}

	void OnAnimatorIK()
	{

		if (!ac.CheckState("death"))
		{
			if (anim.GetBool("Defense") == false)
			{
				anim.SetBoneLocalRotation(HumanBodyBones.LeftUpperArm,
					Quaternion.Euler(LeftArm.localEulerAngles += LeftUpperArmEular));
				Transform LeftLowerArm = anim.GetBoneTransform(HumanBodyBones.LeftLowerArm);
				LeftLowerArm.localEulerAngles += LeftLowerArmEular;
				anim.SetBoneLocalRotation(HumanBodyBones.LeftLowerArm, Quaternion.Euler(LeftLowerArm.localEulerAngles));
			}

			else if (anim.GetBool("Defense") == true)
			{
				anim.SetBoneLocalRotation(HumanBodyBones.LeftUpperArm,
					Quaternion.Euler(LeftArm.localEulerAngles += LeftUpperArmEular));
			}
		}
	}


}
