public abstract class TrackerManager
{
	private static TrackerManager mInstance;

	public static TrackerManager Instance
	{
		get
		{
			if (mInstance == null)
			{
				lock (typeof(TrackerManager))
				{
					if (mInstance == null)
					{
						mInstance = new TrackerManagerImpl();
					}
				}
			}
			return mInstance;
		}
	}

	public abstract Tracker GetTracker(Tracker.Type trackerType);

	public abstract Tracker InitTracker(Tracker.Type trackerType);

	public abstract bool DeinitTracker(Tracker.Type trackerType);

	public abstract StateManager GetStateManager();
}
