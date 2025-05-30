using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using UnityEngine;

public class DataSetImpl : DataSet
{
	private IntPtr mDataSetPtr = IntPtr.Zero;

	private string mPath = string.Empty;

	private StorageType mStorageType = StorageType.STORAGE_APPRESOURCE;

	private readonly Dictionary<int, Trackable> mTrackablesDict = new Dictionary<int, Trackable>();

	public IntPtr DataSetPtr
	{
		get
		{
			return mDataSetPtr;
		}
	}

	public override string Path
	{
		get
		{
			return mPath;
		}
	}

	public override StorageType FileStorageType
	{
		get
		{
			return mStorageType;
		}
	}

	public DataSetImpl(IntPtr dataSetPtr)
	{
		mDataSetPtr = dataSetPtr;
	}

	public static bool ExistsImpl(string path, StorageType storageType)
	{
		if (storageType == StorageType.STORAGE_APPRESOURCE && QCARRuntimeUtilities.IsPlayMode())
		{
			path = "Assets/StreamingAssets/" + path;
		}
		return QCARWrapper.Instance.DataSetExists(path, (int)storageType) == 1;
	}

	public override bool Load(string name)
	{
		string path = "QCAR/" + name + ".xml";
		return Load(path, StorageType.STORAGE_APPRESOURCE);
	}

	public override bool Load(string path, StorageType storageType)
	{
		if (mDataSetPtr == IntPtr.Zero)
		{
			Debug.LogError("Called Load without a data set object");
			return false;
		}
		string text = path;
		if (storageType == StorageType.STORAGE_APPRESOURCE && QCARRuntimeUtilities.IsPlayMode())
		{
			text = "Assets/StreamingAssets/" + text;
		}
		if (QCARWrapper.Instance.DataSetLoad(text, (int)storageType, mDataSetPtr) == 0)
		{
			Debug.LogError("Did not load: " + path);
			return false;
		}
		mPath = path;
		mStorageType = storageType;
		CreateImageTargets();
		CreateMultiTargets();
		CreateCylinderTargets();
		StateManagerImpl stateManagerImpl = (StateManagerImpl)TrackerManager.Instance.GetStateManager();
		stateManagerImpl.AssociateTrackableBehavioursForDataSet(this);
		return true;
	}

	public override IEnumerable<Trackable> GetTrackables()
	{
		return mTrackablesDict.Values;
	}

	public override DataSetTrackableBehaviour CreateTrackable(TrackableSource trackableSource, string gameObjectName)
	{
		GameObject gameObject = new GameObject(gameObjectName);
		return CreateTrackable(trackableSource, gameObject);
	}

