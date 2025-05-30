using System;
using UnityEngine;
using UnityScript.Lang;

[Serializable]
public class TestInterfaceJS : MonoBehaviour
{
	public string[] TestUnlocks;

	public virtual void Start()
	{
	}

	public virtual Rect fluidRect(float x, float y, float width, float height)
	{
		return new Rect(x * (float)Screen.width, y * (float)Screen.height, width * (float)Screen.width, height * (float)Screen.height);
	}

	public virtual void OnGUI()
	{
		for (int i = 0; i < Extensions.get_length((System.Array)TestUnlocks); i++)
		{
			if (GUI.Button(fluidRect(0f, (float)i * 0.15f, 0.2f, 0.15f), TestUnlocks[i]))
			{
				TrackUpgrade.UnlockUpgrade(TestUnlocks[i]);
			}
		}
	}

	public virtual void Main()
	{
	}
}
