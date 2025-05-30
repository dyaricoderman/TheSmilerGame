using System;
using System.Collections;
using System.Collections.Generic;
using Boo.Lang;
using UnityEngine;
using UnityScript.Lang;

[Serializable]
public class TrackSection : MonoBehaviour
{
	[Serializable]
	internal sealed class _0024PrintCosts_0024269 : GenericGenerator<object>
	{
		[Serializable]
		internal sealed class _0024 : GenericGeneratorEnumerator<object>, IEnumerator
		{
			internal int _0024myScore_0024270;

			internal int _0024newScore_0024271;

			internal TrackSection _0024self__0024272;

			public _0024(TrackSection self_)
			{
				_0024self__0024272 = self_;
			}

			public override bool MoveNext()
			{
				int result;
				switch (_state)
				{
				default:
					if ((bool)_0024self__0024272.UpgradesTo)
					{
						_0024self__0024272.UpgradesTo.CalcTrack();
						goto case 2;
					}
					goto IL_011d;
				case 2:
					if (!_0024self__0024272.TrackCaculated || !_0024self__0024272.UpgradesTo.TrackCaculated)
					{
						result = (YieldDefault(2) ? 1 : 0);
						break;
					}
					_0024myScore_0024270 = (int)(_0024self__0024272.GetLength() * _0024self__0024272.ScoreMultiplyer);
					_0024newScore_0024271 = (int)(_0024self__0024272.UpgradesTo.GetLength() * _0024self__0024272.UpgradesTo.ScoreMultiplyer);
					MonoBehaviour.print(_0024self__0024272.UpgradeName + " flat-" + _0024myScore_0024270 + " loop-" + _0024newScore_0024271 + " change-" + (_0024newScore_0024271 - _0024myScore_0024270));
					goto IL_011d;
				case 1:
					{
						result = 0;
						break;
					}
					IL_011d:
					YieldDefault(1);
					goto case 1;
				}
				return (byte)result != 0;
			}
		}

		internal TrackSection _0024self__0024273;

		public _0024PrintCosts_0024269(TrackSection self_)
		{
			_0024self__0024273 = self_;
		}

		public override IEnumerator<object> GetEnumerator()
		{
			return new _0024(_0024self__0024273);
		}
	}

	private Mesh Rail;

	private Vector3[] TrackPoints;

	private Quaternion[] TrackRotation;

	private float[] PointDistance;

	private float totalDistance;

	private AnimationCurve XCurve;

	private AnimationCurve YCurve;

	private AnimationCurve ZCurve;

	public bool Reverse;

	public Renderer HiMesh;

	public Renderer LowMesh;

	public Renderer DistMesh;

	public GameObject SupportsMesh;

	public bool CrankUpChain;

	public float CrankUpSpeed;

	public bool Break;

	public float BreakSpeed;

	public float BreakKickin;

	public float ScoreMultiplyer;

	public bool FlipUp;

	public TrackSection UpgradesTo;

	public string UpgradeName;

	private Vector3[] TrackTilt;

	private bool TrackCaculated;

	private Vector3 AveragePosition;

	public TrackSection()
	{
		Reverse = true;
		CrankUpSpeed = 30f;
		BreakSpeed = 10f;
		BreakKickin = 1f;
		ScoreMultiplyer = 1f;
		FlipUp = true;
		UpgradeName = "NotUpgradable";
	}

	public virtual void Awake()
	{
	}

	public virtual void PlayUpgradeEffect()
	{
	}

	public virtual IEnumerator PrintCosts()
	{
		return new _0024PrintCosts_0024269(this).GetEnumerator();
	}

	public virtual void SetQuality(TrackQuality Qual)
	{
		if (Qual == TrackQuality.High)
		{
			LowMesh.enabled = false;
			DistMesh.enabled = false;
			HiMesh.enabled = true;
		}
		if (Qual == TrackQuality.Low)
		{
			HiMesh.enabled = false;
			DistMesh.enabled = false;
			LowMesh.enabled = true;
		}
		if (Qual == TrackQuality.Distance)
		{
			HiMesh.enabled = false;
			LowMesh.enabled = false;
			DistMesh.enabled = true;
		}
	}

	public virtual void CheckTrackUpgrade(TrackMaster Master, int UpgradeCount, int UID, int TrackID)
	{
		if (UID == -1)
		{
			return;
		}
		if (UpgradeCount == 0)
		{
			gameObject.active = true;
			Master.TrackParts[TrackID] = this;
		}
		else
		{
			gameObject.active = false;
			Component[] componentsInChildren = GetComponentsInChildren(typeof(Renderer));
			int i = 0;
			Component[] array = componentsInChildren;
			for (int length = array.Length; i < length; i++)
			{
				((Renderer)array[i]).enabled = false;
			}
			if ((bool)SupportsMesh)
			{
				UnityEngine.Object.Destroy(SupportsMesh);
			}
		}
		if ((bool)UpgradesTo)
		{
			UpgradeCount--;
			UpgradesTo.CheckTrackUpgrade(Master, UpgradeCount, UID, TrackID);
		}
	}

	public virtual void EditorComputeTrack()
	{
		Rail = ((MeshFilter)gameObject.GetComponent(typeof(MeshFilter))).sharedMesh;
		CalcTrack();
	}

	public virtual void SetStartOfTrackPosition(Transform car)
	{
		CalcTrack();
		car.position = PositionAtDist(0f);
	}

	public virtual void SetStartOfTrackRotation(Transform car)
	{
		CalcTrack();
		car.rotation = TrackRotation[0];
	}

