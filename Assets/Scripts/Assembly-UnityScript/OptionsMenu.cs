using System;
using System.Collections;
using System.Collections.Generic;
using Boo.Lang;
using UnityEngine;

[Serializable]
public class OptionsMenu : MonoBehaviour
{
	[Serializable]
	internal sealed class _0024ShowOptions_0024248 : GenericGenerator<WaitForSeconds>
	{
		[Serializable]
		internal sealed class _0024 : GenericGeneratorEnumerator<WaitForSeconds>, IEnumerator
		{
			internal OptionsMenu _0024self__0024249;

			public _0024(OptionsMenu self_)
			{
				_0024self__0024249 = self_;
			}

			public override bool MoveNext()
			{
				int result;
				switch (_state)
				{
				default:
					_0024self__0024249.OptionsActive = true;
					if (PlayerPrefs.GetInt("RightHanded") == 1)
					{
						_0024self__0024249.RightHanded = true;
					}
					else
					{
						_0024self__0024249.RightHanded = false;
					}
					if ((bool)Cart.inst)
					{
						Cart.inst.Pause();
					}
					perspectiveHud.inst.ShowPopUpWindow("Options", false);
					result = (Yield(2, new WaitForSeconds(1f)) ? 1 : 0);
					break;
				case 2:
					_0024self__0024249.OptionState = Options.OptionPage;
					_0024self__0024249.enabled = true;
					YieldDefault(1);
					goto case 1;
				case 1:
					result = 0;
					break;
				}
				return (byte)result != 0;
			}
		}

		internal OptionsMenu _0024self__0024250;

		public _0024ShowOptions_0024248(OptionsMenu self_)
		{
			_0024self__0024250 = self_;
		}

		public override IEnumerator<WaitForSeconds> GetEnumerator()
		{
			return new _0024(_0024self__0024250);
		}
	}

	[Serializable]
	internal sealed class _0024ClearMemory_0024251 : GenericGenerator<object>
	{
		[Serializable]
		internal sealed class _0024 : GenericGeneratorEnumerator<object>, IEnumerator
		{
			internal OptionsMenu _0024self__0024252;

			public _0024(OptionsMenu self_)
			{
				_0024self__0024252 = self_;
			}

			public override bool MoveNext()
			{
				int result;
				switch (_state)
				{
				default:
					Tracker2.instance.Log("Cleared Game Memory");
					Time.timeScale = 1f;
					TrackUpgrade.UpgradeCredits = 0;
					TrackUpgrade.UpgradesList = null;
					Performance.GamePerformance = 2;
					PlayerPrefs.DeleteAll();
					result = (YieldDefault(2) ? 1 : 0);
					break;
				case 2:
					GlobalInterfaceJS.GIgameObject.SendMessage("Die");
					Application.LoadLevel(0);
					UnityEngine.Object.Destroy(_0024self__0024252);
					YieldDefault(1);
					goto case 1;
				case 1:
					result = 0;
					break;
				}
				return (byte)result != 0;
			}
		}

		internal OptionsMenu _0024self__0024253;

		public _0024ClearMemory_0024251(OptionsMenu self_)
		{
			_0024self__0024253 = self_;
		}

		public override IEnumerator<object> GetEnumerator()
		{
			return new _0024(_0024self__0024253);
		}
	}

	[Serializable]
	internal sealed class _0024ExitOptions_0024254 : GenericGenerator<WaitForSeconds>
	{
		[Serializable]
		internal sealed class _0024 : GenericGeneratorEnumerator<WaitForSeconds>, IEnumerator
		{
			internal bool _0024Continue_0024255;

			internal OptionsMenu _0024self__0024256;

			public _0024(bool Continue, OptionsMenu self_)
			{
				_0024Continue_0024255 = Continue;
				_0024self__0024256 = self_;
			}

			public override bool MoveNext()
			{
				int result;
				switch (_state)
				{
				default:
					if (_0024self__0024256.PerformInt != Performance.GamePerformance)
					{
						_0024self__0024256.OptionState = Options.ConfirmPage;
						goto IL_0114;
					}
					if (_0024Continue_0024255)
					{
						_0024self__0024256.enabled = false;
						perspectiveHud.inst.StartCoroutine(perspectiveHud.inst.HidePopUpWindow());
						result = (Yield(2, new WaitForSeconds(0.5f)) ? 1 : 0);
					}
					else
					{
						_0024self__0024256.enabled = false;
						perspectiveHud.inst.StartCoroutine(perspectiveHud.inst.HidePopUpWindow());
						result = (Yield(3, new WaitForSeconds(0.5f)) ? 1 : 0);
					}
					break;
				case 2:
					if ((bool)Cart.inst)
					{
						Cart.inst.Unpause();
					}
					goto IL_0114;
				case 3:
					if (Application.loadedLevel == 0)
					{
						_0024self__0024256.enabled = false;
						MainMenu.inst.MainMenuDisplay(true);
					}
					else
					{
						LevelLoader.inst.StartCoroutine(LevelLoader.inst.LoadLevel(0));
					}
					goto IL_0114;
				case 1:
					{
						result = 0;
						break;
					}
					IL_0114:
					_0024self__0024256.OptionsActive = false;
					YieldDefault(1);
					goto case 1;
				}
				return (byte)result != 0;
			}
		}

