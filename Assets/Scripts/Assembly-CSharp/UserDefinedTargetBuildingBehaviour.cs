using System.Collections.Generic;
using UnityEngine;

public class UserDefinedTargetBuildingBehaviour : MonoBehaviour, ITrackerEventHandler
{
	private ImageTracker mImageTracker;

	private ImageTargetBuilder.FrameQuality mLastFrameQuality = ImageTargetBuilder.FrameQuality.FRAME_QUALITY_NONE;

	private bool mCurrentlyScanning;

	private bool mWasScanningBeforeDisable;

	private bool mCurrentlyBuilding;

	private bool mWasBuildingBeforeDisable;

	private bool mOnInitializedCalled;

	private readonly List<IUserDefinedTargetEventHandler> mHandlers = new List<IUserDefinedTargetEventHandler>();

	public bool StopTrackerWhileScanning;

	public bool StartScanningAutomatically;

	public bool StopScanningWhenFinshedBuilding;

	public void RegisterEventHandler(IUserDefinedTargetEventHandler eventHandler)
	{
		mHandlers.Add(eventHandler);
		if (mOnInitializedCalled)
		{
			eventHandler.OnInitialized();
		}
	}

	public bool UnregisterEventHandler(IUserDefinedTargetEventHandler eventHandler)
	{
		return mHandlers.Remove(eventHandler);
	}

	public void StartScanning()
	{
		if (mImageTracker != null)
		{
			if (StopTrackerWhileScanning)
			{
				mImageTracker.Stop();
			}
			mImageTracker.ImageTargetBuilder.StartScan();
			mCurrentlyScanning = true;
		}
		SetFrameQuality(ImageTargetBuilder.FrameQuality.FRAME_QUALITY_LOW);
	}

	public void BuildNewTarget(string targetName, float sceenSizeWidth)
	{
		mCurrentlyBuilding = true;
		mImageTracker.ImageTargetBuilder.Build(targetName, sceenSizeWidth);
	}

	public void StopScanning()
	{
		mCurrentlyScanning = false;
		mImageTracker.ImageTargetBuilder.StopScan();
		if (StopTrackerWhileScanning)
		{
			mImageTracker.Start();
		}
		SetFrameQuality(ImageTargetBuilder.FrameQuality.FRAME_QUALITY_NONE);
	}

	private void SetFrameQuality(ImageTargetBuilder.FrameQuality frameQuality)
	{
		if (frameQuality == mLastFrameQuality)
		{
			return;
		}
		foreach (IUserDefinedTargetEventHandler mHandler in mHandlers)
		{
			mHandler.OnFrameQualityChanged(frameQuality);
		}
		mLastFrameQuality = frameQuality;
	}

	private void Start()
	{
		if (KeepAliveBehaviour.Instance != null && KeepAliveBehaviour.Instance.KeepUDTBuildingBehaviourAlive)
		{
			Object.DontDestroyOnLoad(base.gameObject);
		}
		QCARBehaviour qCARBehaviour = (QCARBehaviour)Object.FindObjectOfType(typeof(QCARBehaviour));
		if ((bool)qCARBehaviour)
		{
			qCARBehaviour.RegisterTrackerEventHandler(this);
		}
	}

	private void Update()
	{
		if (!mOnInitializedCalled)
		{
			return;
		}
		if (mCurrentlyScanning)
		{
			SetFrameQuality(mImageTracker.ImageTargetBuilder.GetFrameQuality());
		}
		if (!mCurrentlyBuilding)
		{
			return;
		}
		TrackableSource trackableSource = mImageTracker.ImageTargetBuilder.GetTrackableSource();
		if (trackableSource == null)
		{
			return;
		}
		mCurrentlyBuilding = false;
		foreach (IUserDefinedTargetEventHandler mHandler in mHandlers)
		{
			mHandler.OnNewTrackableSource(trackableSource);
		}
		if (StopScanningWhenFinshedBuilding)
		{
			StopScanning();
		}
	}

	private void OnApplicationPause(bool pause)
	{
		if (pause)
		{
			OnDisable();
		}
		else
		{
			OnEnable();
		}
	}

	private void OnEnable()
	{
		if (mOnInitializedCalled)
		{
			mCurrentlyScanning = mWasScanningBeforeDisable;
			mCurrentlyBuilding = mWasBuildingBeforeDisable;
			if (mWasScanningBeforeDisable)
			{
				StartScanning();
			}
		}
	}

	private void OnDisable()
	{
		if (mOnInitializedCalled)
		{
			mWasScanningBeforeDisable = mCurrentlyScanning;
			mWasBuildingBeforeDisable = mCurrentlyBuilding;
			if (mCurrentlyScanning)
			{
				StopScanning();
			}
		}
	}

	public void OnInitialized()
	{
		mOnInitializedCalled = true;
		mImageTracker = (ImageTracker)TrackerManager.Instance.GetTracker(Tracker.Type.IMAGE_TRACKER);
		foreach (IUserDefinedTargetEventHandler mHandler in mHandlers)
		{
			mHandler.OnInitialized();
		}
		if (StartScanningAutomatically)
		{
			StartScanning();
		}
	}

	public void OnTrackablesUpdated()
	{
	}
}
