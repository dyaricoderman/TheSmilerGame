using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ARGUI : MonoBehaviour
{
	public enum MenuStates
	{
		scanning = 0,
		ARShowing = 1,
		foundNewScannable = 2,
		foundExistingScannable = 3,
		Tutoral = 4
	}

	private GUISkin guiSkin;

	public Texture2D BottemBar;

	public static ARGUI inst;

	public Texture2D[] LaserFrames;

	public Texture2D ScannerIcon;

	private float startTime;

	public Renderer[] ScannerVisuals;

	private MenuStates menuState;

	private TargetDef seenItem;

	private List<TargetDef> newItemQueue = new List<TargetDef>();

	public TrackablesManager trackablesManager;

	private int laserFrame;

	private float t;

	public void TutoralStart()
	{
		menuState = MenuStates.Tutoral;
	}

	public void TutoralEnded()
	{
		ReturnToScan();
	}

	private void Start()
	{
		guiSkin = FontResize.ResizedSkin;
		ScannerOverlay(true);
		startTime = Time.time;
		TutoralDisplay.inst.TriggerTutoralC(4, base.gameObject);
	}

	public void Awake()
	{
		inst = this;
	}

	public void ScannerOverlay(bool state)
	{
		for (int i = 0; i < ScannerVisuals.Length; i++)
		{
			ScannerVisuals[i].enabled = state;
		}
	}

	public void ScannedSeenItem(TargetDef TD)
	{
		ScannerOverlay(false);
		if (menuState != MenuStates.foundNewScannable)
		{
			seenItem = TD;
			menuState = MenuStates.foundExistingScannable;
		}
	}

	public void ScannedNewItem(TargetDef TD)
	{
		ScannerOverlay(false);
		if (TD.ShowPopup)
		{
			newItemQueue.Add(TD);
			StartCoroutine("ShowFoundNewItem");
		}
		GameObject.Find("Tracking").SendMessage("SentFromCLog", "AR Tag Scanned:" + TD.Name);
	}

	public IEnumerator ShowFoundNewItem()
	{
		while (TutoralDisplay.inst.TutoralOn)
		{
			yield return new WaitForSeconds(0.5f);
		}
		perspectiveHud.inst.ShowPopUpWindow("Found", false);
		yield return new WaitForSeconds(0.5f);
		menuState = MenuStates.foundNewScannable;
	}

	public void ItemLost()
	{
	}

	public void ItemsAllLost()
	{
		ScannerOverlay(true);
		if (menuState == MenuStates.foundExistingScannable)
		{
			menuState = MenuStates.scanning;
		}
	}

	private IEnumerator ExistingScannableTimeout()
	{
		yield return new WaitForSeconds(2f);
		if (menuState == MenuStates.foundExistingScannable)
		{
			ReturnToScan();
		}
	}

	private void ReturnToScan()
	{
		if (trackablesManager.trackablesInView > 0)
		{
			menuState = MenuStates.ARShowing;
		}
		else
		{
			menuState = MenuStates.scanning;
		}
	}

	private void OnGUI()
	{
		GUI.skin = guiSkin;
		laserFrame++;
		laserFrame = (int)Mathf.Repeat(laserFrame, LaserFrames.Length);
		if (menuState == MenuStates.scanning || menuState == MenuStates.ARShowing || menuState == MenuStates.foundExistingScannable)
		{
			GUI.DrawTexture(fluidRect(0f, 0.67f, 1f, 0.2f), LaserFrames[laserFrame], ScaleMode.StretchToFill);
			GUI.DrawTexture(fluidRect(0f, 0.8f, 1f, 0.2f), BottemBar, ScaleMode.StretchToFill);
			GUI.DrawTexture(fluidRect(0.35f, 0.88f, 0.125f, 0.1f), ScannerIcon, ScaleMode.StretchToFill);
			if (GUI.Button(fluidRect(0f, 0.84f, 0.3f, 0.2f), "Exit", "BlackTextButton"))
			{
				LevelLoader.inst.LoadLevelC(0);
			}
			t = Mathf.Repeat(Time.time - startTime, 14f);
			if (t < 7f)
			{
				GUI.Label(fluidRect(0.48f, 0.85f, 0.5f, 0.15f), "Use your camera to scan and\nreveal upgrades and bonus content.", "YellowText");
			}
			else
			{
				GUI.Label(fluidRect(0.48f, 0.85f, 0.5f, 0.15f), "Access the latest scannable content by keeping this app up to date.", "YellowText");
			}
		}
		if (menuState == MenuStates.foundNewScannable)
		{
			GUI.Label(AspectfluidRect(0.15f, 0.68f, 0.7f, 0.1f, 1.5f), "You Found " + newItemQueue[0].Name + "\n" + newItemQueue[0].Discription, "YellowText");
			GUI.DrawTexture(AspectfluidRect(0.4f, 0.38f, 0.2f, 0.25f, 1.5f), newItemQueue[0].Icon, ScaleMode.ScaleToFit, true);
			if (GUI.Button(AspectfluidRect(0.4f, 0.85f, 0.2f, 0.12f, 1.5f), "Done", "StandardButton"))
			{
				newItemQueue.RemoveAt(0);
				if (newItemQueue.Count <= 0)
				{
					perspectiveHud.inst.HidePopUpWindowC();
					if (trackablesManager.trackablesInView > 0)
					{
						menuState = MenuStates.ARShowing;
					}
					else
					{
						menuState = MenuStates.scanning;
					}
				}
			}
		}
		if (menuState != MenuStates.foundExistingScannable)
		{
		}
	}

	private Rect fluidRect(float x, float y, float width, float height)
	{
		return new Rect(x * (float)Screen.width, y * (float)Screen.height, width * (float)Screen.width, height * (float)Screen.height);
	}

	private Rect AspectfluidRect(float x, float y, float width, float height, float Aspect)
	{
		int num = Mathf.FloorToInt(Aspect * (float)Screen.height);
		return new Rect(x * (float)num - (float)(num - Screen.width) * 0.5f, y * (float)Screen.height, width * (float)num, height * (float)Screen.height);
	}
}
