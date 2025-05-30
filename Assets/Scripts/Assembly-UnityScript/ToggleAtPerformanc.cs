using System;
using UnityEngine;
using UnityScript.Lang;

[Serializable]
public class ToggleAtPerformanc : MonoBehaviour
{
	public Transform[] GameObjectsAtPerformanceLevel;

	private Transform keep;

	public virtual void Start()
	{
		keep = GameObjectsAtPerformanceLevel[Performance.GamePerformance];
		for (int i = 0; i < Extensions.get_length((System.Array)GameObjectsAtPerformanceLevel); i++)
		{
			if (GameObjectsAtPerformanceLevel[i] != keep)
			{
				UnityEngine.Object.Destroy(GameObjectsAtPerformanceLevel[i].gameObject);
			}
		}
	}

	public virtual void Main()
	{
	}
}
