public class MarkerImpl : TrackableImpl, Marker, Trackable
{
	private readonly float mSize;

	public int MarkerID { get; private set; }

	public MarkerImpl(string name, int id, float size, int markerID)
		: base(name, id)
	{
		base.Type = TrackableType.MARKER;
		mSize = size;
		MarkerID = markerID;
	}

	public float GetSize()
	{
		return mSize;
	}
}
