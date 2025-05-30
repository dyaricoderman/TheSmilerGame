using UnityEngine;

public class BlueprintMaster : MonoBehaviour
{
	private GUISkin guiSkin;

	public Texture footerTex;

	private void Start()
	{
		guiSkin = FontResize.ResizedSkin;
	}

	private void Update()
	{
	}

	private void OnGUI()
	{
		GUI.skin = guiSkin;
		if (GUI.Button(FluidRect(0.05f, 0.8f, 0.275f, 0.15f), "Back", "StandardButton"))
		{
			MonoBehaviour.print(LevelLoader.inst);
			LevelLoader.inst.LoadLevelC(0);
		}
	}

	private Rect FluidRect(float x, float y, float width, float height)
	{
		return new Rect(x * (float)Screen.width, y * (float)Screen.height, width * (float)Screen.width, height * (float)Screen.height);
	}
}
