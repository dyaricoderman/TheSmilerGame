using UnityEngine;
using Vuforia;

public class AltonTowersTrackableEventHandler : MonoBehaviour
{
	public TrackablesManager trackablesManager;

	public TargetDef Definition;

	//private TrackableBehaviour mTrackableBehaviour;

	//private ObserverBehaviour mObserverBehaviour;

	private void Awake()
	{
		if (TrackablesManager.TargetCount < Definition.ID + 1)
		{
			TrackablesManager.TargetCount = Definition.ID + 1;
		}
	}

	private void Start()
	{

		//mTrackableBehaviour = GetComponent<TrackableBehaviour>();
		//if ((bool)mTrackableBehaviour)
		//{
			//mTrackableBehaviour.RegisterTrackableEventHandler(this);
		//}
		//OnTrackingLost();
	}

	//public void OnTrackableStateChanged(TrackableBehaviour.Status previousStatus, TrackableBehaviour.Status newStatus)
	//{
		//if (newStatus == TrackableBehaviour.Status.DETECTED || newStatus == TrackableBehaviour.Status.TRACKED)
		//{
			//OnTrackingFound();
		//}
		//else
		//{
			//OnTrackingLost();
		//}
	//}

	public void OnTrackingFound()
	{
		Renderer[] componentsInChildren = GetComponentsInChildren<Renderer>();
		Renderer[] array = componentsInChildren;
		foreach (Renderer renderer in array)
		{
			renderer.enabled = true;
		}
		base.gameObject.BroadcastMessage("ARStart", SendMessageOptions.DontRequireReceiver);
		trackablesManager.FoundTrackable(Definition);
	}

	public void OnTrackingLost()
	{
		Renderer[] componentsInChildren = GetComponentsInChildren<Renderer>();
		Renderer[] array = componentsInChildren;
		foreach (Renderer renderer in array)
		{
			renderer.enabled = false;
		}
		base.gameObject.BroadcastMessage("ARStop", SendMessageOptions.DontRequireReceiver);
		trackablesManager.LostTrackable(Definition);
	}
}
