using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class QCARBehaviour : MonoBehaviour
{
	public enum WorldCenterMode
	{
		SPECIFIC_TARGET = 0,
		FIRST_TARGET = 1,
		CAMERA = 2
	}

	private enum CameraState
	{
		UNINITED = 0,
		DEVICE_INITED = 1,
		RENDERING_INITED = 2
	}

	[SerializeField]
	private CameraDevice.CameraDeviceMode CameraDeviceModeSetting = CameraDevice.CameraDeviceMode.MODE_DEFAULT;

	[SerializeField]
	private int MaxSimultaneousImageTargets = 1;

	[SerializeField]
	private bool SynchronousVideo;

	[SerializeField]
	[HideInInspector]
	private WorldCenterMode mWorldCenterMode = WorldCenterMode.FIRST_TARGET;

	[SerializeField]
	[HideInInspector]
	private TrackableBehaviour mWorldCenter;

	[SerializeField]
	private CameraDevice.CameraDirection CameraDirection;

	[SerializeField]
	private QCARRenderer.VideoBackgroundReflection MirrorVideoBackground;

	private List<ITrackerEventHandler> mTrackerEventHandlers = new List<ITrackerEventHandler>();

	private List<IVideoBackgroundEventHandler> mVideoBgEventHandlers = new List<IVideoBackgroundEventHandler>();

	private bool mIsInitialized;

	private CameraState mCameraState;

	private Material mClearMaterial;

	private Rect mViewportRect;

	private int mClearBuffers;

	private CameraDevice.VideoModeData mVideoMode;

	private bool mHasStartedOnce;

	private ScreenOrientation mProjectionOrientation;

	private bool mCachedDrawVideoBackground;

	private CameraClearFlags mCachedCameraClearFlags;

	private Color mCachedCameraBackgroundColor;

	public WorldCenterMode WorldCenterModeSetting
	{
		get
		{
			return mWorldCenterMode;
		}
	}

	public TrackableBehaviour WorldCenter
	{
		get
		{
			return mWorldCenter;
		}
	}

	public bool VideoBackGroundMirrored { get; private set; }

	public CameraDevice.CameraDeviceMode CameraDeviceMode
	{
		get
		{
			return CameraDeviceModeSetting;
		}
	}

	public void RegisterTrackerEventHandler(ITrackerEventHandler trackerEventHandler)
	{
		mTrackerEventHandlers.Add(trackerEventHandler);
		if (mIsInitialized)
		{
			trackerEventHandler.OnInitialized();
		}
	}

	public bool UnregisterTrackerEventHandler(ITrackerEventHandler trackerEventHandler)
	{
		return mTrackerEventHandlers.Remove(trackerEventHandler);
	}

	public void RegisterVideoBgEventHandler(IVideoBackgroundEventHandler videoBgEventHandler)
	{
		mVideoBgEventHandlers.Add(videoBgEventHandler);
	}

	public bool UnregisterVideoBgEventHandler(IVideoBackgroundEventHandler videoBgEventHandler)
	{
		return mVideoBgEventHandlers.Remove(videoBgEventHandler);
	}

	public void SetWorldCenterMode(WorldCenterMode value)
	{
		if (!Application.isPlaying)
		{
			mWorldCenterMode = value;
		}
	}

	public void SetWorldCenter(TrackableBehaviour value)
	{
		if (!Application.isPlaying)
		{
			mWorldCenter = value;
		}
	}

	public Rect GetViewportRectangle()
	{
		return mViewportRect;
	}

	public ScreenOrientation GetSurfaceOrientation()
	{
		return QCARRuntimeUtilities.ScreenOrientation;
	}

	public CameraDevice.VideoModeData GetVideoMode()
	{
		return mVideoMode;
	}

	public void ConfigureVideoBackground(bool forceReflectionSetting)
	{
		QCARRenderer.VideoBGCfgData videoBackgroundConfig = QCARRenderer.Instance.GetVideoBackgroundConfig();
		CameraDevice.VideoModeData videoMode = CameraDevice.Instance.GetVideoMode(CameraDeviceModeSetting);
		VideoBackGroundMirrored = videoBackgroundConfig.reflection == QCARRenderer.VideoBackgroundReflection.ON;
		videoBackgroundConfig.enabled = 1;
		videoBackgroundConfig.synchronous = (SynchronousVideo ? 1 : 0);
		videoBackgroundConfig.position = new QCARRenderer.Vec2I(0, 0);
		if (!QCARRuntimeUtilities.IsPlayMode() && forceReflectionSetting)
		{
			videoBackgroundConfig.reflection = MirrorVideoBackground;
		}
		bool flag = Screen.width > Screen.height;
		if (QCARRuntimeUtilities.IsPlayMode())
		{
			flag = true;
		}
		if (flag)
		{
			float num = (float)videoMode.height * ((float)Screen.width / (float)videoMode.width);
			videoBackgroundConfig.size = new QCARRenderer.Vec2I(Screen.width, (int)num);
			if (videoBackgroundConfig.size.y < Screen.height)
			{
				videoBackgroundConfig.size.x = (int)((float)Screen.height * ((float)videoMode.width / (float)videoMode.height));
				videoBackgroundConfig.size.y = Screen.height;
			}
		}
		else
		{
			float num2 = (float)videoMode.height * ((float)Screen.height / (float)videoMode.width);
			videoBackgroundConfig.size = new QCARRenderer.Vec2I((int)num2, Screen.height);
			if (videoBackgroundConfig.size.x < Screen.width)
			{
				videoBackgroundConfig.size.x = Screen.width;
				videoBackgroundConfig.size.y = (int)((float)Screen.width * ((float)videoMode.width / (float)videoMode.height));
			}
		}
		QCARRenderer.Instance.SetVideoBackgroundConfig(videoBackgroundConfig);
		int num3 = videoBackgroundConfig.position.x + (Screen.width - videoBackgroundConfig.size.x) / 2;
		int num4 = videoBackgroundConfig.position.y + (Screen.height - videoBackgroundConfig.size.y) / 2;
		mViewportRect = new Rect(num3, num4, videoBackgroundConfig.size.x, videoBackgroundConfig.size.y);
		foreach (IVideoBackgroundEventHandler mVideoBgEventHandler in mVideoBgEventHandlers)
		{
			mVideoBgEventHandler.OnVideoBackgroundConfigChanged();
		}
	}

	public void ResetClearBuffers()
	{
		mClearBuffers = 12;
	}

	private void Start()
	{
		if (KeepAliveBehaviour.Instance != null && KeepAliveBehaviour.Instance.KeepARCameraAlive)
		{
			Object.DontDestroyOnLoad(base.gameObject);
		}
		Debug.Log("QCARWrapper.Start");
		if (QCARUnity.CheckInitializationError() != QCARUnity.InitError.INIT_SUCCESS)
		{
			mIsInitialized = false;
			return;
		}
		if (TrackerManager.Instance.GetTracker(Tracker.Type.MARKER_TRACKER) == null)
		{
			TrackerManager.Instance.InitTracker(Tracker.Type.MARKER_TRACKER);
		}
		if (TrackerManager.Instance.GetTracker(Tracker.Type.IMAGE_TRACKER) == null)
		{
			TrackerManager.Instance.InitTracker(Tracker.Type.IMAGE_TRACKER);
		}
		mCachedDrawVideoBackground = QCARManager.Instance.DrawVideoBackground;
		mCachedCameraClearFlags = base.GetComponent<Camera>().clearFlags;
		mCachedCameraBackgroundColor = base.GetComponent<Camera>().backgroundColor;
		Screen.sleepTimeout = -1;
		ResetCameraClearFlags();
		mClearMaterial = new Material(Shader.Find("Diffuse"));
		QCARUnity.SetHint(QCARUnity.QCARHint.HINT_MAX_SIMULTANEOUS_IMAGE_TARGETS, MaxSimultaneousImageTargets);
		QCARUnityImpl.SetUnityVersion(Application.persistentDataPath, true);
		StateManagerImpl stateManagerImpl = (StateManagerImpl)TrackerManager.Instance.GetStateManager();
		stateManagerImpl.AssociateMarkerBehaviours();
		StartQCAR();
		QCARManager.Instance.WorldCenterMode = mWorldCenterMode;
		QCARManager.Instance.WorldCenter = mWorldCenter;
		QCARManager.Instance.ARCamera = base.GetComponent<Camera>();
		QCARManager.Instance.Init();
		mIsInitialized = true;
		foreach (ITrackerEventHandler mTrackerEventHandler in mTrackerEventHandlers)
		{
			mTrackerEventHandler.OnInitialized();
		}
		mHasStartedOnce = true;
	}

	private void OnEnable()
	{
		if (QCARManager.Instance.Initialized && mHasStartedOnce)
		{
			StartQCAR();
		}
	}

	private void Update()
	{
		if (QCARManager.Instance.Initialized)
		{
			ScreenOrientation surfaceOrientation = (ScreenOrientation)QCARWrapper.Instance.GetSurfaceOrientation();
			CameraDeviceImpl cameraDeviceImpl = (CameraDeviceImpl)CameraDevice.Instance;
			if (cameraDeviceImpl.CameraReady && (QCARUnity.IsRendererDirty() || mProjectionOrientation != surfaceOrientation))
			{
				ConfigureVideoBackground(false);
				UpdateProjection(surfaceOrientation);
				cameraDeviceImpl.ResetDirtyFlag();
			}
			mClearMaterial.SetPass(0);
			((QCARManagerImpl)QCARManager.Instance).Update(mProjectionOrientation, CameraDeviceMode, ref mVideoMode);
			UpdateCameraClearFlags();
			{
				foreach (ITrackerEventHandler mTrackerEventHandler in mTrackerEventHandlers)
				{
					mTrackerEventHandler.OnTrackablesUpdated();
				}
				return;
			}
		}
		if (QCARRuntimeUtilities.IsPlayMode())
		{
			Debug.LogWarning("Scripts have been recompiled during Play mode, need to restart!");
			QCARWrapper.Create();
			QCARRuntimeUtilities.RestartPlayMode();
		}
	}

	private void OnPreCull()
	{
		((QCARManagerImpl)QCARManager.Instance).PrepareRendering();
	}

	private void OnPreRender()
	{
		GL.invertCulling = VideoBackGroundMirrored;
	}

	private void OnPostRender()
	{
		((QCARManagerImpl)QCARManager.Instance).FinishRendering();
		if (mClearBuffers > 0)
		{
			GL.Clear(false, true, new Color(0f, 0f, 0f, 1f));
			mClearBuffers--;
		}
	}

	private void OnApplicationPause(bool pause)
	{
		if (pause)
		{
			StopQCAR();
			return;
		}
		StartQCAR();
		ResetClearBuffers();
	}

	private void OnDisable()
	{
		StopQCAR();
		ResetCameraClearFlags();
	}

	private void OnDestroy()
	{
		StateManagerImpl stateManagerImpl = (StateManagerImpl)TrackerManager.Instance.GetStateManager();
		stateManagerImpl.ClearTrackableBehaviours();
		ImageTracker imageTracker = (ImageTracker)TrackerManager.Instance.GetTracker(Tracker.Type.IMAGE_TRACKER);
		if (imageTracker != null)
		{
			imageTracker.DestroyAllDataSets(false);
			imageTracker.Stop();
		}
		MarkerTracker markerTracker = (MarkerTracker)TrackerManager.Instance.GetTracker(Tracker.Type.MARKER_TRACKER);
		if (markerTracker != null)
		{
			markerTracker.DestroyAllMarkers(false);
			markerTracker.Stop();
		}
		TextTracker textTracker = (TextTracker)TrackerManager.Instance.GetTracker(Tracker.Type.TEXT_TRACKER);
		if (textTracker != null)
		{
			textTracker.Stop();
		}
		QCARManager.Instance.Deinit();
		if (TrackerManager.Instance.GetTracker(Tracker.Type.MARKER_TRACKER) != null)
		{
			TrackerManager.Instance.DeinitTracker(Tracker.Type.MARKER_TRACKER);
		}
		if (TrackerManager.Instance.GetTracker(Tracker.Type.IMAGE_TRACKER) != null)
		{
			TrackerManager.Instance.DeinitTracker(Tracker.Type.IMAGE_TRACKER);
		}
		if (TrackerManager.Instance.GetTracker(Tracker.Type.TEXT_TRACKER) != null)
		{
			TrackerManager.Instance.DeinitTracker(Tracker.Type.TEXT_TRACKER);
		}
		if (QCARRuntimeUtilities.IsPlayMode())
		{
			QCARWrapper.Instance.QcarDeinit();
		}
	}

	private void StartQCAR()
	{
		Debug.Log("StartQCAR");
		CameraDevice.Instance.Init(CameraDirection);
		CameraDevice.Instance.SelectVideoMode(CameraDeviceModeSetting);
		CameraDevice.Instance.Start();
		if (TrackerManager.Instance.GetTracker(Tracker.Type.MARKER_TRACKER) != null)
		{
			TrackerManager.Instance.GetTracker(Tracker.Type.MARKER_TRACKER).Start();
		}
		if (TrackerManager.Instance.GetTracker(Tracker.Type.IMAGE_TRACKER) != null)
		{
			TrackerManager.Instance.GetTracker(Tracker.Type.IMAGE_TRACKER).Start();
		}
		ScreenOrientation surfaceOrientation = (ScreenOrientation)QCARWrapper.Instance.GetSurfaceOrientation();
		UpdateProjection(surfaceOrientation);
	}

	private void StopQCAR()
	{
		Debug.Log("StopQCAR");
		if (TrackerManager.Instance.GetTracker(Tracker.Type.MARKER_TRACKER) != null)
		{
			TrackerManager.Instance.GetTracker(Tracker.Type.MARKER_TRACKER).Stop();
		}
		if (TrackerManager.Instance.GetTracker(Tracker.Type.IMAGE_TRACKER) != null)
		{
			TrackerManager.Instance.GetTracker(Tracker.Type.IMAGE_TRACKER).Stop();
		}
		if (TrackerManager.Instance.GetTracker(Tracker.Type.TEXT_TRACKER) != null)
		{
			TrackerManager.Instance.GetTracker(Tracker.Type.TEXT_TRACKER).Stop();
		}
		CameraDevice.Instance.Stop();
		CameraDevice.Instance.Deinit();
		QCARRenderer.Instance.ClearVideoBackgroundConfig();
	}

	private void ResetCameraClearFlags()
	{
		mCameraState = CameraState.UNINITED;
		mClearBuffers = 12;
		base.GetComponent<Camera>().clearFlags = mCachedCameraClearFlags;
		base.GetComponent<Camera>().backgroundColor = mCachedCameraBackgroundColor;
	}

	private void UpdateCameraClearFlags()
	{
		if (!QCARRuntimeUtilities.IsQCAREnabled())
		{
			mCameraState = CameraState.UNINITED;
			return;
		}
		switch (mCameraState)
		{
		case CameraState.UNINITED:
			mCameraState = CameraState.DEVICE_INITED;
			break;
		case CameraState.DEVICE_INITED:
			if (QCARUnity.RequiresAlpha())
			{
				base.GetComponent<Camera>().clearFlags = CameraClearFlags.Color;
				base.GetComponent<Camera>().backgroundColor = new Color(0f, 0f, 0f, 0f);
				Debug.Log("Setting camera clear flags to transparent black");
			}
			else if (mCachedDrawVideoBackground)
			{
				base.GetComponent<Camera>().clearFlags = CameraClearFlags.Depth;
				Debug.Log("Setting camera clear flags to depth only");
			}
			else
			{
				base.GetComponent<Camera>().clearFlags = mCachedCameraClearFlags;
				base.GetComponent<Camera>().backgroundColor = mCachedCameraBackgroundColor;
				Debug.Log("Setting camera clear flags to Inspector values");
			}
			mCameraState = CameraState.RENDERING_INITED;
			break;
		case CameraState.RENDERING_INITED:
		{
			bool drawVideoBackground = QCARManager.Instance.DrawVideoBackground;
			if (drawVideoBackground != mCachedDrawVideoBackground)
			{
				mCameraState = CameraState.DEVICE_INITED;
				mCachedDrawVideoBackground = drawVideoBackground;
			}
			break;
		}
		}
	}

	private void UpdateProjection(ScreenOrientation orientation)
	{
		if (QCARRuntimeUtilities.IsQCAREnabled())
		{
			mProjectionOrientation = orientation;
			QCARRuntimeUtilities.CacheSurfaceOrientation(orientation);
			Matrix4x4 projectionGL = QCARUnity.GetProjectionGL(base.GetComponent<Camera>().nearClipPlane, base.GetComponent<Camera>().farClipPlane, mProjectionOrientation);
			if (mViewportRect.width != (float)Screen.width)
			{
				float num = mViewportRect.width / (float)Screen.width;
				int index2;
				int index = (index2 = 0);
				float num2 = projectionGL[index2];
				projectionGL[index] = num2 * num;
			}
			if (mViewportRect.height != (float)Screen.height)
			{
				float num3 = mViewportRect.height / (float)Screen.height;
				int index2;
				int index3 = (index2 = 5);
				float num2 = projectionGL[index2];
				projectionGL[index3] = num2 * num3;
			}
			base.GetComponent<Camera>().projectionMatrix = projectionGL;
		}
	}
}
