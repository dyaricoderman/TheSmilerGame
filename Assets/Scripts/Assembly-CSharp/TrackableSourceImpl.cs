using System;

public class TrackableSourceImpl : TrackableSource
{
	public IntPtr TrackableSourcePtr { get; private set; }

	public TrackableSourceImpl(IntPtr trackableSourcePtr)
	{
		TrackableSourcePtr = trackableSourcePtr;
	}
}
