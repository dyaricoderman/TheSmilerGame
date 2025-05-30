using System;
using UnityEngine;
using UnityScript.Lang;

[Serializable]
[ExecuteInEditMode]
public class LightProbesOnTrack : MonoBehaviour
{
	[NonSerialized]
	public static LightProbesOnTrack inst;

	public TrackMaster track;

	public float ProbeSpaceing;

	private Vector3[] probLocations;

	private UnityScript.Lang.Array DynamicProbes;

	public LightProbesOnTrack()
	{
		ProbeSpaceing = 10f;
		DynamicProbes = new UnityScript.Lang.Array();
	}

	public virtual void Awake()
	{
		inst = this;
	}

	public virtual void CaculateProbes()
	{
		DynamicProbes = new UnityScript.Lang.Array();
		MonoBehaviour.print("Recaculateing Light Probes");
		for (int i = 0; i < Extensions.get_length((System.Array)track.TrackParts); i++)
		{
			CaculateForTrackSection(track.TrackParts[i]);
		}
	}

	public virtual void CaculateForTrackSection(TrackSection ts)
	{
		ts.EditorComputeTrack();
		for (float num = 0f; num < ts.GetLength(); num += ProbeSpaceing)
		{
			DynamicProbes.Add(ts.PositionAtDist(num));
		}
		probLocations = (Vector3[])DynamicProbes.ToBuiltin(typeof(Vector3));
		if ((bool)ts.UpgradesTo)
		{
			CaculateForTrackSection(ts.UpgradesTo);
		}
	}

	public virtual void OnDrawGizmos()
	{
		if (probLocations != null)
		{
			for (int i = 0; i < Extensions.get_length((System.Array)probLocations); i++)
			{
				Gizmos.color = Color.blue;
				Gizmos.DrawSphere(probLocations[i], 3f);
			}
		}
	}

	public virtual void Main()
	{
	}
}
