using System;
using System.Collections;
using System.Collections.Generic;
using Boo.Lang;
using UnityEngine;
using UnityScript.Lang;

[Serializable]
public class CreepFrame : MonoBehaviour
{
	[Serializable]
	internal sealed class _0024PlayNoise_0024220 : GenericGenerator<WaitForSeconds>
	{
		[Serializable]
		internal sealed class _0024 : GenericGeneratorEnumerator<WaitForSeconds>, IEnumerator
		{
			internal CreepFrame _0024self__0024221;

			public _0024(CreepFrame self_)
			{
				_0024self__0024221 = self_;
			}

			public override bool MoveNext()
			{
				int result;
				switch (_state)
				{
				default:
					_0024self__0024221.GetComponent<AudioSource>().Play();
					_0024self__0024221.Noise = true;
					_0024self__0024221.enabled = true;
					result = (Yield(2, new WaitForSeconds(_0024self__0024221.MinFrameTime)) ? 1 : 0);
					break;
				case 2:
					_0024self__0024221.enabled = false;
					_0024self__0024221.Noise = false;
					_0024self__0024221.GetComponent<AudioSource>().Pause();
					YieldDefault(1);
					goto case 1;
				case 1:
					result = 0;
					break;
				}
				return (byte)result != 0;
			}
		}

		internal CreepFrame _0024self__0024222;

		public _0024PlayNoise_0024220(CreepFrame self_)
		{
			_0024self__0024222 = self_;
		}

		public override IEnumerator<WaitForSeconds> GetEnumerator()
		{
			return new _0024(_0024self__0024222);
		}
	}

	[Serializable]
	internal sealed class _0024PlayCreep_0024223 : GenericGenerator<WaitForSeconds>
	{
		[Serializable]
		internal sealed class _0024 : GenericGeneratorEnumerator<WaitForSeconds>, IEnumerator
		{
			internal CreepFrame _0024self__0024224;

			public _0024(CreepFrame self_)
			{
				_0024self__0024224 = self_;
			}

			public override bool MoveNext()
			{
				int result;
				switch (_state)
				{
				default:
					_0024self__0024224.frame = (int)(UnityEngine.Random.value * (float)Extensions.get_length((System.Array)_0024self__0024224.CreapyFrames));
					_0024self__0024224.Noise = false;
					_0024self__0024224.enabled = true;
					result = (Yield(2, new WaitForSeconds(_0024self__0024224.MinFrameTime)) ? 1 : 0);
					break;
				case 2:
					_0024self__0024224.enabled = false;
					YieldDefault(1);
					goto case 1;
				case 1:
					result = 0;
					break;
				}
				return (byte)result != 0;
			}
		}

		internal CreepFrame _0024self__0024225;

		public _0024PlayCreep_0024223(CreepFrame self_)
		{
			_0024self__0024225 = self_;
		}

		public override IEnumerator<WaitForSeconds> GetEnumerator()
		{
			return new _0024(_0024self__0024225);
		}
	}

	[Serializable]
	internal sealed class _0024Transition_0024226 : GenericGenerator<WaitForSeconds>
	{
		[Serializable]
		internal sealed class _0024 : GenericGeneratorEnumerator<WaitForSeconds>, IEnumerator
		{
			internal float _0024Count_0024227;

			internal CreepFrame _0024self__0024228;

			public _0024(CreepFrame self_)
			{
				_0024self__0024228 = self_;
			}

			public override bool MoveNext()
			{
				int result;
				switch (_state)
				{
				default:
					_0024Count_0024227 = _0024self__0024228.TransitionTime;
					goto IL_00a3;
				case 2:
					if (!(UnityEngine.Random.value <= 0.5f))
					{
						_0024self__0024228.StartCoroutine(_0024self__0024228.PlayNoise());
					}
					else
					{
						_0024self__0024228.StartCoroutine(_0024self__0024228.PlayCreep());
					}
					_0024Count_0024227 -= _0024self__0024228.MinFrameTime;
					goto IL_00a3;
				case 1:
					{
						result = 0;
						break;
					}
					IL_00a3:
					if (_0024Count_0024227 > 0f)
					{
						result = (Yield(2, new WaitForSeconds(_0024self__0024228.MinFrameTime)) ? 1 : 0);
						break;
					}
					YieldDefault(1);
					goto case 1;
				}
				return (byte)result != 0;
			}
		}

