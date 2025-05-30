using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using UnityEngine;

public class StateManagerImpl : StateManager
{
	private readonly Dictionary<int, TrackableBehaviour> mTrackableBehaviours = new Dictionary<int, TrackableBehaviour>();

	private readonly List<int> mAutomaticallyCreatedBehaviours = new List<int>();

	private readonly List<TrackableBehaviour> mBehavioursMarkedForDeletion = new List<TrackableBehaviour>();

	private readonly List<TrackableBehaviour> mActiveTrackableBehaviours = new List<TrackableBehaviour>();

	private readonly WordManagerImpl mWordManager = new WordManagerImpl();

	public override IEnumerable<TrackableBehaviour> GetActiveTrackableBehaviours()
	{
		return mActiveTrackableBehaviours;
	}

	public override IEnumerable<TrackableBehaviour> GetTrackableBehaviours()
	{
		return mTrackableBehaviours.Values;
	}

	public override WordManager GetWordManager()
	{
		return mWordManager;
	}

	public override void DestroyTrackableBehavioursForTrackable(Trackable trackable, bool destroyGameObjects = true)
	{
		TrackableBehaviour value;
		if (mTrackableBehaviours.TryGetValue(trackable.ID, out value))
		{
			if (destroyGameObjects)
			{
				mBehavioursMarkedForDeletion.Add(mTrackableBehaviours[trackable.ID]);
				UnityEngine.Object.Destroy(value.gameObject);
			}
			else
			{
				IEditorTrackableBehaviour editorTrackableBehaviour = value;
				editorTrackableBehaviour.UnregisterTrackable();
			}
			mTrackableBehaviours.Remove(trackable.ID);
			mAutomaticallyCreatedBehaviours.Remove(trackable.ID);
		}
	}

	public void AssociateMarkerBehaviours()
	{
		MarkerTrackerImpl markerTrackerImpl = (MarkerTrackerImpl)TrackerManager.Instance.GetTracker(Tracker.Type.MARKER_TRACKER);
		if (markerTrackerImpl == null)
		{
			return;
		}
		MarkerBehaviour[] array = (MarkerBehaviour[])UnityEngine.Object.FindObjectsOfType(typeof(MarkerBehaviour));
		MarkerBehaviour[] array2 = array;
		foreach (MarkerBehaviour markerBehaviour in array2)
		{
			if (mBehavioursMarkedForDeletion.Contains(markerBehaviour))
			{
				mTrackableBehaviours.Remove(markerBehaviour.Trackable.ID);
				mBehavioursMarkedForDeletion.Remove(markerBehaviour);
				continue;
			}
			IEditorMarkerBehaviour editorMarkerBehaviour = markerBehaviour;
			Marker markerByMarkerID = markerTrackerImpl.GetMarkerByMarkerID(editorMarkerBehaviour.MarkerID);
			if (markerByMarkerID != null)
			{
				InitializeMarkerBehaviour(markerBehaviour, markerByMarkerID);
				continue;
			}
			markerByMarkerID = markerTrackerImpl.InternalCreateMarker(editorMarkerBehaviour.MarkerID, editorMarkerBehaviour.TrackableName, editorMarkerBehaviour.transform.localScale.x);
			if (markerByMarkerID == null)
			{
				Debug.LogWarning("Disabling MarkerBehaviour named " + editorMarkerBehaviour.TrackableName);
				markerBehaviour.enabled = false;
			}
			else
			{
				InitializeMarkerBehaviour(markerBehaviour, markerByMarkerID);
				markerBehaviour.enabled = true;
			}
		}
	}

