using System;
using System.Collections;
using System.Collections.Generic;
using Boo.Lang;
using UnityEngine;
using UnityScript.Lang;

[Serializable]
public class TutoralDisplay : MonoBehaviour
{
	[Serializable]
	internal sealed class _0024TriggerTutoral_0024102 : GenericGenerator<WaitForSeconds>
	{
		[Serializable]
		internal sealed class _0024 : GenericGeneratorEnumerator<WaitForSeconds>, IEnumerator
		{
			internal int _0024TutNum_0024103;

			internal GameObject _0024CallBack_0024104;

			internal TutoralDisplay _0024self__0024105;

			public _0024(int TutNum, GameObject CallBack, TutoralDisplay self_)
			{
				_0024TutNum_0024103 = TutNum;
				_0024CallBack_0024104 = CallBack;
				_0024self__0024105 = self_;
			}

			public override bool MoveNext()
			{
				int result;
				switch (_state)
				{
				default:
					MonoBehaviour.print("PopTutoral");
					if (!_0024self__0024105.TutoralViewed[_0024TutNum_0024103])
					{
						_0024self__0024105.TutoralOn = true;
						_0024self__0024105.callback = _0024CallBack_0024104;
						_0024self__0024105.callback.SendMessage("TutoralStart");
						_0024self__0024105.page = 0;
						_0024self__0024105.showtutoral = _0024TutNum_0024103;
						_0024self__0024105.TutoralViewed[_0024TutNum_0024103] = true;
						perspectiveHud.inst.ShowPopUpWindow("Tutorial", false);
						result = (Yield(2, new WaitForSeconds(1.5f)) ? 1 : 0);
						break;
					}
					_0024CallBack_0024104.SendMessage("TutoralSkiped", SendMessageOptions.DontRequireReceiver);
					goto IL_011b;
				case 2:
					_0024self__0024105.enabled = true;
					_0024self__0024105.StartCoroutine("AnimateGraphic");
					PlayerPrefsX.SetBoolArray("Tutoral", _0024self__0024105.TutoralViewed);
					goto IL_011b;
				case 1:
					{
						result = 0;
						break;
					}
					IL_011b:
					YieldDefault(1);
					goto case 1;
				}
				return (byte)result != 0;
			}
		}

		internal int _0024TutNum_0024106;

		internal GameObject _0024CallBack_0024107;

		internal TutoralDisplay _0024self__0024108;

		public _0024TriggerTutoral_0024102(int TutNum, GameObject CallBack, TutoralDisplay self_)
		{
			_0024TutNum_0024106 = TutNum;
			_0024CallBack_0024107 = CallBack;
			_0024self__0024108 = self_;
		}

		public override IEnumerator<WaitForSeconds> GetEnumerator()
		{
			return new _0024(_0024TutNum_0024106, _0024CallBack_0024107, _0024self__0024108);
		}
	}

	[Serializable]
	internal sealed class _0024EndTutoral_0024109 : GenericGenerator<WaitForSeconds>
	{
		[Serializable]
		internal sealed class _0024 : GenericGeneratorEnumerator<WaitForSeconds>, IEnumerator
		{
			internal TutoralDisplay _0024self__0024110;

			public _0024(TutoralDisplay self_)
			{
				_0024self__0024110 = self_;
			}

			public override bool MoveNext()
			{
				int result;
				switch (_state)
				{
				default:
					_0024self__0024110.enabled = false;
					_0024self__0024110.showtutoral = -1;
					perspectiveHud.inst.StartCoroutine(perspectiveHud.inst.HidePopUpWindow());
					result = (Yield(2, new WaitForSeconds(0.5f)) ? 1 : 0);
					break;
				case 2:
					_0024self__0024110.callback.SendMessage("TutoralEnded");
					_0024self__0024110.TutoralOn = false;
					YieldDefault(1);
					goto case 1;
				case 1:
					result = 0;
					break;
				}
				return (byte)result != 0;
			}
		}

		internal TutoralDisplay _0024self__0024111;

		public _0024EndTutoral_0024109(TutoralDisplay self_)
		{
			_0024self__0024111 = self_;
		}

		public override IEnumerator<WaitForSeconds> GetEnumerator()
		{
			return new _0024(_0024self__0024111);
		}
	}

	[Serializable]
	internal sealed class _0024AnimateGraphic_0024112 : GenericGenerator<WaitForSeconds>
	{
		[Serializable]
		internal sealed class _0024 : GenericGeneratorEnumerator<WaitForSeconds>, IEnumerator
		{
			internal int _0024framePerPage_0024113;

