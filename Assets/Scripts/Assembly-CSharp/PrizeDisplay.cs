using System;
using UnityEngine;

[Serializable]
public class PrizeDisplay : MonoBehaviour
{
	private GUISkin skin;

	private Competition competition;

	public PrizeContainer[] prizes;

	private int currentPage;

	public Rect currentPageRect = new Rect(0.25f, 0.8f, 0.5f, 0.15f);

	public Rect descRect = new Rect(0.25f, 0.65f, 0.5f, 0.15f);

	public Rect iconRect = new Rect(0.25f, 0.25f, 275f, 0.275f);

	public Rect prevRect = new Rect(0.25f, 0.5f, 0.075f, 0.1f);

	public GUIContent prevContent = new GUIContent();

	public Rect nextRect = new Rect(0.2f, 0.5f, 0.075f, 0.1f);

	public GUIContent nextContent = new GUIContent();

	public Rect backRect = new Rect(0.3f, 0.85f, 0.15f, 0.1f);

	public GUIContent backContent = new GUIContent("Cancel");

	private void Start()
	{
		skin = FontResize.ResizedSkin;
		competition = base.gameObject.GetComponent<Competition>();
	}

	private void Update()
	{
	}

	private void OnGUI()
	{
		GUI.skin = skin;
		if (currentPage > 0)
		{
			GUI.color = Color.yellow;
			if (GUI.Button(AspectfluidRect(prevRect, 1.6f), prevContent))
			{
				currentPage--;
			}
			GUI.color = Color.white;
		}
		if (currentPage < prizes.Length - 1)
		{
			GUI.color = Color.yellow;
			if (GUI.Button(AspectfluidRect(nextRect, 1.6f), nextContent))
			{
				currentPage++;
			}
			GUI.color = Color.white;
		}
		GUI.Label(AspectfluidRect(currentPageRect, 1.6f), currentPage + 1 + "/" + prizes.Length, "WhiteLabel");
		GUI.Label(AspectfluidRect(descRect, 1.6f), prizes[currentPage].description, "WhiteLabel");
		GUI.DrawTexture(FluidRect(iconRect), prizes[currentPage].icon, ScaleMode.ScaleToFit);
		if (GUI.Button(FluidRect(backRect), backContent, "ContinueButton"))
		{
			competition.ClosePrizes();
			base.enabled = false;
		}
	}

	public Rect FluidRect(Rect rect)
	{
		rect.x *= Screen.width;
		rect.y *= Screen.height;
		rect.width *= Screen.width;
		rect.height *= Screen.height;
		return rect;
	}

	public Rect FluidRect(float x, float y, float width, float height)
	{
		return new Rect(x * (float)Screen.width, y * (float)Screen.height, width * (float)Screen.width, height * (float)Screen.height);
	}

	public Rect AspectfluidRect(Rect rect, float Aspect)
	{
		float num = Aspect * (float)Screen.height;
		rect.x = rect.x * num - (num - (float)Screen.width) * 0.5f;
		rect.y *= Screen.height;
		rect.width *= num;
		rect.height *= Screen.height;
		return rect;
	}

	public Rect AspectfluidRect(float x, float y, float width, float height, float Aspect)
	{
		float num = Aspect * (float)Screen.height;
		return new Rect(x * num - (num - (float)Screen.width) * 0.5f, y * (float)Screen.height, width * num, height * (float)Screen.height);
	}
}
