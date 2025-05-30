using System;
using System.Collections;
using Boo.Lang.Runtime;
using UnityEngine;
using UnityScript.Lang;

[Serializable]
public class ViewTrack : MonoBehaviour
{
	public ViewPoints[] viewPoints;

	[NonSerialized]
	public static string[] priorityViews = new string[0];

	public string UpgradeAssociation;

	private AnimationCurve ViewCurveX;

	private AnimationCurve ViewCurveY;

	private AnimationCurve ViewCurveZ;

	private AnimationCurve LookCurveX;

	private AnimationCurve LookCurveY;

	private AnimationCurve LookCurveZ;

	private AnimationCurve RollCurve;

	private float TotalViewTime;

	public Transform TrackTarget;

	private bool ComputeFinished;

	public ViewTrack()
	{
		UpgradeAssociation = "None";
	}

	public virtual void Awake()
	{
		ComputeViewPath();
	}

	public virtual void FollowDrone(Transform Drone)
	{
		TrackTarget = Drone;
	}

	public static void ResetPrioritysList()
	{
		priorityViews = new string[0];
	}

	public static void AddPriorityView(string TU)
	{
		UnityScript.Lang.Array array = new UnityScript.Lang.Array((IEnumerable)priorityViews);
		bool flag = false;
		for (int i = 0; i < array.length; i++)
		{
			if (RuntimeServices.EqualityOperator(array[i], TU))
			{
				flag = true;
			}
		}
		if (!flag)
		{
			array.Add(TU);
			priorityViews = (string[])array.ToBuiltin(typeof(string));
		}
	}

	public static string PopPriorityView()
	{
		UnityScript.Lang.Array array = new UnityScript.Lang.Array((IEnumerable)priorityViews);
		object obj = array.Pop();
		if (!(obj is string))
		{
			obj = RuntimeServices.Coerce(obj, typeof(string));
		}
		string result = (string)obj;
		priorityViews = (string[])array.ToBuiltin(typeof(string));
		return result;
	}

	public virtual void SetCamera(Transform cam, float time)
	{
		cam.position = ViewPath(time);
		cam.rotation = ViewRotation(cam, time) * ViewRoll(time);
	}

	public virtual Vector3 ViewPath(float time)
	{
		time = Mathf.PingPong(time, TotalViewTime);
		float time2 = Mathf.SmoothStep(0f, TotalViewTime, time / TotalViewTime);
		return new Vector3(ViewCurveX.Evaluate(time2), ViewCurveY.Evaluate(time2), ViewCurveZ.Evaluate(time2));
	}

	public virtual Vector3 LookPath(float time)
	{
		Vector3 result;
		if ((bool)TrackTarget)
		{
			result = TrackTarget.position;
		}
		else
		{
			time = Mathf.PingPong(time, TotalViewTime);
			float time2 = Mathf.SmoothStep(0f, TotalViewTime, time / TotalViewTime);
			result = new Vector3(LookCurveX.Evaluate(time2), LookCurveY.Evaluate(time2), LookCurveZ.Evaluate(time2));
		}
		return result;
	}

	public virtual Quaternion ViewRoll(float time)
	{
		time = Mathf.PingPong(time, TotalViewTime);
		float time2 = Mathf.SmoothStep(0f, TotalViewTime, time / TotalViewTime);
		return Quaternion.Euler(0f, 0f, RollCurve.Evaluate(time2));
	}

	public virtual Quaternion ViewRotation(Transform Cam, float time)
	{
		time = Mathf.PingPong(time, TotalViewTime);
		return Quaternion.LookRotation(LookPath(time) - Cam.position);
	}

	public virtual float GetTotalTime()
	{
		return TotalViewTime;
	}

	public virtual void ComputeViewPath()
	{
		if (ComputeFinished)
		{
			return;
		}
		Keyframe[] array = new Keyframe[Extensions.get_length((System.Array)viewPoints)];
		Keyframe[] array2 = new Keyframe[Extensions.get_length((System.Array)viewPoints)];
		Keyframe[] array3 = new Keyframe[Extensions.get_length((System.Array)viewPoints)];
		Keyframe[] array4 = new Keyframe[Extensions.get_length((System.Array)viewPoints)];
		Keyframe[] array5 = new Keyframe[Extensions.get_length((System.Array)viewPoints)];
		Keyframe[] array6 = new Keyframe[Extensions.get_length((System.Array)viewPoints)];
		Keyframe[] array7 = new Keyframe[Extensions.get_length((System.Array)viewPoints)];
		for (int i = 0; i < Extensions.get_length((System.Array)viewPoints); i++)
		{
			array[i] = new Keyframe(TotalViewTime, viewPoints[i].transform.position.x);
			array2[i] = new Keyframe(TotalViewTime, viewPoints[i].transform.position.y);
			array3[i] = new Keyframe(TotalViewTime, viewPoints[i].transform.position.z);
			array4[i] = new Keyframe(TotalViewTime, viewPoints[i].LookTarget.position.x);
			array5[i] = new Keyframe(TotalViewTime, viewPoints[i].LookTarget.position.y);
			array6[i] = new Keyframe(TotalViewTime, viewPoints[i].LookTarget.position.z);
			array7[i] = new Keyframe(TotalViewTime, viewPoints[i].zRoll);
			if (i < Extensions.get_length((System.Array)viewPoints) - 1)
			{
				TotalViewTime += viewPoints[i].TrackTime;
			}
		}
		ViewCurveX = new AnimationCurve(array);
		ViewCurveY = new AnimationCurve(array2);
		ViewCurveZ = new AnimationCurve(array3);
		LookCurveX = new AnimationCurve(array4);
		LookCurveY = new AnimationCurve(array5);
		LookCurveZ = new AnimationCurve(array6);
		RollCurve = new AnimationCurve(array7);
		for (int i = 0; i < Extensions.get_length((System.Array)viewPoints); i++)
		{
			ViewCurveX.SmoothTangents(i, 0f);
			ViewCurveY.SmoothTangents(i, 0f);
			ViewCurveZ.SmoothTangents(i, 0f);
			LookCurveX.SmoothTangents(i, 0f);
			LookCurveY.SmoothTangents(i, 0f);
			LookCurveZ.SmoothTangents(i, 0f);
			RollCurve.SmoothTangents(i, 0f);
		}
		ComputeFinished = true;
	}

	public virtual void OnDrawGizmosSelected()
	{
		int num = 40;
		if (ComputeFinished)
		{
			for (float num2 = 0f; num2 < TotalViewTime; num2 += TotalViewTime / (float)num)
			{
				Gizmos.color = Color.yellow;
				Gizmos.DrawLine(ViewPath(num2), ViewPath(num2 + TotalViewTime / (float)num));
				Gizmos.color = Color.red;
				Gizmos.DrawLine(LookPath(num2), LookPath(num2 + TotalViewTime / (float)num));
				Gizmos.color = Color.green;
				Gizmos.DrawLine(ViewPath(num2), LookPath(num2));
			}
		}
	}

	public virtual void Main()
	{
	}
}
