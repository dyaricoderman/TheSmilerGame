using System;
using UnityEngine;

[Serializable]
public class TestTrackerInterface : MonoBehaviour
{
	public virtual void Start()
	{
	}

	public virtual void OnGUI()
	{
		if (GUI.Button(new Rect(0f, 0f, (float)Screen.width * 0.15f, (float)Screen.height * 0.15f), "Track events"))
		{
			for (int i = 0; i < 10; i++)
			{
				Tracker2.instance.LogWithParameters("Purchase Item", "Item " + i);
			}
			Debug.Log("Tracking done!");
		}
	}

	public virtual void Main()
	{
	}
}