		internal bool _0024Continue_0024257;

		internal OptionsMenu _0024self__0024258;

		public _0024ExitOptions_0024254(bool Continue, OptionsMenu self_)
		{
			_0024Continue_0024257 = Continue;
			_0024self__0024258 = self_;
		}

		public override IEnumerator<WaitForSeconds> GetEnumerator()
		{
			return new _0024(_0024Continue_0024257, _0024self__0024258);
		}
	}

	public Texture2D toggle1;

	public Texture2D toggle2;

	public Texture2D SwitchBackground;

	public Texture2D SwitchState1;

	public Texture2D SwitchState2;

	public Texture2D SwitchState3;

	private GUISkin skin;

	//public Texture2D LeftHand;

	//public Texture2D RightHand;

	public bool OptionsActive;

	[NonSerialized]
	public static OptionsMenu inst;

	private Options OptionState;

	private int SoundInt;

	private string[] SoundMuteString;

	private int MusicInt;

	private string[] MusicMuteString;

	private int PerformInt;

	private string[] PerformanceString;

	private bool RightHanded;

	public OptionsMenu()
	{
		SoundMuteString = new string[2] { "ON", "OFF" };
		MusicMuteString = new string[2] { "ON", "OFF" };
		PerformanceString = new string[3] { "LOW", "MID", "HI" };
		RightHanded = true;
	}

	public virtual void DrawToggle(Rect Position, int State)
	{
		if (State == 0)
		{
			GUI.DrawTexture(Position, toggle1, ScaleMode.StretchToFill, true);
		}
		else
		{
			GUI.DrawTexture(Position, toggle2, ScaleMode.StretchToFill, true);
		}
	}

	public virtual void DrawSwitch(Rect Position, int State)
	{
		GUI.DrawTexture(Position, SwitchBackground, ScaleMode.StretchToFill, true);
		if (State == 0)
		{
			GUI.DrawTexture(Position, SwitchState1, ScaleMode.StretchToFill, true);
		}
		if (State == 1)
		{
			GUI.DrawTexture(Position, SwitchState2, ScaleMode.StretchToFill, true);
		}
		if (State == 2)
		{
			GUI.DrawTexture(Position, SwitchState3, ScaleMode.StretchToFill, true);
		}
	}

	public virtual void Awake()
	{
		if (!inst)
		{
			inst = this;
		}
		else
		{
			UnityEngine.Object.Destroy(this);
		}
	}

	public virtual Rect fluidRect(float x, float y, float width, float height)
	{
		return new Rect(x * (float)Screen.width, y * (float)Screen.height, width * (float)Screen.width, height * (float)Screen.height);
	}

	public virtual void Start()
	{
		skin = FontResize.ResizedSkin;
		SoundInt = PlayerPrefs.GetInt("SoundMute");
		MusicInt = PlayerPrefs.GetInt("MusicMute");
		PerformInt = Performance.GamePerformance;
	}

	public virtual IEnumerator ShowOptions()
	{
		return new _0024ShowOptions_0024248(this).GetEnumerator();
	}

