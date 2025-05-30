using UnityEngine;

public class LoadSceneButton : MonoBehaviour
{
	public GUISkin gui;

	public Texture btnTexture;

	private void Start()
	{
	}

	private void Update()
	{
	}

	private void OnGUI()
	{
		GUI.skin = gui;
		if (GUI.Button(new Rect(fluidRect(0.4f, 0.83f, 0.2f, 0.17f)), string.Empty, "BackButton"))
		{
			Application.LoadLevel(0);
		}
	}

	private Rect fluidRect(float x, float y, float width, float height)
	{
		return new Rect(x * (float)Screen.width, y * (float)Screen.height, width * (float)Screen.width, height * (float)Screen.height);
	}
}
