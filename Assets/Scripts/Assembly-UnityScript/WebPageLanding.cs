using System;
using UnityEngine;
using UnityScript.Lang;

[Serializable]
public class WebPageLanding : MonoBehaviour
{
	public Texture2D BottemBar;

	public Texture2D SmilerText;

	public Texture2D AltonLogo;

	public Texture2D TheSmiller;

	public Renderer[] WebFaces;

	public Texture2D MessagePreLaunch;

	public Texture2D MessagePostLaunch;

	private GUISkin skin;

	private int index;

	public virtual void Start()
	{
		skin = FontResize.ResizedSkin;
		enabled = false;
	}

	public virtual void ShowWebPage()
	{
		enabled = true;
		for (int i = 0; i < Extensions.get_length((System.Array)WebFaces); i++)
		{
			WebFaces[i].enabled = true;
		}
	}

	public virtual void OnGUI()
	{
		GUI.skin = skin;
		GUI.DrawTexture(Global.fluidRect(0f, 0.85f, 1f, 0.15f), BottemBar, ScaleMode.StretchToFill, true);
		GUI.DrawTexture(Global.fluidRect(0.35f, 0.4f, 0.3f, 0.1f), TheSmiller, ScaleMode.ScaleToFit, true);
		GUI.DrawTexture(Global.fluidRect(0.35f, 0.1f, 0.3f, 0.3f), AltonLogo, ScaleMode.ScaleToFit, true);
		if (GUI.Button(Global.fluidRect(0f, 0.88f, 0.333f, 0.15f), "Back", "BlackInvisButton"))
		{
			Exit();
		}
		if (GUI.Button(Global.fluidRect(0.666f, 0.88f, 0.333f, 0.15f), "Book Now", "BlackInvisButton"))
		{
			AchievementManager.SetProgressToAchievement("Smiley Surfer", 1f);
			Tracker2.instance.Log("Clicked to Website");
			Application.OpenURL("http://www.altontowers.com/mobile/?utm_source=TheSmilerGame&utm_medium=iosmobile&utm_campaign=mobile");
		}
		int unixTime = UnixTime.GetUnixTime(new DateTime(2013, 3, 16, 11, 0, 0));
		int unixTime2 = UnixTime.GetUnixTime();
		int num = unixTime - unixTime2;
		GUI.Label(Global.fluidRect(0.3f, 0.6f, 0.4f, 0.15f), "Book tickets on your mobile in advance for the best savings!", "WhiteLabel");
	}

	public virtual void Exit()
	{
		gameObject.SendMessage("MainMenuDisplay", true);
		enabled = false;
		for (int i = 0; i < Extensions.get_length((System.Array)WebFaces); i++)
		{
			WebFaces[i].enabled = false;
		}
	}

	public virtual void Main()
	{
	}
}
