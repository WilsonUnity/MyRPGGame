using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace IUserInterface
{
	public class UIMeskMgr : MonoBehaviour
	{

		private static UIMeskMgr _instance;

		private Transform _transRootCanvas;

		private Transform _topPanel;

		private GameObject _meskPanel;

		private Camera _camera;

		private Transform _transScript;

		private float _originalCameraDepth;

		public static UIMeskMgr GetInstance()
		{
			if (_instance == null)
			{
				_instance = new GameObject("_UIMeskMgr").AddComponent<UIMeskMgr>();
			}

			return _instance;
		}
		
		
		//--------------------------------------------------------------------------------------
		

		private void Awake()
		{
			_transRootCanvas = GameObject.FindGameObjectWithTag(UISysDefined.SyS_TAG_CANVAS).transform;
			_transScript = UnityHelper.DeepFind(_transRootCanvas, UISysDefined.SYS_SCRIPTMANAGER_NODE);
			UnityHelper.SetParent(_transScript, this.gameObject.transform);
			_topPanel = _transRootCanvas;
			_meskPanel = UnityHelper.DeepFind(_transRootCanvas.transform, "_UIMaskPanel").gameObject;
			_camera = GameObject.FindGameObjectWithTag("_TagUICamera").gameObject.GetComponent<Camera>();

			if (_camera != null)
			{
				_originalCameraDepth = _camera.depth;
			}
			else
			{
				Debug.Log("无法找到相机");
			}
		}

		
		//--------------------------------------------------------------------------------------
		
		/// <summary>
		/// 激活遮罩，用来阻止对非栈顶窗体的操作
		/// </summary>
		/// <param name="goUIForm">窗体物件</param>
		/// <param name="_lucency">透明度类型</param>
		public void ShowMesk(GameObject goUIForm, UIFormLucency _lucency = UIFormLucency.Lucency)
		{

			//顶层窗体下移
			_topPanel.transform.SetAsLastSibling();

			//启用遮罩窗体以及设置透明度
			switch (_lucency)
			{
				//完全透明，不能穿透
				case UIFormLucency.Lucency:
					_meskPanel.SetActive(true);
					Color newColor1 = new Color(255 / 255F, 255 / 255F, 255 / 255F, 0F / 255F);
					_meskPanel.GetComponent<Image>().color = newColor1;
					break;
				//半透明，不能穿透
				case UIFormLucency.Translucence:
					_meskPanel.SetActive(true);
					Color newColor2 = new Color(220 / 255F, 220 / 255F, 220 / 255F, 50 / 255F);
					_meskPanel.GetComponent<Image>().color = newColor2;
					break;
				//低透明，不能穿透
				case UIFormLucency.ImPenetrable:
					_meskPanel.SetActive(true);
					Color newColor3 = new Color(50 / 255F, 50 / 255F, 50 / 255F, 200F / 255F);
					_meskPanel.GetComponent<Image>().color = newColor3;
					break;
				//可以穿透
				case UIFormLucency.Pentrate:
					if (_meskPanel.activeInHierarchy)
					{
						_meskPanel.SetActive(false);
					}

					break;

				default:
					break;
			}
			
			//遮罩窗体下移
			_meskPanel.transform.SetAsLastSibling();
			//显示窗体的下移
			goUIForm.transform.SetAsLastSibling();
			
//			//增加当前UI摄像机的层深（保证当前摄像机为最前显示）
//			if (_camera!=null)
//			{
//				_camera.depth = _camera.depth + 100;    //增加层深
//			}
			
		}//ShowMesk_End
		
		
		//--------------------------------------------------------------------------------------
		

		/// <summary>
		/// 禁用遮罩
		/// </summary>
		public void CancelMesk()
		{
			_topPanel.SetAsFirstSibling();

			if (_meskPanel.activeInHierarchy)
			{
				_meskPanel.SetActive(false);
			}
			
			//恢复景深，待测试
		}
	}
}

