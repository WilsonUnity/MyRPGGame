/*
 * 此类负责物品交换逻辑
 * 此类挂载在每个物品对象上
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PickUpDrop : MonoBehaviour,IDropHandler
{

	//获取对象ID,有BagView负责获取
	public int gridID;

	//捡起的物品
	public static Item pickItem;

	public static BagView bv;

	private void Start()
	{
		bv = GameObject.Find("InvGrid").GetComponent<BagView>();
		pickItem = new Item("", null);
	}

	/// <summary>
	/// 交换物品
	/// 请记住，pickItem最终都是new Item("", null)。最终指的是鼠标按键松开
	/// </summary>
	/// <param name="gridID"></param>
	public static void SwapItem(int gridID)
	{
		Item temp = pickItem;
		pickItem = ItemModel.Items[gridID];
		ItemModel.Items[gridID] = temp;
		
		//重新刷新
		bv.ShowItems();
	}

	public void OnDrop(PointerEventData eventData)
	{
		//如果ID不一致就交换物品栏位
		if (gridID != DragEvent.lastID)
		{
			SwapItem(gridID);
		}
	}
}
