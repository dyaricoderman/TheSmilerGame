using System;
using UnityEngine;

[Serializable]
public class SocalShareGUI : MonoBehaviour
{
	public Texture2D PopBackground;

	public Texture2D Popgraphic;

	private bool postComplete;

	private bool loginFailed;

	public AnimationCurve popupCurve;

	private float popupCurveTime;

	private bool popping;

	private GUISkin guiSkin;

	public SocalShareGUI()
	{
		popupCurve = new AnimationCurve(new Keyframe(0f, -0.2f), new Keyframe(2f, 0f), new Keyframe(3f, 0.025f), new Keyframe(4f, 0f), new Keyframe(6f, -0.2f));
	}

	public virtual void Start()
	{
		guiSkin = FontResize.ResizedSkin;
	}

	public virtual void OnGUI()
	{
		if (popping)
		{
			GUI.skin = guiSkin;
			GUI.depth = -31;
			popupCurveTime += Time.deltaTime;
			float num = popupCurve.Evaluate(popupCurveTime);
			if (!(popupCurveTime < popupCurve.keys[popupCurve.length - 1].time))
			{
				popping = false;
				popupCurveTime = 0f;
			}
			GUI.DrawTexture(Global.fluidRect(0.3f, 0f + num, 0.4f, 0.3f), Popgraphic);
			GUI.DrawTexture(FluidRect(0.3f, 0f + num, 0.4f, 0.3f), PopBackground);
			GUI.Label(FluidRect(0.47f, 0f + num, 0.2f, 0.25f), "Thank you for sharing", "WhiteLabel");
			GUI.DrawTexture(FluidRect(0.3f, 0f + num, 0.2f, 0.25f), Popgraphic, ScaleMode.ScaleToFit);
		}
	}

	public virtual void PostComplete()
	{
		Debug.Log("Social post complete");
		popping = true;
		AchievementManager.AddProgressToAchievement("Social Smile", 1f);
	}

	public virtual void LoginFailed()
	{
		popupCurveTime = 0f;
		enabled = true;
	}

	public virtual Rect FluidRect(float x, float y, float width, float height)
	{
		return new Rect(x * (float)Screen.width, y * (float)Screen.height, width * (float)Screen.width, height * (float)Screen.height);
	}

	public virtual void Main()
	{
	}
}
