using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckController{

	private static string str = null;
	private static float num = 0;
	
	public static int GetCurController()
	{
		if (Input.anyKeyDown)
		{
			foreach (KeyCode keycode in Enum.GetValues(typeof(KeyCode)))
			{
				if (Input.GetKeyDown(keycode))
				{
					str = keycode.ToString();
					if (str.Length >= 3 && str.Substring(0, 3).Equals("Joy"))
					{
						return -1;
					}
					else
					{
						return 1;
					}
				}
			}
		}

		else
		{
			num = Input.GetAxis("axisX") + Input.GetAxis("axisY") + Input.GetAxis("axis4") + Input.GetAxis("axis5") +
			      Input.GetAxis("LT") + Input.GetAxis("RT") + Input.GetAxis("axis6") + Input.GetAxis("axis7");
			if (num != 0)
			{
				return -1;
			}
		}

		return 0;
	}
}
