using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class OnGroundSensor : MonoBehaviour
{

	public CapsuleCollider capcol;
	private float radius;
	private Vector3 point1;
	private Vector3 point2;
	public float offset;
	
	private void Awake()
	{
		radius = capcol.radius;
	}

	private void FixedUpdate()
	{
		point1 = transform.position + transform.up * radius;
		point2 = transform.position + transform.up * capcol.height - transform.up * radius;
		Collider[] colliders = Physics.OverlapCapsule(point1, point2, radius,LayerMask.GetMask("Ground"));
		
		if (colliders.Length != 0)
		{
			 SendMessageUpwards("IsGround");
		}
		else
		{
			SendMessageUpwards("NotGround");
		}
	}
}
