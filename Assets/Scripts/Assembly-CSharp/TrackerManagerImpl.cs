using UnityEngine;

public class TrackerManagerImpl : TrackerManager
{
	private ImageTracker mImageTracker;

	private MarkerTracker mMarkerTracker;

	private TextTracker mTextTracker;

	private StateManager mStateManager = new StateManagerImpl();

	public override Tracker GetTracker(Tracker.Type trackerType)
	{
		switch (trackerType)
		{
		case Tracker.Type.IMAGE_TRACKER:
			return mImageTracker;
		case Tracker.Type.MARKER_TRACKER:
			return mMarkerTracker;
		case Tracker.Type.TEXT_TRACKER:
			return mTextTracker;
		default:
			Debug.LogError("Could not return tracker. Unknow tracker type.");
			return null;
		}
	}

	public override Tracker InitTracker(Tracker.Type trackerType)
	{
		if (!QCARRuntimeUtilities.IsQCAREnabled())
		{
			return null;
		}
		if (QCARWrapper.Instance.TrackerManagerInitTracker((int)trackerType) == 0)
		{
			Debug.LogError("Could not initialize the tracker.");
			return null;
		}
		switch (trackerType)
		{
		case Tracker.Type.IMAGE_TRACKER:
			if (mImageTracker == null)
			{
				mImageTracker = new ImageTrackerImpl();
			}
			return mImageTracker;
		case Tracker.Type.MARKER_TRACKER:
			if (mMarkerTracker == null)
			{
				mMarkerTracker = new MarkerTrackerImpl();
			}
			return mMarkerTracker;
		case Tracker.Type.TEXT_TRACKER:
			if (mTextTracker == null)
			{
				mTextTracker = new TextTrackerImpl();
			}
			return mTextTracker;
		default:
			Debug.LogError("Could not initialize tracker. Unknown tracker type.");
			return null;
		}
	}

	public override bool DeinitTracker(Tracker.Type trackerType)
	{
		if (QCARWrapper.Instance.TrackerManagerDeinitTracker((int)trackerType) == 0)
		{
			Debug.LogError("Could not deinitialize the tracker.");
			return false;
		}
		switch (trackerType)
		{
		case Tracker.Type.IMAGE_TRACKER:
			mImageTracker = null;
			break;
		case Tracker.Type.MARKER_TRACKER:
			mMarkerTracker = null;
			break;
		case Tracker.Type.TEXT_TRACKER:
			mTextTracker = null;
			break;
		default:
			Debug.LogError("Could not deinitialize tracker. Unknown tracker type.");
			return false;
		}
		return true;
	}

	public override StateManager GetStateManager()
	{
		return mStateManager;
	}
}
