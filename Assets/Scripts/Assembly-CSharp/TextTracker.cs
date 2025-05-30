using UnityEngine;

public abstract class TextTracker : Tracker
{
	public abstract WordList WordList { get; }

	public abstract bool SetRegionOfInterest(Rect detectionRegion, Rect trackingRegion);

	public abstract bool GetRegionOfInterest(out Rect detectionRegion, out Rect trackingRegion);
}
