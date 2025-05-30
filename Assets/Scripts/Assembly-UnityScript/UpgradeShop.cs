using System;
using System.Collections;
using System.Collections.Generic;
using Boo.Lang;
using UnityEngine;
using UnityScript.Lang;

[Serializable]
public class UpgradeShop : MonoBehaviour
{
	[Serializable]
	internal sealed class _0024ConfirmBuyUpgrade_0024209 : GenericGenerator<WaitForSeconds>
	{
		[Serializable]
		internal sealed class _0024 : GenericGeneratorEnumerator<WaitForSeconds>, IEnumerator
		{
			internal string _0024Name_0024210;

			internal UpgradeShop _0024self__0024211;

			public _0024(string Name, UpgradeShop self_)
			{
				_0024Name_0024210 = Name;
				_0024self__0024211 = self_;
			}

			public override bool MoveNext()
			{
				int result;
				switch (_state)
				{
				default:
					CartCam.inst.RandomView();
					CartCam.inst.SetCameraRendering(true);
					_0024self__0024211.UnlockBuy = TrackUpgrade.UpgradeID(_0024Name_0024210);
					if (!TrackUpgrade.CanAfford(_0024Name_0024210))
					{
						perspectiveHud.inst.ShowPopUpWindow("Buy Error", true);
					}
					else
					{
						perspectiveHud.inst.ShowPopUpWindow("Buy Upgrade", false);
					}
					_0024self__0024211.enabled = false;
					result = (Yield(2, new WaitForSeconds(1f)) ? 1 : 0);
					break;
				case 2:
					_0024self__0024211.enabled = true;
					_0024self__0024211.BuyConfirm = true;
					YieldDefault(1);
					goto case 1;
				case 1:
					result = 0;
					break;
				}
				return (byte)result != 0;
			}
		}

		internal string _0024Name_0024212;

		internal UpgradeShop _0024self__0024213;

		public _0024ConfirmBuyUpgrade_0024209(string Name, UpgradeShop self_)
		{
			_0024Name_0024212 = Name;
			_0024self__0024213 = self_;
		}

		public override IEnumerator<WaitForSeconds> GetEnumerator()
		{
			return new _0024(_0024Name_0024212, _0024self__0024213);
		}
	}

	[Serializable]
	internal sealed class _0024BuyClicked_0024214 : GenericGenerator<WaitForSeconds>
	{
		[Serializable]
		internal sealed class _0024 : GenericGeneratorEnumerator<WaitForSeconds>, IEnumerator
		{
			internal UpgradeShop _0024self__0024215;

			public _0024(UpgradeShop self_)
			{
				_0024self__0024215 = self_;
			}

			public override bool MoveNext()
			{
				int result;
				switch (_state)
				{
				default:
					_0024self__0024215.BuyConfirm = false;
					_0024self__0024215.enabled = false;
					perspectiveHud.inst.StartCoroutine(perspectiveHud.inst.HidePopUpWindow());
					result = (Yield(2, new WaitForSeconds(0.5f)) ? 1 : 0);
					break;
				case 2:
					_0024self__0024215.enabled = true;
					_0024self__0024215.BuyUpgrade();
					YieldDefault(1);
					goto case 1;
				case 1:
					result = 0;
					break;
				}
				return (byte)result != 0;
			}
		}

		internal UpgradeShop _0024self__0024216;

		public _0024BuyClicked_0024214(UpgradeShop self_)
		{
			_0024self__0024216 = self_;
		}

		public override IEnumerator<WaitForSeconds> GetEnumerator()
		{
			return new _0024(_0024self__0024216);
		}
	}

	[Serializable]
	internal sealed class _0024CancelClicked_0024217 : GenericGenerator<WaitForSeconds>
	{
		[Serializable]
		internal sealed class _0024 : GenericGeneratorEnumerator<WaitForSeconds>, IEnumerator
		{
			internal UpgradeShop _0024self__0024218;

