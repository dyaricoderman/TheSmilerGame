using System;
using UnityEngine;

[Serializable]
public class ParticlePerformance : MonoBehaviour
{
	public ParticleSystem PS;

	public float[] EmitCount;

	public float[] ParticalSize;

	public ParticlePerformance()
	{
		EmitCount = new float[3];
		ParticalSize = new float[3];
	}

	public virtual void Start()
	{
		if (!PS)
		{
			PS = (ParticleSystem)gameObject.GetComponent("ParticleSystem");
		}
		if ((bool)PS)
		{
			PS.emissionRate = EmitCount[Performance.GamePerformance];
			PS.startSize = ParticalSize[Performance.GamePerformance];
		}
	}

	public virtual void Main()
	{
	}
}