	public void AssociateTrackableBehavioursForDataSet(DataSet dataSet)
	{
		DataSetTrackableBehaviour[] array = (DataSetTrackableBehaviour[])UnityEngine.Object.FindObjectsOfType(typeof(DataSetTrackableBehaviour));
		DataSetTrackableBehaviour[] array2 = array;
		foreach (DataSetTrackableBehaviour dataSetTrackableBehaviour in array2)
		{
			if (mBehavioursMarkedForDeletion.Contains(dataSetTrackableBehaviour))
			{
				continue;
			}
			IEditorDataSetTrackableBehaviour editorDataSetTrackableBehaviour = dataSetTrackableBehaviour;
			if (editorDataSetTrackableBehaviour.TrackableName == null)
			{
				Debug.LogError("Found Trackable without name.");
			}
			else
			{
				if (!editorDataSetTrackableBehaviour.DataSetPath.Equals(dataSet.Path))
				{
					continue;
				}
				bool flag = false;
				foreach (Trackable trackable in dataSet.GetTrackables())
				{
					if (!trackable.Name.Equals(editorDataSetTrackableBehaviour.TrackableName))
					{
						continue;
					}
					if (mTrackableBehaviours.ContainsKey(trackable.ID))
					{
						if (!mAutomaticallyCreatedBehaviours.Contains(trackable.ID) && !mBehavioursMarkedForDeletion.Contains(mTrackableBehaviours[trackable.ID]))
						{
							flag = true;
							continue;
						}
						UnityEngine.Object.Destroy(mTrackableBehaviours[trackable.ID].gameObject);
						mTrackableBehaviours.Remove(trackable.ID);
						mAutomaticallyCreatedBehaviours.Remove(trackable.ID);
					}
					if (dataSetTrackableBehaviour is ImageTargetBehaviour && trackable is ImageTarget)
					{
						IEditorImageTargetBehaviour editorImageTargetBehaviour = (ImageTargetBehaviour)dataSetTrackableBehaviour;
						flag = true;
						editorImageTargetBehaviour.InitializeImageTarget((ImageTarget)trackable);
						mTrackableBehaviours[trackable.ID] = dataSetTrackableBehaviour;
						Debug.Log("Found Trackable named " + dataSetTrackableBehaviour.Trackable.Name + " with id " + dataSetTrackableBehaviour.Trackable.ID);
					}
					else if (dataSetTrackableBehaviour is MultiTargetBehaviour && trackable is MultiTarget)
					{
						flag = true;
						IEditorMultiTargetBehaviour editorMultiTargetBehaviour = (MultiTargetBehaviour)dataSetTrackableBehaviour;
						editorMultiTargetBehaviour.InitializeMultiTarget((MultiTarget)trackable);
						mTrackableBehaviours[trackable.ID] = dataSetTrackableBehaviour;
						Debug.Log("Found Trackable named " + dataSetTrackableBehaviour.Trackable.Name + " with id " + dataSetTrackableBehaviour.Trackable.ID);
					}
					else if (dataSetTrackableBehaviour is CylinderTargetBehaviour && trackable is CylinderTarget)
					{
						flag = true;
						IEditorCylinderTargetBehaviour editorCylinderTargetBehaviour = (CylinderTargetBehaviour)dataSetTrackableBehaviour;
						editorCylinderTargetBehaviour.InitializeCylinderTarget((CylinderTarget)trackable);
						mTrackableBehaviours[trackable.ID] = dataSetTrackableBehaviour;
						Debug.Log("Found Trackable named " + dataSetTrackableBehaviour.Trackable.Name + " with id " + dataSetTrackableBehaviour.Trackable.ID);
					}
				}
				if (!flag)
				{
					Debug.LogError("Could not associate DataSetTrackableBehaviour '" + editorDataSetTrackableBehaviour.TrackableName + "' - no matching Trackable found in DataSet!");
				}
			}
		}
		VirtualButtonBehaviour[] vbBehaviours = (VirtualButtonBehaviour[])UnityEngine.Object.FindObjectsOfType(typeof(VirtualButtonBehaviour));
		AssociateVirtualButtonBehaviours(vbBehaviours, dataSet);
		CreateMissingDataSetTrackableBehaviours(dataSet);
	}

	public void RemoveDestroyedTrackables()
	{
		int[] array = mTrackableBehaviours.Keys.ToArray();
		int[] array2 = array;
		foreach (int num in array2)
		{
			if (mTrackableBehaviours[num] == null)
			{
				mTrackableBehaviours.Remove(num);
				mAutomaticallyCreatedBehaviours.Remove(num);
			}
		}
	}

	public void ClearTrackableBehaviours()
	{
		mTrackableBehaviours.Clear();
		mActiveTrackableBehaviours.Clear();
		mAutomaticallyCreatedBehaviours.Clear();
		mBehavioursMarkedForDeletion.Clear();
	}

