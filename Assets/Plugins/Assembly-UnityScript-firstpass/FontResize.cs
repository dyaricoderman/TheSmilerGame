using System;
using UnityEngine;
using UnityScript.Lang;

[Serializable]
public class FontResize : MonoBehaviour
{
	public GUISkin Skin;

	[NonSerialized]
	public static GUISkin ResizedSkin;

	public int[] ResolutionCuttOff;

	public SwapDef[] Swaps;

	private int ResolutionSize;

	[NonSerialized]
	public static bool skinResized;

	public virtual void Awake()
	{
		if (!skinResized)
		{
			skinResized = true;
			ResizedSkin = CloneSkin(Skin);
			UpdateFonts();
		}
		UnityEngine.Object.Destroy(this);
	}

	public virtual GUISkin CloneSkin(GUISkin original)
	{
		GUISkin gUISkin = (GUISkin)ScriptableObject.CreateInstance(typeof(GUISkin));
		gUISkin.customStyles = new GUIStyle[Extensions.get_length((System.Array)original.customStyles)];
		gUISkin.font = original.font;
		for (int i = 0; i < Extensions.get_length((System.Array)original.customStyles); i++)
		{
			gUISkin.customStyles[i] = new GUIStyle(original.customStyles[i]);
		}
		return gUISkin;
	}

	public virtual void getResizedSkin(GameObject callBack)
	{
		callBack.SendMessage("SetSkin", ResizedSkin);
	}

	public virtual void UpdateFonts()
	{
		int num = (int)Mathf.Sqrt(Mathf.Pow(Screen.width, 2f) + Mathf.Pow(Screen.height, 2f));
		for (int i = 0; i < Extensions.get_length((System.Array)ResolutionCuttOff); i++)
		{
			if (num > ResolutionCuttOff[i])
			{
				ResolutionSize = i;
			}
		}
		MonoBehaviour.print("Font Resolution " + num + " level:" + ResolutionSize);
		ResizedSkin.font = SwapFont(ResizedSkin.font);
		for (int i = 0; i < Extensions.get_length((System.Array)ResizedSkin.customStyles); i++)
		{
			ResizedSkin.customStyles[i].font = SwapFont(ResizedSkin.customStyles[i].font);
		}
	}

	public virtual Font SwapFont(Font original)
	{
		int num = 0;
		object result;
		while (true)
		{
			if (num < Extensions.get_length((System.Array)Swaps))
			{
				if (Swaps[num].fonts[0] == original)
				{
					result = Swaps[num].fonts[ResolutionSize];
					break;
				}
				num++;
				continue;
			}
			result = original;
			break;
		}
		return (Font)result;
	}

	public virtual void Main()
	{
	}
}
