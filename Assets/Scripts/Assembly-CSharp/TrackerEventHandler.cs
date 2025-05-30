using UnityEngine;

public class TrackerEventHandler : MonoBehaviour, ITrackerEventHandler
{
	private void Start()
	{
		QCARBehaviour component = GetComponent<QCARBehaviour>();
		if ((bool)component)
		{
			component.RegisterTrackerEventHandler(this);
		}
	}

	public void OnTrackablesUpdated()
	{
	}

	public void OnInitialized()
	{
	}
}
