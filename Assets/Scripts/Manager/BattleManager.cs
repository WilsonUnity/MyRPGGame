using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.Animations;
using UnityEngine;


[RequireComponent(typeof(CapsuleCollider))]
public class BattleManager : IActionInterface
{

	 
	private CapsuleCollider col;

	private void Awake()
	{
		col = GetComponent<CapsuleCollider>();
	}

	private void Start()
	{
		col.center = new Vector3(0, 2.3f, 0);
		col.isTrigger = true;
		col.radius = 0.25f;
		col.height = 2;
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("Weapon"))
		{
			WeaponController targetWc = other.GetComponentInParent<WeaponController>();
			GameObject attacker = targetWc.wm.am.gameObject;
			GameObject receiver = am.gameObject;

			Vector3 attackerDir = receiver.transform.position - attacker.transform.position;
			Vector3 receiverDir = attacker.transform.position - receiver.transform.position;

			float usefulattackingAngle = Vector3.Angle(attacker.transform.forward, attackerDir);
			float usefulcounterbackAngle = Vector3.Angle(attacker.transform.forward, receiver.transform.forward);

			bool attackVaild = usefulattackingAngle < 45;
			bool counterbackVaild = Mathf.Abs(usefulcounterbackAngle - 180) < 30;

			am.DoDamage(targetWc, attackVaild, counterbackVaild);
			 
		 }
	}
}
