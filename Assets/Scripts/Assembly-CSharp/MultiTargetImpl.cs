public class MultiTargetImpl : TrackableImpl, MultiTarget, Trackable
{
	public MultiTargetImpl(string name, int id)
		: base(name, id)
	{
		base.Type = TrackableType.MULTI_TARGET;
	}
}
