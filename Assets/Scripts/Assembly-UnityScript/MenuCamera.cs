using System;
using UnityEngine;

[Serializable]
public class MenuCamera : MonoBehaviour
{
	public Vector3 ViewPoint1;

	public Vector3 ViewPoint2;

	public float ScanTime;

	public Camera Cam;

	private float Move;

	private float AttentionTimeOut;

	private Vector3 AttentionFocus;

	public MenuCamera()
	{
		ScanTime = 3f;
	}

	public virtual void CamState(bool state)
	{
		enabled = state;
	}

	public virtual void Update()
	{
		Move += Time.deltaTime;
		if (!(AttentionTimeOut <= 0f))
		{
			transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(AttentionFocus - transform.position), Time.deltaTime * 4f);
			AttentionTimeOut -= Time.deltaTime;
		}
		else
		{
			transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(LookTarget() - transform.position), Time.deltaTime * 4f);
		}
		if (Input.touchCount > 0)
		{
			AttentionTimeOut = 1.5f + UnityEngine.Random.value * 2f;
			AttentionFocus = Cam.ScreenToWorldPoint(new Vector3(Input.touches[0].position.x, Input.touches[0].position.y, Cam.nearClipPlane));
		}
	}

	public virtual Vector3 LookTarget()
	{
		return Vector3.Lerp(transform.parent.TransformPoint(ViewPoint1), transform.parent.TransformPoint(ViewPoint2), Mathf.PingPong(Move, ScanTime) / ScanTime);
	}

	public virtual void OnDrawGizmosSelected()
	{
		Gizmos.color = Color.blue;
		Gizmos.DrawSphere(LookTarget(), 0.5f);
		Gizmos.DrawLine(transform.parent.TransformPoint(ViewPoint1), transform.parent.TransformPoint(ViewPoint2));
	}

	public virtual void Main()
	{
	}
}
