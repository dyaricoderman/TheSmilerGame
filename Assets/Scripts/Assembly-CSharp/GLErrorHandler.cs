using UnityEngine;

public class GLErrorHandler : MonoBehaviour
{
	private const string WINDOW_TITLE = "Sample Error";

	private static string mErrorText = string.Empty;

	private static bool mErrorOccurred;

	public static void SetError(string errorText)
	{
		mErrorText = errorText;
		mErrorOccurred = true;
	}

	private void OnGUI()
	{
		if (mErrorOccurred)
		{
			GUI.Window(0, new Rect(0f, 0f, Screen.width, Screen.height), DrawWindowContent, "Sample Error");
		}
	}

	private void DrawWindowContent(int id)
	{
		GUI.Label(new Rect(10f, 25f, Screen.width - 20, Screen.height - 95), mErrorText);
		if (GUI.Button(new Rect(Screen.width / 2 - 75, Screen.height - 60, 150f, 50f), "Close"))
		{
			Application.Quit();
		}
	}
}
