using System;
using UnityEngine;

[Serializable]
public class CameraDuck : MonoBehaviour
{
	public float newCameraHeight;

	public CameraDuck()
	{
		newCameraHeight = 12f;
	}

	public virtual void OnTriggerEnter(Collider other)
	{
		CartCam.inst.StartCoroutine(CartCam.inst.SetCamHeight(newCameraHeight));
	}

	public virtual void Main()
	{
	}
}
