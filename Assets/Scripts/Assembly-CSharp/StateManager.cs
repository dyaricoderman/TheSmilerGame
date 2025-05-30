using System.Collections.Generic;

public abstract class StateManager
{
	public abstract IEnumerable<TrackableBehaviour> GetActiveTrackableBehaviours();

	public abstract IEnumerable<TrackableBehaviour> GetTrackableBehaviours();

	public abstract void DestroyTrackableBehavioursForTrackable(Trackable trackable, bool destroyGameObjects = true);

	public abstract WordManager GetWordManager();
}
