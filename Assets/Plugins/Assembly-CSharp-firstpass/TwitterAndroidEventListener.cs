using Prime31;
using UnityEngine;

public class TwitterAndroidEventListener : MonoBehaviour
{
	public MonoBehaviour socialNetworkController;

	private void OnEnable()
	{
		TwitterAndroidManager.loginDidSucceedEvent += loginDidSucceedEvent;
		TwitterAndroidManager.loginDidFailEvent += loginDidFailEvent;
		TwitterAndroidManager.requestSucceededEvent += requestSucceededEvent;
		TwitterAndroidManager.requestFailedEvent += requestFailedEvent;
		TwitterAndroidManager.twitterInitializedEvent += twitterInitializedEvent;
	}

	private void OnDisable()
	{
		TwitterAndroidManager.loginDidSucceedEvent -= loginDidSucceedEvent;
		TwitterAndroidManager.loginDidFailEvent -= loginDidFailEvent;
		TwitterAndroidManager.requestSucceededEvent -= requestSucceededEvent;
		TwitterAndroidManager.requestFailedEvent -= requestFailedEvent;
		TwitterAndroidManager.twitterInitializedEvent -= twitterInitializedEvent;
	}

	private void loginDidSucceedEvent(string username)
	{
		Debug.Log("loginDidSucceedEvent.  username: " + username);
	}

	private void loginDidFailEvent(string error)
	{
		if ((bool)socialNetworkController)
		{
			socialNetworkController.Invoke("SocialNetworkLoginFailed", 0f);
		}
		Debug.Log("loginDidFailEvent. error: " + error);
	}

	private void requestSucceededEvent(object response)
	{
		Debug.Log("requestSucceededEvent");
		Utils.logObject(response);
	}

	private void requestFailedEvent(string error)
	{
		Debug.Log("requestFailedEvent. error: " + error);
	}

	private void twitterInitializedEvent()
	{
		Debug.Log("twitterInitializedEvent");
	}
}
