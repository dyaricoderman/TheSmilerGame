using UnityEngine;

public abstract class Tracker
{
	public enum Type
	{
		IMAGE_TRACKER = 0,
		MARKER_TRACKER = 1,
		TEXT_TRACKER = 2
	}

	public abstract bool Start();

	public abstract void Stop();

	protected void PositionCamera(TrackableBehaviour trackableBehaviour, Camera arCamera, QCARManagerImpl.PoseData camToTargetPose)
	{
		arCamera.transform.localPosition = trackableBehaviour.transform.rotation * Quaternion.AngleAxis(90f, Vector3.left) * Quaternion.Inverse(camToTargetPose.orientation) * -camToTargetPose.position + trackableBehaviour.transform.position;
		arCamera.transform.rotation = trackableBehaviour.transform.rotation * Quaternion.AngleAxis(90f, Vector3.left) * Quaternion.Inverse(camToTargetPose.orientation);
	}

	protected void PositionTrackable(TrackableBehaviour trackableBehaviour, Camera arCamera, QCARManagerImpl.PoseData camToTargetPose)
	{
		trackableBehaviour.transform.position = arCamera.transform.TransformPoint(camToTargetPose.position);
		trackableBehaviour.transform.rotation = arCamera.transform.rotation * camToTargetPose.orientation * Quaternion.AngleAxis(270f, Vector3.left);
	}
}
