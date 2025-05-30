using System;
using UnityEngine;

[Serializable]
public class SlaveCart : MonoBehaviour
{
	public Cart Target;

	public float FollowDistance;

	public Transform MeshTransform;

	public float TiltRespondRate;

	private float currentDistance;

	private TrackMaster track;

	private Vector3 CartGraphicPosition;

	private float Tilt;

	private float targetTilt;

	public SlaveCart()
	{
		TiltRespondRate = 1f;
	}

	public virtual void Awake()
	{
		track = Target.track;
		CartGraphicPosition = MeshTransform.localPosition;
	}

	public virtual void FixedUpdate()
	{
		currentDistance = Target.currentDistance + FollowDistance;
		transform.position = track.PositionAtDist(currentDistance);
		transform.rotation = track.RotationAtDist(currentDistance);
		MeshTransform.rotation = transform.rotation;
		MeshTransform.position = transform.TransformPoint(CartGraphicPosition);
		if (Target.RunOver)
		{
			targetTilt = 0f;
		}
		else
		{
			targetTilt = Target.MeshTiltDeadSpot;
		}
		Tilt = Mathf.Lerp(Tilt, targetTilt, TiltRespondRate);
		if (!(Tilt <= 0f))
		{
			MeshTransform.RotateAround(transform.TransformPoint(new Vector3(Target.CartWheelWidth, 0f, 0f)), transform.TransformDirection(Vector3.forward), Tilt * -180f);
		}
		else
		{
			MeshTransform.RotateAround(transform.TransformPoint(new Vector3(0f - Target.CartWheelWidth, 0f, 0f)), transform.TransformDirection(Vector3.forward), Tilt * -180f);
		}
	}

	public virtual void Main()
	{
	}
}
