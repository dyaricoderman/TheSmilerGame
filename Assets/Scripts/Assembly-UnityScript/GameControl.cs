using System;
using System.Collections;
using System.Collections.Generic;
using Boo.Lang;
using UnityEngine;
using UnityScript.Lang;

[Serializable]
public class GameControl : MonoBehaviour
{
	[Serializable]
	internal sealed class _0024FinishLevel_0024153 : GenericGenerator<WaitForSeconds>
	{
		[Serializable]
		internal sealed class _0024 : GenericGeneratorEnumerator<WaitForSeconds>, IEnumerator
		{
			internal int _0024WA_0024154;

			internal Cart _0024cart_0024155;

			internal GameControl _0024self__0024156;

			public _0024(Cart cart, GameControl self_)
			{
				_0024cart_0024155 = cart;
				_0024self__0024156 = self_;
			}

			public override bool MoveNext()
			{
				int result;
				switch (_state)
				{
				default:
					CartCam.inst.indicatorCam.enabled = false;
					_0024self__0024156.CurrentTopScore = PlayerPrefs.GetInt("TopScore");
					_0024self__0024156.page = MenuPages.blank;
					if (!((float)_0024self__0024156.CurrentTopScore >= Cart.currentScore))
					{
						PlayerPrefs.SetInt("TopScore", (int)Cart.currentScore);
					}
					_0024self__0024156.UpgradeCreditsMade = (int)(Cart.currentScore / 500f);
					_0024cart_0024155.StartCoroutine(_0024cart_0024155.SetCartRunning(false));
					_0024self__0024156.enabled = true;
					CartCam.inst.SetHud(false);
					result = (Yield(2, new WaitForSeconds(2f)) ? 1 : 0);
					break;
				case 2:
					CartCam.inst.SetCameraMode(CameraMode.StaticView);
					perspectiveHud.inst.ShowPopUpWindow("Ride Results", false);
					result = (Yield(3, new WaitForSeconds(1.5f)) ? 1 : 0);
					break;
				case 3:
					CartCam.inst.SetBrightness(0f);
					_0024self__0024156.SumAnProg = 0f;
					_0024self__0024156.page = MenuPages.Summery;
					AchievementManager.AddProgressToAchievement("Baby's First Smile", 1f);
					AchievementManager.AddProgressToAchievement("Repetetron", 1f);
					AchievementManager.AddProgressToAchievement("Sisyphean", 1f);
					if (!(Cart.currentScore < CartCam.inst.Cart.FullMarmalizeScore))
					{
						MonoBehaviour.print("fullMar");
						AchievementManager.AddProgressToAchievement("Mr. Marmaliser", 1f);
					}
					_0024WA_0024154 = _0024cart_0024155.GetWorstAcc();
					AchievementManager.SetProgressToAchievement("Tilt Trainee", _0024WA_0024154);
					AchievementManager.SetProgressToAchievement("Tilt Master", _0024WA_0024154);
					YieldDefault(1);
					goto case 1;
				case 1:
					result = 0;
					break;
				}
				return (byte)result != 0;
			}
		}

		internal Cart _0024cart_0024157;

		internal GameControl _0024self__0024158;

		public _0024FinishLevel_0024153(Cart cart, GameControl self_)
		{
			_0024cart_0024157 = cart;
			_0024self__0024158 = self_;
		}

		public override IEnumerator<WaitForSeconds> GetEnumerator()
		{
			return new _0024(_0024cart_0024157, _0024self__0024158);
		}
	}

	[Serializable]
	internal sealed class _0024StartGame_0024159 : GenericGenerator<WaitForSeconds>
	{
		[Serializable]
		internal sealed class _0024 : GenericGeneratorEnumerator<WaitForSeconds>, IEnumerator
		{
			internal GameControl _0024self__0024160;

			public _0024(GameControl self_)
			{
				_0024self__0024160 = self_;
			}

			public override bool MoveNext()
			{
				int result;
				switch (_state)
				{
				default:
					ShowStartPage = false;
					_0024self__0024160.enabled = false;
					CartCam.inst.SetCameraMode(CameraMode.PlayGame);
					result = (Yield(2, new WaitForSeconds(1f)) ? 1 : 0);
					break;
				case 2:
					_0024self__0024160.cart.StartCoroutine(_0024self__0024160.cart.SetCartRunning(true));
					YieldDefault(1);
					goto case 1;
				case 1:
					result = 0;
					break;
				}
				return (byte)result != 0;
			}
		}

		internal GameControl _0024self__0024161;

		public _0024StartGame_0024159(GameControl self_)
		{
			_0024self__0024161 = self_;
		}

