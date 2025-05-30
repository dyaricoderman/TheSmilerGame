using System;
using UnityEngine;

public class ImageTargetBuilderImpl : ImageTargetBuilder
{
	private TrackableSource mTrackableSource;

	public override bool Build(string targetName, float sceenSizeWidth)
	{
		if (targetName.Length > 64)
		{
			Debug.LogError("Invalid parameters to build User Defined Target:Target name exceeds 64 character limit");
			return false;
		}
		mTrackableSource = null;
		return QCARWrapper.Instance.ImageTargetBuilderBuild(targetName, sceenSizeWidth) == 1;
	}

	public override void StartScan()
	{
		QCARWrapper.Instance.ImageTargetBuilderStartScan();
	}

	public override void StopScan()
	{
		QCARWrapper.Instance.ImageTargetBuilderStopScan();
	}

	public override FrameQuality GetFrameQuality()
	{
		return (FrameQuality)QCARWrapper.Instance.ImageTargetBuilderGetFrameQuality();
	}

	public override TrackableSource GetTrackableSource()
	{
		IntPtr intPtr = QCARWrapper.Instance.ImageTargetBuilderGetTrackableSource();
		if (mTrackableSource == null && intPtr != IntPtr.Zero)
		{
			mTrackableSource = new TrackableSourceImpl(intPtr);
		}
		return mTrackableSource;
	}
}
