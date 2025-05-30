using System;
using UnityEngine;
using UnityScript.Lang;

[Serializable]
public class MattCameraSwitchScript : MonoBehaviour
{
	public Camera[] CameraList;

	private int currentCam;

	public virtual void Start()
	{
		ActivateCam(currentCam);
	}

	public virtual void Update()
	{
		if (Input.GetKeyDown(KeyCode.C))
		{
			currentCam++;
			currentCam = (int)Mathf.Repeat(currentCam, Extensions.get_length((System.Array)CameraList));
			ActivateCam(currentCam);
		}
	}

	public virtual void ActivateCam(int id)
	{
		for (int i = 0; i < Extensions.get_length((System.Array)CameraList); i++)
		{
			CameraList[i].enabled = false;
		}
		CameraList[id].enabled = true;
	}

	public virtual void Main()
	{
	}
}