	public override DataSetTrackableBehaviour CreateTrackable(TrackableSource trackableSource, GameObject gameObject)
	{
		TrackableSourceImpl trackableSourceImpl = (TrackableSourceImpl)trackableSource;
		int num = 128;
		StringBuilder stringBuilder = new StringBuilder(num);
		IntPtr intPtr = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(SimpleTargetData)));
		TrackableType trackableType = (TrackableType)QCARWrapper.Instance.DataSetCreateTrackable(mDataSetPtr, trackableSourceImpl.TrackableSourcePtr, stringBuilder, num, intPtr);
		SimpleTargetData simpleTargetData = (SimpleTargetData)Marshal.PtrToStructure(intPtr, typeof(SimpleTargetData));
		Marshal.FreeHGlobal(intPtr);
		if (trackableType == TrackableType.IMAGE_TARGET)
		{
			ImageTarget imageTarget = new ImageTargetImpl(stringBuilder.ToString(), simpleTargetData.id, ImageTargetType.USER_DEFINED, this);
			mTrackablesDict[simpleTargetData.id] = imageTarget;
			Debug.Log(string.Format("Trackable created: {0}, {1}", trackableType, stringBuilder));
			StateManagerImpl stateManagerImpl = (StateManagerImpl)TrackerManager.Instance.GetStateManager();
			return stateManagerImpl.FindOrCreateImageTargetBehaviourForTrackable(imageTarget, gameObject, this);
		}
		Debug.LogError("DataSet.CreateTrackable returned unknown or incompatible trackable type!");
		return null;
	}

	public override bool Destroy(Trackable trackable, bool destroyGameObject)
	{
		if (QCARWrapper.Instance.DataSetDestroyTrackable(mDataSetPtr, trackable.ID) == 0)
		{
			Debug.LogError("Could not destroy trackable with id " + trackable.ID + ".");
			return false;
		}
		mTrackablesDict.Remove(trackable.ID);
		if (destroyGameObject)
		{
			StateManager stateManager = TrackerManager.Instance.GetStateManager();
			stateManager.DestroyTrackableBehavioursForTrackable(trackable);
		}
		return true;
	}

	public override bool HasReachedTrackableLimit()
	{
		return QCARWrapper.Instance.DataSetHasReachedTrackableLimit(mDataSetPtr) == 1;
	}

	public override bool Contains(Trackable trackable)
	{
		return mTrackablesDict.ContainsValue(trackable);
	}

	public override void DestroyAllTrackables(bool destroyGameObject)
	{
		List<Trackable> list = new List<Trackable>(mTrackablesDict.Values);
		foreach (Trackable item in list)
		{
			Destroy(item, destroyGameObject);
		}
	}

	private void CreateImageTargets()
	{
		int num = QCARWrapper.Instance.DataSetGetNumTrackableType(1, mDataSetPtr);
		IntPtr intPtr = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(ImageTargetData)) * num);
		if (QCARWrapper.Instance.DataSetGetTrackablesOfType(1, intPtr, num, mDataSetPtr) == 0)
		{
			Debug.LogError("Could not create Image Targets");
			return;
		}
		for (int i = 0; i < num; i++)
		{
			IntPtr ptr = new IntPtr(intPtr.ToInt32() + i * Marshal.SizeOf(typeof(ImageTargetData)));
			ImageTargetData imageTargetData = (ImageTargetData)Marshal.PtrToStructure(ptr, typeof(ImageTargetData));
			if (!mTrackablesDict.ContainsKey(imageTargetData.id))
			{
				int num2 = 128;
				StringBuilder stringBuilder = new StringBuilder(num2);
				QCARWrapper.Instance.DataSetGetTrackableName(mDataSetPtr, imageTargetData.id, stringBuilder, num2);
				ImageTarget value = new ImageTargetImpl(stringBuilder.ToString(), imageTargetData.id, ImageTargetType.PREDEFINED, this);
				mTrackablesDict[imageTargetData.id] = value;
			}
		}
		Marshal.FreeHGlobal(intPtr);
	}

	private void CreateMultiTargets()
	{
		int num = QCARWrapper.Instance.DataSetGetNumTrackableType(2, mDataSetPtr);
		if (num <= 0)
		{
			return;
		}
		IntPtr intPtr = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(SimpleTargetData)) * num);
		if (QCARWrapper.Instance.DataSetGetTrackablesOfType(2, intPtr, num, mDataSetPtr) == 0)
		{
			Debug.LogError("Could not create Multi Targets");
			return;
		}
		for (int i = 0; i < num; i++)
		{
			IntPtr ptr = new IntPtr(intPtr.ToInt32() + i * Marshal.SizeOf(typeof(SimpleTargetData)));
			SimpleTargetData simpleTargetData = (SimpleTargetData)Marshal.PtrToStructure(ptr, typeof(SimpleTargetData));
			if (!mTrackablesDict.ContainsKey(simpleTargetData.id))
			{
				int num2 = 128;
				StringBuilder stringBuilder = new StringBuilder(num2);
				QCARWrapper.Instance.DataSetGetTrackableName(mDataSetPtr, simpleTargetData.id, stringBuilder, num2);
				MultiTarget value = new MultiTargetImpl(stringBuilder.ToString(), simpleTargetData.id);
				mTrackablesDict[simpleTargetData.id] = value;
			}
		}
		Marshal.FreeHGlobal(intPtr);
	}

	private void CreateCylinderTargets()
	{
		int num = QCARWrapper.Instance.DataSetGetNumTrackableType(3, mDataSetPtr);
		if (num <= 0)
		{
			return;
		}
		IntPtr intPtr = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(SimpleTargetData)) * num);
		if (QCARWrapper.Instance.DataSetGetTrackablesOfType(3, intPtr, num, mDataSetPtr) == 0)
		{
			Debug.LogError("Could not create Cylinder Targets");
			return;
		}
		for (int i = 0; i < num; i++)
		{
			IntPtr ptr = new IntPtr(intPtr.ToInt32() + i * Marshal.SizeOf(typeof(SimpleTargetData)));
			SimpleTargetData simpleTargetData = (SimpleTargetData)Marshal.PtrToStructure(ptr, typeof(SimpleTargetData));
			if (!mTrackablesDict.ContainsKey(simpleTargetData.id))
			{
				int num2 = 128;
				StringBuilder stringBuilder = new StringBuilder(num2);
				QCARWrapper.Instance.DataSetGetTrackableName(mDataSetPtr, simpleTargetData.id, stringBuilder, num2);
				CylinderTarget value = new CylinderTargetImpl(stringBuilder.ToString(), simpleTargetData.id, this);
				mTrackablesDict[simpleTargetData.id] = value;
			}
		}
		Marshal.FreeHGlobal(intPtr);
	}
}
