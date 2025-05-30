using UnityEngine;

public class DefaultInitializationErrorHandler : MonoBehaviour
{
	private const string WINDOW_TITLE = "QCAR Initialization Error";

	private string mErrorText = string.Empty;

	private bool mErrorOccurred;

	private void Start()
	{
		QCARUnity.InitError initError = QCARUnity.CheckInitializationError();
		if (initError != QCARUnity.InitError.INIT_SUCCESS)
		{
			SetErrorCode(initError);
			SetErrorOccurred(true);
		}
	}

	private void OnGUI()
	{
		if (mErrorOccurred)
		{
			GUI.Window(0, new Rect(0f, 0f, Screen.width, Screen.height), DrawWindowContent, "QCAR Initialization Error");
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

	private void SetErrorCode(QCARUnity.InitError errorCode)
	{
		switch (errorCode)
		{
		case QCARUnity.InitError.INIT_DEVICE_NOT_SUPPORTED:
			mErrorText = "Failed to initialize QCAR because this device is not supported.";
			break;
		case QCARUnity.InitError.INIT_ERROR:
			mErrorText = "Failed to initialize QCAR.";
			break;
		}
	}

	private void SetErrorOccurred(bool errorOccurred)
	{
		mErrorOccurred = errorOccurred;
		if (errorOccurred)
		{
			base.GetComponent<Camera>().clearFlags = CameraClearFlags.Color;
		}
	}
}
