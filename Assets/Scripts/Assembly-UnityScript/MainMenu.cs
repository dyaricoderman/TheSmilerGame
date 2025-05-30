using System;
using System.Collections;
using System.Collections.Generic;
using Boo.Lang;
using CompilerGenerated;
using UnityEngine;
using UnityEngine.Video;
using UnityScript.Lang;

[Serializable]
public class MainMenu : MonoBehaviour
{
	[Serializable]
	internal sealed class _0024LoadSequenceTimer_0024233 : GenericGenerator<WaitForSeconds>
	{
		[Serializable]
		internal sealed class _0024 : GenericGeneratorEnumerator<WaitForSeconds>, IEnumerator
		{
			internal MainMenu _0024self__0024234;

			public _0024(MainMenu self_)
			{
				_0024self__0024234 = self_;
			}

			public override bool MoveNext()
			{
				int result;
				switch (_state)
				{
				default:
					result = (Yield(2, new WaitForSeconds(8f)) ? 1 : 0);
					break;
				case 2:
					_0024self__0024234.CF.StartCoroutine(_0024self__0024234.CF.Transition());
					_0024self__0024234.backgroundRender.enabled = false;
					_0024self__0024234.SetMainFace(false);
					MenuState = MainMenuStates.Video;
					#if UNITY_ANDROID
					if (Input.deviceOrientation != DeviceOrientation.PortraitUpsideDown && Input.deviceOrientation != DeviceOrientation.Portrait)
					{
						Handheld.PlayFullScreenMovie("MainIntro_audio.mp4", Color.black, FullScreenMovieControlMode.CancelOnInput);
					}
					#endif
					if (Application.platform == RuntimePlatform.WindowsPlayer)
					{
							//_0024self__0024234.GetComponent<VideoPlayer>().enabled = true;
					}
					result = (Yield(3, new WaitForSeconds(2f)) ? 1 : 0);
					break;
				case 3:
					_0024self__0024234.backgroundRender.enabled = true;
					_0024self__0024234.SetMainFace(true);
					_0024self__0024234.CF.StartCoroutine(_0024self__0024234.CF.Transition());
					_0024self__0024234.music.Play();
					MenuState = MainMenuStates.Loaded;
					YieldDefault(1);
					goto case 1;
				case 1:
					result = 0;
					break;
				}
				return (byte)result != 0;
			}
		}

		internal MainMenu _0024self__0024235;

		public _0024LoadSequenceTimer_0024233(MainMenu self_)
		{
			_0024self__0024235 = self_;
		}

		public override IEnumerator<WaitForSeconds> GetEnumerator()
		{
			return new _0024(_0024self__0024235);
		}
	}

	[Serializable]
	internal sealed class _0024LoadScan_0024236 : GenericGenerator<WaitForSeconds>
	{
		[Serializable]
		internal sealed class _0024 : GenericGeneratorEnumerator<WaitForSeconds>, IEnumerator
		{
			internal MainMenu _0024self__0024237;

			public _0024(MainMenu self_)
			{
				_0024self__0024237 = self_;
			}

			public override bool MoveNext()
			{
				int result;
				switch (_state)
				{
				default:
					Tracker2.instance.Log("AR Scanner loaded");
					MenuState = MainMenuStates.Loading;
					_0024self__0024237.CF.StartCoroutine(_0024self__0024237.CF.Transition());
					result = (Yield(2, new WaitForSeconds(_0024self__0024237.CF.TransitionTime)) ? 1 : 0);
					break;
				case 2:
					UnityEngine.Object.Destroy(_0024self__0024237.CF);
					result = (YieldDefault(3) ? 1 : 0);
					break;
				case 3:
					LevelLoader.inst.StartCoroutine(LevelLoader.inst.LoadLevel(1));
					YieldDefault(1);
					goto case 1;
				case 1:
					result = 0;
					break;
				}
				return (byte)result != 0;
			}
		}

		internal MainMenu _0024self__0024238;

		public _0024LoadScan_0024236(MainMenu self_)
		{
			_0024self__0024238 = self_;
		}

		public override IEnumerator<WaitForSeconds> GetEnumerator()
		{
			return new _0024(_0024self__0024238);
		}
	}

	[Serializable]
	internal sealed class _0024LoadGame_0024239 : GenericGenerator<WaitForSeconds>
	{
		[Serializable]
		internal sealed class _0024 : GenericGeneratorEnumerator<WaitForSeconds>, IEnumerator
		{
			internal MainMenu _0024self__0024240;

			public _0024(MainMenu self_)
			{
				_0024self__0024240 = self_;
			}

			public override bool MoveNext()
			{
				int result;
				switch (_state)
				{
				default:
					CartCam.IntroPlayed = false;
					MenuState = MainMenuStates.Loading;
					_0024self__0024240.CF.StartCoroutine(_0024self__0024240.CF.Transition());
					result = (Yield(2, new WaitForSeconds(_0024self__0024240.CF.TransitionTime * 1f)) ? 1 : 0);
					break;
				case 2:
					UnityEngine.Object.Destroy(_0024self__0024240.CF);
					result = (YieldDefault(3) ? 1 : 0);
					break;
				case 3:
					GameControl.InitGame();
					YieldDefault(1);
					goto case 1;
				case 1:
					result = 0;
					break;
				}
				return (byte)result != 0;
			}
		}

