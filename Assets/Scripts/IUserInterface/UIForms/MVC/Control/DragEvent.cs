/*
 * MVC_Control
 *此类挂载在item下的image中，负责拖拽事件 
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragEvent : MonoBehaviour,IBeginDragHandler,IDragHandler,IEndDragHandler
{
	private int gridID;
    public static int lastID;

	private void Start()
	{
		gridID = GetComponentInParent<PickUpDrop>().gridID;
	}

	public void OnBeginDrag(PointerEventData eventData)
	{
		lastID = gridID;
		PickUpDrop.SwapItem(gridID);
	}
	
	public void OnDrag(PointerEventData eventData)
	{
		//不需要做任何事，但此函数必须存在
	}

	public void OnEndDrag(PointerEventData eventData)
	{
		PickUpDrop.SwapItem(gridID);
	}
}
