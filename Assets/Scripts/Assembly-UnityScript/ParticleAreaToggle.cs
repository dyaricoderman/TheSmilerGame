using System;
using UnityEngine;

[Serializable]
public class ParticleAreaToggle : MonoBehaviour
{
	public ParticleSystem PS;

	public virtual void OnTriggerEnter(Collider other)
	{
		PS.Play();
	}

	public virtual void OnTriggerExit(Collider other)
	{
		PS.Stop();
		PS.Clear();
	}

	public virtual void Main()
	{
	}
}