		internal MainMenu _0024self__0024241;

		public _0024LoadGame_0024239(MainMenu self_)
		{
			_0024self__0024241 = self_;
		}

		public override IEnumerator<WaitForSeconds> GetEnumerator()
		{
			return new _0024(_0024self__0024241);
		}
	}

	[Serializable]
	internal sealed class _0024LoadComp_0024242 : GenericGenerator<WaitForSeconds>
	{
		[Serializable]
		internal sealed class _0024 : GenericGeneratorEnumerator<WaitForSeconds>, IEnumerator
		{
			internal MainMenu _0024self__0024243;

			public _0024(MainMenu self_)
			{
				_0024self__0024243 = self_;
			}

			public override bool MoveNext()
			{
				int result;
				switch (_state)
				{
				default:
					MenuState = MainMenuStates.Loading;
					_0024self__0024243.CF.StartCoroutine(_0024self__0024243.CF.Transition());
					result = (Yield(2, new WaitForSeconds(_0024self__0024243.CF.TransitionTime)) ? 1 : 0);
					break;
				case 2:
					UnityEngine.Object.Destroy(_0024self__0024243.CF);
					result = (YieldDefault(3) ? 1 : 0);
					break;
				case 3:
					LevelLoader.inst.StartCoroutine(LevelLoader.inst.LoadLevel(3));
					YieldDefault(1);
					goto case 1;
				case 1:
					result = 0;
					break;
				}
				return (byte)result != 0;
			}
		}

		internal MainMenu _0024self__0024244;

		public _0024LoadComp_0024242(MainMenu self_)
		{
			_0024self__0024244 = self_;
		}

		public override IEnumerator<WaitForSeconds> GetEnumerator()
		{
			return new _0024(_0024self__0024244);
		}
	}

	[Serializable]
	internal sealed class _0024LoadBlueprint_0024245 : GenericGenerator<WaitForSeconds>
	{
		[Serializable]
		internal sealed class _0024 : GenericGeneratorEnumerator<WaitForSeconds>, IEnumerator
		{
			internal MainMenu _0024self__0024246;

			public _0024(MainMenu self_)
			{
				_0024self__0024246 = self_;
			}

			public override bool MoveNext()
			{
				int result;
				switch (_state)
				{
				default:
					MenuState = MainMenuStates.Loading;
					_0024self__0024246.CF.StartCoroutine(_0024self__0024246.CF.Transition());
					result = (Yield(2, new WaitForSeconds(_0024self__0024246.CF.TransitionTime)) ? 1 : 0);
					break;
				case 2:
					UnityEngine.Object.Destroy(_0024self__0024246.CF);
					result = (YieldDefault(3) ? 1 : 0);
					break;
				case 3:
					LevelLoader.inst.StartCoroutine(LevelLoader.inst.LoadLevel(3));
					YieldDefault(1);
					goto case 1;
				case 1:
					result = 0;
					break;
				}
				return (byte)result != 0;
			}
		}

		internal MainMenu _0024self__0024247;

		public _0024LoadBlueprint_0024245(MainMenu self_)
		{
			_0024self__0024247 = self_;
		}

		public override IEnumerator<WaitForSeconds> GetEnumerator()
		{
			return new _0024(_0024self__0024247);
		}
	}

	public CreepFrame CF;

	public Texture2D Warning;

	public Texture2D Background;

	public Texture2D LogoText;

	public Texture2D OptionsIcon;

	public Texture2D AcheavementsIcon;

	public Texture2D TutoralIcon;

	public Renderer backgroundRender;

	public AudioSource music;

	private GUISkin Skin;

	public string versionNumber;

	public Renderer[] MainFace;

	[NonSerialized]
	public static MainMenuStates MenuState = MainMenuStates.Warning;

	[NonSerialized]
	public static MainMenu inst;

	public WebCamDevice[] devices;

	public MainMenu()
	{
		versionNumber = "1.0";
	}

	public virtual Rect fluidRect(float x, float y, float width, float height)
	{
		return new Rect(x * (float)Screen.width, y * (float)Screen.height, width * (float)Screen.width, height * (float)Screen.height);
	}

	public virtual void Start()
	{
		Skin = FontResize.ResizedSkin;
		devices = WebCamTexture.devices;
	}

	public virtual void Awake()
	{
		inst = this;
		if (!PlayerPrefs.HasKey("RightHanded"))
		{
			PlayerPrefs.SetInt("RightHanded", 1);
		}
		if (MenuState == MainMenuStates.Warning)
		{
			StartCoroutine(LoadSequenceTimer());
			Tracker2.instance.LogWithParameters("Started Game With Device", SystemInfo.deviceModel);
		}
		else
		{
			music.Play();
		}
		if (MenuState == MainMenuStates.Loading)
		{
			MenuState = MainMenuStates.Loaded;
		}
	}