			public _0024(UpgradeShop self_)
			{
				_0024self__0024218 = self_;
			}

			public override bool MoveNext()
			{
				int result;
				switch (_state)
				{
				default:
					_0024self__0024218.BuyConfirm = false;
					_0024self__0024218.enabled = false;
					perspectiveHud.inst.StartCoroutine(perspectiveHud.inst.HidePopUpWindow());
					result = (Yield(2, new WaitForSeconds(0.5f)) ? 1 : 0);
					break;
				case 2:
					_0024self__0024218.enabled = true;
					YieldDefault(1);
					goto case 1;
				case 1:
					result = 0;
					break;
				}
				return (byte)result != 0;
			}
		}

		internal UpgradeShop _0024self__0024219;

		public _0024CancelClicked_0024217(UpgradeShop self_)
		{
			_0024self__0024219 = self_;
		}

		public override IEnumerator<WaitForSeconds> GetEnumerator()
		{
			return new _0024(_0024self__0024219);
		}
	}

	[NonSerialized]
	public static UpgradeShop inst;

	public Texture2D HiddenUpgrade;

	public Texture2D BuyableBackground;

	public Texture2D MaxLevelBackground;

	public Texture2D BottemBackground;

	public Texture2D LeftButton;

	public Texture2D MidButton;

	public Texture2D RightButton;

	public Texture2D LeftBottem;

	public Texture2D RightBottem;

	public Texture2D SwipeArrow;

	public Texture2D creditIcon;

	private float[] UnlockedPosition;

	private float[] SecretUnlockedPosition;

	private float[] PageScroll;

	private GUISkin skin;

	private ShopPages ShopMode;

	private UpgradeClass[] ShopPageList;

	private float ScrollSpeed;

	private float ScrollStart;

	private bool ScollableClick;

	private int UnlockBuy;

	private bool BuyConfirm;

	private float UpgradeWidth;

	private float UpgradeHeight;

	private bool DevSkipCountDown;

	public UpgradeShop()
	{
		PageScroll = new float[3];
		ShopMode = ShopPages.LegShop;
		ScollableClick = true;
		UnlockBuy = -1;
		UpgradeWidth = 0.4f;
		UpgradeHeight = 0.8f;
	}

	public virtual void Awake()
	{
		inst = this;
		enabled = false;
	}

	public virtual void Start()
	{
		skin = FontResize.ResizedSkin;
	}

	public virtual void StartShop()
	{
		UnlockedPosition = new float[TrackUpgrade.TotalUpgrades()];
		SecretUnlockedPosition = new float[TrackUpgrade.TotalUpgrades()];
		ChangePage(ShopPages.TrackShop);
		enabled = true;
		CartCam.inst.enabled = false;
		GameControl.inst.enabled = false;
		ViewTrack.ResetPrioritysList();
		CartCam.inst.SetCameraRendering(false);
		CartCam.inst.RandomView();
	}

	public virtual void EndShop()
	{
		GameControl.inst.StartCoroutine(GameControl.inst.StartGame());
	}

	public virtual Rect fluidRect(float x, float y, float width, float height)
	{
		return new Rect(x * (float)Screen.width, y * (float)Screen.height, width * (float)Screen.width, height * (float)Screen.height);
	}

	public virtual Vector2 GetAverageTouch()
	{
		Vector2 vector = default(Vector2);
		for (int i = 0; i < Extensions.get_length((System.Array)Input.touches); i++)
		{
			if (Input.touches[i].fingerId < 5)
			{
				vector += Input.touches[i].position;
			}
		}
		return vector / Input.touchCount;
	}

	public virtual void ChangePage(ShopPages SP)
	{
		if (ShopMode != SP)
		{
			ShopMode = SP;
			ShopPageList = TrackUpgrade.GetShopPage(SP);
			ScrollSpeed = 0f;
		}
		if (ShopMode == ShopPages.TopSecret)
		{
			CartCam.inst.RandomView();
		}
	}

