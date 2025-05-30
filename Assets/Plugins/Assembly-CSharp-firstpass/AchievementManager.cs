using System.Linq;
using UnityEngine;

public class AchievementManager : MonoBehaviour
{
	public static AchievementManager AMInst;

	public Achievement[] Achievements;

	public AudioSource EarnedSound;

	public float[] CurrentProgress;

	public bool[] Earned;

	private Achievement unlockedAchievement;

	public GUISkin skin;

	public Texture2D PopBackground;

	private float popupCurveTime;

	private float yPos;

	public AnimationCurve popupCurve = new AnimationCurve(new Keyframe(0f, -0.2f), new Keyframe(2f, 0f), new Keyframe(3f, 0.025f), new Keyframe(4f, 0f), new Keyframe(6f, -0.2f));

	private void Awake()
	{
		if ((bool)AMInst)
		{
			Object.Destroy(base.gameObject);
			return;
		}
		Object.DontDestroyOnLoad(base.gameObject);
		AMInst = this;
		if (HasAchievementData())
		{
			GetAchievementData();
			return;
		}
		InitAchievementData();
		SetAchievementData();
	}

	public void Start()
	{
		base.enabled = false;
		base.gameObject.SendMessage("getResizedSkin", base.gameObject);
	}

	public static Achievement[] GetAchievements()
	{
		return AMInst.Achievements;
	}

	public static void AddProgressToAchievement(string achievementName, float progressAmount)
	{
		AMInst.ChangeProgress(achievementName, progressAmount, "add");
	}

	public static void SetProgressToAchievement(string achievementName, float newProgress)
	{
		AMInst.ChangeProgress(achievementName, newProgress, "set");
	}

	public void ChangeProgress(string achievementName, float progressAmount, string type)
	{
		Achievement achievementByName = GetAchievementByName(achievementName);
		if (achievementByName == null)
		{
			Debug.LogWarning("AchievementManager::AddProgress() - Trying to add progress to an achievemnet that doesn't exist: " + achievementName);
			return;
		}
		int achievementID = GetAchievementID(achievementByName);
		if (type == "add")
		{
			if (Achievements[achievementID].AddProgress(progressAmount))
			{
				AchievementEarned(achievementByName);
				Earned[achievementID] = true;
			}
		}
		else if (type == "set" && Achievements[achievementID].SetProgress(progressAmount))
		{
			AchievementEarned(achievementByName);
			Earned[achievementID] = true;
		}
		CurrentProgress[achievementID] = Achievements[achievementID].CurrentProgress;
		SetAchievementData();
	}

	public int GetAchievementID(Achievement achievement)
	{
		int num = 0;
		for (num = 0; num < Achievements.Length && Achievements[num] != achievement; num++)
		{
		}
		return num;
	}

	private void AchievementEarned(Achievement achievement)
	{
		MonoBehaviour.print("popAchevements");
		GameObject.Find("Tracking").SendMessage("SentFromCLog", "Unlocked Achevement:" + achievement.DisplayName);
		unlockedAchievement = achievement;
		EarnedSound.Play();
		base.enabled = true;
		popupCurveTime = 0f;
	}

	public static int AchevementUnlockedCount()
	{
		int num = 0;
		for (int i = 0; i < AMInst.Achievements.Length; i++)
		{
			if (AMInst.Achievements[i].Earned)
			{
				num++;
			}
		}
		return num;
	}

	private Achievement GetAchievementByName(string achievementName)
	{
		return Achievements.FirstOrDefault((Achievement achievement) => achievement.Name == achievementName);
	}

	private void InitAchievementData()
	{
		int num = AMInst.Achievements.Length;
		CurrentProgress = new float[num];
		Earned = new bool[num];
		for (int i = 0; i < AMInst.Achievements.Length; i++)
		{
			CurrentProgress[i] = (AMInst.Achievements[i].CurrentProgress = 0f);
			Earned[i] = (AMInst.Achievements[i].Earned = false);
		}
	}

	private bool HasAchievementData()
	{
		if (PlayerPrefs.HasKey("CurrentProgress"))
		{
			return true;
		}
		return false;
	}

	private void SetAchievementData()
	{
		string text = string.Empty;
		string text2 = string.Empty;
		for (int i = 0; i < AMInst.Achievements.Length; i++)
		{
			text += AMInst.Achievements[i].CurrentProgress;
			text2 += AMInst.Achievements[i].Earned;
			text += ",";
			text2 += ",";
		}
		PlayerPrefs.SetString("CurrentProgress", text);
		PlayerPrefs.SetString("Earned", text2);
	}

	private void GetAchievementData()
	{
		string[] array = PlayerPrefs.GetString("CurrentProgress").Split(","[0]);
		string[] array2 = PlayerPrefs.GetString("Earned").Split(","[0]);
		CurrentProgress = new float[array.Length];
		Earned = new bool[array.Length];
		for (int i = 0; i < AMInst.Achievements.Length; i++)
		{
			float.TryParse(array[i], out CurrentProgress[i]);
			bool.TryParse(array2[i], out Earned[i]);
			AMInst.Achievements[i].CurrentProgress = CurrentProgress[i];
			AMInst.Achievements[i].Earned = Earned[i];
		}
	}

	public void Die()
	{
		Object.Destroy(this);
	}

	public static void ClearAchievementData()
	{
		PlayerPrefs.DeleteAll();
		AMInst.InitAchievementData();
	}

	public void SetSkin(GUISkin Skin)
	{
		skin = Skin;
	}

	private void OnGUI()
	{
		GUI.skin = skin;
		GUI.depth = -30;
		yPos = popupCurve.Evaluate(popupCurveTime += Time.deltaTime);
		if (popupCurveTime >= popupCurve.keys[popupCurve.length - 1].time)
		{
			base.enabled = false;
		}
		GUI.DrawTexture(FluidRect(0.3f, 0f + yPos, 0.4f, 0.3f), PopBackground);
		GUI.Label(FluidRect(0.47f, 0f + yPos, 0.2f, 0.25f), "Unlocked\n" + unlockedAchievement.DisplayName, "WhiteLabel");
		GUI.DrawTexture(FluidRect(0.3f, 0f + yPos, 0.2f, 0.25f), unlockedAchievement.Icon, ScaleMode.ScaleToFit);
	}

	public Rect FluidRect(float x, float y, float width, float height)
	{
		return new Rect(x * (float)Screen.width, y * (float)Screen.height, width * (float)Screen.width, height * (float)Screen.height);
	}
}
