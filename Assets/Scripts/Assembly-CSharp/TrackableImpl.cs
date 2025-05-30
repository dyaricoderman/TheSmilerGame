public abstract class TrackableImpl : Trackable
{
	public TrackableType Type { get; protected set; }

	public string Name { get; protected set; }

	public int ID { get; protected set; }

	protected TrackableImpl(string name, int id)
	{
		Name = name;
		ID = id;
	}
}
