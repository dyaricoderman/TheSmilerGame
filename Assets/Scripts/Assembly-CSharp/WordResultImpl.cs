using UnityEngine;

public class WordResultImpl : WordResult
{
	private OrientedBoundingBox mObb;

	private Vector3 mPosition;

	private Quaternion mOrientation;

	private readonly Word mWord;

	private TrackableBehaviour.Status mStatus;

	public override Word Word
	{
		get
		{
			return mWord;
		}
	}

	public override Vector3 Position
	{
		get
		{
			return mPosition;
		}
	}

	public override Quaternion Orientation
	{
		get
		{
			return mOrientation;
		}
	}

	public override OrientedBoundingBox Obb
	{
		get
		{
			return mObb;
		}
	}

	public override TrackableBehaviour.Status CurrentStatus
	{
		get
		{
			return mStatus;
		}
	}

	public WordResultImpl(Word word)
	{
		mWord = word;
	}

	public void SetPose(Vector3 position, Quaternion orientation)
	{
		mPosition = position;
		mOrientation = orientation;
	}

	public void SetObb(OrientedBoundingBox obb)
	{
		mObb = obb;
	}

	public void SetStatus(TrackableBehaviour.Status status)
	{
		mStatus = status;
	}
}