	public virtual void Update()
	{
		if (Input.touchCount > 0)
		{
			Vector2 averageTouch = GetAverageTouch();
			if (!(averageTouch.y <= (float)Screen.height * 0.2f) && !(averageTouch.y >= 0.8f * (float)Screen.height))
			{
				if (Input.touchCount == 1 && Input.touches[0].phase == TouchPhase.Began)
				{
					ScrollStart = averageTouch.x;
					ScollableClick = true;
				}
				if (Input.touchCount == 1 && Input.touches[0].phase != TouchPhase.Ended)
				{
					ScrollSpeed = (averageTouch.x - ScrollStart) / (float)Screen.width;
				}
				ScrollStart = averageTouch.x;
			}
		}
		for (int i = 0; i < Extensions.get_length((System.Array)ShopPageList); i++)
		{
			if (ShopMode == ShopPages.TopSecret)
			{
				if (TrackUpgrade.IsUnlocked(ShopPageList[i].getID()))
				{
					SecretUnlockedPosition[ShopPageList[i].getID()] = Mathf.Lerp(SecretUnlockedPosition[ShopPageList[i].getID()], 1f, Time.deltaTime * 5f);
				}
				else
				{
					SecretUnlockedPosition[ShopPageList[i].getID()] = Mathf.Lerp(SecretUnlockedPosition[ShopPageList[i].getID()], 0f, Time.deltaTime * 5f);
				}
			}
			else
			{
				UnlockedPosition[ShopPageList[i].getID()] = Mathf.Lerp(UnlockedPosition[ShopPageList[i].getID()], ShopPageList[i].currentLevel, Time.deltaTime * 5f);
			}
		}
		PageScroll[(int)ShopMode] = PageScroll[(int)ShopMode] - ScrollSpeed;
		PageScroll[(int)ShopMode] = ClampScroll(PageScroll[(int)ShopMode], 0.4f * (float)TrackUpgrade.ShopPageCount(ShopMode));
		ScrollSpeed += Mathf.Clamp(0f - ScrollSpeed, -0.4f, 0.4f) * Time.deltaTime;
		if (!(Mathf.Abs(ScrollSpeed) <= 0.03f))
		{
			ScollableClick = false;
		}
		ScrollSpeed -= Time.deltaTime * ScrollSpeed;
	}

	public virtual float ClampScroll(float scroll, float ScrollDist)
	{
		return Mathf.Clamp(scroll, 0f, Mathf.Max(0f, ScrollDist - 1f));
	}

	public virtual IEnumerator ConfirmBuyUpgrade(string Name)
	{
		return new _0024ConfirmBuyUpgrade_0024209(Name, this).GetEnumerator();
	}

	public virtual void BuyUpgrade()
	{
		Tracker2.instance.LogWithParameters("Upgraded Ride", TrackUpgrade.getDisplayName(UnlockBuy));
		TrackUpgrade.Upgrade(UnlockBuy);
		ViewTrack.AddPriorityView(TrackUpgrade.getName(UnlockBuy));
		if (TrackUpgrade.isUpgradeType(UnlockBuy, ShopPages.TrackShop))
		{
			AchievementManager.AddProgressToAchievement("Junior Mechanic", 1f);
			if (TrackUpgrade.getCurrentLevel(UnlockBuy) == 1)
			{
				AchievementManager.AddProgressToAchievement("Pro Mechanic", 1f);
			}
		}
		if (TrackUpgrade.isUpgradeType(UnlockBuy, ShopPages.LegShop))
		{
			AchievementManager.AddProgressToAchievement("Pimp my Ride", 1f);
			if (TrackUpgrade.isUpgradeType(UnlockBuy, ShopPages.LegShop) && TrackUpgrade.getCurrentLevel(UnlockBuy) == 3)
			{
				AchievementManager.AddProgressToAchievement("Fully Pimped", 1f);
			}
		}
	}

