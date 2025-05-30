using UnityEngine;

public abstract class QCARManager
{
	private static QCARManager sInstance;

	public static QCARManager Instance
	{
		get
		{
			if (sInstance == null)
			{
				lock (typeof(QCARManager))
				{
					if (sInstance == null)
					{
						sInstance = new QCARManagerImpl();
					}
				}
			}
			return sInstance;
		}
	}

	public abstract QCARBehaviour.WorldCenterMode WorldCenterMode { get; set; }

	public abstract TrackableBehaviour WorldCenter { get; set; }

	public abstract Camera ARCamera { get; set; }

	public abstract bool DrawVideoBackground { get; set; }

	public abstract bool Initialized { get; }

	public abstract bool Init();

	public abstract void Deinit();
}
