using System.Collections.Generic;
using UnityEngine;

public class MarkerTrackerImpl : MarkerTracker
{
	private readonly Dictionary<int, Marker> mMarkerDict = new Dictionary<int, Marker>();

	public override bool Start()
	{
		if (QCARWrapper.Instance.MarkerTrackerStart() == 0)
		{
			Debug.LogError("Could not start tracker.");
			return false;
		}
		return true;
	}

	public override void Stop()
	{
		QCARWrapper.Instance.MarkerTrackerStop();
		StateManagerImpl stateManagerImpl = (StateManagerImpl)TrackerManager.Instance.GetStateManager();
		foreach (Marker value in mMarkerDict.Values)
		{
			stateManagerImpl.SetTrackableBehavioursForTrackableToNotFound(value);
		}
	}

	public override MarkerBehaviour CreateMarker(int markerID, string trackableName, float size)
	{
		int num = RegisterMarker(markerID, trackableName, size);
		if (num == -1)
		{
			Debug.LogError("Could not create marker with id " + markerID + ".");
			return null;
		}
		Marker marker = new MarkerImpl(trackableName, num, size, markerID);
		mMarkerDict[num] = marker;
		StateManagerImpl stateManagerImpl = (StateManagerImpl)TrackerManager.Instance.GetStateManager();
		return stateManagerImpl.CreateNewMarkerBehaviourForMarker(marker, trackableName);
	}

	public override bool DestroyMarker(Marker marker, bool destroyGameObject)
	{
		if (QCARWrapper.Instance.MarkerTrackerDestroyMarker(marker.ID) == 0)
		{
			Debug.LogError("Could not destroy marker with id " + marker.MarkerID + ".");
			return false;
		}
		mMarkerDict.Remove(marker.ID);
		if (destroyGameObject)
		{
			StateManager stateManager = TrackerManager.Instance.GetStateManager();
			stateManager.DestroyTrackableBehavioursForTrackable(marker);
		}
		return true;
	}

	public override IEnumerable<Marker> GetMarkers()
	{
		return mMarkerDict.Values;
	}

	public override Marker GetMarkerByMarkerID(int markerID)
	{
		foreach (Marker value in mMarkerDict.Values)
		{
			if (value.MarkerID == markerID)
			{
				return value;
			}
		}
		return null;
	}

	public Marker InternalCreateMarker(int markerID, string name, float size)
	{
		int num = RegisterMarker(markerID, name, size);
		if (num == -1)
		{
			Debug.LogWarning("Marker named " + name + " could not be created");
			return null;
		}
		if (!mMarkerDict.ContainsKey(num))
		{
			Marker marker = new MarkerImpl(name, num, size, markerID);
			mMarkerDict[num] = marker;
			Debug.Log("Created Marker named " + marker.Name + " with id " + marker.ID);
		}
		return mMarkerDict[num];
	}

	public override void DestroyAllMarkers(bool destroyGameObject)
	{
		List<Marker> list = new List<Marker>(mMarkerDict.Values);
		foreach (Marker item in list)
		{
			DestroyMarker(item, destroyGameObject);
		}
	}

	private int RegisterMarker(int markerID, string trackableName, float size)
	{
		return QCARWrapper.Instance.MarkerTrackerCreateMarker(markerID, trackableName, size);
	}
}