		internal CreepFrame _0024self__0024229;

		public _0024Transition_0024226(CreepFrame self_)
		{
			_0024self__0024229 = self_;
		}

		public override IEnumerator<WaitForSeconds> GetEnumerator()
		{
			return new _0024(_0024self__0024229);
		}
	}

	[Serializable]
	internal sealed class _0024CreepLoop_0024230 : GenericGenerator<WaitForSeconds>
	{
		[Serializable]
		internal sealed class _0024 : GenericGeneratorEnumerator<WaitForSeconds>, IEnumerator
		{
			internal CreepFrame _0024self__0024231;

			public _0024(CreepFrame self_)
			{
				_0024self__0024231 = self_;
			}

			public override bool MoveNext()
			{
				int result;
				switch (_state)
				{
				default:
					result = (Yield(2, new WaitForSeconds(_0024self__0024231.MinWait + UnityEngine.Random.value * (_0024self__0024231.MaxWait - _0024self__0024231.MinWait))) ? 1 : 0);
					break;
				case 2:
					_0024self__0024231.StartCoroutine(_0024self__0024231.PlayNoise());
					result = (Yield(3, new WaitForSeconds(_0024self__0024231.MinFrameTime)) ? 1 : 0);
					break;
				case 3:
					_0024self__0024231.StartCoroutine(_0024self__0024231.PlayCreep());
					result = (Yield(4, new WaitForSeconds(_0024self__0024231.MinFrameTime)) ? 1 : 0);
					break;
				case 4:
					_0024self__0024231.StartCoroutine(_0024self__0024231.PlayNoise());
					goto default;
				case 1:
					result = 0;
					break;
				}
				return (byte)result != 0;
			}
		}

		internal CreepFrame _0024self__0024232;

		public _0024CreepLoop_0024230(CreepFrame self_)
		{
			_0024self__0024232 = self_;
		}

		public override IEnumerator<WaitForSeconds> GetEnumerator()
		{
			return new _0024(_0024self__0024232);
		}
	}

	public float alpha;

	public Texture2D[] CreapyFrames;

	public Texture2D[] noiseFrames;

	public float MinWait;

	public float MaxWait;

	public float MinFrameTime;

	public bool AutoCreep;

	public float TransitionTime;

	[NonSerialized]
	private static bool FirstLoad = true;

	private int frame;

	private bool Noise;

	public CreepFrame()
	{
		alpha = 1f;
		MinWait = 2f;
		MaxWait = 15f;
		MinFrameTime = 0.25f;
		AutoCreep = true;
		TransitionTime = 1f;
	}

	public virtual void Awake()
	{
		enabled = false;
		if (AutoCreep)
		{
			if (!FirstLoad)
			{
				StartCoroutine(Transition());
			}
			else
			{
				FirstLoad = false;
			}
			StartCoroutine("CreepLoop");
		}
	}

	public virtual void OnGUI()
	{
		GUI.depth = -20;
		GUI.color = new Color(1f, 1f, 1f, alpha);
		if (Noise)
		{
			GUI.DrawTexture(new Rect(0f, 0f, Screen.width, Screen.height), noiseFrames[(int)(UnityEngine.Random.value * (float)Extensions.get_length((System.Array)noiseFrames))], ScaleMode.StretchToFill);
		}
		else
		{
			GUI.DrawTexture(new Rect(0f, 0f, Screen.width, Screen.height), CreapyFrames[frame], ScaleMode.StretchToFill);
		}
	}

	public virtual IEnumerator PlayNoise()
	{
		return new _0024PlayNoise_0024220(this).GetEnumerator();
	}

	public virtual IEnumerator PlayCreep()
	{
		return new _0024PlayCreep_0024223(this).GetEnumerator();
	}

	public virtual IEnumerator Transition()
	{
		return new _0024Transition_0024226(this).GetEnumerator();
	}

	public virtual IEnumerator CreepLoop()
	{
		return new _0024CreepLoop_0024230(this).GetEnumerator();
	}

	public virtual void Main()
	{
	}
}