	public virtual void RenderShopItems()
	{
		CartCam.inst.SetCameraRendering(false);
		float num = default(float);
		num = PageScroll[(int)ShopMode];
		for (int i = 0; i < Extensions.get_length((System.Array)ShopPageList); i++)
		{
			int id = TrackUpgrade.UpgradeID(ShopPageList[i].Name);
			int num2 = ((ShopMode != ShopPages.TopSecret) ? TrackUpgrade.MaxLevel(id) : 2);
			for (int j = 0; j <= num2; j++)
			{
				float num3 = ((ShopMode != ShopPages.TopSecret) ? UnlockedPosition[ShopPageList[i].getID()] : SecretUnlockedPosition[ShopPageList[i].getID()]);
				if (Mathf.Abs(num3 - (float)j) >= 0.99f)
				{
					continue;
				}
				Rect source = fluidRect((float)i * UpgradeWidth - PageScroll[(int)ShopMode], (1f - UpgradeHeight) * 0.5f + num3 * UpgradeHeight - (float)j * UpgradeHeight, UpgradeWidth, UpgradeHeight);
				Rect position = new Rect(source);
				Rect position2 = new Rect(source);
				Rect position3 = new Rect(source);
				position3.height *= 0.4f;
				position3.y += source.height * 0.3f;
				position3.x += position3.width * 0.15f;
				position3.width *= 0.7f;
				position2.x -= position2.width * 0.05f;
				position2.width *= 1.1f;
				position.height *= 0.3f;
				Rect position4 = new Rect(source);
				position4.y += position4.height * 0.8f;
				position4.height *= 0.4f;
				position4.height *= 0.2f;
				Rect position5 = new Rect(source);
				position5.y += position5.height * 0.8f;
				position5.x += source.width * 0.07f;
				position5.width *= 0.35f;
				position5.height *= 0.1f;
				Rect position6 = new Rect(source);
				position6.y += source.height * 0.8f;
				position6.width *= 0.15f;
				position6.height *= 0.1f;
				Rect position7 = new Rect(source);
				position7.y += position7.height * 0.8f;
				position7.width *= 0.4f;
				position7.x += position7.width;
				position7.height *= 0.1f;
				if (ShopMode == ShopPages.TopSecret)
				{
					if (TrackUpgrade.IsUnlocked(ShopPageList[i].getID()))
					{
						GUI.DrawTexture(position2, MaxLevelBackground, ScaleMode.StretchToFill, true);
						GUI.color = Color.black;
						if (TrackUpgrade.MaxLevel(ShopPageList[i].getID()) > 1)
						{
							GUI.Label(position, ShopPageList[i].DisplayName + " Lv" + TrackUpgrade.MaxLevel(ShopPageList[i].getID()), "WhiteLabel");
						}
						else
						{
							GUI.Label(position, ShopPageList[i].DisplayName, "WhiteLabel");
						}
						GUI.DrawTexture(position3, ShopPageList[i].ShopImage[TrackUpgrade.MaxLevel(ShopPageList[i].getID())], ScaleMode.ScaleToFit);
						GUI.Label(position4, "UNLOCKED", "WhiteLabel");
					}
					else
					{
						GUI.DrawTexture(position2, BuyableBackground, ScaleMode.StretchToFill, true);
						GUI.Label(position4, "Visit and scan to unlock", "WhiteLabel");
						GUI.DrawTexture(position3, HiddenUpgrade, ScaleMode.ScaleToFit);
					}
				}
				else
				{
					if (TrackUpgrade.MaxLevel(ShopPageList[i].getID()) == j)
					{
						GUI.DrawTexture(position2, MaxLevelBackground, ScaleMode.StretchToFill, true);
						GUI.color = Color.black;
					}
					else
					{
						GUI.DrawTexture(position2, BuyableBackground, ScaleMode.StretchToFill, true);
					}
					if (TrackUpgrade.MaxLevel(ShopPageList[i].getID()) > 1)
					{
						if (TrackUpgrade.IsMaxLevel(ShopPageList[i].getID()))
						{
							GUI.Label(position, ShopPageList[i].DisplayName + " Lv" + TrackUpgrade.UpgradeCurrentLevel(ShopPageList[i].Name), "WhiteLabel");
						}
						else
						{
							GUI.Label(position, ShopPageList[i].DisplayName + " Lv" + (TrackUpgrade.UpgradeCurrentLevel(ShopPageList[i].Name) + 1), "WhiteLabel");
						}
					}
					else
					{
						GUI.Label(position, ShopPageList[i].DisplayName, "WhiteLabel");
					}
					GUI.DrawTexture(position3, ShopPageList[i].ShopImage[j], ScaleMode.ScaleToFit);
					if (TrackUpgrade.IsMaxLevel(ShopPageList[i].getID()))
					{
						GUI.Label(position4, "FULLY\nUPGRADED", "WhiteLabel");
					}
					else
					{
						GUI.Label(position5, string.Empty + TrackUpgrade.UpgradeCost(TrackUpgrade.UpgradeID(ShopPageList[i].Name)), "WhiteLabel");
						GUI.Label(position6, string.Empty, "CreditIcon");
						if (GUI.Button(position7, "BUY", "StandardButton") && ScollableClick)
						{
							StartCoroutine(ConfirmBuyUpgrade(ShopPageList[i].Name));
						}
					}
				}
				GUI.color = Color.white;
			}
		}
	}

