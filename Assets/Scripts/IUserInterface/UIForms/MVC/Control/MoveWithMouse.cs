/*
 * 该类实现拖拽图跟随鼠标移动
 * 该类挂载在PickItem上
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MoveWithMouse : MonoBehaviour,ICanvasRaycastFilter
{

	private RectTransform rect;
	private Image ico;

	private void Awake()
	{
		rect = transform.GetComponent<RectTransform>();
		ico = transform.GetChild(0).GetComponent<Image>();
	}

	private void Update()
	{
		rect.anchoredPosition3D = Input.mousePosition;
		if (PickUpDrop.pickItem != null)
		{
			if (PickUpDrop.pickItem.img != null)
			{
				ico.color = Color.white;
				ico.sprite = PickUpDrop.pickItem.img;
			}

			else
			{
				ico.color = Color.clear;
			}
		}
	}

	public bool IsRaycastLocationValid(Vector2 sp, Camera eventCamera)
	{
		return false;
	}
}
