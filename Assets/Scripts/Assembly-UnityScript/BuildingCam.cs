using System;
using UnityEngine;

[Serializable]
public class BuildingCam : MonoBehaviour
{
	public Vector3 StartRotation;

	public Vector3 EndRotation;

	public float trackTime;

	private Quaternion startRot;

	private Quaternion endRot;

	private float movement;

	private Quaternion initLoc;

	private Transform target;

	public BuildingCam()
	{
		trackTime = 1f;
	}

	public virtual void Awake()
	{
		initLoc = transform.rotation;
		startRot = Quaternion.Euler(StartRotation);
		endRot = Quaternion.Euler(EndRotation);
	}

	public virtual void Start()
	{
		target = Cart.inst.transform;
		if (Performance.GamePerformance == 0)
		{
			UnityEngine.Object.Destroy(gameObject);
		}
	}

	public virtual void FixedUpdate()
	{
		transform.rotation = Quaternion.LookRotation(transform.position - target.position);
	}

	public virtual void OnBecameVisible()
	{
		if (Performance.GamePerformance > 1)
		{
			enabled = true;
		}
	}

	public virtual void OnBecameInvisible()
	{
		enabled = false;
	}

	public virtual void OnDrawGizmosSelected()
	{
		Gizmos.color = Color.blue;
		Gizmos.DrawLine(transform.position, transform.position + transform.rotation * (Quaternion.Euler(StartRotation) * (Vector3.forward * 10f)));
		Gizmos.DrawLine(transform.position, transform.position + transform.rotation * (Quaternion.Euler(EndRotation) * (Vector3.forward * 10f)));
	}

	public virtual void Main()
	{
	}
}
