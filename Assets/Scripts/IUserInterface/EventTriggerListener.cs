using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class EventTriggerListener : EventTrigger
{

	public delegate void EventDelegate(GameObject gameObject);
	
	public EventDelegate onClick;
	public EventDelegate onDown;
	public EventDelegate onUp;
	public EventDelegate onEnter;
	public EventDelegate onExit;
	public EventDelegate onSelect;
	public EventDelegate onUpdateSelect;

	public static EventTriggerListener Get(GameObject go)
	{
		EventTriggerListener listener = go.GetComponent<EventTriggerListener>();

		if (listener == null)
		{
			listener = go.AddComponent<EventTriggerListener>();
		}

		return listener;
	}

	public override void OnPointerClick(PointerEventData eventData)
	{
		if (onClick != null)
		{
			onClick(gameObject);
		}
	}
	
	public override void OnPointerDown(PointerEventData eventData)
	{
		if (onDown != null)
		{
			onDown(gameObject);
		}
	}

	public override void OnPointerEnter(PointerEventData eventData)
	{
		if (onEnter != null)
		{
			onEnter(gameObject);
		}
	}

	public override void OnPointerExit(PointerEventData eventData)
	{
		if (onExit != null)
		{
			onExit(gameObject);
		}
	}

	public override void OnPointerUp(PointerEventData eventData)
	{
		if (onUp != null)
		{
			onUp(gameObject);
		}
	}
    
	public override void OnSelect(BaseEventData eventBaseData)
	{
		if (onSelect != null)
		{
			onSelect(gameObject);
		}
	}

	public override void OnUpdateSelected(BaseEventData eventBaseData)
	{
		if (onUpdateSelect != null)
		{
			onUpdateSelect(gameObject);
		}
	}

}
