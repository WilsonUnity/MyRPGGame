using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionManager : IActionInterface
{

	private CapsuleCollider col;

	public List<EventCasterManager> overlapEvent = new List<EventCasterManager>();
	 
	// Use this for initialization
	void Start ()
	{
		col = GetComponent<CapsuleCollider>();
	}
	

	private void OnTriggerEnter(Collider other)
	{
		EventCasterManager[] ecastmgs = other.GetComponents<EventCasterManager>();
		foreach (var ecastmg in ecastmgs)
		{
			if (!overlapEvent.Contains(ecastmg))
				overlapEvent.Add(ecastmg);
		}
	}

	private void OnTriggerExit(Collider other)
	{
		EventCasterManager[] ecastmgs = other.GetComponents<EventCasterManager>();
		foreach (var ecastmg in ecastmgs)
		{
			if (overlapEvent.Contains(ecastmg))
				overlapEvent.Remove(ecastmg);
		}
	}
}
