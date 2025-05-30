using System;
using UnityEngine;

[Serializable]
public class ViewPoints : MonoBehaviour
{
	public Transform LookTarget;

	public float TrackTime;

	public float zRoll;

	public ViewPoints()
	{
		TrackTime = 2f;
	}

	public virtual void OnDrawGizmosSelected()
	{
		if ((bool)LookTarget)
		{
			Gizmos.color = Color.green;
			Gizmos.DrawSphere(transform.position, 4f);
			Gizmos.DrawLine(transform.position, LookTarget.position);
			Gizmos.color = Color.red;
			Gizmos.DrawSphere(LookTarget.position, 1f);
		}
	}

	public virtual void Main()
	{
	}
}
