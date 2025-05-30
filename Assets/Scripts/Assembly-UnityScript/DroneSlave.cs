using System;
using UnityEngine;

[Serializable]
public class DroneSlave : MonoBehaviour
{
	public CartDrone Target;

	public float FollowDistance;

	private TrackMaster track;

	private float currentDistance;

	public virtual void Start()
	{
		track = Target.track;
	}

	public virtual void FixedUpdate()
	{
		currentDistance = Target.currentDistance + FollowDistance;
		transform.position = track.PositionAtDist(currentDistance);
		transform.rotation = track.RotationAtDist(currentDistance);
	}

	public virtual void Main()
	{
	}
}
