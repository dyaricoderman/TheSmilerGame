using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ImageTrackerImpl : ImageTracker
{
	private List<DataSetImpl> mActiveDataSets = new List<DataSetImpl>();

	private List<DataSet> mDataSets = new List<DataSet>();

	private ImageTargetBuilder mImageTargetBuilder;

	private TargetFinder mTargetFinder;

	public override ImageTargetBuilder ImageTargetBuilder
	{
		get
		{
			return mImageTargetBuilder;
		}
	}

	public override TargetFinder TargetFinder
	{
		get
		{
			return mTargetFinder;
		}
	}

	public ImageTrackerImpl()
	{
		mImageTargetBuilder = new ImageTargetBuilderImpl();
		mTargetFinder = new TargetFinderImpl();
	}

	public override bool Start()
	{
		if (QCARWrapper.Instance.ImageTrackerStart() == 0)
		{
			Debug.LogError("Could not start tracker.");
			return false;
		}
		return true;
	}

	public override void Stop()
	{
		QCARWrapper.Instance.ImageTrackerStop();
		StateManagerImpl stateManagerImpl = (StateManagerImpl)TrackerManager.Instance.GetStateManager();
		foreach (DataSetImpl mActiveDataSet in mActiveDataSets)
		{
			foreach (Trackable trackable in mActiveDataSet.GetTrackables())
			{
				stateManagerImpl.SetTrackableBehavioursForTrackableToNotFound(trackable);
			}
		}
	}

	public override DataSet CreateDataSet()
	{
		IntPtr intPtr = QCARWrapper.Instance.ImageTrackerCreateDataSet();
		if (intPtr == IntPtr.Zero)
		{
			Debug.LogError("Could not create dataset.");
			return null;
		}
		DataSet dataSet = new DataSetImpl(intPtr);
		mDataSets.Add(dataSet);
		return dataSet;
	}

	public override bool DestroyDataSet(DataSet dataSet, bool destroyTrackables)
	{
		if (dataSet == null)
		{
			Debug.LogError("Dataset is null.");
			return false;
		}
		if (destroyTrackables)
		{
			dataSet.DestroyAllTrackables(true);
		}
		DataSetImpl dataSetImpl = (DataSetImpl)dataSet;
		if (QCARWrapper.Instance.ImageTrackerDestroyDataSet(dataSetImpl.DataSetPtr) == 0)
		{
			Debug.LogError("Could not destroy dataset.");
			return false;
		}
		mDataSets.Remove(dataSet);
		return true;
	}

	public override bool ActivateDataSet(DataSet dataSet)
	{
		if (dataSet == null)
		{
			Debug.LogError("Dataset is null.");
			return false;
		}
		DataSetImpl dataSetImpl = (DataSetImpl)dataSet;
		if (QCARWrapper.Instance.ImageTrackerActivateDataSet(dataSetImpl.DataSetPtr) == 0)
		{
			Debug.LogError("Could not activate dataset.");
			return false;
		}
		StateManagerImpl stateManagerImpl = (StateManagerImpl)TrackerManager.Instance.GetStateManager();
		foreach (Trackable trackable in dataSetImpl.GetTrackables())
		{
			stateManagerImpl.EnableTrackableBehavioursForTrackable(trackable, true);
		}
		mActiveDataSets.Add(dataSetImpl);
		return true;
	}

	public override bool DeactivateDataSet(DataSet dataSet)
	{
		if (dataSet == null)
		{
			Debug.LogError("Dataset is null.");
			return false;
		}
		DataSetImpl dataSetImpl = (DataSetImpl)dataSet;
		if (QCARWrapper.Instance.ImageTrackerDeactivateDataSet(dataSetImpl.DataSetPtr) == 0)
		{
			Debug.LogError("Could not deactivate dataset.");
			return false;
		}
		StateManagerImpl stateManagerImpl = (StateManagerImpl)TrackerManager.Instance.GetStateManager();
		foreach (Trackable trackable in dataSet.GetTrackables())
		{
			stateManagerImpl.EnableTrackableBehavioursForTrackable(trackable, false);
		}
		mActiveDataSets.Remove(dataSetImpl);
		return true;
	}

	public override IEnumerable<DataSet> GetActiveDataSets()
	{
		return mActiveDataSets.Cast<DataSet>();
	}

	public override IEnumerable<DataSet> GetDataSets()
	{
		return mDataSets;
	}

	public override void DestroyAllDataSets(bool destroyTrackables)
	{
		List<DataSetImpl> list = new List<DataSetImpl>(mActiveDataSets);
		foreach (DataSetImpl item in list)
		{
			DeactivateDataSet(item);
		}
		for (int num = mDataSets.Count - 1; num >= 0; num--)
		{
			DestroyDataSet(mDataSets[num], destroyTrackables);
		}
		mDataSets.Clear();
	}
}