		public override IEnumerator<WaitForSeconds> GetEnumerator()
		{
			return new _0024(_0024self__0024161);
		}
	}

	[Serializable]
	internal sealed class _0024ShowUpgradeShop_0024162 : GenericGenerator<WaitForSeconds>
	{
		[Serializable]
		internal sealed class _0024 : GenericGeneratorEnumerator<WaitForSeconds>, IEnumerator
		{
			internal GameControl _0024self__0024163;

			public _0024(GameControl self_)
			{
				_0024self__0024163 = self_;
			}

			public override bool MoveNext()
			{
				int result;
				switch (_state)
				{
				default:
					_0024self__0024163.enabled = true;
					_0024self__0024163.page = MenuPages.blank;
					perspectiveHud.inst.StartCoroutine(perspectiveHud.inst.HidePopUpWindow());
					result = (Yield(2, new WaitForSeconds(1.5f)) ? 1 : 0);
					break;
				case 2:
					TrackUpgrade.AddCredits(_0024self__0024163.UpgradeCreditsMade);
					_0024self__0024163.page = MenuPages.Shop;
					UpgradeShop.inst.StartShop();
					YieldDefault(1);
					goto case 1;
				case 1:
					result = 0;
					break;
				}
				return (byte)result != 0;
			}
		}

		internal GameControl _0024self__0024164;

		public _0024ShowUpgradeShop_0024162(GameControl self_)
		{
			_0024self__0024164 = self_;
		}

		public override IEnumerator<WaitForSeconds> GetEnumerator()
		{
			return new _0024(_0024self__0024164);
		}
	}

	[NonSerialized]
	public static GameControl inst;

	public Cart cart;

	private GUISkin Skin;

	public MenuPages page;

	public Texture2D FaceBlank;

	public Texture2D FaceMar;

	public Transform tutoral;

	public Texture2D Coin;

	public ButtonControl ButtonCon;

	public Texture2D FullMarTex;

	[NonSerialized]
	public static bool ShowStartPage = true;

	private int UpgradeCreditsMade;

	private float SumAnProg;

	private int CurrentTopScore;

	private Rect FacesAreaBig;

	private Rect FacesAreaSmall;

	private float fade;

	private Vector2 CoinStart;

	private Vector2 CoinEnd;

	public GameControl()
	{
		page = MenuPages.MainPage;
		FacesAreaBig = Global.AspectfluidRect(0.38f, 0.37f, 0.24f, 0.36f, 1.5f);
		FacesAreaSmall = Global.AspectfluidRect(0.2f, 0.4f, 0.2f, 0.3f, 1.5f);
		CoinStart = new Vector2((float)Screen.width * 0.5f, (float)Screen.height * 0.45f);
		CoinEnd = new Vector2((float)Screen.width * 0.5f, (float)Screen.height * 0.75f);
	}

	public virtual void Awake()
	{
		inst = this;
	}

	public static void InitGame()
	{
		ShowStartPage = true;
		LevelLoader.inst.StartCoroutine(LevelLoader.inst.LoadLevel(2));
	}

	public virtual void Start()
	{
		Skin = FontResize.ResizedSkin;
		Cart.currentScore = 0f;
		CartCam.inst.SetHud(false);
		StartViewMode();
		ShowStartPage = false;
		Tracker2.instance.Log("Game Round Started");
	}

	public virtual void StartViewMode()
	{
		ButtonCon.SetButtonState(true);
		page = MenuPages.ViewMode;
		CartCam.inst.SetCameraMode(CameraMode.ScenicView);
		enabled = false;
	}

	public virtual IEnumerator FinishLevel(Cart cart)
	{
		return new _0024FinishLevel_0024153(cart, this).GetEnumerator();
	}

	public virtual Rect fluidRect(float x, float y, float width, float height)
	{
		return new Rect(x * (float)Screen.width, y * (float)Screen.height, width * (float)Screen.width, height * (float)Screen.height);
	}

	public virtual IEnumerator StartGame()
	{
		return new _0024StartGame_0024159(this).GetEnumerator();
	}

	public virtual void ShowUpgradeCredits()
	{
		enabled = true;
		if (!(Cart.currentScore < CartCam.inst.Cart.FullMarmalizeScore))
		{
			AchievementManager.AddProgressToAchievement("Mr. Marmaliser", 1f);
		}
		SumAnProg = 0f;
		page = MenuPages.UpgradePoints;
		AchievementManager.SetProgressToAchievement("Happy Horder", TrackUpgrade.GetCredits());
	}

