using UnityEngine;

public abstract class WordResult
{
	public abstract Word Word { get; }

	public abstract OrientedBoundingBox Obb { get; }

	public abstract Vector3 Position { get; }

	public abstract Quaternion Orientation { get; }

	public abstract TrackableBehaviour.Status CurrentStatus { get; }
}
