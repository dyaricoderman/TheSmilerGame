using UnityEngine;

[RequireComponent(typeof(Camera))]
public class WebCamBehaviour : MonoBehaviour
{
	public Camera BackgroundCameraPrefab;

	public int RenderTextureLayer;

	[SerializeField]
	[HideInInspector]
	private string mDeviceNameSetInEditor;

	[HideInInspector]
	[SerializeField]
	private bool mFlipHorizontally;

	[SerializeField]
	[HideInInspector]
	private bool mTurnOffWebCam;

	private WebCamImpl mWebCamImpl;

	private Camera mBackgroundCameraInstance;

	public string DeviceName
	{
		get
		{
			return mDeviceNameSetInEditor;
		}
		set
		{
			mDeviceNameSetInEditor = value;
		}
	}

	public WebCamImpl ImplementationClass
	{
		get
		{
			return mWebCamImpl;
		}
	}

	public bool FlipHorizontally
	{
		get
		{
			return mFlipHorizontally;
		}
		set
		{
			mFlipHorizontally = value;
		}
	}

	public bool TurnOffWebCam
	{
		get
		{
			return mTurnOffWebCam;
		}
		set
		{
			mTurnOffWebCam = value;
		}
	}

	public bool IsPlaying
	{
		get
		{
			return mWebCamImpl.IsPlaying;
		}
	}

	public void InitCamera()
	{
		if (mWebCamImpl != null)
		{
			return;
		}
		Application.runInBackground = true;
		Camera component = base.gameObject.GetComponent<Camera>();
		if (BackgroundCameraPrefab != null)
		{
			mBackgroundCameraInstance = Object.Instantiate(BackgroundCameraPrefab) as Camera;
			if (KeepAliveBehaviour.Instance != null && KeepAliveBehaviour.Instance.KeepARCameraAlive)
			{
				Object.DontDestroyOnLoad(mBackgroundCameraInstance.gameObject);
			}
		}
		mWebCamImpl = new WebCamImpl(component, mBackgroundCameraInstance, RenderTextureLayer, mDeviceNameSetInEditor, mFlipHorizontally);
	}

	public void StartCamera()
	{
		mWebCamImpl.StartCamera();
	}

	public void StopCamera()
	{
		mWebCamImpl.StopCamera();
	}

	public bool CheckNativePluginSupport()
	{
		return true;
	}

	public bool IsWebCamUsed()
	{
		return !mTurnOffWebCam && CheckNativePluginSupport() && WebCamTexture.devices.Length > 0;
	}

	private void OnLevelWasLoaded()
	{
		if (QCARRuntimeUtilities.IsPlayMode() && mWebCamImpl != null)
		{
			mWebCamImpl.ResetPlaying();
		}
	}
}
