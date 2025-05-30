using System;
using System.Collections;
using System.Collections.Generic;
using Boo.Lang;
using UnityEngine;
using UnityScript.Lang;

[Serializable]
public class Flasher : MonoBehaviour
{
	[Serializable]
	internal sealed class _0024OnTriggerEnter_0024264 : GenericGenerator<WaitForSeconds>
	{
		[Serializable]
		internal sealed class _0024 : GenericGeneratorEnumerator<WaitForSeconds>, IEnumerator
		{
			internal Collider _0024other_0024265;

			internal Flasher _0024self__0024266;

			public _0024(Collider other, Flasher self_)
			{
				_0024other_0024265 = other;
				_0024self__0024266 = self_;
			}

			public override bool MoveNext()
			{
				int result;
				switch (_state)
				{
				default:
					if ((bool)_0024self__0024266.GetComponent<AudioSource>())
					{
						_0024self__0024266.GetComponent<AudioSource>().clip = _0024self__0024266.FlashSingle;
						_0024self__0024266.GetComponent<AudioSource>().loop = true;
						_0024self__0024266.GetComponent<AudioSource>().Play();
					}
					Cart.PointsBoost(_0024self__0024266.Points[_0024self__0024266.level]);
					_0024other_0024265.gameObject.SendMessage("TriggerFlasher", _0024self__0024266.StrobeCurve);
					_0024self__0024266.target = _0024other_0024265.transform;
					if (_0024self__0024266.trackCart)
					{
						_0024self__0024266.enabled = true;
					}
					_0024self__0024266.SetFlare(true);
					result = (Yield(2, new WaitForSeconds(_0024self__0024266.FlashTime)) ? 1 : 0);
					break;
				case 2:
					_0024self__0024266.SetFlare(false);
					_0024self__0024266.enabled = false;
					_0024self__0024266.GetComponent<AudioSource>().loop = false;
					goto case 3;
				case 3:
					if (_0024self__0024266.GetComponent<AudioSource>().isPlaying)
					{
						result = (YieldDefault(3) ? 1 : 0);
						break;
					}
					_0024self__0024266.GetComponent<AudioSource>().clip = _0024self__0024266.FlashOut;
					_0024self__0024266.GetComponent<AudioSource>().Play();
					YieldDefault(1);
					goto case 1;
				case 1:
					result = 0;
					break;
				}
				return (byte)result != 0;
			}
		}

		internal Collider _0024other_0024267;

		internal Flasher _0024self__0024268;

		public _0024OnTriggerEnter_0024264(Collider other, Flasher self_)
		{
			_0024other_0024267 = other;
			_0024self__0024268 = self_;
		}

		public override IEnumerator<WaitForSeconds> GetEnumerator()
		{
			return new _0024(_0024other_0024267, _0024self__0024268);
		}
	}

	public Transform spotlight;

	public AnimationCurve StrobeCurve;

	public int[] FlashCount;

	public int[] Points;

	private Transform target;

	public Light[] flare;

	private float FlashTime;

	public bool trackCart;

	public string UpgradeName;

	public AudioClip FlashSingle;

	public AudioClip FlashOut;

	private int level;

	public Flasher()
	{
		FlashTime = 5f;
	}

	public virtual IEnumerator OnTriggerEnter(Collider other)
	{
		return new _0024OnTriggerEnter_0024264(other, this).GetEnumerator();
	}

	public virtual void SetFlare(bool state)
	{
		for (int i = 0; i < Extensions.get_length((System.Array)flare); i++)
		{
			if ((bool)flare[i])
			{
				flare[i].enabled = state;
			}
		}
	}

	public virtual void MultiplyStrobes()
	{
		Keyframe[] array = new Keyframe[StrobeCurve.length * FlashCount[level]];
		float time = StrobeCurve.keys[Extensions.get_length((System.Array)StrobeCurve.keys) - 1].time;
		for (int i = 0; i < FlashCount[level] * StrobeCurve.length; i++)
		{
			array[i].time = StrobeCurve.keys[(int)Mathf.Repeat(i, StrobeCurve.length)].time + Mathf.Floor(i / Extensions.get_length((System.Array)StrobeCurve.keys)) * time;
			array[i].value = StrobeCurve.keys[(int)Mathf.Repeat(i, StrobeCurve.length)].value;
			array[i].inTangent = StrobeCurve.keys[(int)Mathf.Repeat(i, StrobeCurve.length)].inTangent;
			array[i].outTangent = StrobeCurve.keys[(int)Mathf.Repeat(i, StrobeCurve.length)].outTangent;
		}
		StrobeCurve = new AnimationCurve(array);
	}

	public virtual void Start()
	{
		level = TrackUpgrade.getCurrentLevel(UpgradeName);
		FlashTime = StrobeCurve.keys[StrobeCurve.length - 1].time * (float)FlashCount[level];
		enabled = false;
		SetFlare(false);
		MultiplyStrobes();
	}

	public virtual void FixedUpdate()
	{
		Quaternion to = Quaternion.LookRotation(target.position - spotlight.position);
		spotlight.rotation = Quaternion.Lerp(spotlight.rotation, to, 0.4f);
	}

	public virtual void Main()
	{
	}
}
