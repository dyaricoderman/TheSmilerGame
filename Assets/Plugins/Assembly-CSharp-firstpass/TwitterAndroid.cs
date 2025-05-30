using System.Collections.Generic;
using Prime31;
using UnityEngine;

public class TwitterAndroid
{
	private static AndroidJavaObject _plugin;

	static TwitterAndroid()
	{
		if (Application.platform != RuntimePlatform.Android)
		{
			return;
		}
		using (AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.prime31.TwitterPlugin"))
		{
			_plugin = androidJavaClass.CallStatic<AndroidJavaObject>("instance", new object[0]);
		}
	}

	public static void init(string consumerKey, string consumerSecret)
	{
		if (Application.platform == RuntimePlatform.Android)
		{
			_plugin.Call("init", consumerKey, consumerSecret);
		}
	}

	public static bool isLoggedIn()
	{
		if (Application.platform != RuntimePlatform.Android)
		{
			return false;
		}
		return _plugin.Call<bool>("isLoggedIn", new object[0]);
	}

	public static void showLoginDialog()
	{
		if (Application.platform == RuntimePlatform.Android)
		{
			_plugin.Call("showLoginDialog");
		}
	}

	public static void logout()
	{
		if (Application.platform == RuntimePlatform.Android)
		{
			_plugin.Call("logout");
		}
	}

	public static void postUpdate(string update)
	{
		Dictionary<string, string> dictionary = new Dictionary<string, string>();
		dictionary.Add("status", update);
		Dictionary<string, string> parameters = dictionary;
		performRequest("post", "/1/statuses/update.json", parameters);
	}

	public static void postUpdateWithImage(string update, byte[] image)
	{
		if (Application.platform == RuntimePlatform.Android)
		{
			_plugin.Call("postUpdateWithImage", update, image);
		}
	}

	public static void getHomeTimeline()
	{
		performRequest("get", "/1/statuses/home_timeline.json", null);
	}

	public static void getFollowers()
	{
		performRequest("get", "/1/statuses/followers.json", null);
	}

	public static void performRequest(string methodType, string path, Dictionary<string, string> parameters)
	{
		if (Application.platform == RuntimePlatform.Android)
		{
			string text = ((parameters == null) ? string.Empty : parameters.toJson());
			_plugin.Call("performRequest", methodType, path, text);
		}
	}
}