	public virtual void OnGUI()
	{
		GUI.skin = skin;
		GUI.depth = -10;
		if (OptionState == Options.ResetTutoral)
		{
			GUI.Label(Global.AspectfluidRect(0.2f, 0.55f, 0.6f, 0.1f, 1.5f), "Do you want to reactivate the in-game tutorials?", "WhiteLabel");
			if (GUI.Button(Global.AspectfluidRect(0.175f, 0.825f, 0.3f, 0.175f, 1.5f), "Cancel", "CancelButton"))
			{
				OptionState = Options.OptionPage;
			}
			if (GUI.Button(Global.AspectfluidRect(0.5f, 0.825f, 0.3f, 0.175f, 1.5f), "Activate", "ContinueButton"))
			{
				TutoralDisplay.ResetTutorals();
				OptionState = Options.OptionPage;
			}
		}
		if (OptionState == Options.OptionPage)
		{
			int soundInt = SoundInt;
			Rect position = Global.AspectfluidRect(0.25f, 0.4f, 0.3f, 0.12f, 1.5f);
			DrawToggle(position, SoundInt);
			SoundInt = GUI.Toolbar(position, SoundInt, SoundMuteString, "MenuToggleOverlay");
			if (SoundInt != soundInt)
			{
				if (SoundInt == 0)
				{
					SoundMute.UpdatedMuteState(false);
				}
				else
				{
					SoundMute.UpdatedMuteState(true);
				}
			}
			int musicInt = MusicInt;
			Rect position2 = Global.AspectfluidRect(0.25f, 0.52f, 0.3f, 0.12f, 1.5f);
			DrawToggle(position2, MusicInt);
			MusicInt = GUI.Toolbar(position2, MusicInt, MusicMuteString, "MenuToggleOverlay");
			if (MusicInt != musicInt)
			{
				if (MusicInt == 0)
				{
					MusicMute.UpdatedMusicState(false);
				}
				else
				{
					MusicMute.UpdatedMusicState(true);
				}
			}
			GUI.Button(Global.AspectfluidRect(0.11f, 0.4f, 0.2f, 0.15f, 1.5f), "SFX", "YellowText");
			GUI.Button(Global.AspectfluidRect(0.1f, 0.52f, 0.2f, 0.15f, 1.5f), "MUSIC", "YellowText");
			GUI.Button(Global.AspectfluidRect(0.11f, 0.63f, 0.2f, 0.15f, 1.5f), "GFX", "YellowText");
			if (GUI.Button(Global.AspectfluidRect(0.67f, 0.38f, 0.2f, 0.15f, 1.5f), "Reactivate\nTutorials", "StandardButton"))
			{
				OptionState = Options.ResetTutoral;
			}
			Rect position3 = Global.AspectfluidRect(0.25f, 0.64f, 0.4f, 0.12f, 1.5f);
			DrawSwitch(position3, PerformInt);
			PerformInt = GUI.Toolbar(position3, PerformInt, PerformanceString, "MenuToggleOverlay");
			if (GUI.Button(Global.AspectfluidRect(0.67f, 0.6f, 0.2f, 0.15f, 1.5f), "Clear\nMemory", "StandardButton"))
			{
				OptionState = Options.ClearMem;
			}
			if (GUI.Button(Global.AspectfluidRect(0.175f, 0.825f, 0.3f, 0.175f, 1.5f), "Main Menu", "LeftPopup"))
			{
				StartCoroutine(ExitOptions(false));
			}
			if (Application.loadedLevel != 0 && GUI.Button(Global.AspectfluidRect(0.5f, 0.825f, 0.3f, 0.175f, 1.5f), "Continue", "RightPopup"))
			{
				StartCoroutine(ExitOptions(true));
			}
		}
		if (OptionState == Options.ConfirmPage)
		{
			GUI.Box(Global.AspectfluidRect(0.2f, 0.4f, 0.6f, 0.3f, 1.5f), "To apply these changes the game must be restarted", "YellowText");
			if (GUI.Button(Global.AspectfluidRect(0.5f, 0.825f, 0.3f, 0.175f, 1.5f), "Apply", "ContinueButton"))
			{
				Tracker2.instance.LogWithParameters("Performance Changed To ", PerformInt + string.Empty);
				Time.timeScale = 1f;
				enabled = false;
				PlayerPrefs.SetInt("DevicePerformance", PerformInt);
				LevelLoader.inst.StartCoroutine(LevelLoader.inst.LoadLevel(0));
			}
			if (GUI.Button(Global.AspectfluidRect(0.175f, 0.825f, 0.3f, 0.175f, 1.5f), "Cancel", "CancelButton"))
			{
				Time.timeScale = 1f;
				PerformInt = Performance.GamePerformance;
				OptionState = Options.OptionPage;
			}
		}
		if (OptionState == Options.ClearMem)
		{
			GUI.Box(Global.AspectfluidRect(0.2f, 0.4f, 0.6f, 0.3f, 1.5f), "Are you sure you want to delete all your progress and reset your score?", "YellowText");
			if (GUI.Button(Global.AspectfluidRect(0.5f, 0.825f, 0.3f, 0.175f, 1.5f), "Delete", "ContinueButton"))
			{
				StartCoroutine(ClearMemory());
			}
			if (GUI.Button(Global.AspectfluidRect(0.175f, 0.825f, 0.3f, 0.175f, 1.5f), "Cancel", "CancelButton"))
			{
				OptionState = Options.OptionPage;
			}
		}
	}

	public virtual IEnumerator ClearMemory()
	{
		return new _0024ClearMemory_0024251(this).GetEnumerator();
	}

	public virtual IEnumerator ExitOptions(bool Continue)
	{
		return new _0024ExitOptions_0024254(Continue, this).GetEnumerator();
	}

	public virtual void Main()
	{
	}
}