	public ImageTargetBehaviour FindOrCreateImageTargetBehaviourForTrackable(ImageTarget trackable, GameObject gameObject)
	{
		return FindOrCreateImageTargetBehaviourForTrackable(trackable, gameObject, null);
	}

	public ImageTargetBehaviour FindOrCreateImageTargetBehaviourForTrackable(ImageTarget trackable, GameObject gameObject, DataSet dataSet)
	{
		DataSetTrackableBehaviour dataSetTrackableBehaviour = gameObject.GetComponent<DataSetTrackableBehaviour>();
		if (dataSetTrackableBehaviour == null)
		{
			dataSetTrackableBehaviour = gameObject.AddComponent<ImageTargetBehaviour>();
			((IEditorTrackableBehaviour)dataSetTrackableBehaviour).SetInitializedInEditor(true);
		}
		if (!(dataSetTrackableBehaviour is ImageTargetBehaviour))
		{
			Debug.LogError(string.Format("DataSet.CreateTrackable: Trackable of type ImageTarget was created, but behaviour of type {0} was provided!", dataSetTrackableBehaviour.GetType()));
			return null;
		}
		IEditorImageTargetBehaviour editorImageTargetBehaviour = (ImageTargetBehaviour)dataSetTrackableBehaviour;
		if (dataSet != null)
		{
			editorImageTargetBehaviour.SetDataSetPath(dataSet.Path);
		}
		editorImageTargetBehaviour.SetImageTargetType(trackable.ImageTargetType);
		editorImageTargetBehaviour.SetNameForTrackable(trackable.Name);
		editorImageTargetBehaviour.InitializeImageTarget(trackable);
		mTrackableBehaviours[trackable.ID] = dataSetTrackableBehaviour;
		return dataSetTrackableBehaviour as ImageTargetBehaviour;
	}

	public MarkerBehaviour CreateNewMarkerBehaviourForMarker(Marker trackable, string gameObjectName)
	{
		GameObject gameObject = new GameObject(gameObjectName);
		return CreateNewMarkerBehaviourForMarker(trackable, gameObject);
	}

	public MarkerBehaviour CreateNewMarkerBehaviourForMarker(Marker trackable, GameObject gameObject)
	{
		MarkerBehaviour markerBehaviour = gameObject.AddComponent<MarkerBehaviour>();
		float size = trackable.GetSize();
		Debug.Log("Creating Marker with values: \n MarkerID:     " + trackable.MarkerID + "\n TrackableID:  " + trackable.ID + "\n Name:         " + trackable.Name + "\n Size:         " + size + "x" + size);
		IEditorMarkerBehaviour editorMarkerBehaviour = markerBehaviour;
		editorMarkerBehaviour.SetMarkerID(trackable.MarkerID);
		editorMarkerBehaviour.SetNameForTrackable(trackable.Name);
		editorMarkerBehaviour.transform.localScale = new Vector3(size, size, size);
		editorMarkerBehaviour.InitializeMarker(trackable);
		mTrackableBehaviours[trackable.ID] = markerBehaviour;
		return markerBehaviour;
	}

	public void SetTrackableBehavioursForTrackableToNotFound(Trackable trackable)
	{
		TrackableBehaviour value;
		if (mTrackableBehaviours.TryGetValue(trackable.ID, out value))
		{
			value.OnTrackerUpdate(TrackableBehaviour.Status.NOT_FOUND);
		}
	}

	public void EnableTrackableBehavioursForTrackable(Trackable trackable, bool enabled)
	{
		TrackableBehaviour value;
		if (mTrackableBehaviours.TryGetValue(trackable.ID, out value) && value != null)
		{
			value.enabled = enabled;
		}
	}

	public void RemoveDisabledTrackablesFromQueue(ref LinkedList<int> trackableIDs)
	{
		LinkedListNode<int> linkedListNode = trackableIDs.First;
		while (linkedListNode != null)
		{
			LinkedListNode<int> next = linkedListNode.Next;
			TrackableBehaviour value;
			if (mTrackableBehaviours.TryGetValue(linkedListNode.Value, out value) && !value.enabled)
			{
				trackableIDs.Remove(linkedListNode);
			}
			linkedListNode = next;
		}
	}

