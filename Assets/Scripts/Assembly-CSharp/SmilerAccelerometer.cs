using UnityEngine;

public class SmilerAccelerometer : MonoBehaviour
{
	public float ReturnRate = 1f;

	public float AccelerometerStrength = 1f;

	private Quaternion StartRot;

	private Vector3 lastAcc = Vector3.zero;

	private int SmoothIndex;

	private Vector3[] SmoothArray = new Vector3[10];

	private void Start()
	{
		InitPosition();
	}

	[ContextMenu("Init Position")]
	private void InitPosition()
	{
		StartRot = base.transform.rotation;
	}

	private void Update()
	{
		Vector3 toDirection = SmoothAcc(Quaternion.Euler(90f, 90f, 0f) * Input.acceleration);
		Quaternion quaternion = Quaternion.FromToRotation(lastAcc, toDirection);
		quaternion.w *= 1f / AccelerometerStrength;
		base.transform.rotation *= quaternion;
		base.transform.rotation = Quaternion.RotateTowards(base.transform.rotation, StartRot, Time.deltaTime * ReturnRate);
		lastAcc = toDirection;
	}

	private Vector3 SmoothAcc(Vector3 Acc)
	{
		SmoothArray[SmoothIndex] = Acc;
		SmoothIndex++;
		SmoothIndex = (int)Mathf.Repeat(SmoothIndex, SmoothArray.Length);
		Vector3 zero = Vector3.zero;
		for (int i = 0; i < SmoothArray.Length; i++)
		{
			zero += SmoothArray[i];
		}
		return zero / SmoothArray.Length;
	}
}
