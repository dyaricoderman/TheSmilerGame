using System;
using Prime31;

public class TwitterAndroidManager : AbstractManager
{
	public static event Action<string> loginDidSucceedEvent;

	public static event Action<string> loginDidFailEvent;

	public static event Action<object> requestSucceededEvent;

	public static event Action<string> requestFailedEvent;

	public static event Action twitterInitializedEvent;

	static TwitterAndroidManager()
	{
		AbstractManager.initialize(typeof(TwitterAndroidManager));
	}

	public void loginDidSucceed(string username)
	{
		TwitterAndroidManager.loginDidSucceedEvent.fire(username);
	}

	public void loginDidFail(string error)
	{
		TwitterAndroidManager.loginDidFailEvent.fire(error);
	}

	public void requestSucceeded(string response)
	{
		TwitterAndroidManager.requestSucceededEvent.fire(Json.jsonDecode(response));
	}

	public void requestFailed(string error)
	{
		TwitterAndroidManager.requestFailedEvent.fire(error);
	}

	public void twitterInitialized(string empty)
	{
		TwitterAndroidManager.twitterInitializedEvent.fire();
	}
}