	public virtual IEnumerator BuyClicked()
	{
		return new _0024BuyClicked_0024214(this).GetEnumerator();
	}

	public virtual IEnumerator CancelClicked()
	{
		return new _0024CancelClicked_0024217(this).GetEnumerator();
	}

	public virtual void OnGUI()
	{
		GUI.skin = skin;
		if (BuyConfirm)
		{
			CartCam.inst.SetCameraRendering(true);
			if (TrackUpgrade.UpgradeCredits >= TrackUpgrade.UpgradeCost(UnlockBuy))
			{
				GUI.Label(Global.AspectfluidRect(0.2f, 0.3f, 0.6f, 0.3f, 1.5f), string.Empty + TrackUpgrade.GetDisplayName(UnlockBuy), "WhiteTitle");
				GUI.Label(Global.AspectfluidRect(0.2f, 0.45f, 0.6f, 0.3f, 1.5f), string.Empty + TrackUpgrade.getDescription(UnlockBuy), "YellowText");
				GUI.DrawTexture(Global.AspectfluidRect(0.3f, 0.7f, 0.1f, 0.1f, 1.5f), creditIcon, ScaleMode.ScaleToFit);
				GUI.Label(Global.AspectfluidRect(0.2f, 0.7f, 0.6f, 0.1f, 1.5f), string.Empty + TrackUpgrade.UpgradeCost(UnlockBuy), "WhiteLabel");
				if (GUI.Button(Global.AspectfluidRect(0.2f, 0.8f, 0.2f, 0.15f, 1.5f), "Cancel", "StandardButton"))
				{
					StartCoroutine(CancelClicked());
				}
				if (GUI.Button(Global.AspectfluidRect(0.5f, 0.825f, 0.3f, 0.175f, 1.5f), "Buy", "ContinueButton"))
				{
					StartCoroutine(BuyClicked());
				}
			}
			else
			{
				GUI.Label(Global.AspectfluidRect(0.2f, 0.3f, 0.6f, 0.3f, 1.5f), "You don't have enough credits for " + TrackUpgrade.GetDisplayName(UnlockBuy), "WhiteLabel");
				GUI.Label(Global.AspectfluidRect(0.2f, 0.5f, 0.6f, 0.3f, 1.5f), "Ride The Smiler again to earn more credits", "YellowText");
				if (GUI.Button(Global.AspectfluidRect(0.4f, 0.8f, 0.2f, 0.15f, 1.5f), "Continue", "StandardButton"))
				{
					StartCoroutine(CancelClicked());
				}
			}
			return;
		}
		if (ShopMode == ShopPages.TrackShop)
		{
			RenderShopItems();
		}
		if (ShopMode == ShopPages.LegShop)
		{
			RenderShopItems();
		}
		if (ShopMode == ShopPages.TopSecret)
		{
			int unixTime = UnixTime.GetUnixTime(new DateTime(2013, 3, 16, 11, 0, 0));
			int unixTime2 = UnixTime.GetUnixTime();
			int num = unixTime - unixTime2;
			RenderShopItems();
		}
		if (ShopMode == ShopPages.TopSecret)
		{
			GUI.DrawTexture(fluidRect(0f, 0f, 1f, 0.15f), RightButton, ScaleMode.StretchToFill, true);
		}
		if (ShopMode == ShopPages.LegShop)
		{
			GUI.DrawTexture(fluidRect(0f, 0f, 1f, 0.15f), MidButton, ScaleMode.StretchToFill, true);
		}
		if (ShopMode == ShopPages.TrackShop)
		{
			GUI.DrawTexture(fluidRect(0f, 0f, 1f, 0.15f), LeftButton, ScaleMode.StretchToFill, true);
		}
		GUI.DrawTexture(fluidRect(0f, 0.85f, 1f, 0.15f), BottemBackground, ScaleMode.StretchToFill, true);
		if (ShopMode == ShopPages.TrackShop)
		{
			GUI.color = Color.black;
		}
		else
		{
			GUI.color = Color.yellow;
		}
		GUI.Label(fluidRect(0.05f, 0f, 0.07f, 0.1f), string.Empty, "LoopIcon");
		if (GUI.Button(fluidRect(0.1f, 0f, 0.2333f, 0.1f), "LOOPS", "TextOnlyButton"))
		{
			ChangePage(ShopPages.TrackShop);
		}
		if (ShopMode == ShopPages.LegShop)
		{
			GUI.color = Color.black;
		}
		else
		{
			GUI.color = Color.yellow;
		}
		GUI.Label(fluidRect(0.39f, 0f, 0.07f, 0.1f), string.Empty, "MarIcon");
		if (GUI.Button(fluidRect(0.46f, 0f, 0.25f, 0.1f), "MARMALISER", "TextOnlyButton"))
		{
			ChangePage(ShopPages.LegShop);
		}
		if (ShopMode == ShopPages.TopSecret)
		{
			GUI.color = Color.black;
		}
		else
		{
			GUI.color = Color.yellow;
		}
		GUI.Label(fluidRect(0.74f, 0f, 0.07f, 0.1f), string.Empty, "SecretIcon");
		if (GUI.Button(fluidRect(0.78f, 0f, 0.22f, 0.1f), "TOP\nSECRET", "TextOnlyButton"))
		{
			ChangePage(ShopPages.TopSecret);
		}
		GUI.color = Color.white;
		GUI.DrawTexture(fluidRect(0f, 0.855f, 0.265f, 0.145f), LeftBottem, ScaleMode.StretchToFill, true);
		GUI.DrawTexture(fluidRect(0.635f, 0.86f, 0.36f, 0.145f), RightBottem, ScaleMode.StretchToFill, true);
		if (GUI.Button(fluidRect(0.333f, 0.9f, 0.2f, 0.1f), "Main Menu", "StandardButton"))
		{
			LevelLoader.inst.StartCoroutine(LevelLoader.inst.LoadLevel(0));
		}
		GUI.color = Color.black;
		if (GUI.Button(fluidRect(0.7f, 0.9f, 0.2f, 0.1f), "Continue", "BlackInvisButton"))
		{
			Application.LoadLevel(2);
		}
		GUI.Label(fluidRect(0.05f, 0.9f, 0.15f, 0.1f), string.Empty + TrackUpgrade.UpgradeCredits, "BlackLabel");
		GUI.color = Color.white;
		GUI.DrawTexture(fluidRect(0.05f, 0.91f, 0.05f, 0.065f), creditIcon, ScaleMode.ScaleToFit);
	}

	public virtual void Main()
	{
	}
}
