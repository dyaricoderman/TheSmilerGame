using UnityEngine;

public class TrackablesManager : MonoBehaviour
{
	public static int TargetCount;

	public static bool[] hasScanned;

	public int trackablesInView;

	public ImageTracker imageTracker;

	public ARGUI ARGUI;

	public GameObject Scanner;

	private void Start()
	{
		trackablesInView = TargetCount;
		if (PlayerPrefs.HasKey("scannedTrackables"))
		{
			hasScanned = PlayerPrefsX.GetBoolArray("scannedTrackables");
			if (hasScanned.Length != TargetCount)
			{
				hasScanned = new bool[TargetCount];
			}
		}
		else
		{
			hasScanned = new bool[TargetCount];
			PlayerPrefsX.SetBoolArray("scannedTrackables", hasScanned);
		}
	}

	public void ItemLost(int ID)
	{
	}

	public void FoundTrackable(TargetDef targ)
	{
		trackablesInView++;
		if (hasScanned[targ.ID])
		{
			ARGUI.ScannedSeenItem(targ);
			return;
		}
		AchievementManager.AddProgressToAchievement("Detective", 1f);
		ARGUI.ScannedNewItem(targ);
		DecidePrize(targ);
		hasScanned[targ.ID] = true;
		PlayerPrefsX.SetBoolArray("scannedTrackables", hasScanned);
	}

	public void LostTrackable(TargetDef TD)
	{
		trackablesInView--;
		ARGUI.ItemLost();
		if (trackablesInView < 1)
		{
			ARGUI.ItemsAllLost();
		}
	}

	private void DecidePrize(TargetDef TD)
	{
		if (TD.Unlocks != string.Empty)
		{
			AchievementManager.AddProgressToAchievement("On Location", 1f);
			TrackUpgrade.UnlockUpgrade(TD.Unlocks);
		}
		if (TD.Credits > 0)
		{
			TrackUpgrade.AddCredits(TD.Credits);
		}
	}
}
