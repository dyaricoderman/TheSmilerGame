using System;
using UnityEngine;

[Serializable]
public class Performance : MonoBehaviour
{
	[NonSerialized]
	public static int GamePerformance = 2;

	public virtual void CheckPhonePerformance()
	{
		int graphicsPixelFillrate = SystemInfo.graphicsPixelFillrate;
		int graphicsMemorySize = SystemInfo.graphicsMemorySize;
		int systemMemorySize = SystemInfo.systemMemorySize;
		if (systemMemorySize < 520)
		{
			GamePerformance = 1;
		}
		if (systemMemorySize < 300)
		{
			GamePerformance = 0;
		}
		MonoBehaviour.print("Performance level " + GamePerformance + " detected");
		if (PlayerPrefs.HasKey("DevicePerformance"))
		{
			GamePerformance = PlayerPrefs.GetInt("DevicePerformance");
			MonoBehaviour.print("Performance level " + GamePerformance + " Loaded");
		}
		PlayerPrefs.SetInt("DevicePerformance", GamePerformance);
	}

	public virtual void ApplyQualitySettings()
	{
		QualitySettings.SetQualityLevel(GamePerformance, true);
		if (GamePerformance == 2)
		{
			Application.targetFrameRate = 120;
		}
		if (GamePerformance == 1)
        {
			Application.targetFrameRate = 60;
        }
		else
        {
			Application.targetFrameRate = 30;
        }
	}

	public virtual void Awake()
	{
		CheckPhonePerformance();
		ApplyQualitySettings();
	}

	public virtual void Main()
	{
	}
}
