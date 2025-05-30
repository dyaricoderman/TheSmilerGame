public abstract class ImageTargetBuilder
{
	public enum FrameQuality
	{
		FRAME_QUALITY_NONE = -1,
		FRAME_QUALITY_LOW = 0,
		FRAME_QUALITY_MEDIUM = 1,
		FRAME_QUALITY_HIGH = 2
	}

	public abstract bool Build(string targetName, float sceenSizeWidth);

	public abstract void StartScan();

	public abstract void StopScan();

	public abstract FrameQuality GetFrameQuality();

	public abstract TrackableSource GetTrackableSource();
}