	public virtual Vector3 getAveragePosition()
	{
		return AveragePosition;
	}

	public virtual void CalcTrack()
	{
		if (!TrackCaculated)
		{
			Rail = ((MeshFilter)gameObject.GetComponent(typeof(MeshFilter))).mesh;
			Vector3[] array = Rail.vertices;
			int num = Extensions.get_length((System.Array)array) / 2;
			TrackPoints = new Vector3[num];
			TrackTilt = new Vector3[num];
			TrackRotation = new Quaternion[num];
			PointDistance = new float[num];
			if (Reverse)
			{
				UnityScript.Lang.Array array2 = new UnityScript.Lang.Array(array);
				array2.Reverse();
				array = (Vector3[])array2.ToBuiltin(typeof(Vector3));
			}
			for (int i = 0; i < Extensions.get_length((System.Array)array) - 1; i += 2)
			{
				Vector3 vector = transform.TransformPoint((array[i] + array[i + 1]) / 2f);
				TrackPoints[(int)Mathf.Floor(i / 2)] = vector;
				if (FlipUp)
				{
					TrackTilt[(int)Mathf.Floor(i / 2)] = transform.TransformDirection(array[i] - array[i + 1]);
				}
				else
				{
					TrackTilt[(int)Mathf.Floor(i / 2)] = transform.TransformDirection(array[i + 1] - array[i]);
				}
				if (i > 0)
				{
					PointDistance[(int)Mathf.Floor(i / 2)] = PointDistance[(int)(Mathf.Floor(i / 2) - 1f)] + (vector - TrackPoints[(int)(Mathf.Floor(i / 2) - 1f)]).magnitude;
				}
			}
			for (int i = 0; i < Extensions.get_length((System.Array)TrackPoints) - 1; i++)
			{
				Quaternion quaternion = default(Quaternion);
				Vector3 vector2 = TrackPoints[i + 1] - TrackPoints[i];
				Vector3 up = Vector3.Cross(TrackTilt[i], vector2);
				quaternion.SetLookRotation(vector2, up);
				TrackRotation[i] = quaternion;
			}
			for (int i = 0; i < Extensions.get_length((System.Array)TrackPoints); i++)
			{
				AveragePosition += TrackPoints[i];
			}
			AveragePosition /= (float)TrackPoints.Length;
			TrackRotation[TrackRotation.Length - 1] = TrackRotation[TrackRotation.Length - 2];
			totalDistance = PointDistance[TrackRotation.Length - 1];
			GenerateCurves();
		}
		TrackCaculated = true;
	}

	public virtual float GetLength()
	{
		return totalDistance;
	}

	public virtual void JoinEndToTrack(TrackSection TS)
	{
		TrackRotation[TrackRotation.Length - 1] = TS.GetStartRot();
	}

	public virtual Quaternion GetStartRot()
	{
		return TrackRotation[0];
	}

	public virtual void GenerateCurves()
	{
		Keyframe[] array = new Keyframe[Extensions.get_length((System.Array)TrackPoints)];
		Keyframe[] array2 = new Keyframe[Extensions.get_length((System.Array)TrackPoints)];
		Keyframe[] array3 = new Keyframe[Extensions.get_length((System.Array)TrackPoints)];
		for (int i = 0; i < Extensions.get_length((System.Array)TrackPoints); i++)
		{
			array[i] = new Keyframe(PointDistance[i], TrackPoints[i].x);
			array2[i] = new Keyframe(PointDistance[i], TrackPoints[i].y);
			array3[i] = new Keyframe(PointDistance[i], TrackPoints[i].z);
		}
		XCurve = new AnimationCurve(array);
		YCurve = new AnimationCurve(array2);
		ZCurve = new AnimationCurve(array3);
		for (int i = 0; i < Extensions.get_length((System.Array)TrackPoints); i++)
		{
			XCurve.SmoothTangents(i, 0f);
			YCurve.SmoothTangents(i, 0f);
			ZCurve.SmoothTangents(i, 0f);
		}
	}

	public virtual Vector3 PositionAtDist(float dist)
	{
		dist = Mathf.Repeat(dist, totalDistance);
		return new Vector3(XCurve.Evaluate(dist), YCurve.Evaluate(dist), ZCurve.Evaluate(dist));
	}

	public virtual Quaternion RotationAtDist(float dist)
	{
		dist = Mathf.Repeat(dist, totalDistance);
		int i;
		for (i = 0; dist > PointDistance[i]; i++)
		{
		}
		return Quaternion.Lerp(TrackRotation[i], TrackRotation[i - 1], (dist - PointDistance[i]) / (PointDistance[i - 1] - PointDistance[i]));
	}

	public virtual void OnDrawGizmos()
	{
		if (TrackPoints != null)
		{
			Gizmos.color = Color.yellow;
			for (int i = 0; i < Extensions.get_length((System.Array)TrackPoints); i++)
			{
				Gizmos.DrawSphere(TrackPoints[i], 0.1f);
			}
		}
		if (TrackRotation != null && (bool)Rail)
		{
			Vector3[] vertices = Rail.vertices;
			for (int i = 0; i < Extensions.get_length((System.Array)TrackPoints); i++)
			{
				Gizmos.color = Color.green;
				Gizmos.DrawLine(TrackPoints[i], TrackPoints[i] + TrackRotation[i] * Vector3.up * 2f);
				Gizmos.color = Color.blue;
				Gizmos.DrawLine(TrackPoints[i], TrackPoints[i] + TrackRotation[i] * Vector3.forward * 2f);
			}
		}
	}

	public virtual void Main()
	{
	}
}
