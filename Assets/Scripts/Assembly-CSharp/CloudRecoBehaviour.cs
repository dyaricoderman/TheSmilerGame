using System.Collections.Generic;
using UnityEngine;

public class CloudRecoBehaviour : MonoBehaviour, ITrackerEventHandler
{
	private ImageTracker mImageTracker;

	private bool mCurrentlyInitializing;

	private bool mInitSuccess;

	private bool mCloudRecoStarted;

	private bool mOnInitializedCalled;

	private readonly List<ICloudRecoEventHandler> mHandlers = new List<ICloudRecoEventHandler>();

	private bool mTargetFinderStartedBeforeDisable = true;

	public string AccessKey = string.Empty;

	public string SecretKey = string.Empty;

	public Color ScanlineColor = new Color(1f, 1f, 1f);

	public Color FeaturePointColor = new Color(0.427f, 0.988f, 0.286f);

	public bool CloudRecoEnabled
	{
		get
		{
			return mCloudRecoStarted;
		}
		set
		{
			if (value)
			{
				StartCloudReco();
			}
			else
			{
				StopCloudReco();
			}
		}
	}

	public bool CloudRecoInitialized
	{
		get
		{
			return mInitSuccess;
		}
	}

	private void Initialize()
	{
		mCurrentlyInitializing = mImageTracker.TargetFinder.StartInit(AccessKey, SecretKey);
		if (!mCurrentlyInitializing)
		{
			Debug.LogError("CloudRecoBehaviour: TargetFinder initialization failed!");
		}
	}

	private void Deinitialize()
	{
		mCurrentlyInitializing = !mImageTracker.TargetFinder.Deinit();
		if (mCurrentlyInitializing)
		{
			Debug.LogError("CloudRecoBehaviour: TargetFinder deinitialization failed!");
		}
		else
		{
			mInitSuccess = false;
		}
	}

	private void CheckInitialization()
	{
		TargetFinder.InitState initState = mImageTracker.TargetFinder.GetInitState();
		if (initState == TargetFinder.InitState.INIT_SUCCESS)
		{
			foreach (ICloudRecoEventHandler mHandler in mHandlers)
			{
				mHandler.OnInitialized();
			}
			mImageTracker.TargetFinder.SetUIScanlineColor(ScanlineColor);
			mImageTracker.TargetFinder.SetUIPointColor(FeaturePointColor);
			mCurrentlyInitializing = false;
			mInitSuccess = true;
			StartCloudReco();
		}
		else
		{
			if (initState >= TargetFinder.InitState.INIT_DEFAULT)
			{
				return;
			}
			foreach (ICloudRecoEventHandler mHandler2 in mHandlers)
			{
				mHandler2.OnInitError(initState);
			}
			mCurrentlyInitializing = false;
		}
	}

	private void StartCloudReco()
	{
		if (mImageTracker == null || mCloudRecoStarted)
		{
			return;
		}
		mCloudRecoStarted = mImageTracker.TargetFinder.StartRecognition();
		foreach (ICloudRecoEventHandler mHandler in mHandlers)
		{
			mHandler.OnStateChanged(true);
		}
	}

	private void StopCloudReco()
	{
		if (!mCloudRecoStarted)
		{
			return;
		}
		mCloudRecoStarted = !mImageTracker.TargetFinder.Stop();
		if (mCloudRecoStarted)
		{
			Debug.LogError("Cloud Reco could not be stopped at this point!");
			return;
		}
		foreach (ICloudRecoEventHandler mHandler in mHandlers)
		{
			mHandler.OnStateChanged(false);
		}
	}

	public void RegisterEventHandler(ICloudRecoEventHandler eventHandler)
	{
		mHandlers.Add(eventHandler);
		if (mOnInitializedCalled)
		{
			eventHandler.OnInitialized();
		}
	}

	public bool UnregisterEventHandler(ICloudRecoEventHandler eventHandler)
	{
		return mHandlers.Remove(eventHandler);
	}

	private void OnEnable()
	{
		if (mOnInitializedCalled && mTargetFinderStartedBeforeDisable)
		{
			StartCloudReco();
		}
	}

	private void OnDisable()
	{
		if (QCARManager.Instance.Initialized && mOnInitializedCalled)
		{
			mTargetFinderStartedBeforeDisable = mCloudRecoStarted;
			StopCloudReco();
		}
	}

	private void Start()
	{
		if (KeepAliveBehaviour.Instance.KeepCloudRecoBehaviourAlive)
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
		if (mCurrentlyInitializing)
		{
			CheckInitialization();
		}
		else
		{
			if (!mInitSuccess)
			{
				return;
			}
			TargetFinder.UpdateState updateState = mImageTracker.TargetFinder.Update();
			if (updateState == TargetFinder.UpdateState.UPDATE_RESULTS_AVAILABLE)
			{
				IEnumerable<TargetFinder.TargetSearchResult> results = mImageTracker.TargetFinder.GetResults();
				{
					foreach (TargetFinder.TargetSearchResult item in results)
					{
						foreach (ICloudRecoEventHandler mHandler in mHandlers)
						{
							mHandler.OnNewSearchResult(item);
						}
					}
					return;
				}
			}
			if (updateState >= TargetFinder.UpdateState.UPDATE_NO_MATCH)
			{
				return;
			}
			foreach (ICloudRecoEventHandler mHandler2 in mHandlers)
			{
				mHandler2.OnUpdateError(updateState);
			}
		}
	}

	private void OnDestroy()
	{
		if (QCARManager.Instance.Initialized && mOnInitializedCalled)
		{
			Deinitialize();
		}
		QCARBehaviour qCARBehaviour = (QCARBehaviour)Object.FindObjectOfType(typeof(QCARBehaviour));
		if ((bool)qCARBehaviour)
		{
			qCARBehaviour.UnregisterTrackerEventHandler(this);
		}
	}

	public void OnInitialized()
	{
		mImageTracker = (ImageTracker)TrackerManager.Instance.GetTracker(Tracker.Type.IMAGE_TRACKER);
		if (mImageTracker != null)
		{
			Initialize();
		}
		mOnInitializedCalled = true;
	}

	public void OnTrackablesUpdated()
	{
	}
}
