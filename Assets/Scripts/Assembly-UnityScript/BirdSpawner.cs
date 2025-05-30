using System;
using System.Collections;
using System.Collections.Generic;
using Boo.Lang;
using UnityEngine;
using UnityScript.Lang;

[Serializable]
public class BirdSpawner : MonoBehaviour
{
	[Serializable]
	internal sealed class _0024updateLoop_0024187 : GenericGenerator<WaitForSeconds>
	{
		[Serializable]
		internal sealed class _0024 : GenericGeneratorEnumerator<WaitForSeconds>, IEnumerator
		{
			internal int _0024i_0024188;

			internal BirdSpawner _0024self__0024189;

			public _0024(BirdSpawner self_)
			{
				_0024self__0024189 = self_;
			}

			public override bool MoveNext()
			{
				int result;
				switch (_state)
				{
				default:
					result = (((Boid.flock != null) ? Yield(3, new WaitForSeconds(0.5f)) : YieldDefault(2)) ? 1 : 0);
					break;
				case 3:
				case 4:
					TargetPos = _0024self__0024189.ViewPath(_0024self__0024189.CurrentTime);
					_0024self__0024189.CurrentTime += 0.2f;
					for (_0024i_0024188 = 0; _0024i_0024188 < Extensions.get_length((System.Array)Boid.flock); _0024i_0024188++)
					{
						_0024self__0024189.Qs[_0024i_0024188] = Boid.flock[_0024i_0024188].rotation;
					}
					Boid.AverageHeading = _0024self__0024189.AverageQuaterion(_0024self__0024189.Qs);
					result = (Yield(4, new WaitForSeconds(0.2f)) ? 1 : 0);
					break;
				case 1:
					result = 0;
					break;
				}
				return (byte)result != 0;
			}
		}

		internal BirdSpawner _0024self__0024190;

		public _0024updateLoop_0024187(BirdSpawner self_)
		{
			_0024self__0024190 = self_;
		}

		public override IEnumerator<WaitForSeconds> GetEnumerator()
		{
			return new _0024(_0024self__0024190);
		}
	}

	public Transform Spawn;

	public int[] Count;

	public float moveSpeed;

	public float spawnArea;

	public Transform[] WayPoints;

	[NonSerialized]
	public static BirdSpawner inst;

	[NonSerialized]
	public static Vector3 TargetPos;

	private AnimationCurve ViewCurveX;

	private AnimationCurve ViewCurveY;

	private AnimationCurve ViewCurveZ;

	private float CurrentTime;

	private Quaternion[] Qs;

	private Quaternion avg;

	private float mag;

	private float TotalViewTime;

	private bool ComputeFinished;

	public BirdSpawner()
	{
		moveSpeed = 40f;
		spawnArea = 30f;
		avg = new Quaternion(0f, 0f, 0f, 0f);
	}

	public virtual void Awake()
	{
		inst = this;
		TargetPos = this.transform.position;
		ComputeViewPath();
		for (int i = 0; i < Count[Performance.GamePerformance]; i++)
		{
			Transform transform = (Transform)UnityEngine.Object.Instantiate(Spawn, WayPoints[0].position + UnityEngine.Random.insideUnitSphere * spawnArea, Quaternion.LookRotation(WayPoints[1].position - WayPoints[0].position));
			transform.parent = this.transform;
		}
	}

	public virtual void Start()
	{
		Qs = new Quaternion[Count[Performance.GamePerformance]];
		if (Count[Performance.GamePerformance] > 0)
		{
			StartCoroutine(updateLoop());
		}
	}

	public virtual Vector3 ViewPath(float time)
	{
		time = Mathf.PingPong(time, TotalViewTime);
		return new Vector3(ViewCurveX.Evaluate(time), ViewCurveY.Evaluate(time), ViewCurveZ.Evaluate(time));
	}

	public virtual IEnumerator updateLoop()
	{
		return new _0024updateLoop_0024187(this).GetEnumerator();
	}

	public virtual Quaternion AverageQuaterion(Quaternion[] quats)
	{
		Quaternion identity;
		if (quats.Length > 0)
		{
			avg = new Quaternion(0f, 0f, 0f, 0f);
			int i = 0;
			for (int length = quats.Length; i < length; i++)
			{
				if (!(Quaternion.Dot(quats[i], avg) <= 0f))
				{
					avg.x += quats[i].x;
					avg.y += quats[i].y;
					avg.z += quats[i].z;
					avg.w += quats[i].w;
				}
				else
				{
					avg.x += 0f - quats[i].x;
					avg.y += 0f - quats[i].y;
					avg.z += 0f - quats[i].z;
					avg.w += 0f - quats[i].w;
				}
			}
			mag = Mathf.Sqrt(avg.x * avg.x + avg.y * avg.y + avg.z * avg.z + avg.w * avg.w);
			if (!(mag <= 0.0001f))
			{
				avg.x /= mag;
				avg.y /= mag;
				avg.z /= mag;
				avg.w /= mag;
			}
			else
			{
				avg = quats[0];
			}
			identity = avg;
		}
		else
		{
			identity = Quaternion.identity;
		}
		return identity;
	}

	public virtual void ComputeViewPath()
	{
		if (ComputeFinished)
		{
			return;
		}
		Keyframe[] array = new Keyframe[Extensions.get_length((System.Array)WayPoints)];
		Keyframe[] array2 = new Keyframe[Extensions.get_length((System.Array)WayPoints)];
		Keyframe[] array3 = new Keyframe[Extensions.get_length((System.Array)WayPoints)];
		for (int i = 0; i < Extensions.get_length((System.Array)WayPoints); i++)
		{
			array[i] = new Keyframe(TotalViewTime, WayPoints[i].transform.position.x);
			array2[i] = new Keyframe(TotalViewTime, WayPoints[i].transform.position.y);
			array3[i] = new Keyframe(TotalViewTime, WayPoints[i].transform.position.z);
			if (i < Extensions.get_length((System.Array)WayPoints) - 1)
			{
				TotalViewTime += (WayPoints[i].position - WayPoints[i + 1].position).magnitude / moveSpeed;
			}
		}
		ViewCurveX = new AnimationCurve(array);
		ViewCurveY = new AnimationCurve(array2);
		ViewCurveZ = new AnimationCurve(array3);
		for (int i = 0; i < Extensions.get_length((System.Array)WayPoints); i++)
		{
			ViewCurveX.SmoothTangents(i, 0f);
			ViewCurveY.SmoothTangents(i, 0f);
			ViewCurveZ.SmoothTangents(i, 0f);
		}
		ComputeFinished = true;
	}

	public virtual void OnDrawGizmos()
	{
		Gizmos.color = Color.yellow;
		Gizmos.DrawSphere(TargetPos, 10f);
		Gizmos.color = Color.green;
		Gizmos.DrawWireSphere(WayPoints[0].position, spawnArea);
	}

	public virtual void Main()
	{
	}
}
