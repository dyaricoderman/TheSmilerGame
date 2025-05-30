using System;
using UnityEngine;

[Serializable]
public class Submission : MonoBehaviour
{
	public Transform mainArm;

	public Transform Cart;

	public float Speed;

	public virtual void Awake()
	{
		if (Performance.GamePerformance < 2)
		{
			enabled = false;
		}
		if (Performance.GamePerformance < 1)
		{
			UnityEngine.Object.Destroy(this);
		}
	}

	public virtual void Update()
	{
		mainArm.RotateAround(mainArm.position, Cart.TransformDirection(Vector3.up), Speed * -0.5f * Time.deltaTime);
		Cart.RotateAround(Cart.position, Cart.TransformDirection(Vector3.up), Speed * Time.deltaTime);
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