	public virtual IEnumerator LoadSequenceTimer()
	{
		return new _0024LoadSequenceTimer_0024233(this).GetEnumerator();
	}

	public virtual void MainMenuDisplay(bool state)
	{
		enabled = state;
		if (state)
		{
			for (int i = 0; i < Extensions.get_length((System.Array)MainFace); i++)
			{
				MainFace[i].enabled = true;
			}
			CF.StartCoroutine(CF.Transition());
		}
		else
		{
			for (int i = 0; i < Extensions.get_length((System.Array)MainFace); i++)
			{
				MainFace[i].enabled = false;
			}
		}
	}

	public virtual void SetMainFace(bool state)
	{
		for (int i = 0; i < Extensions.get_length((System.Array)MainFace); i++)
		{
			MainFace[i].enabled = state;
		}
	}

	public virtual void OnGUI()
	{
		GUI.skin = Skin;
		if (MenuState == MainMenuStates.Warning)
		{
			GUI.DrawTexture(fluidRect(0f, 0f, 1f, 1f), Background, ScaleMode.StretchToFill, true);
			GUI.DrawTexture(fluidRect(0.3f, 0.2f, 0.4f, 0.4f), Warning, ScaleMode.ScaleToFit, true);
			GUI.Label(fluidRect(0.2f, 0.7f, 0.6f, 0.2f), "Warning this game uses flashing images and is not appropriate for people with photosensitive epilepsy", "YellowText");
		}
		if (MenuState == MainMenuStates.Loaded)
		{
			GUI.DrawTexture(Global.AspectRect(0.15f, 0f, 0.4f, 0.2f, PositionType.BottemLeft), LogoText, ScaleMode.ScaleToFit, true);
			if (GUI.Button(Global.AspectRect(0f, 0.15f, 0.7f, 0.2f, PositionType.TopRight), string.Empty, "MainPlayButton"))
			{
				StartCoroutine(LoadGame());
			}
			if (Extensions.get_length((System.Array)devices) > 0 && GUI.Button(Global.AspectRect(0f, 0.36f, 0.6f, 0.2f, PositionType.TopRight), string.Empty, "MainScannerButton"))
			{
				StartCoroutine(LoadScan());
			}
			if (GUI.Button(Global.AspectRect(0f, 0.6f, 0.6f, 0.2f, PositionType.TopRight), string.Empty, "MainBlueprintButton"))
			{
				StartCoroutine(LoadBlueprint());
			}
			if (GUI.Button(Global.AspectRect(0f, 0f, 0.6f, 0.18f, PositionType.BottemRight), string.Empty, "MenuWebsiteButton"))
			{
				LoadWebsite();
			}
			if (GUI.Button(Global.AspectRect(0.2f, 0.02f, 0.18f, 0.14f, PositionType.TopRight), string.Empty, "MenuOptionsButton"))
			{
				CF.StartCoroutine(CF.Transition());
				enabled = false;
				OptionsMenu.inst.StartCoroutine(OptionsMenu.inst.ShowOptions());
			}
			if (GUI.Button(Global.AspectRect(0.45f, 0.02f, 0.18f, 0.14f, PositionType.TopRight), string.Empty, "MenuAchevementsButton"))
			{
				loadAcheavements();
			}
			if (GUI.Button(Global.AspectRect(0.02f, 0.02f, 0.18f, 0.14f, PositionType.BottemLeft), string.Empty, "MenuQuit"))
			{
				CF.StartCoroutine(CF.Transition());
				Application.Quit();
			}
			GUI.color = new Color(1f, 1f, 1f, 0.2f);
			GUI.Label(Global.AspectRect(0.1f, 0f, 0.25f, 0.08f, PositionType.BottemLeft), "V" + versionNumber, "YellowText");
			GUI.color = new Color(1f, 1f, 1f, 1f);
		}
	}

	public virtual void loadAcheavements()
	{
		Tracker2.instance.Log("Achevements Page loaded:" + new __MainMenu_loadAcheavements_0024callable0_0024169_76__(AchievementManager.AchevementUnlockedCount));
		MainMenuDisplay(false);
		CF.StartCoroutine(CF.Transition());
		gameObject.SendMessage("AchievementsDisplay", true);
	}

	public virtual void LoadWebsite()
	{
		Tracker2.instance.Log("Website Landing Page loaded");
		CF.StartCoroutine(CF.Transition());
		MainMenuDisplay(false);
		gameObject.SendMessage("ShowWebPage");
	}

	public virtual IEnumerator LoadScan()
	{
		return new _0024LoadScan_0024236(this).GetEnumerator();
	}

	public virtual IEnumerator LoadGame()
	{
		return new _0024LoadGame_0024239(this).GetEnumerator();
	}

	public virtual IEnumerator LoadComp()
	{
		return new _0024LoadComp_0024242(this).GetEnumerator();
	}

	public virtual IEnumerator LoadBlueprint()
	{
		return new _0024LoadBlueprint_0024245(this).GetEnumerator();
	}

	public virtual void Main()
	{
	}
}
