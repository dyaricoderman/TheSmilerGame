using UnityEngine;

public class WheelGUI : MonoBehaviour
{
	public GUISkin skin;

	public Texture backTexture;

	public Texture winningsTexture;

	public string levelToLoad;

	private void Start()
	{
	}

	private void Update()
	{
	}

	private void OnGUI()
	{
		GUI.skin = skin;
		if (GUI.Button(FluidRect(0f, 0.85f, 0.3f, 0.15f), "Exit", "StandardButton"))
		{
			Application.LoadLevel(levelToLoad);
		}
		if (GUI.Button(FluidRect(0.7f, 0.85f, 0.3f, 0.15f), string.Empty, "MenuWebsiteButton"))
		{
			Debug.Log("Clicked the button with text");
		}
	}

	public Rect FluidRect(float x, float y, float width, float height)
	{
		return new Rect(x * (float)Screen.width, y * (float)Screen.height, width * (float)Screen.width, height * (float)Screen.height);
	}
}