			internal TutoralDisplay _0024self__0024114;

			public _0024(TutoralDisplay self_)
			{
				_0024self__0024114 = self_;
			}

			public override bool MoveNext()
			{
				int result;
				switch (_state)
				{
				default:
					if (_0024self__0024114.enabled)
					{
						_0024self__0024114.AnimationFrame++;
						_0024framePerPage_0024113 = Extensions.get_length((System.Array)_0024self__0024114.Tutorals[_0024self__0024114.showtutoral].AnimationFrames) / Extensions.get_length((System.Array)_0024self__0024114.Tutorals[_0024self__0024114.showtutoral].PageText);
						_0024self__0024114.AnimationFrame %= _0024framePerPage_0024113;
						_0024self__0024114.AnimationFrame += _0024framePerPage_0024113 * _0024self__0024114.page;
						result = (Yield(2, new WaitForSeconds(0.5f)) ? 1 : 0);
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

		internal TutoralDisplay _0024self__0024115;

		public _0024AnimateGraphic_0024112(TutoralDisplay self_)
		{
			_0024self__0024115 = self_;
		}

		public override IEnumerator<WaitForSeconds> GetEnumerator()
		{
			return new _0024(_0024self__0024115);
		}
	}

	[NonSerialized]
	public static TutoralDisplay inst;

	private GUISkin skin;

	public TutoralClass[] Tutorals;

	private bool[] TutoralViewed;

	private int showtutoral;

	private int page;

	public bool TutoralOn;

	private GameObject callback;

	private int AnimationFrame;

	public TutoralDisplay()
	{
		showtutoral = -1;
	}

	public virtual void Start()
	{
		skin = FontResize.ResizedSkin;
	}

	public static void ResetTutorals()
	{
		inst.TutoralViewed = new bool[Extensions.get_length((System.Array)inst.Tutorals)];
		PlayerPrefsX.SetBoolArray("Tutoral", inst.TutoralViewed);
	}

	public virtual void Awake()
	{
		inst = this;
		if (PlayerPrefs.HasKey("Tutoral"))
		{
			TutoralViewed = PlayerPrefsX.GetBoolArray("Tutoral");
		}
		else
		{
			TutoralViewed = new bool[Extensions.get_length((System.Array)Tutorals)];
		}
		enabled = false;
	}

	public virtual void TriggerTutoralC(int TutNum, GameObject CallBack)
	{
		StartCoroutine(TriggerTutoral(TutNum, CallBack));
	}

	public virtual void ForceTutoral(int TutNum, GameObject CallBack)
	{
		TutoralViewed[TutNum] = false;
		StartCoroutine(TriggerTutoral(TutNum, CallBack));
	}

	public virtual IEnumerator TriggerTutoral(int TutNum, GameObject CallBack)
	{
		return new _0024TriggerTutoral_0024102(TutNum, CallBack, this).GetEnumerator();
	}

	public virtual IEnumerator EndTutoral()
	{
		return new _0024EndTutoral_0024109(this).GetEnumerator();
	}

	public virtual Rect fluidRect(float x, float y, float width, float height)
	{
		return new Rect(x * (float)Screen.width, y * (float)Screen.height, width * (float)Screen.width, height * (float)Screen.height);
	}

	public virtual IEnumerator AnimateGraphic()
	{
		return new _0024AnimateGraphic_0024112(this).GetEnumerator();
	}

	public virtual void OnGUI()
	{
		GUI.skin = skin;
		GUI.DrawTexture(Global.AspectfluidRect(0.2f, 0.37f, 0.6f, 0.35f, 1.5f), Tutorals[showtutoral].AnimationFrames[AnimationFrame], ScaleMode.ScaleToFit, true);
		GUI.Label(Global.AspectfluidRect(0.15f, 0.68f, 0.7f, 0.2f, 1.5f), Tutorals[showtutoral].PageText[page], "YellowText");
		if (page == Tutorals[showtutoral].PageText.Length - 1)
		{
			if (GUI.Button(Global.AspectfluidRect(0.4f, 0.84f, 0.2f, 0.15f, 1.5f), "Continue", "StandardButton"))
			{
				StartCoroutine(EndTutoral());
			}
		}
		else if (GUI.Button(Global.AspectfluidRect(0.4f, 0.84f, 0.2f, 0.15f, 1.5f), "Next", "StandardButton"))
		{
			page++;
			StopCoroutine("AnimateGraphic");
			StartCoroutine("AnimateGraphic");
		}
	}

	public virtual void Main()
	{
	}
}
