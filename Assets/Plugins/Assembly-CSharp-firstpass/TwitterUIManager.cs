using System.Collections.Generic;
using System.IO;
using Prime31;
using UnityEngine;

public class TwitterUIManager : MonoBehaviourGUI
{
	private void OnGUI()
	{
		beginColumn();
		if (GUILayout.Button("Initialize Twitter"))
		{
			TwitterAndroid.init("jZVHZaGxJkOLenVPe23fnQ", "7nZQtvTjIXnKqYHbjAUKneUTp1QEWEkeD6nKVfPw");
		}
		if (GUILayout.Button("Login"))
		{
			TwitterAndroid.showLoginDialog();
		}
		if (GUILayout.Button("Is Logged In?"))
		{
			bool flag = TwitterAndroid.isLoggedIn();
			Debug.Log("Is logged in?: " + flag);
		}
		if (GUILayout.Button("Post Update with Image"))
		{
			string path = Application.persistentDataPath + "/" + FacebookUIManager.screenshotFilename;
			byte[] image = File.ReadAllBytes(path);
			TwitterAndroid.postUpdateWithImage("test update from Unity!", image);
		}
		endColumn(true);
		if (GUILayout.Button("Logout"))
		{
			TwitterAndroid.logout();
		}
		if (GUILayout.Button("Post Update"))
		{
			TwitterAndroid.postUpdate("im an update from the Twitter Android Plugin");
		}
		if (GUILayout.Button("Get Home Timeline"))
		{
			TwitterAndroid.getHomeTimeline();
		}
		if (GUILayout.Button("Get Followers"))
		{
			TwitterAndroid.getFollowers();
		}
		if (GUILayout.Button("Custom Request"))
		{
			Dictionary<string, string> dictionary = new Dictionary<string, string>();
			dictionary.Add("screen_name", "prime_31");
			dictionary.Add("test", "paramters");
			dictionary.Add("test2", "asdf");
			dictionary.Add("test3", "wer");
			dictionary.Add("test4", "vbn");
			TwitterAndroid.performRequest("get", "/1/users/show.json", dictionary);
		}
		endColumn();
		if (bottomLeftButton("Facebook Scene"))
		{
			Application.LoadLevel("FacebookTestScene");
		}
	}
}
