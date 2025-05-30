using Prime31;
using UnityEngine;

public class FacebookEventListener : MonoBehaviour
{
	public MonoBehaviour socialNetworkController;

	private void OnEnable()
	{
		FacebookManager.sessionOpenedEvent += sessionOpenedEvent;
		FacebookManager.loginFailedEvent += loginFailedEvent;
		FacebookManager.dialogCompletedWithUrlEvent += dialogCompletedEvent;
		FacebookManager.dialogFailedEvent += dialogFailedEvent;
		FacebookManager.dialogCompletedEvent += facebokDialogCompleted;
		FacebookManager.dialogDidNotCompleteEvent += dialogDidNotCompleteEvent;
		FacebookManager.graphRequestCompletedEvent += graphRequestCompletedEvent;
		FacebookManager.graphRequestFailedEvent += facebookCustomRequestFailed;
		FacebookManager.restRequestCompletedEvent += restRequestCompletedEvent;
		FacebookManager.restRequestFailedEvent += restRequestFailedEvent;
		FacebookManager.facebookComposerCompletedEvent += facebookComposerCompletedEvent;
		FacebookManager.reauthorizationFailedEvent += reauthorizationFailedEvent;
		FacebookManager.reauthorizationSucceededEvent += reauthorizationSucceededEvent;
	}

	private void OnDisable()
	{
		FacebookManager.sessionOpenedEvent -= sessionOpenedEvent;
		FacebookManager.loginFailedEvent -= loginFailedEvent;
		FacebookManager.dialogCompletedEvent -= facebokDialogCompleted;
		FacebookManager.dialogCompletedWithUrlEvent -= dialogCompletedEvent;
		FacebookManager.dialogDidNotCompleteEvent -= dialogDidNotCompleteEvent;
		FacebookManager.dialogFailedEvent -= dialogFailedEvent;
		FacebookManager.graphRequestCompletedEvent -= graphRequestCompletedEvent;
		FacebookManager.graphRequestFailedEvent -= facebookCustomRequestFailed;
		FacebookManager.restRequestCompletedEvent -= restRequestCompletedEvent;
		FacebookManager.restRequestFailedEvent -= restRequestFailedEvent;
		FacebookManager.facebookComposerCompletedEvent -= facebookComposerCompletedEvent;
		FacebookManager.reauthorizationFailedEvent -= reauthorizationFailedEvent;
		FacebookManager.reauthorizationSucceededEvent -= reauthorizationSucceededEvent;
	}

	private void sessionOpenedEvent()
	{
		Debug.Log("Successfully logged in to Facebook");
	}

	private void loginFailedEvent(string error)
	{
		if ((bool)socialNetworkController)
		{
			socialNetworkController.Invoke("SocialNetworkLoginFailed", 0f);
		}
		Debug.Log("Facebook login failed: " + error);
	}

	private void dialogCompletedEvent(string url)
	{
		Debug.Log("dialogCompletedEvent: " + url);
	}

	private void dialogFailedEvent(string error)
	{
		Debug.Log("dialogFailedEvent: " + error);
	}

	private void facebokDialogCompleted()
	{
		Debug.Log("facebokDialogCompleted");
	}

	private void dialogDidNotCompleteEvent()
	{
		Debug.Log("facebookDialogDidntComplete");
	}

	private void graphRequestCompletedEvent(object obj)
	{
		Debug.Log("graphRequestCompletedEvent");
		Utils.logObject(obj);
	}

	private void facebookCustomRequestFailed(string error)
	{
		Debug.Log("facebookCustomRequestFailed failed: " + error);
	}

	private void restRequestCompletedEvent(object obj)
	{
		Debug.Log("restRequestSucceededEvent");
		Utils.logObject(obj);
	}

	private void restRequestFailedEvent(string error)
	{
		Debug.Log("restRequestFailedEvent failed: " + error);
	}

	private void facebookComposerCompletedEvent(bool didSucceed)
	{
		Debug.Log("facebookComposerCompletedEvent did succeed: " + didSucceed);
	}

	private void reauthorizationSucceededEvent()
	{
		Debug.Log("reauthorizationSucceededEvent");
	}

	private void reauthorizationFailedEvent(string error)
	{
		Debug.Log("reauthorizationFailedEvent: " + error);
	}
}
