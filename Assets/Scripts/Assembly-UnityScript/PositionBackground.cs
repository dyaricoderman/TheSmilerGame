using System;
using UnityEngine;

[Serializable]
public class PositionBackground : MonoBehaviour
{
	public Camera cam;

	public float depth;

	public PositionBackground()
	{
		depth = 100f;
	}

	public virtual void Start()
	{
		transform.position = cam.transform.TransformPoint(new Vector3(0f, 0f, depth));
		float z = cam.orthographicSize / 10f * 2f;
		Vector3 localScale = transform.localScale;
		float num = (localScale.z = z);
		Vector3 vector = (transform.localScale = localScale);
		float x = cam.orthographicSize / 10f * 2f * cam.aspect;
		Vector3 localScale2 = transform.localScale;
		float num2 = (localScale2.x = x);
		Vector3 vector3 = (transform.localScale = localScale2);
	}

	public virtual void Main()
	{
	}
}
