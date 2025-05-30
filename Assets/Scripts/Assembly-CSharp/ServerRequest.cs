using System.Collections;
using LitJson;
using UnityEngine;

public class ServerRequest
{
	public WWW www;

	public string json;

	public string method;

	public JsonData data;

	public ServerRequest(string path, object obj)
	{
		method = path;
		json = JsonMapper.ToJson(obj);
		CreateWWW();
	}

	public ServerRequest(string path, string _json)
	{
		method = path;
		json = _json;
		CreateWWW();
	}

	private void CreateWWW()
	{
	}

	public IEnumerator AsyncHandleWWW()
	{
		yield return www;
		HandleWWW();
	}

	public void HandleWWW()
	{
		if (www.error != null)
		{
			Debug.Log(www.error);
			int num = PlayerPrefs.GetInt("ServerQueue", 0);
			try
			{
				PlayerPrefs.SetString("ServerRequest" + num, method + "|" + json);
				PlayerPrefs.SetInt("ServerQueue", num++);
				return;
			}
			catch (PlayerPrefsException message)
			{
				Debug.Log(message);
				Debug.Log("Player Prefs Storage Full");
				return;
			}
		}
		Debug.Log(www.text);
	}
}
