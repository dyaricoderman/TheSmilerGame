using System;
using UnityEngine;

[Serializable]
public class AchievementInterfaceJS : MonoBehaviour
{
	public virtual void OnGUI()
	{
		if (GUI.Button(FluidRect(0.1f, 0.1f, 0.8f, 0.1f), "Spin the wheel!"))
		{
			AchievementManager.SetProgressToAchievement("Go for a Spin", 1f);
		}
		if (GUI.Button(FluidRect(0.1f, 0.2f, 0.8f, 0.1f), "Scan Something!"))
		{
			AchievementManager.AddProgressToAchievement("Detective", 1f);
		}
		if (GUI.Button(FluidRect(0.1f, 0.3f, 0.8f, 0.1f), "Boost"))
		{
			AchievementManager.AddProgressToAchievement("Flame On!", 1f);
			AchievementManager.AddProgressToAchievement("Danger Dan", 1f);
		}
		if (GUI.Button(FluidRect(0.1f, 0.8f, 0.8f, 0.1f), "Clear Data!"))
		{
			AchievementManager.ClearAchievementData();
		}
	}

	public virtual Rect FluidRect(float x, float y, float width, float height)
	{
		return new Rect(x * (float)Screen.width, y * (float)Screen.height, width * (float)Screen.width, height * (float)Screen.height);
	}

	public virtual void Main()
	{
	}
}
