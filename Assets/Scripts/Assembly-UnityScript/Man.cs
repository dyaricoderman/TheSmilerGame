using System;
using UnityEngine;
using UnityScript.Lang;

[Serializable]
public class Man : MonoBehaviour
{
	public Cart cart;

	public Transform LeftArm;

	public Transform RightArm;

	public SmileTag icon;

	private Quaternion HeadTarget;

	private Quaternion LeftTarget;

	private Quaternion RightTarget;

	private Quaternion defaultHead;

	private Quaternion defaultLeft;

	private Quaternion defaultRight;

	[NonSerialized]
	public static UnityScript.Lang.Array ManList = new UnityScript.Lang.Array();

	private bool Marmalized;

	private float armWave;

	public virtual void Awake()
	{
		enabled = false;
		if (ManList.length >= 16)
		{
			ManList = new UnityScript.Lang.Array();
		}
		ManList.Add(this);
		RandomizeArray(ManList);
		defaultLeft = LeftArm.localRotation;
		defaultRight = RightArm.localRotation;
	}

	public static void SetMenState(bool @bool)
	{
		for (int i = 0; i < ManList.length; i++)
		{
			(ManList[i] as Man).enabled = @bool;
		}
	}

	public static void RandomizeArray(UnityScript.Lang.Array arr)
	{
		for (int num = arr.length - 1; num > 0; num--)
		{
			int index = UnityEngine.Random.Range(0, num);
			object value = arr[num];
			arr[num] = arr[index];
			arr[index] = value;
		}
	}

	public virtual void Update()
	{
		if (Marmalized)
		{
			armWave = Mathf.Repeat(Time.time * 160f, 180f);
			LeftTarget = defaultLeft * Quaternion.Euler(0f, 0f, Mathf.PingPong(armWave, 90f) - 45f);
			RightTarget = defaultRight * Quaternion.Euler(0f, 0f, Mathf.PingPong(armWave, 90f) - 45f);
		}
		else if (cart.AccuracyIcon > 1 && (bool)cart.currentSection && !cart.currentSection.CrankUpChain)
		{
			LeftTarget = defaultLeft * Quaternion.Euler(0f, 0f, 60f * (0f - cart.tilt.x));
			RightTarget = defaultRight * Quaternion.Euler(0f, 0f, 60f * (0f - cart.tilt.x));
		}
		else
		{
			LeftTarget = defaultLeft * Quaternion.Euler(0f, -150f, -30f);
			RightTarget = defaultRight * Quaternion.Euler(0f, -150f, 30f);
		}
		LeftArm.localRotation = Quaternion.Slerp(LeftArm.localRotation, LeftTarget, Time.deltaTime * 4f);
		RightArm.localRotation = Quaternion.Slerp(RightArm.localRotation, RightTarget, Time.deltaTime * 4f);
	}

	public virtual void SetMarmalized()
	{
		Marmalized = true;
		icon.Show();
	}

	public virtual void Main()
	{
	}
}
