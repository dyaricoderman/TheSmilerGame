using System.Collections.Generic;

public interface ILoadLevelEventHandler
{
	void OnLevelLoaded(IEnumerable<TrackableBehaviour> keptAliveTrackables);

	void OnDuplicateTrackablesDisabled(IEnumerable<TrackableBehaviour> disabledTrackables);
}
