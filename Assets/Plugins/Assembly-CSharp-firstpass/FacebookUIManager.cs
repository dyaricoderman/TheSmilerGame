using System.Collections.Generic;
using System.IO;
using Prime31;
using UnityEngine;

public class FacebookUIManager : MonoBehaviourGUI
{
	public static string screenshotFilename = "someScreenshot.png";

	private void completionHandler(string error, object result)
	{
		if (error != null)
		{
			Debug.LogError(error);
		}
		else
		{
			Utils.logObject(result);
		}
	}

	private void Start()
	{
		ScreenCapture.CaptureScreenshot(screenshotFilename);
	}

	private void OnGUI()
	{
		beginColumn();
		if (GUILayout.Button("Initialize Facebook"))
		{
			FacebookAndroid.init();
		}
		if (GUILayout.Button("Login"))
		{
			FacebookAndroid.loginWithReadPermissions(new string[0]);
		}
		if (GUILayout.Button("Reauthorize with Publish Permissions"))
		{
			FacebookAndroid.reauthorizeWithPublishPermissions(new string[1] { "publish_actions" }, FacebookSessionDefaultAudience.EVERYONE);
		}
		if (GUILayout.Button("Logout"))
		{
			FacebookAndroid.logout();
		}
		if (GUILayout.Button("Is Session Valid?"))
		{
			bool flag = FacebookAndroid.isSessionValid();
			Debug.Log("Is session valid?: " + flag);
		}
		if (GUILayout.Button("Get Session Token"))
		{
			string accessToken = FacebookAndroid.getAccessToken();
			Debug.Log("session token: " + accessToken);
		}
		if (GUILayout.Button("Get Granted Permissions"))
		{
			List<object> sessionPermissions = FacebookAndroid.getSessionPermissions();
			Debug.Log("granted permissions: " + sessionPermissions.Count);
			Utils.logObject(sessionPermissions);
		}
		endColumn(true);
		if (GUILayout.Button("Post Image"))
		{
			string path = Application.persistentDataPath + "/" + screenshotFilename;
			byte[] image = File.ReadAllBytes(path);
			Facebook.instance.postImage(image, "im an image posted from Android", completionHandler);
		}
		if (GUILayout.Button("Graph Request (me)"))
		{
			Facebook.instance.graphRequest("me", completionHandler);
		}
		if (GUILayout.Button("Post Message"))
		{
			Facebook.instance.postMessage("im posting this from Unity: " + Time.deltaTime, completionHandler);
		}
		if (GUILayout.Button("Post Message & Extras"))
		{
			Facebook.instance.postMessageWithLinkAndLinkToImage("link post from Unity: " + Time.deltaTime, "http://prime31.com", "Prime31 Studios", "http://prime31.com/assets/images/prime31logo.png", "Prime31 Logo", completionHandler);
		}
		if (GUILayout.Button("Show Post Dialog"))
		{
			Dictionary<string, string> dictionary = new Dictionary<string, string>();
			dictionary.Add("link", "http://prime31.com");
			dictionary.Add("name", "link name goes here");
			dictionary.Add("picture", "http://prime31.com/assets/images/prime31logo.png");
			dictionary.Add("caption", "the caption for the image is here");
			Dictionary<string, string> parameters = dictionary;
			FacebookAndroid.showDialog("stream.publish", parameters);
		}
		if (GUILayout.Button("Get Friends"))
		{
			Facebook.instance.getFriends(completionHandler);
		}
		endColumn();
		if (bottomLeftButton("Twitter Scene"))
		{
			Application.LoadLevel("TwitterTestScene");
		}
	}
}
