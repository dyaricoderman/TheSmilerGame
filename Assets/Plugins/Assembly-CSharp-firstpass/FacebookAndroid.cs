using System;
using System.Collections.Generic;
using Prime31;
using UnityEngine;

public class FacebookAndroid
{
	private static AndroidJavaObject _facebookPlugin;

	static FacebookAndroid()
	{
		if (Application.platform == RuntimePlatform.Android)
		{
			using (AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.prime31.FacebookPlugin"))
			{
				_facebookPlugin = androidJavaClass.CallStatic<AndroidJavaObject>("instance", new object[0]);
			}
			FacebookManager.preLoginSucceededEvent += delegate
			{
				Facebook.instance.accessToken = getAccessToken();
			};
		}
	}

	public static void init()
	{
		if (Application.platform == RuntimePlatform.Android)
		{
			_facebookPlugin.Call("init");
		}
	}

	public static bool isSessionValid()
	{
		if (Application.platform != RuntimePlatform.Android)
		{
			return false;
		}
		return _facebookPlugin.Call<bool>("isSessionValid", new object[0]);
	}

	public static string getAccessToken()
	{
		if (Application.platform != RuntimePlatform.Android)
		{
			return string.Empty;
		}
		return _facebookPlugin.Call<string>("getAccessToken", new object[0]);
	}

	public static List<object> getSessionPermissions()
	{
		if (Application.platform == RuntimePlatform.Android)
		{
			string json = _facebookPlugin.Call<string>("getSessionPermissions", new object[0]);
			return json.listFromJson();
		}
		return new List<object>();
	}

	public static void login()
	{
		loginWithReadPermissions(new string[0]);
	}

	public static void loginWithReadPermissions(string[] permissions)
	{
		if (Application.platform == RuntimePlatform.Android)
		{
			_facebookPlugin.Call("loginWithReadPermissions", new object[1] { permissions });
		}
	}

	public static void reauthorizeWithReadPermissions(string[] permissions)
	{
		if (Application.platform == RuntimePlatform.Android)
		{
			_facebookPlugin.Call("reauthorizeWithReadPermissions", permissions.toJson());
		}
	}

	public static void reauthorizeWithPublishPermissions(string[] permissions, FacebookSessionDefaultAudience defaultAudience)
	{
		if (Application.platform == RuntimePlatform.Android)
		{
			_facebookPlugin.Call("reauthorizeWithPublishPermissions", permissions.toJson(), defaultAudience.ToString());
		}
	}

	public static void logout()
	{
		if (Application.platform == RuntimePlatform.Android)
		{
			_facebookPlugin.Call("logout");
			Facebook.instance.accessToken = string.Empty;
		}
	}

	public static void showDialog(string dialogType, Dictionary<string, string> parameters)
	{
		if (Application.platform != RuntimePlatform.Android)
		{
			return;
		}
		using (AndroidJavaObject androidJavaObject = new AndroidJavaObject("android.os.Bundle"))
		{
			IntPtr methodID = AndroidJNI.GetMethodID(androidJavaObject.GetRawClass(), "putString", "(Ljava/lang/String;Ljava/lang/String;)V");
			object[] array = new object[2];
			if (parameters != null)
			{
				foreach (KeyValuePair<string, string> parameter in parameters)
				{
					array[0] = new AndroidJavaObject("java.lang.String", parameter.Key);
					array[1] = new AndroidJavaObject("java.lang.String", parameter.Value);
					AndroidJNI.CallVoidMethod(androidJavaObject.GetRawObject(), methodID, AndroidJNIHelper.CreateJNIArgArray(array));
				}
			}
			_facebookPlugin.Call("showDialog", dialogType, androidJavaObject);
		}
	}

	public static void graphRequest(string graphPath, string httpMethod, Dictionary<string, string> parameters)
	{
		if (Application.platform != RuntimePlatform.Android)
		{
			return;
		}
		using (AndroidJavaObject androidJavaObject = new AndroidJavaObject("android.os.Bundle"))
		{
			IntPtr methodID = AndroidJNI.GetMethodID(androidJavaObject.GetRawClass(), "putString", "(Ljava/lang/String;Ljava/lang/String;)V");
			object[] array = new object[2];
			if (parameters != null)
			{
				foreach (KeyValuePair<string, string> parameter in parameters)
				{
					array[0] = new AndroidJavaObject("java.lang.String", parameter.Key);
					array[1] = new AndroidJavaObject("java.lang.String", parameter.Value);
					AndroidJNI.CallObjectMethod(androidJavaObject.GetRawObject(), methodID, AndroidJNIHelper.CreateJNIArgArray(array));
				}
			}
			_facebookPlugin.Call("graphRequest", graphPath, httpMethod, androidJavaObject);
		}
	}
}
