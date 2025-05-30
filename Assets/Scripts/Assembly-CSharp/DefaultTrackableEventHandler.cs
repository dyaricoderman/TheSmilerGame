using UnityEngine;

public class DefaultTrackableEventHandler : MonoBehaviour, ITrackableEventHandler
{
	private TrackableBehaviour mTrackableBehaviour;

	private void Start()
	{
		mTrackableBehaviour = GetComponent<TrackableBehaviour>();
		if ((bool)mTrackableBehaviour)
		{
			mTrackableBehaviour.RegisterTrackableEventHandler(this);
		}
	}

	public void OnTrackableStateChanged(TrackableBehaviour.Status previousStatus, TrackableBehaviour.Status newStatus)
	{
		if (newStatus == TrackableBehaviour.Status.DETECTED || newStatus == TrackableBehaviour.Status.TRACKED)
		{
			OnTrackingFound();
		}
		else
		{
			OnTrackingLost();
		}
	}

	private void OnTrackingFound()
	{
		Renderer[] componentsInChildren = GetComponentsInChildren<Renderer>(true);
		Collider[] componentsInChildren2 = GetComponentsInChildren<Collider>(true);
		Renderer[] array = componentsInChildren;
		foreach (Renderer renderer in array)
		{
			renderer.enabled = true;
		}
		Collider[] array2 = componentsInChildren2;
		foreach (Collider collider in array2)
		{
			collider.enabled = true;
		}
		base.gameObject.BroadcastMessage("ARStart");
		Debug.Log("Trackable " + mTrackableBehaviour.TrackableName + " found");
	}

	private void OnTrackingLost()
	{
		Renderer[] componentsInChildren = GetComponentsInChildren<Renderer>(true);
		Collider[] componentsInChildren2 = GetComponentsInChildren<Collider>(true);
		Renderer[] array = componentsInChildren;
		foreach (Renderer renderer in array)
		{
			renderer.enabled = false;
		}
		Collider[] array2 = componentsInChildren2;
		foreach (Collider collider in array2)
		{
			collider.enabled = false;
		}
		base.gameObject.BroadcastMessage("ARStop");
		Debug.Log("Trackable " + mTrackableBehaviour.TrackableName + " lost");
	}
}
