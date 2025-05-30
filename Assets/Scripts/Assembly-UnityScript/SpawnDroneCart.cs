using System;
using System.Collections;
using System.Collections.Generic;
using Boo.Lang;
using UnityEngine;

[Serializable]
public class SpawnDroneCart : MonoBehaviour
{
	[Serializable]
	internal sealed class _0024Start_0024200 : GenericGenerator<object>
	{
		[Serializable]
		internal sealed class _0024 : GenericGeneratorEnumerator<object>, IEnumerator
		{
			internal SpawnDroneCart _0024self__0024201;

			public _0024(SpawnDroneCart self_)
			{
				_0024self__0024201 = self_;
			}

			public override bool MoveNext()
			{
				int result;
				switch (_state)
				{
				default:
					if (!TrackMaster.inst.TrackComputationFinished)
					{
						result = (YieldDefault(2) ? 1 : 0);
						break;
					}
					_0024self__0024201.StartCaculated = true;
					if ((bool)_0024self__0024201.DroneTrack)
					{
						_0024self__0024201.DroneStartDist = _0024self__0024201.DroneTrack.ReverseLookUp(_0024self__0024201.DroneStart.position);
						_0024self__0024201.DroneEndDist = _0024self__0024201.DroneTrack.ReverseLookUp(_0024self__0024201.DroneEnd.position);
					}
					else
					{
						_0024self__0024201.DroneStartDist = TrackMaster.inst.ReverseLookUp(_0024self__0024201.DroneStart.position);
						_0024self__0024201.DroneEndDist = TrackMaster.inst.ReverseLookUp(_0024self__0024201.DroneEnd.position);
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

		internal SpawnDroneCart _0024self__0024202;

		public _0024Start_0024200(SpawnDroneCart self_)
		{
			_0024self__0024202 = self_;
		}

		public override IEnumerator<object> GetEnumerator()
		{
			return new _0024(_0024self__0024202);
		}
	}

	[Serializable]
	internal sealed class _0024ActivateDrone_0024203 : GenericGenerator<object>
	{
		[Serializable]
		internal sealed class _0024 : GenericGeneratorEnumerator<object>, IEnumerator
		{
			internal SpawnDroneCart _0024self__0024204;

			public _0024(SpawnDroneCart self_)
			{
				_0024self__0024204 = self_;
			}

			public override bool MoveNext()
			{
				int result;
				switch (_state)
				{
				default:
					if (!_0024self__0024204.StartCaculated)
					{
						result = (YieldDefault(2) ? 1 : 0);
						break;
					}
					_0024self__0024204.DroneInst.currentDistance = _0024self__0024204.DroneStartDist;
					_0024self__0024204.DroneInst.speed = _0024self__0024204.DroneStartSpeed;
					if ((bool)_0024self__0024204.DroneTrack)
					{
						_0024self__0024204.DroneInst.track = _0024self__0024204.DroneTrack;
					}
					if (_0024self__0024204.OneShotCart)
					{
						_0024self__0024204.DroneInst.SetDieDist(_0024self__0024204.DroneEndDist);
					}
					if (_0024self__0024204.OverrideFocus)
					{
						_0024self__0024204.gameObject.SendMessage("FollowDrone", _0024self__0024204.DroneInst.transform, SendMessageOptions.DontRequireReceiver);
					}
						//_0024self__0024204.DroneTransform.gameObject.SetActiveRecursively(true);
						_0024self__0024204.DroneTransform.gameObject.SetActive(true);
						YieldDefault(1);
					goto case 1;
				case 1:
					result = 0;
					break;
				}
				return (byte)result != 0;
			}
		}

		internal SpawnDroneCart _0024self__0024205;

		public _0024ActivateDrone_0024203(SpawnDroneCart self_)
		{
			_0024self__0024205 = self_;
		}

		public override IEnumerator<object> GetEnumerator()
		{
			return new _0024(_0024self__0024205);
		}
	}

	public Transform DroneCart;

	public float DroneStartSpeed;

	public Transform DroneStart;

	public Transform DroneEnd;

	public TrackMaster DroneTrack;

	public bool OverrideFocus;

	private Transform DroneTransform;

	private CartDrone DroneInst;

	private float DroneStartDist;

	private float DroneEndDist;

	private bool StartCaculated;

	private bool OneShotCart;

	public SpawnDroneCart()
	{
		DroneStartSpeed = 30f;
		OverrideFocus = true;
	}

	public virtual void Awake()
	{
		DroneTransform = (Transform)UnityEngine.Object.Instantiate(DroneCart, DroneStart.position, Quaternion.identity);
		DroneInst = (CartDrone)DroneTransform.GetComponentInChildren(typeof(CartDrone));
		//DroneTransform.gameObject.SetActiveRecursively(false);
		DroneTransform.gameObject.SetActive(false);
	}

	public virtual IEnumerator Start()
	{
		return new _0024Start_0024200(this).GetEnumerator();
	}

	public virtual void OnTriggerEnter(Collider other)
	{
		OneShotCart = true;
		StartCoroutine(ActivateDrone());
	}

	public virtual IEnumerator ActivateDrone()
	{
		return new _0024ActivateDrone_0024203(this).GetEnumerator();
	}

	public virtual void DeactivateDrone()
	{
		if (DroneInst)
		{
			//DroneTransform.gameObject.SetActiveRecursively(false);
			DroneTransform.gameObject.SetActive(false);
		}
	}

	public virtual void IsOn()
	{
		if ((bool)DroneInst)
		{
			MonoBehaviour.print(string.Empty);
		}
	}

	public virtual void OnDrawGizmos()
	{
		Gizmos.color = Color.red;
		Gizmos.DrawSphere(DroneStart.position, 4f);
		Gizmos.color = Color.red;
		Gizmos.DrawSphere(DroneEnd.position, 2f);
		Gizmos.color = Color.yellow;
		Gizmos.DrawLine(DroneStart.position, DroneEnd.position);
	}

	public virtual void Main()
	{
	}
}
