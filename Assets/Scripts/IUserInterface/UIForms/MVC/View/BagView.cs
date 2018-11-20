/*
 * MVC_View
 * 复制背包的显示
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BagView : MonoBehaviour {

	public static int row = 5; //行
	public static int col = 5; //列

	public int Space = 10;

	public GameObject grid; //格子预制体

	public GameObject WeaponSlot;

	public GameObject[] slots;
	
	private float _width; //格子的宽度
	private float _height; //格子的高度
	private int _finID;

	private void Awake()
	{

	}

	private void Start()
	{
		//获取格子宽高
		_width = grid.GetComponent<RectTransform>().rect.width + Space;
		_height = grid.GetComponent<RectTransform>().rect.height + Space;
		
		for (int x = 0; x < row; x++)
		{
			for (int y = 0; y < col; y++)
			{
				//计算每个格子的ID值
				int id = y + x * col;
				
				//生成格子
				GameObject go = Instantiate(grid, GetCenterPosition(y,x), Quaternion.identity);

				go.name = "Item" + (id + 1);
				
				//格子置于父对象Bag
				go.transform.SetParent(this.transform);
				
				ShowItem(go, id);

				go.transform.GetComponent<PickUpDrop>().gridID = id;

				_finID = id;
			}
		}

		//(67行)由于生命周期问题，武器栏的gridID直接在属性面板赋予,不然会有BUG
		//另外一种解决方案是将DragEvent中的赋值操作转到Update
		for (int i = 0; i < slots.Length; i++)
		{
			++_finID;
			ShowItem(slots[i], _finID);
			//slots[i].transform.GetComponent<PickUpDrop>().gridID = _finID;
		}
	}

	
	
	//将生成的格子整体居中
	private Vector2 GetCenterPosition(int x, int y)
	{
		return new Vector2((transform.position.x - row * 0.5f * _width) + x * _width,
			(transform.position.y + col * 0.5f * _height) - y * _height);
	}

	//显示每个格子的图片
	public void ShowItem(GameObject go, int id)
	{
		if (id < 24)
		{
			Text ItemText = go.GetComponentInChildren<Text>();
			ItemText.text = ItemModel.Items[id].name;
		}

		Image ItemImg = go.transform.GetChild(0).GetComponent<Image>();

		
		if (ItemModel.Items[id].img != null){
			ItemImg.color = Color.white;
		}else{
			ItemImg.color = Color.clear;
		}

		ItemImg.sprite = ItemModel.Items[id].img;

	}
	
	//交换后刷新背包
	public void ShowItems()
	{
		for (int i = 0; i < col * row + 14; i++)
		{
			if (i <= 24)
			{
				GameObject tempGo = this.transform.GetChild(i).gameObject;
				ShowItem(tempGo, i);
			}
			else
			{
				int temp = i - 25;
				GameObject tempGo = WeaponSlot.transform.GetChild(temp).gameObject;
				ShowItem(tempGo, i);
			}
		}
	}
	
	//重新整齐排列，按钮触发，先立个Flag,这段代码以后要优化
	public void ReArrange()
	{
		List<Sprite> sprites = new List<Sprite>();
		for (int i = 0; i < row * col; i++)
		{
			if (ItemModel.Items[i].img!=null)
			{
				sprites.Add(ItemModel.Items[i].img);
			}
			Debug.Log(sprites.Count);
		}

		for (int i = 0; i < row * col; i++)
		{
			GameObject tempObject = transform.GetChild(i).gameObject;
			Image img = tempObject.transform.GetChild(0).GetComponent<Image>();

			if (i < sprites.Count)
			{
				img.sprite = sprites[i];
				img.color = Color.white;
				ItemModel.Items[i].img = sprites[i];
			}
			else
			{
				img.sprite = null;
				img.color = Color.clear;
				ItemModel.Items[i].img = null;
			}
			
		}
		
	}
}
