using System.Collections.Generic;
using UnityEngine;

public abstract class TrackableBehaviour : MonoBehaviour, IEditorTrackableBehaviour
{
	public enum Status
	{
		NOT_FOUND = -1,
		UNKNOWN = 0,
		UNDEFINED = 1,
		DETECTED = 2,
		TRACKED = 3
	}

	[SerializeField]
	[HideInInspector]
	protected string mTrackableName = string.Empty;

	[HideInInspector]
	[SerializeField]
	protected Vector3 mPreviousScale = Vector3.zero;

	[HideInInspector]
	[SerializeField]
	protected bool mPreserveChildSize;

	[SerializeField]
	[HideInInspector]
	protected bool mInitializedInEditor;

	protected Status mStatus;

	protected Trackable mTrackable;

	private List<ITrackableEventHandler> mTrackableEventHandlers = new List<ITrackableEventHandler>();

	Vector3 IEditorTrackableBehaviour.PreviousScale
	{
		get
		{
			return mPreviousScale;
		}
	}

	bool IEditorTrackableBehaviour.PreserveChildSize
	{
		get
		{
			return mPreserveChildSize;
		}
	}

	bool IEditorTrackableBehaviour.InitializedInEditor
	{
		get
		{
			return mInitializedInEditor;
		}
	}

	public Status CurrentStatus
	{
		get
		{
			return mStatus;
		}
	}

	public Trackable Trackable
	{
		get
		{
			return mTrackable;
		}
	}

	public string TrackableName
	{
		get
		{
			return mTrackableName;
		}
	}

	public bool enabled
	{
		get
		{
			return base.enabled;
		}
		set
		{
			base.enabled = value;
		}
	}

	public Transform transform
	{
		get
		{
			return base.transform;
		}
	}

	public GameObject gameObject
	{
		get
		{
			return base.gameObject;
		}
	}

	public Renderer renderer
	{
		get
		{
			return base.GetComponent<Renderer>();
		}
	}

	bool IEditorTrackableBehaviour.CorrectScale()
	{
		return CorrectScaleImpl();
	}

	bool IEditorTrackableBehaviour.SetNameForTrackable(string name)
	{
		if (mTrackable == null)
		{
			mTrackableName = name;
			return true;
		}
		return false;
	}

	bool IEditorTrackableBehaviour.SetPreviousScale(Vector3 previousScale)
	{
		if (Trackable == null)
		{
			mPreviousScale = previousScale;
			return true;
		}
		return false;
	}

	bool IEditorTrackableBehaviour.SetPreserveChildSize(bool preserveChildSize)
	{
		if (Trackable == null)
		{
			mPreserveChildSize = preserveChildSize;
			return true;
		}
		return false;
	}

	bool IEditorTrackableBehaviour.SetInitializedInEditor(bool initializedInEditor)
	{
		if (Trackable == null)
		{
			mInitializedInEditor = initializedInEditor;
			return true;
		}
		return false;
	}

	void IEditorTrackableBehaviour.UnregisterTrackable()
	{
		InternalUnregisterTrackable();
	}

	public void RegisterTrackableEventHandler(ITrackableEventHandler trackableEventHandler)
	{
		mTrackableEventHandlers.Add(trackableEventHandler);
		trackableEventHandler.OnTrackableStateChanged(Status.UNKNOWN, mStatus);
	}

	public bool UnregisterTrackableEventHandler(ITrackableEventHandler trackableEventHandler)
	{
		return mTrackableEventHandlers.Remove(trackableEventHandler);
	}

	public void OnTrackerUpdate(Status newStatus)
	{
		Status status = mStatus;
		mStatus = newStatus;
		if (status == newStatus)
		{
			return;
		}
		foreach (ITrackableEventHandler mTrackableEventHandler in mTrackableEventHandlers)
		{
			mTrackableEventHandler.OnTrackableStateChanged(status, newStatus);
		}
	}

	public virtual void OnFrameIndexUpdate(int newFrameIndex)
	{
	}

	protected abstract void InternalUnregisterTrackable();

	private void Start()
	{
		if (KeepAliveBehaviour.Instance != null && KeepAliveBehaviour.Instance.KeepTrackableBehavioursAlive)
		{
			Object.DontDestroyOnLoad(base.gameObject);
		}
	}

	private void OnDisable()
	{
		Status status = mStatus;
		mStatus = Status.NOT_FOUND;
		if (status == Status.NOT_FOUND)
		{
			return;
		}
		foreach (ITrackableEventHandler mTrackableEventHandler in mTrackableEventHandlers)
		{
			mTrackableEventHandler.OnTrackableStateChanged(status, Status.NOT_FOUND);
		}
	}

	protected virtual bool CorrectScaleImpl()
	{
		return false;
	}
}