	public void UpdateCameraPose(Camera arCamera, QCARManagerImpl.TrackableResultData[] trackableResultDataArray, int originTrackableID)
	{
		if (originTrackableID < 0)
		{
			return;
		}
		for (int i = 0; i < trackableResultDataArray.Length; i++)
		{
			QCARManagerImpl.TrackableResultData trackableResultData = trackableResultDataArray[i];
			if (trackableResultData.id == originTrackableID)
			{
				TrackableBehaviour value;
				if ((trackableResultData.status == TrackableBehaviour.Status.DETECTED || trackableResultData.status == TrackableBehaviour.Status.TRACKED) && mTrackableBehaviours.TryGetValue(originTrackableID, out value) && value.enabled)
				{
					PositionCamera(value, arCamera, trackableResultData.pose);
				}
				break;
			}
		}
	}

	public void UpdateTrackablePoses(Camera arCamera, QCARManagerImpl.TrackableResultData[] trackableResultDataArray, int originTrackableID, int frameIndex)
	{
		Dictionary<int, QCARManagerImpl.TrackableResultData> dictionary = new Dictionary<int, QCARManagerImpl.TrackableResultData>();
		for (int i = 0; i < trackableResultDataArray.Length; i++)
		{
			QCARManagerImpl.TrackableResultData value = trackableResultDataArray[i];
			dictionary.Add(value.id, value);
			TrackableBehaviour value2;
			if (mTrackableBehaviours.TryGetValue(value.id, out value2) && value.id != originTrackableID && (value.status == TrackableBehaviour.Status.DETECTED || value.status == TrackableBehaviour.Status.TRACKED) && value2.enabled)
			{
				PositionTrackable(value2, arCamera, value.pose);
			}
		}
		mActiveTrackableBehaviours.Clear();
		foreach (TrackableBehaviour value4 in mTrackableBehaviours.Values)
		{
			if (value4.enabled)
			{
				QCARManagerImpl.TrackableResultData value3;
				if (dictionary.TryGetValue(value4.Trackable.ID, out value3))
				{
					value4.OnTrackerUpdate(value3.status);
					value4.OnFrameIndexUpdate(frameIndex);
				}
				else
				{
					value4.OnTrackerUpdate(TrackableBehaviour.Status.NOT_FOUND);
				}
				if (value4.CurrentStatus == TrackableBehaviour.Status.TRACKED || value4.CurrentStatus == TrackableBehaviour.Status.DETECTED)
				{
					mActiveTrackableBehaviours.Add(value4);
				}
			}
		}
	}

	public void UpdateVirtualButtons(int numVirtualButtons, IntPtr virtualButtonPtr)
	{
		Dictionary<int, QCARManagerImpl.VirtualButtonData> dictionary = new Dictionary<int, QCARManagerImpl.VirtualButtonData>();
		for (int i = 0; i < numVirtualButtons; i++)
		{
			IntPtr ptr = new IntPtr(virtualButtonPtr.ToInt32() + i * Marshal.SizeOf(typeof(QCARManagerImpl.VirtualButtonData)));
			QCARManagerImpl.VirtualButtonData value = (QCARManagerImpl.VirtualButtonData)Marshal.PtrToStructure(ptr, typeof(QCARManagerImpl.VirtualButtonData));
			dictionary.Add(value.id, value);
		}
		List<VirtualButtonBehaviour> list = new List<VirtualButtonBehaviour>();
		foreach (TrackableBehaviour value3 in mTrackableBehaviours.Values)
		{
			ImageTargetBehaviour imageTargetBehaviour = value3 as ImageTargetBehaviour;
			if (!(imageTargetBehaviour != null) || !imageTargetBehaviour.enabled)
			{
				continue;
			}
			foreach (VirtualButtonBehaviour virtualButtonBehaviour in imageTargetBehaviour.GetVirtualButtonBehaviours())
			{
				if (virtualButtonBehaviour.enabled)
				{
					list.Add(virtualButtonBehaviour);
				}
			}
		}
		foreach (VirtualButtonBehaviour item in list)
		{
			QCARManagerImpl.VirtualButtonData value2;
			if (dictionary.TryGetValue(item.VirtualButton.ID, out value2))
			{
				item.OnTrackerUpdated(value2.isPressed > 0);
			}
			else
			{
				item.OnTrackerUpdated(false);
			}
		}
	}

