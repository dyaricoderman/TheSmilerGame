using System;
using UnityEngine;

[Serializable]
public class DoorOpen : MonoBehaviour
{
	public float Force;

	private RaycastHit hit;

	public DoorOpen()
	{
		Force = 100f;
	}

	public virtual void Start()
	{
	}

	public virtual void Update()
	{
		if (!Input.GetMouseButtonDown(0))
		{
			return;
		}
		Debug.Log("Clicked " + Camera.main.gameObject);
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		if (Physics.Raycast(ray, out hit, float.PositiveInfinity))
		{
			Debug.Log(hit.collider.gameObject);
			Debug.DrawLine(Camera.main.transform.position, hit.point);
			if ((bool)hit.collider.attachedRigidbody)
			{
				hit.collider.GetComponent<Rigidbody>().AddForceAtPosition(ray.direction.normalized * Force, hit.point, ForceMode.VelocityChange);
			}
		}
	}

	public virtual void Main()
	{
	}
}
