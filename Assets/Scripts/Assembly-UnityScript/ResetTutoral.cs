using System;
using System.Collections;
using System.Collections.Generic;
using Boo.Lang;
using UnityEngine;

[Serializable]
public class ResetTutoral : MonoBehaviour
{
	[Serializable]
	internal sealed class _0024SetTutoralState_0024259 : GenericGenerator<WaitForSeconds>
	{
		[Serializable]
		internal sealed class _0024 : GenericGeneratorEnumerator<WaitForSeconds>, IEnumerator
		{
			internal bool _0024state_0024260;

			internal ResetTutoral _0024self__0024261;

			public _0024(bool state, ResetTutoral self_)
			{
				_0024state_0024260 = state;
				_0024self__0024261 = self_;
			}

			public override bool MoveNext()
			{
				int result;
				switch (_state)
				{
				default:
					if (_0024state_0024260)
					{
						MainMenu.inst.MainMenuDisplay(false);
						perspectiveHud.inst.ShowPopUpWindow("Tutorials", false);
						result = (Yield(2, new WaitForSeconds(0.5f)) ? 1 : 0);
					}
					else
					{
						_0024self__0024261.enabled = false;
						perspectiveHud.inst.StartCoroutine(perspectiveHud.inst.HidePopUpWindow());
						result = (Yield(3, new WaitForSeconds(0.5f)) ? 1 : 0);
					}
					break;
				case 2:
					_0024self__0024261.enabled = true;
					goto IL_00aa;
				case 3:
					MainMenu.inst.MainMenuDisplay(true);
					goto IL_00aa;
				case 1:
					{
						result = 0;
						break;
					}
					IL_00aa:
					YieldDefault(1);
					goto case 1;
				}
				return (byte)result != 0;
			}
		}

		internal bool _0024state_0024262;

		internal ResetTutoral _0024self__0024263;

		public _0024SetTutoralState_0024259(bool state, ResetTutoral self_)
		{
			_0024state_0024262 = state;
			_0024self__0024263 = self_;
		}

		public override IEnumerator<WaitForSeconds> GetEnumerator()
		{
			return new _0024(_0024state_0024262, _0024self__0024263);
		}
	}

	[NonSerialized]
	public static ResetTutoral inst;

	private GUISkin Skin;

	public virtual void Awake()
	{
		inst = this;
		enabled = false;
	}

	public virtual void Start()
	{
		Skin = FontResize.ResizedSkin;
	}

	public virtual IEnumerator SetTutoralState(bool state)
	{
		return new _0024SetTutoralState_0024259(state, this).GetEnumerator();
	}

	public virtual void OnGUI()
	{
		GUI.skin = Skin;
		GUI.Label(Global.AspectfluidRect(0.2f, 0.55f, 0.6f, 0.1f, 1.5f), "Do you want to reactivate the in-game tutorials?", "WhiteLabel");
		if (GUI.Button(Global.AspectfluidRect(0.175f, 0.825f, 0.3f, 0.175f, 1.5f), "Cancel", "CancelButton"))
		{
			StartCoroutine(SetTutoralState(false));
		}
		if (GUI.Button(Global.AspectfluidRect(0.5f, 0.825f, 0.3f, 0.175f, 1.5f), "Activate", "ContinueButton"))
		{
			TutoralDisplay.ResetTutorals();
			StartCoroutine(SetTutoralState(false));
		}
	}

	public virtual void Main()
	{
	}
}
