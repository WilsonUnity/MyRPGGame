using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockTarget
{

	public GameObject obj;
	public float halfHeight;
	public ActorManager am;
	public LockTarget(GameObject obj, float halfHeight)
	{
		this.obj = obj;
		this.halfHeight = halfHeight;
		am = obj.GetComponent<ActorManager>();
	}
}
