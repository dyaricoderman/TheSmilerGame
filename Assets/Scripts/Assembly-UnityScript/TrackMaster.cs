using System;
using UnityEngine;
using UnityScript.Lang;

[Serializable]
public class TrackMaster : MonoBehaviour
{
	public TrackSection[] TrackParts;

	[NonSerialized]
	public static TrackMaster inst;

	private float[] PartStartDist;

	private float TrackTotalDist;

	public bool NonGameTrack;

	public bool Upgradeable;

	public bool TrackComputationFinished;

	public TrackMaster()
	{
		Upgradeable = true;
	}

	public virtual void Awake()
	{
		if (!NonGameTrack)
		{
			inst = this;
		}
	}

	public virtual void Start()
	{
		CalcTrack();
		TrackComputationFinished = true;
	}

	public virtual float GetTotalTrackDist()
	{
		return TrackTotalDist;
	}

	public virtual float ReverseLookUp(Vector3 pos)
	{
		float num = float.PositiveInfinity;
		float num2 = float.PositiveInfinity;
		float num3 = 0f;
		float result = 0f;
		for (; num3 < TrackTotalDist; num3 += 2f)
		{
			num = (pos - PositionAtDist(num3)).magnitude;
			if (!(num >= num2))
			{
				num2 = num;
				result = num3;
			}
		}
		return result;
	}

	public virtual TrackSection GetCurrentTrackSection(float dist)
	{
		dist = Mathf.Repeat(dist, TrackTotalDist);
		int i;
		for (i = 0; dist > PartStartDist[i]; i++)
		{
		}
		return TrackParts[i];
	}

	public virtual void SetLod(Transform cart, float dist, float drawDist)
	{
		if (NonGameTrack)
		{
			return;
		}
		for (int i = 0; i < Extensions.get_length((System.Array)TrackParts); i++)
		{
			if (Performance.GamePerformance > 0)
			{
				if (!((cart.position - TrackParts[i].getAveragePosition()).magnitude <= drawDist))
				{
					TrackParts[i].SetQuality(TrackQuality.Distance);
				}
				else
				{
					TrackParts[i].SetQuality(TrackQuality.Low);
				}
			}
			else
			{
				TrackParts[i].SetQuality(TrackQuality.Distance);
			}
		}
		Debug.DrawLine(PositionAtDist(dist), PositionAtDist(dist + drawDist));
		dist = Mathf.Repeat(dist, TrackTotalDist);
		float t = dist + drawDist;
		t = Mathf.Repeat(t, TrackTotalDist);
		int j = 0;
		int k = 0;
		for (; dist > PartStartDist[j]; j++)
		{
		}
		for (; t > PartStartDist[k]; k++)
		{
		}
		k = (int)Mathf.Repeat(k + 1, Extensions.get_length((System.Array)TrackParts));
		while (j != k)
		{
			if (Performance.GamePerformance > 0)
			{
				TrackParts[j].SetQuality(TrackQuality.High);
			}
			else
			{
				TrackParts[j].SetQuality(TrackQuality.Low);
			}
			j++;
			j = (int)Mathf.Repeat(j, Extensions.get_length((System.Array)TrackParts));
		}
	}

	public virtual void ApplyUpgrades()
	{
		if (!Upgradeable)
		{
			return;
		}
		for (int i = 0; i < TrackParts.Length; i++)
		{
			int num = TrackUpgrade.UpgradeID(TrackParts[i].UpgradeName);
			if (num != -1)
			{
				TrackParts[i].CheckTrackUpgrade(this, TrackUpgrade.getCurrentLevel(num), num, i);
			}
		}
	}

	public virtual void CalcTrack()
	{
		ApplyUpgrades();
		float num = 0f;
		PartStartDist = new float[Extensions.get_length((System.Array)TrackParts)];
		for (int i = 0; i < TrackParts.Length; i++)
		{
			TrackParts[i].CalcTrack();
			num += TrackParts[i].GetLength();
			PartStartDist[i] = num;
		}
		for (int i = 0; i < TrackParts.Length; i++)
		{
			if (i < Extensions.get_length((System.Array)TrackParts) - 1)
			{
				TrackParts[i].JoinEndToTrack(TrackParts[i + 1]);
			}
			else
			{
				TrackParts[i].JoinEndToTrack(TrackParts[0]);
			}
		}
		TrackTotalDist = num;
	}

	public virtual void SetToStartLoc(Transform car)
	{
		TrackParts[0].SetStartOfTrackPosition(car);
		TrackParts[0].SetStartOfTrackRotation(car);
	}

	public virtual float DistanceToSectionEnd(float dist)
	{
		dist = Mathf.Repeat(dist, TrackTotalDist);
		int i;
		for (i = 0; dist > PartStartDist[i]; i++)
		{
		}
		return PartStartDist[i] - dist;
	}

	public virtual Quaternion RotationAtDist(float dist)
	{
		dist = Mathf.Repeat(dist, TrackTotalDist);
		int i;
		for (i = 0; dist > PartStartDist[i]; i++)
		{
		}
		return (i <= 0) ? TrackParts[0].RotationAtDist(dist) : TrackParts[i].RotationAtDist(dist - PartStartDist[i - 1]);
	}

	public virtual Vector3 PositionAtDist(float dist)
	{
		dist = Mathf.Repeat(dist, TrackTotalDist);
		int i;
		for (i = 0; dist > PartStartDist[i]; i++)
		{
		}
		return (i <= 0) ? TrackParts[0].PositionAtDist(dist) : TrackParts[i].PositionAtDist(dist - PartStartDist[i - 1]);
	}

	public virtual void Main()
	{
	}
}
