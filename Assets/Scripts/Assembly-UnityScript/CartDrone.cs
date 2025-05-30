using System;
using System.Collections;
using System.Collections.Generic;
using Boo.Lang;
using Boo.Lang.Runtime;
using UnityEngine;
using UnityScript.Lang;

[Serializable]
public class CartDrone : MonoBehaviour
{
	[Serializable]
	internal sealed class StartDrone : GenericGenerator<WaitForSeconds>
	{
		[Serializable]
		internal sealed class MasterSpawn : GenericGeneratorEnumerator<WaitForSeconds>, IEnumerator
		{
			internal CartDrone _self;

			public MasterSpawn(CartDrone self_)
			{
				_self = self_;
			}

			public override bool MoveNext()
			{
				int result;
				switch (_state)
				{
				default:
					if (!_self.track)
					{
							_self.track = TrackMaster.inst;
					}
						_self.aud = (AudioSource)_self.gameObject.GetComponent(typeof(AudioSource));
						_self.enabled = false;
					result = (Yield(2, new WaitForSeconds(_self.StartWait)) ? 1 : 0);
					break;
				case 2:
						_self.enabled = true;
					YieldDefault(1);
					goto case 1;
				case 1:
					result = 0;
					break;
				}
				return (byte)result != 0;
			}
		}

		internal CartDrone __self;

		public StartDrone(CartDrone self_)
		{
			__self = self_;
		}

		public override IEnumerator<WaitForSeconds> GetEnumerator()
		{
			return new MasterSpawn(__self);
		}
	}

	public TrackMaster track;

	public float speed;

	public float currentDistance;

	public TrackSection currentSection;

	public float StartWait;

	[NonSerialized]
	public static CartDrone[] DroneList;

	private AudioSource aud;

	private float DieAfter;

	public CartDrone()
	{
		speed = 30f;
		DieAfter = float.PositiveInfinity;
	}

	public virtual void Awake()
	{
		UnityScript.Lang.Array array = ((DroneList != null) ? new UnityScript.Lang.Array((IEnumerable)DroneList) : new UnityScript.Lang.Array());
		for (int i = 0; i < array.length; i++)
		{
			if (!RuntimeServices.ToBool(array[i]))
			{
				array.RemoveAt(i);
				i--;
			}
		}
		array.Add(this);
		DroneList = (CartDrone[])array.ToBuiltin(typeof(CartDrone));
	}

	public virtual IEnumerator Start()
	{
		return new StartDrone(this).GetEnumerator();
	}

	public virtual void SetDieDist(float DieDist)
	{
		DieAfter = DieDist;
	}

	public virtual void FixedUpdate()
	{
		currentSection = track.GetCurrentTrackSection(currentDistance);
		speed += transform.TransformDirection(Vector3.forward).y * Physics.gravity.y * Time.deltaTime;
		currentDistance += speed * Time.deltaTime;
		transform.position = track.PositionAtDist(currentDistance);
		transform.rotation = track.RotationAtDist(currentDistance);
		if (currentSection.CrankUpChain && !(speed >= currentSection.CrankUpSpeed))
		{
			speed = currentSection.CrankUpSpeed;
		}
		if (!(currentDistance <= DieAfter))
		{
			UnityEngine.Object.Destroy(transform.parent.gameObject);
		}
		if (currentSection.Break)
		{
			if (!(speed <= currentSection.BreakSpeed))
			{
				speed -= (speed - currentSection.BreakSpeed) / track.DistanceToSectionEnd(currentDistance) * Time.deltaTime * speed;
			}
			else
			{
				speed = currentSection.BreakSpeed;
			}
		}
		if ((bool)aud)
		{
			aud.volume = Mathf.Clamp01(speed / 40f);
			aud.pitch = 0.5f + Mathf.Clamp01(speed / 100f);
		}
	}

	public virtual void Main()
	{
	}
}
