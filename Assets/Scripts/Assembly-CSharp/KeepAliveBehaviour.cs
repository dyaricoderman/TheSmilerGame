using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(QCARBehaviour))]
public class KeepAliveBehaviour : MonoBehaviour
{
	[SerializeField]
	[HideInInspector]
	private bool mKeepARCameraAlive;

	[SerializeField]
	[HideInInspector]
	private bool mKeepTrackableBehavioursAlive;

	[HideInInspector]
	[SerializeField]
	private bool mKeepTextRecoBehaviourAlive;

	[HideInInspector]
	[SerializeField]
	private bool mKeepUDTBuildingBehaviourAlive;

	[SerializeField]
	[HideInInspector]
	private bool mKeepCloudRecoBehaviourAlive;

	private static KeepAliveBehaviour sKeepAliveBehaviour;

	private readonly List<ILoadLevelEventHandler> mHandlers = new List<ILoadLevelEventHandler>();

	public bool KeepARCameraAlive
	{
		get
		{
			return mKeepARCameraAlive;
		}
		set
		{
			if (!Application.isPlaying)
			{
				mKeepARCameraAlive = value;
			}
		}
	}

	public bool KeepTrackableBehavioursAlive
	{
		get
		{
			return mKeepTrackableBehavioursAlive;
		}
		set
		{
			if (!Application.isPlaying)
			{
				mKeepTrackableBehavioursAlive = value;
			}
		}
	}

	public bool KeepTextRecoBehaviourAlive
	{
		get
		{
			return mKeepTextRecoBehaviourAlive;
		}
		set
		{
			if (!Application.isPlaying)
			{
				mKeepTextRecoBehaviourAlive = value;
			}
		}
	}

	public bool KeepUDTBuildingBehaviourAlive
	{
		get
		{
			return mKeepUDTBuildingBehaviourAlive;
		}
		set
		{
			if (!Application.isPlaying)
			{
				mKeepUDTBuildingBehaviourAlive = value;
			}
		}
	}

	public bool KeepCloudRecoBehaviourAlive
	{
		get
		{
			return mKeepCloudRecoBehaviourAlive;
		}
		set
		{
			if (!Application.isPlaying)
			{
				mKeepCloudRecoBehaviourAlive = value;
			}
		}
	}

	public static KeepAliveBehaviour Instance
	{
		get
		{
			if (sKeepAliveBehaviour == null)
			{
				sKeepAliveBehaviour = (KeepAliveBehaviour)Object.FindObjectOfType(typeof(KeepAliveBehaviour));
			}
			return sKeepAliveBehaviour;
		}
	}

	public void RegisterEventHandler(ILoadLevelEventHandler eventHandler)
	{
		mHandlers.Add(eventHandler);
	}

	public bool UnregisterEventHandler(ILoadLevelEventHandler eventHandler)
	{
		return mHandlers.Remove(eventHandler);
	}

	private void OnLevelWasLoaded()
	{
		if (!mKeepARCameraAlive)
		{
			return;
		}
		StateManagerImpl stateManagerImpl = (StateManagerImpl)TrackerManager.Instance.GetStateManager();
		List<TrackableBehaviour> list;
		if (mKeepTrackableBehavioursAlive)
		{
			list = stateManagerImpl.GetTrackableBehaviours().ToList();
			foreach (WordBehaviour trackableBehaviour2 in stateManagerImpl.GetWordManager().GetTrackableBehaviours())
			{
				list.Add(trackableBehaviour2);
			}
		}
		else
		{
			list = new List<TrackableBehaviour>();
		}
		foreach (ILoadLevelEventHandler mHandler in mHandlers)
		{
			mHandler.OnLevelLoaded(list);
		}
		TrackableBehaviour[] array = (TrackableBehaviour[])Object.FindObjectsOfType(typeof(TrackableBehaviour));
		stateManagerImpl.RemoveDestroyedTrackables();
		stateManagerImpl.AssociateMarkerBehaviours();
		ImageTracker imageTracker = (ImageTracker)TrackerManager.Instance.GetTracker(Tracker.Type.IMAGE_TRACKER);
		if (imageTracker != null)
		{
			IEnumerable<DataSet> dataSets = imageTracker.GetDataSets();
			List<DataSet> list2 = imageTracker.GetActiveDataSets().ToList();
			foreach (DataSet item in dataSets)
			{
				if (list2.Contains(item))
				{
					imageTracker.DeactivateDataSet(item);
				}
				stateManagerImpl.AssociateTrackableBehavioursForDataSet(item);
				if (list2.Contains(item))
				{
					imageTracker.ActivateDataSet(item);
				}
			}
		}
		bool flag = false;
		TextRecoBehaviour textRecoBehaviour = (TextRecoBehaviour)Object.FindObjectOfType(typeof(TextRecoBehaviour));
		if (textRecoBehaviour != null)
		{
			if (!textRecoBehaviour.IsInitialized)
			{
				flag = true;
			}
			else
			{
				WordManagerImpl wordManagerImpl = (WordManagerImpl)stateManagerImpl.GetWordManager();
				wordManagerImpl.RemoveDestroyedTrackables();
				wordManagerImpl.InitializeWordBehaviourTemplates();
			}
		}
		List<TrackableBehaviour> list3 = new List<TrackableBehaviour>();
		IEnumerable<TrackableBehaviour> trackableBehaviours = stateManagerImpl.GetTrackableBehaviours();
		IEnumerable<WordBehaviour> trackableBehaviours2 = stateManagerImpl.GetWordManager().GetTrackableBehaviours();
		TrackableBehaviour[] array2 = array;
		foreach (TrackableBehaviour trackableBehaviour in array2)
		{
			if (trackableBehaviour is WordBehaviour)
			{
				if (!flag && !trackableBehaviours2.Contains(trackableBehaviour as WordBehaviour))
				{
					trackableBehaviour.gameObject.SetActiveRecursively(false);
					list3.Add(trackableBehaviour);
				}
			}
			else if ((!(trackableBehaviour is ImageTargetBehaviour) || ((IEditorImageTargetBehaviour)trackableBehaviour).ImageTargetType == ImageTargetType.PREDEFINED) && !trackableBehaviours.Contains(trackableBehaviour))
			{
				trackableBehaviour.gameObject.SetActiveRecursively(false);
				list3.Add(trackableBehaviour);
			}
		}
		foreach (ILoadLevelEventHandler mHandler2 in mHandlers)
		{
			mHandler2.OnDuplicateTrackablesDisabled(list3);
		}
	}
}
