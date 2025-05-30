using System.Collections.Generic;
using UnityEngine;

public abstract class DataSet
{
	public enum StorageType
	{
		STORAGE_APP = 0,
		STORAGE_APPRESOURCE = 1,
		STORAGE_ABSOLUTE = 2
	}

	public abstract string Path { get; }

	public abstract StorageType FileStorageType { get; }

	public static bool Exists(string name)
	{
		string path = "QCAR/" + name + ".xml";
		return Exists(path, StorageType.STORAGE_APPRESOURCE);
	}

	public static bool Exists(string path, StorageType storageType)
	{
		return DataSetImpl.ExistsImpl(path, storageType);
	}

	public abstract bool Load(string name);

	public abstract bool Load(string path, StorageType storageType);

	public abstract IEnumerable<Trackable> GetTrackables();

	public abstract DataSetTrackableBehaviour CreateTrackable(TrackableSource trackableSource, string gameObjectName);

	public abstract DataSetTrackableBehaviour CreateTrackable(TrackableSource trackableSource, GameObject gameObject);

	public abstract bool Destroy(Trackable trackable, bool destroyGameObject);

	public abstract bool HasReachedTrackableLimit();

	public abstract bool Contains(Trackable trackable);

	public abstract void DestroyAllTrackables(bool destroyGameObject);
}
