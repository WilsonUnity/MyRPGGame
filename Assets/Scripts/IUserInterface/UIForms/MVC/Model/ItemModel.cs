/*
 * MVC_Model
 * 保存背包数据
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemModel : MonoBehaviour {

	public static List<Item> Items;
	public int size = 18;
	private Sprite[] sprite;
	
	private void Awake()
	{
		sprite = new Sprite[size];
		Items = new List<Item>();

		for (int i = 0; i < BagView.row * BagView.col + 14; i++)
		{
			Items.Add(new Item("", null));
		}


		for (int i = 0; i < size; i++)
		{
			string name = i < 9 ? "0" + (i + 1) : "" + (i + 1);
			sprite[i] = Resources.Load(name,typeof(Sprite)) as Sprite;
			Items[i] = new Item("", sprite[i]);
		}
	}
}
