using System.Collections.Generic;
using UnityEngine;

public class DataSetLoadBehaviour : MonoBehaviour
{
	[HideInInspector]
	[SerializeField]
	public List<string> mDataSetsToActivate = new List<string>();

	[SerializeField]
	[HideInInspector]
	public List<string> mDataSetsToLoad = new List<string>();

	private void Start()
	{
		if (!QCARRuntimeUtilities.IsQCAREnabled())
		{
			return;
		}
		if (QCARRuntimeUtilities.IsPlayMode())
		{
			QCARUnity.CheckInitializationError();
		}
		if (TrackerManager.Instance.GetTracker(Tracker.Type.IMAGE_TRACKER) == null)
		{
			TrackerManager.Instance.InitTracker(Tracker.Type.IMAGE_TRACKER);
		}
		foreach (string item in mDataSetsToLoad)
		{
			if (!DataSet.Exists(item))
			{
				Debug.LogError("Data set " + item + " does not exist.");
				continue;
			}
			ImageTracker imageTracker = (ImageTracker)TrackerManager.Instance.GetTracker(Tracker.Type.IMAGE_TRACKER);
			DataSet dataSet = imageTracker.CreateDataSet();
			if (!dataSet.Load(item))
			{
				Debug.LogError("Failed to load data set " + item + ".");
			}
			else if (mDataSetsToActivate.Contains(item))
			{
				imageTracker.ActivateDataSet(dataSet);
			}
		}
	}
}