	public void UpdateWords(Camera arCamera, QCARManagerImpl.WordData[] wordData, QCARManagerImpl.WordResultData[] wordResultData)
	{
		mWordManager.UpdateWords(arCamera, wordData, wordResultData);
	}

	private void AssociateVirtualButtonBehaviours(VirtualButtonBehaviour[] vbBehaviours, DataSet dataSet)
	{
		for (int i = 0; i < vbBehaviours.Length; i++)
		{
			VirtualButtonBehaviour virtualButtonBehaviour = vbBehaviours[i];
			if (virtualButtonBehaviour.VirtualButtonName == null)
			{
				Debug.LogError("VirtualButton at " + i + " has no name.");
				continue;
			}
			ImageTargetBehaviour imageTargetBehaviour = virtualButtonBehaviour.GetImageTargetBehaviour();
			if (imageTargetBehaviour == null)
			{
				Debug.LogError("VirtualButton named " + virtualButtonBehaviour.VirtualButtonName + " is not attached to an ImageTarget.");
			}
			else if (dataSet.Contains(imageTargetBehaviour.Trackable))
			{
				((IEditorImageTargetBehaviour)imageTargetBehaviour).AssociateExistingVirtualButtonBehaviour(virtualButtonBehaviour);
			}
		}
	}

	private void CreateMissingDataSetTrackableBehaviours(DataSet dataSet)
	{
		foreach (Trackable trackable in dataSet.GetTrackables())
		{
			if (!mTrackableBehaviours.ContainsKey(trackable.ID))
			{
				if (trackable is ImageTarget)
				{
					ImageTargetBehaviour imageTargetBehaviour = CreateImageTargetBehaviour((ImageTarget)trackable);
					((IEditorImageTargetBehaviour)imageTargetBehaviour).CreateMissingVirtualButtonBehaviours();
					mTrackableBehaviours[trackable.ID] = imageTargetBehaviour;
					mAutomaticallyCreatedBehaviours.Add(trackable.ID);
				}
				else if (trackable is MultiTarget)
				{
					MultiTargetBehaviour value = CreateMultiTargetBehaviour((MultiTarget)trackable);
					mTrackableBehaviours[trackable.ID] = value;
					mAutomaticallyCreatedBehaviours.Add(trackable.ID);
				}
				else if (trackable is CylinderTarget)
				{
					CylinderTargetBehaviour value2 = CreateCylinderTargetBehaviour((CylinderTarget)trackable);
					mTrackableBehaviours[trackable.ID] = value2;
					mAutomaticallyCreatedBehaviours.Add(trackable.ID);
				}
			}
		}
	}

	private ImageTargetBehaviour CreateImageTargetBehaviour(ImageTarget imageTarget)
	{
		GameObject gameObject = new GameObject();
		ImageTargetBehaviour imageTargetBehaviour = gameObject.AddComponent<ImageTargetBehaviour>();
		IEditorImageTargetBehaviour editorImageTargetBehaviour = imageTargetBehaviour;
		Debug.Log("Creating Image Target with values: \n ID:           " + imageTarget.ID + "\n Name:         " + imageTarget.Name + "\n Path:         " + editorImageTargetBehaviour.DataSetPath + "\n Size:         " + imageTarget.GetSize().x + "x" + imageTarget.GetSize().y);
		editorImageTargetBehaviour.SetNameForTrackable(imageTarget.Name);
		editorImageTargetBehaviour.SetDataSetPath(editorImageTargetBehaviour.DataSetPath);
		editorImageTargetBehaviour.transform.localScale = new Vector3(imageTarget.GetSize().x, 1f, imageTarget.GetSize().y);
		editorImageTargetBehaviour.CorrectScale();
		editorImageTargetBehaviour.SetAspectRatio(imageTarget.GetSize()[1] / imageTarget.GetSize()[0]);
		editorImageTargetBehaviour.InitializeImageTarget(imageTarget);
		return imageTargetBehaviour;
	}

