using System;
using UnityEngine;

[Serializable]
public class RotateOnSpot : MonoBehaviour
{
	public float DegreePerSecond;

	public Vector3 Direction;

	public RotateOnSpot()
	{
		DegreePerSecond = 100f;
		Direction = new Vector3(0f, 0f, 0f);
	}

	public virtual void Awake()
	{
		if (Performance.GamePerformance == 0)
		{
			UnityEngine.Object.Destroy(this);
		}
	}

	public virtual void FixedUpdate()
	{
		transform.Rotate(Direction * DegreePerSecond * Time.deltaTime);
	}

	public virtual void OnBecameVisible()
	{
		enabled = true;
	}

	public virtual void OnBecameInvisible()
	{
		enabled = false;
	}

	public virtual void Main()
	{
	}
}
