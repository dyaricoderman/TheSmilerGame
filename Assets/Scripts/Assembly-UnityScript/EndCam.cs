using System;
using UnityEngine;

[Serializable]
public class EndCam : MonoBehaviour
{
	public Transform EndCamLook;

	public virtual void OnTriggerEnter(Collider other)
	{
		CartCam.inst.SetEndCam(EndCamLook);
	}

	public virtual void Main()
	{
	}
}