	public virtual IEnumerator ShowUpgradeShop()
	{
		return new _0024ShowUpgradeShop_0024162(this).GetEnumerator();
	}

	public virtual void RestartGame()
	{
		Application.LoadLevel(Application.loadedLevel);
	}

	private Rect LerpRect(Rect StartRect, Rect EndRect, float lerp)
	{
		lerp = Mathf.Clamp01(lerp);
		return new Rect(Mathf.Lerp(StartRect.x, EndRect.x, lerp), Mathf.Lerp(StartRect.y, EndRect.y, lerp), Mathf.Lerp(StartRect.width, EndRect.width, lerp), Mathf.Lerp(StartRect.height, EndRect.height, lerp));
	}

	private float floatForTimePeriod(float start, float End)
	{
		return (Mathf.Clamp(SumAnProg, start, End) - start) / (End - start);
	}

	public virtual void OnGUI()
	{
		GUI.skin = Skin;
		if (page == MenuPages.Summery)
		{
			float num = floatForTimePeriod(1f, 5f);
			float lerp = floatForTimePeriod(5f, 6.5f);
			int value = (int)(Cart.currentScore / CartCam.inst.Cart.FullMarmalizeScore * 16f);
			value = Mathf.Clamp(value, 0, 16);
			Rect rect = LerpRect(FacesAreaBig, FacesAreaSmall, lerp);
			Rect position = new Rect(rect);
			position.y += rect.height * 1.1f;
			position.height *= 0.25f;
			GUI.Label(position, Mathf.Min(Mathf.Floor(Cart.currentScore / CartCam.inst.Cart.FullMarmalizeScore * 100f * num), 100f) + "% Smiling Advocates", "YellowText");
			if (!(num < 1f) && !(Cart.currentScore <= CartCam.inst.Cart.FullMarmalizeScore))
			{
				GUI.DrawTexture(rect, FullMarTex, ScaleMode.ScaleToFit, true);
			}
			else
			{
				for (int i = 0; i < 16; i++)
				{
					float num2 = Mathf.Clamp01((num - 0.0625f * (float)i) * 16f);
					int num3 = i % 4;
					int num4 = i / 4;
					if (Mathf.Approximately(num2, 0f) || value < i + 1)
					{
						GUI.DrawTexture(new Rect(rect.x + (float)num3 * (rect.width / 4f), rect.y + (float)num4 * (rect.height / 4f), rect.width / 4f, rect.height / 4f), FaceBlank, ScaleMode.StretchToFill, true);
					}
					else if (!(num2 <= 0.5f))
					{
						num2 = (num2 - 0.5f) * 2f;
						GUI.DrawTexture(new Rect(rect.x + (float)num3 * (rect.width / 4f), rect.y + (float)num4 * (rect.height / 4f) + rect.height / 4f * (1f - num2) * 0.5f, rect.width / 4f, rect.height / 4f * num2), FaceMar, ScaleMode.StretchToFill, true);
					}
					else
					{
						num2 *= 2f;
						GUI.DrawTexture(new Rect(rect.x + (float)num3 * (rect.width / 4f), rect.y + (float)num4 * (rect.height / 4f) + rect.height / 4f * (1f - num2) * 0.5f, rect.width / 4f, rect.height / 4f * num2), FaceBlank, ScaleMode.StretchToFill, true);
					}
				}
			}
			if (!(SumAnProg <= 6.5f))
			{
				float t = floatForTimePeriod(6.5f, 9f);
				int num5 = (int)Mathf.Floor(Mathf.Lerp(0f, Cart.currentScore, t));
				if (CurrentTopScore < num5)
				{
					float num6 = 0.8f + Mathf.PingPong(Time.time, 0.4f);
					GUI.Label(Global.AspectfluidRect(0.5f, 0.64f, 0.3f, 0.15f, 1.5f), string.Empty + num5, "WhiteTitle");
					GUI.Label(Global.AspectfluidRect(0.7f, 0.7f, 0.1f, 0.11f, 1.5f), string.Empty, "PBicon");
				}
				else
				{
					GUI.Label(Global.AspectfluidRect(0.5f, 0.64f, 0.3f, 0.15f, 1.5f), string.Empty + CurrentTopScore, "WhiteTitle");
				}
				GUI.Label(Global.AspectfluidRect(0.5f, 0.36f, 0.3f, 0.15f, 1.5f), "Score", "YellowText");
				GUI.Label(Global.AspectfluidRect(0.5f, 0.56f, 0.3f, 0.15f, 1.5f), "Personal Best", "YellowText");
				GUI.Label(Global.AspectfluidRect(0.5f, 0.46f, 0.3f, 0.15f, 1.5f), string.Empty + num5, "WhiteTitle");
			}
			SumAnProg += Time.deltaTime;
			if (GUI.Button(Global.AspectfluidRect(0.79f, 0.38f, 0.15f, 0.17f, 1.5f), string.Empty, "FacebookButton"))
			{
				Tracker2.instance.Log("Facebook Share");
				string shareString = "I’m playing The Smiler game by Alton Towers and scored " + UnityBuiltins.parseInt(Cart.currentScore) + ". See if you can beat my score – download the game today";
				string iconURL = "http://www.the-smiler.com/site/static/images/The_Smiler_Official_Logo.jpg";
				string facebookAppLink = "http://www.the-smiler.com/the-game/";
				string linkName = "The Smiler";
				string iconCaption = "The Smiler Logo";
				SharingControl.inst.StartCoroutine(SharingControl.inst.FacebookSharePost(shareString, facebookAppLink, linkName, iconURL, iconCaption));
			}
			if (GUI.Button(Global.AspectfluidRect(0.79f, 0.58f, 0.15f, 0.17f, 1.5f), string.Empty, "TwitterButton"))
			{
				string shareString2 = "I’m playing The Smiler game by Alton Towers & scored " + UnityBuiltins.parseInt(Cart.currentScore) + ". Beat my score-get the game http://www.the-smiler.com/the-game #thesmilergame";
				Tracker2.instance.Log("Twitter Share");
				SharingControl.inst.StartCoroutine(SharingControl.inst.TwitterSharePost(shareString2));
			}
			if (GUI.Button(Global.AspectfluidRect(0.4f, 0.85f, 0.2f, 0.15f, 1.5f), "Continue", "StandardButton"))
			{
				ShowUpgradeCredits();
			}
		}
		if (page != MenuPages.UpgradePoints)
		{
			return;
		}
		float num7 = floatForTimePeriod(0f, 3f);
		float num8 = floatForTimePeriod(6f, 9f);
		GUI.Label(Global.AspectfluidRect(0.3f, 0.35f, 0.4f, 0.1f, 1.5f), "You Earned", "YellowText");
		GUI.Label(Global.AspectfluidRect(0.3f, 0.55f, 0.4f, 0.15f, 1.5f), "Upgrade Points", "YellowText");
		GUI.Label(Global.AspectfluidRect(0.3f, 0.65f, 0.4f, 0.15f, 1.5f), "Total Points", "YellowText");
		if (!(num7 >= 1f))
		{
			GUI.Label(Global.AspectfluidRect(0.3f, 0.45f, 0.4f, 0.15f, 1.5f), string.Empty + Mathf.Floor(Mathf.Lerp(0f, UpgradeCreditsMade, num7)), "WhiteTitle");
			GUI.Label(Global.AspectfluidRect(0.3f, 0.75f, 0.4f, 0.15f, 1.5f), string.Empty + TrackUpgrade.UpgradeCredits, "WhiteTitle");
		}
		else
		{
			if (!(num8 >= 1f) && !(num8 <= 0f))
			{
				float t2 = Mathf.Repeat(num8, 1f / (float)UpgradeCreditsMade) * (float)UpgradeCreditsMade;
				Vector2 vector = Vector2.Lerp(CoinStart, CoinEnd, t2);
				float num9 = (float)Screen.width * 0.05f;
				GUI.DrawTexture(new Rect(vector.x - num9 * 0.5f, vector.y + num9 * 0.5f, num9, num9), Coin, ScaleMode.StretchToFill, true);
			}
			GUI.Label(Global.AspectfluidRect(0.3f, 0.45f, 0.4f, 0.15f, 1.5f), string.Empty + UpgradeCreditsMade, "WhiteTitle");
			GUI.Label(Global.AspectfluidRect(0.3f, 0.75f, 0.4f, 0.15f, 1.5f), string.Empty + Mathf.Floor(Mathf.Lerp(TrackUpgrade.UpgradeCredits, TrackUpgrade.UpgradeCredits + UpgradeCreditsMade, num8)), "WhiteTitle");
		}
		SumAnProg += Time.deltaTime;
		if (GUI.Button(Global.AspectfluidRect(0.4f, 0.85f, 0.2f, 0.15f, 1.5f), "Continue", "StandardButton"))
		{
			StartCoroutine(ShowUpgradeShop());
		}
	}

	public virtual void SkipWait()
	{
		ButtonCon.StartCoroutine(ButtonCon.SkipWait());
	}

	public virtual void Main()
	{
	}
}
