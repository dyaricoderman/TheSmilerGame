using System;
using UnityEngine;

[Serializable]
public class RemoveForPerformance : MonoBehaviour
{
	public int RemoveIfPerformanceLessThan;

	public RemoveForPerformance()
	{
		RemoveIfPerformanceLessThan = 2;
	}

	public virtual void Awake()
	{
		if (Performance.GamePerformance < RemoveIfPerformanceLessThan)
		{
			UnityEngine.Object.Destroy(gameObject);
		}
	}

	public virtual void Main()
	{
	}
}
