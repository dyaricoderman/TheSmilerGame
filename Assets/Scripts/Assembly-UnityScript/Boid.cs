using System;
using System.Collections;
using System.Collections.Generic;
using Boo.Lang;
using Boo.Lang.Runtime;
using UnityEngine;
using UnityScript.Lang;

[Serializable]
public class Boid : MonoBehaviour
{
	[Serializable]
	internal sealed class _0024Animate_0024191 : GenericGenerator<WaitForSeconds>
	{
		[Serializable]
		internal sealed class _0024 : GenericGeneratorEnumerator<WaitForSeconds>, IEnumerator
		{
			internal Boid _0024self__0024192;

			public _0024(Boid self_)
			{
				_0024self__0024192 = self_;
			}

			public override bool MoveNext()
			{
				int result;
				switch (_state)
				{
				case 2:
					_0024self__0024192.frame++;
					_0024self__0024192.frame = (int)Mathf.Repeat(_0024self__0024192.frame, Extensions.get_length((System.Array)_0024self__0024192.meshes));
					goto default;
				default:
					if (!_0024self__0024192.CheapMove)
					{
						_0024self__0024192.MF.mesh = _0024self__0024192.meshes[_0024self__0024192.frame];
						result = (Yield(2, new WaitForSeconds(0.1f + UnityEngine.Random.value * 0.01f)) ? 1 : 0);
						break;
					}
					YieldDefault(1);
					goto case 1;
				case 1:
					result = 0;
					break;
				}
				return (byte)result != 0;
			}
		}

		internal Boid _0024self__0024193;

		public _0024Animate_0024191(Boid self_)
		{
			_0024self__0024193 = self_;
		}

		public override IEnumerator<WaitForSeconds> GetEnumerator()
		{
			return new _0024(_0024self__0024193);
		}
	}

	[Serializable]
	internal sealed class _0024CalcLoop_0024194 : GenericGenerator<object>
	{
		[Serializable]
		internal sealed class _0024 : GenericGeneratorEnumerator<object>, IEnumerator
		{
			internal Boid _0024self__0024195;

			public _0024(Boid self_)
			{
				_0024self__0024195 = self_;
			}

			public override bool MoveNext()
			{
				int result;
				switch (_state)
				{
				default:
					_0024self__0024195.lastTime = Time.time - 0.1f;
					goto case 2;
				case 2:
					if (Time.timeScale != 0f)
					{
						if (_0024self__0024195.CheapMove)
						{
							if (_0024self__0024195.lastPos != _0024self__0024195.transform.position)
							{
								_0024self__0024195.transform.rotation = Quaternion.LookRotation(_0024self__0024195.transform.position - _0024self__0024195.lastPos);
							}
							_0024self__0024195.lastPos = _0024self__0024195.transform.position;
							_0024self__0024195.transform.position = BirdSpawner.TargetPos + _0024self__0024195.Offset;
						}
						else
						{
							_0024self__0024195.DTime = Time.time - _0024self__0024195.lastTime;
							_0024self__0024195.lastTime = Time.time;
							_0024self__0024195.Closeist = _0024self__0024195.transform.position;
							_0024self__0024195.dist = float.PositiveInfinity;
							_0024self__0024195.i = 0;
							while (_0024self__0024195.i < Extensions.get_length((System.Array)flock))
							{
								if (_0024self__0024195.transform != flock[_0024self__0024195.i] && !(_0024self__0024195.dist <= (_0024self__0024195.transform.position - flock[_0024self__0024195.i].position).magnitude))
								{
									_0024self__0024195.dist = (_0024self__0024195.transform.position - flock[_0024self__0024195.i].position).magnitude;
									_0024self__0024195.Closeist = flock[_0024self__0024195.i].position;
								}
								_0024self__0024195.i++;
							}
							if (!(_0024self__0024195.dist >= 20f))
							{
								_0024self__0024195.transform.rotation = Quaternion.RotateTowards(_0024self__0024195.transform.rotation, _0024self__0024195.StearTarget(_0024self__0024195.transform.position + (_0024self__0024195.transform.position - _0024self__0024195.Closeist)), 90f * _0024self__0024195.DTime);
							}
							_0024self__0024195.transform.rotation = Quaternion.RotateTowards(_0024self__0024195.transform.rotation, AverageHeading, 20f * _0024self__0024195.DTime);
							_0024self__0024195.transform.rotation = Quaternion.RotateTowards(_0024self__0024195.transform.rotation, _0024self__0024195.StearTarget(BirdSpawner.TargetPos), 90f * _0024self__0024195.DTime);
							_0024self__0024195.transform.position = _0024self__0024195.transform.position + _0024self__0024195.transform.TransformDirection(new Vector3(0f, 0f, _0024self__0024195.speed * _0024self__0024195.DTime));
						}
					}
					result = (YieldDefault(2) ? 1 : 0);
					break;
				case 1:
					result = 0;
					break;
				}
				return (byte)result != 0;
			}
		}

		internal Boid _0024self__0024196;

		public _0024CalcLoop_0024194(Boid self_)
		{
			_0024self__0024196 = self_;
		}

		public override IEnumerator<object> GetEnumerator()
		{
			return new _0024(_0024self__0024196);
		}
	}

	public float speed;

	private float ScanRange;

	[NonSerialized]
	public static Transform[] flock;

	public Mesh[] meshes;

	public MeshFilter MF;

	private int frame;

	private bool CheapMove;

	private Vector3 Offset;

	private Vector3 Closeist;

	private float dist;

	private Vector3 lastPos;

	[NonSerialized]
	public static Quaternion AverageHeading;

	private float lastTime;

	private float DTime;

	private int i;

	public Boid()
	{
		speed = 25f;
		ScanRange = 20f;
	}

	public virtual void Awake()
	{
		UnityScript.Lang.Array array = ((flock != null) ? new UnityScript.Lang.Array((IEnumerable)flock) : new UnityScript.Lang.Array());
		for (int i = 0; i < array.length; i++)
		{
			if (!RuntimeServices.ToBool(array[i]))
			{
				array.RemoveAt(i);
				i--;
			}
		}
		array.Add(transform);
		flock = (Transform[])array.ToBuiltin(typeof(Transform));
		frame = (int)(UnityEngine.Random.value * (float)meshes.Length);
		lastPos = transform.position;
		Offset = transform.position - BirdSpawner.TargetPos;
		StartCoroutine(CalcLoop());
	}

	public virtual IEnumerator Animate()
	{
		return new _0024Animate_0024191(this).GetEnumerator();
	}

	public virtual void OnBecameVisible()
	{
		CheapMove = false;
		StartCoroutine(Animate());
	}

	public virtual void OnBecameInvisible()
	{
		Offset = transform.position - BirdSpawner.TargetPos;
		CheapMove = true;
	}

	public virtual Quaternion StearTarget(Vector3 point)
	{
		return (!(point != transform.position)) ? Quaternion.identity : Quaternion.LookRotation(point - transform.position);
	}

	public virtual IEnumerator CalcLoop()
	{
		return new _0024CalcLoop_0024194(this).GetEnumerator();
	}

	public virtual void Main()
	{
	}
}
