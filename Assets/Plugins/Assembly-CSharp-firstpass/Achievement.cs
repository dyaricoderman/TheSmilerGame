using System;
using UnityEngine;

[Serializable]
public class Achievement
{
	public string Name;

	public string DisplayName;

	public string Description;

	public Texture2D Icon;

	public float CurrentProgress;

	public float TargetProgress;

	public bool Earned;

	[HideInInspector]
	public bool AddProgress(float progress)
	{
		if (Earned)
		{
			return false;
		}
		CurrentProgress += progress;
		if (CurrentProgress >= TargetProgress)
		{
			Earned = true;
			return true;
		}
		return false;
	}

	public bool SetProgress(float progress)
	{
		if (Earned)
		{
			return false;
		}
		CurrentProgress = progress;
		if (progress >= TargetProgress)
		{
			Earned = true;
			return true;
		}
		return false;
	}
}
