using System;
using System.Collections;
using System.Collections.Generic;
using Boo.Lang;
using UnityEngine;

[Serializable]
public class LegEffects : MonoBehaviour
{
	[Serializable]
	internal sealed class _0024TriggerInoc_0024165 : GenericGenerator<WaitForSeconds>
	{
		[Serializable]
		internal sealed class _0024 : GenericGeneratorEnumerator<WaitForSeconds>, IEnumerator
		{
			internal AnimationCurve _0024IC_0024166;

			internal LegEffects _0024self__0024167;

			public _0024(AnimationCurve IC, LegEffects self_)
			{
				_0024IC_0024166 = IC;
				_0024self__0024167 = self_;
			}

			public override bool MoveNext()
			{
				int result;
				switch (_state)
				{
				default:
					TutoralDisplay.inst.StartCoroutine(TutoralDisplay.inst.TriggerTutoral(3, Cart.inst.gameObject));
					goto case 2;
				case 2:
					if (TutoralDisplay.inst.TutoralOn)
					{
						result = (YieldDefault(2) ? 1 : 0);
						break;
					}
					_0024self__0024167.InocCurve = _0024IC_0024166;
					_0024self__0024167.inocTime = 0f;
					goto case 3;
				case 3:
					if (_0024self__0024167.inocTime < _0024IC_0024166.keys[_0024IC_0024166.length - 1].time)
					{
						if (!(_0024self__0024167.InocCurve.Evaluate(_0024self__0024167.inocTime) <= UnityEngine.Random.value))
						{
							if (!(UnityEngine.Random.value <= 0.5f))
							{
								_0024self__0024167.CF.StartCoroutine(_0024self__0024167.CF.PlayCreep());
							}
							else
							{
								_0024self__0024167.CF.StartCoroutine(_0024self__0024167.CF.PlayNoise());
							}
						}
						_0024self__0024167.inocTime += _0024self__0024167.CF.MinFrameTime;
						result = (Yield(3, new WaitForSeconds(_0024self__0024167.CF.MinFrameTime)) ? 1 : 0);
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

		internal AnimationCurve _0024IC_0024168;

		internal LegEffects _0024self__0024169;

		public _0024TriggerInoc_0024165(AnimationCurve IC, LegEffects self_)
		{
			_0024IC_0024168 = IC;
			_0024self__0024169 = self_;
		}

		public override IEnumerator<WaitForSeconds> GetEnumerator()
		{
			return new _0024(_0024IC_0024168, _0024self__0024169);
		}
	}

	[Serializable]
	internal sealed class _0024TriggerTickler_0024170 : GenericGenerator<WaitForSeconds>
	{
		[Serializable]
		internal sealed class _0024 : GenericGeneratorEnumerator<WaitForSeconds>, IEnumerator
		{
			internal LegEffects _0024self__0024171;

			public _0024(LegEffects self_)
			{
				_0024self__0024171 = self_;
			}

			public override bool MoveNext()
			{
				int result;
				switch (_state)
				{
				default:
					TutoralDisplay.inst.StartCoroutine(TutoralDisplay.inst.TriggerTutoral(3, Cart.inst.gameObject));
					goto case 2;
				case 2:
					if (TutoralDisplay.inst.TutoralOn)
					{
						result = (YieldDefault(2) ? 1 : 0);
						break;
					}
					_0024self__0024171.Laugh.Play();
					CartCam.inst.StartCoroutine(CartCam.inst.LaughShake(_0024self__0024171.LaughCurve));
					result = (Yield(3, new WaitForSeconds(_0024self__0024171.LaughCurve.keys[_0024self__0024171.LaughCurve.length - 1].time)) ? 1 : 0);
					break;
				case 3:
					_0024self__0024171.Laugh.Stop();
					YieldDefault(1);
					goto case 1;
				case 1:
					result = 0;
					break;
				}
				return (byte)result != 0;
			}
		}

		internal LegEffects _0024self__0024172;

		public _0024TriggerTickler_0024170(LegEffects self_)
		{
			_0024self__0024172 = self_;
		}

		public override IEnumerator<WaitForSeconds> GetEnumerator()
		{
			return new _0024(_0024self__0024172);
		}
	}

	[Serializable]
	internal sealed class _0024TriggerFlasher_0024173 : GenericGenerator<object>
	{
		[Serializable]
		internal sealed class _0024 : GenericGeneratorEnumerator<object>, IEnumerator
		{
			internal AnimationCurve _0024Flash_0024174;

			public _0024(AnimationCurve Flash)
			{
				_0024Flash_0024174 = Flash;
			}

			public override bool MoveNext()
			{
				int result;
				switch (_state)
				{
				default:
					TutoralDisplay.inst.StartCoroutine(TutoralDisplay.inst.TriggerTutoral(3, Cart.inst.gameObject));
					goto case 2;
				case 2:
					if (TutoralDisplay.inst.TutoralOn)
					{
						result = (YieldDefault(2) ? 1 : 0);
						break;
					}
					CartCam.inst.ApplyExposureChange(_0024Flash_0024174);
					YieldDefault(1);
					goto case 1;
				case 1:
					result = 0;
					break;
				}
				return (byte)result != 0;
			}
		}

		internal AnimationCurve _0024Flash_0024175;

		public _0024TriggerFlasher_0024173(AnimationCurve Flash)
		{
			_0024Flash_0024175 = Flash;
		}

		public override IEnumerator<object> GetEnumerator()
		{
			return new _0024(_0024Flash_0024175);
		}
	}

	[Serializable]
	internal sealed class _0024TriggerHypno_0024176 : GenericGenerator<object>
	{
		[Serializable]
		internal sealed class _0024 : GenericGeneratorEnumerator<object>, IEnumerator
		{
			internal float _0024startTime_0024177;

			internal float _0024Fade_0024178;

			internal float _0024runTime_0024179;

			internal AnimationCurve _0024ac_0024180;

			internal LegEffects _0024self__0024181;

			public _0024(AnimationCurve ac, LegEffects self_)
			{
				_0024ac_0024180 = ac;
				_0024self__0024181 = self_;
			}

			public override bool MoveNext()
			{
				int result;
				switch (_state)
				{
				default:
					TutoralDisplay.inst.StartCoroutine(TutoralDisplay.inst.TriggerTutoral(3, Cart.inst.gameObject));
					goto case 2;
				case 2:
					if (TutoralDisplay.inst.TutoralOn)
					{
						result = (YieldDefault(2) ? 1 : 0);
						break;
					}
					_0024startTime_0024177 = Time.time;
					_0024Fade_0024178 = 0f;
					_0024runTime_0024179 = 0f;
					_0024self__0024181.Hypno1.enabled = true;
					_0024self__0024181.Hypno2.enabled = true;
					goto case 3;
				case 3:
					if (_0024runTime_0024179 < _0024ac_0024180.keys[_0024ac_0024180.length - 1].time)
					{
						_0024runTime_0024179 = Time.time - _0024startTime_0024177;
						_0024Fade_0024178 = _0024ac_0024180.Evaluate(_0024runTime_0024179);
						_0024self__0024181.Hypno1.material.SetColor("_Color", Color.Lerp(Color.white, Color.black, _0024Fade_0024178));
						_0024self__0024181.Hypno2.material.SetColor("_Color", Color.Lerp(Color.white, Color.black, _0024Fade_0024178));
						result = (YieldDefault(3) ? 1 : 0);
						break;
					}
					_0024self__0024181.Hypno1.enabled = false;
					_0024self__0024181.Hypno2.enabled = false;
					YieldDefault(1);
					goto case 1;
				case 1:
					result = 0;
					break;
				}
				return (byte)result != 0;
			}
		}

		internal AnimationCurve _0024ac_0024182;

		internal LegEffects _0024self__0024183;

		public _0024TriggerHypno_0024176(AnimationCurve ac, LegEffects self_)
		{
			_0024ac_0024182 = ac;
			_0024self__0024183 = self_;
		}

		public override IEnumerator<object> GetEnumerator()
		{
			return new _0024(_0024ac_0024182, _0024self__0024183);
		}
	}

	[Serializable]
	internal sealed class _0024TriggerGas_0024184 : GenericGenerator<WaitForSeconds>
	{
		[Serializable]
		internal sealed class _0024 : GenericGeneratorEnumerator<WaitForSeconds>, IEnumerator
		{
			internal LegEffects _0024self__0024185;

			public _0024(LegEffects self_)
			{
				_0024self__0024185 = self_;
			}

			public override bool MoveNext()
			{
				int result;
				switch (_state)
				{
				default:
					TutoralDisplay.inst.StartCoroutine(TutoralDisplay.inst.TriggerTutoral(3, Cart.inst.gameObject));
					goto case 2;
				case 2:
					if (TutoralDisplay.inst.TutoralOn)
					{
						result = (YieldDefault(2) ? 1 : 0);
						break;
					}
					_0024self__0024185.GasEffect.Play();
					result = (Yield(3, new WaitForSeconds(4f)) ? 1 : 0);
					break;
				case 3:
					_0024self__0024185.GasEffect.Stop();
					YieldDefault(1);
					goto case 1;
				case 1:
					result = 0;
					break;
				}
				return (byte)result != 0;
			}
		}

		internal LegEffects _0024self__0024186;

		public _0024TriggerGas_0024184(LegEffects self_)
		{
			_0024self__0024186 = self_;
		}

		public override IEnumerator<WaitForSeconds> GetEnumerator()
		{
			return new _0024(_0024self__0024186);
		}
	}

	public ParticleSystem GasEffect;

	public Renderer Hypno1;

	public Renderer Hypno2;

	public AudioSource Laugh;

	public Texture2D[] InocFrames;

	private AnimationCurve InocCurve;

	public AnimationCurve LaughCurve;

	public CreepFrame CF;

	private float inocTime;

	private float inocProgress;

	public virtual void Awake()
	{
		enabled = false;
	}

	public virtual IEnumerator TriggerInoc(AnimationCurve IC)
	{
		return new _0024TriggerInoc_0024165(IC, this).GetEnumerator();
	}

	public virtual IEnumerator TriggerTickler()
	{
		return new _0024TriggerTickler_0024170(this).GetEnumerator();
	}

	public virtual IEnumerator TriggerFlasher(AnimationCurve Flash)
	{
		return new _0024TriggerFlasher_0024173(Flash).GetEnumerator();
	}

	public virtual IEnumerator TriggerHypno(AnimationCurve ac)
	{
		return new _0024TriggerHypno_0024176(ac, this).GetEnumerator();
	}

	public virtual IEnumerator TriggerGas()
	{
		return new _0024TriggerGas_0024184(this).GetEnumerator();
	}

	public virtual void Main()
	{
	}
}