	private MultiTargetBehaviour CreateMultiTargetBehaviour(MultiTarget multiTarget)
	{
		GameObject gameObject = new GameObject();
		MultiTargetBehaviour multiTargetBehaviour = gameObject.AddComponent<MultiTargetBehaviour>();
		IEditorMultiTargetBehaviour editorMultiTargetBehaviour = multiTargetBehaviour;
		Debug.Log("Creating Multi Target with values: \n ID:           " + multiTarget.ID + "\n Name:         " + multiTarget.Name + "\n Path:         " + editorMultiTargetBehaviour.DataSetPath);
		editorMultiTargetBehaviour.SetNameForTrackable(multiTarget.Name);
		editorMultiTargetBehaviour.SetDataSetPath(editorMultiTargetBehaviour.DataSetPath);
		editorMultiTargetBehaviour.InitializeMultiTarget(multiTarget);
		return multiTargetBehaviour;
	}

	private CylinderTargetBehaviour CreateCylinderTargetBehaviour(CylinderTarget cylinderTarget)
	{
		GameObject gameObject = new GameObject();
		CylinderTargetBehaviour cylinderTargetBehaviour = gameObject.AddComponent<CylinderTargetBehaviour>();
		IEditorCylinderTargetBehaviour editorCylinderTargetBehaviour = cylinderTargetBehaviour;
		Debug.Log("Creating Cylinder Target with values: \n ID:           " + cylinderTarget.ID + "\n Name:         " + cylinderTarget.Name + "\n Path:         " + editorCylinderTargetBehaviour.DataSetPath + "\n Side Length:  " + cylinderTarget.GetSideLength() + "\n Top Diameter: " + cylinderTarget.GetTopDiameter() + "\n Bottom Diam.: " + cylinderTarget.GetBottomDiameter());
		editorCylinderTargetBehaviour.SetNameForTrackable(cylinderTarget.Name);
		editorCylinderTargetBehaviour.SetDataSetPath(editorCylinderTargetBehaviour.DataSetPath);
		float sideLength = cylinderTarget.GetSideLength();
		editorCylinderTargetBehaviour.transform.localScale = new Vector3(sideLength, sideLength, sideLength);
		editorCylinderTargetBehaviour.CorrectScale();
		editorCylinderTargetBehaviour.SetAspectRatio(cylinderTarget.GetTopDiameter() / sideLength, cylinderTarget.GetBottomDiameter() / sideLength);
		editorCylinderTargetBehaviour.InitializeCylinderTarget(cylinderTarget);
		return cylinderTargetBehaviour;
	}

	private void InitializeMarkerBehaviour(MarkerBehaviour markerBehaviour, Marker marker)
	{
		((IEditorMarkerBehaviour)markerBehaviour).InitializeMarker(marker);
		if (!mTrackableBehaviours.ContainsKey(marker.ID))
		{
			mTrackableBehaviours[marker.ID] = markerBehaviour;
			Debug.Log("Found Marker named " + marker.Name + " with id " + marker.ID);
		}
	}

	private void PositionCamera(TrackableBehaviour trackableBehaviour, Camera arCamera, QCARManagerImpl.PoseData camToTargetPose)
	{
		arCamera.transform.localPosition = trackableBehaviour.transform.rotation * Quaternion.AngleAxis(90f, Vector3.left) * Quaternion.Inverse(camToTargetPose.orientation) * -camToTargetPose.position + trackableBehaviour.transform.position;
		arCamera.transform.rotation = trackableBehaviour.transform.rotation * Quaternion.AngleAxis(90f, Vector3.left) * Quaternion.Inverse(camToTargetPose.orientation);
	}

	private void PositionTrackable(TrackableBehaviour trackableBehaviour, Camera arCamera, QCARManagerImpl.PoseData camToTargetPose)
	{
		trackableBehaviour.transform.position = arCamera.transform.TransformPoint(camToTargetPose.position);
		trackableBehaviour.transform.rotation = arCamera.transform.rotation * camToTargetPose.orientation * Quaternion.AngleAxis(270f, Vector3.left);
	}
}
