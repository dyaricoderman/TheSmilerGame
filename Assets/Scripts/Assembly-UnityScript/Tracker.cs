using System;
using System.Collections.Generic;
using UnityEngine;
using UnityScript.Lang;

[Serializable]
public class Tracker2 : MonoBehaviour
{
	[NonSerialized]
	public static Tracker2 instance;

	private bool allowDebug;

	private bool flurry;

	private bool testFlight;

	private string thisFlurryKey;

	private bool flurryInitialised;

	public Tracker2()
	{
		flurry = true;
		testFlight = true;
	}

	public virtual void Awake()
	{
		instance = this;
	}

	//public virtual void StartFlurrySession(string flurryKey)
	//{
		//thisFlurryKey = flurryKey;
		//FlurryAndroid.onStartSession(thisFlurryKey, false, false);
	//}

	public virtual void OnApplicationQuit()
	{
		//FlurryAndroid.onEndSession();
		if (allowDebug)
		{
			Debug.Log("Flurry session ended");
		}
	}

	public virtual void OnApplicationPause(bool pause)
	{
		if (pause)
		{
			//FlurryAndroid.onEndSession();
			if (allowDebug)
			{
				Debug.Log("Flurry session ended");
			}
		}
		else
		{
			//FlurryAndroid.onStartSession(thisFlurryKey, false, false);
			if (allowDebug)
			{
				Debug.Log("Flurry session started");
			}
		}
	}

	public virtual void Log(string eventDescription)
	{
		if (testFlight && flurry)
		{
			//FlurryAndroid.logEvent(eventDescription, false);
		}
		if (allowDebug)
		{
			Debug.Log("Tracked event - " + eventDescription);
		}
		if (flurry)
		{
			//FlurryAndroid.logEvent(eventDescription, false);
		}
	}

	public virtual void SentFromCLog(string SentString)
	{
		string[] array = SentString.Split(":"[0]);
		if (Extensions.get_length((System.Array)array) == 1)
		{
			Log(array[0]);
		}
		if (Extensions.get_length((System.Array)array) == 2)
		{
			LogWithParameters(array[0], array[1]);
		}
	}

	public virtual void LogWithParameters(string eventDescription, string parameters)
	{
		Dictionary<string, string> dictionary = new Dictionary<string, string>();
		dictionary.Add("Event Parameters", parameters);
		//FlurryAndroid.logEvent(eventDescription, dictionary, false);
		if (allowDebug)
		{
			Debug.Log("Tracked event - " + eventDescription + ". Parameters - " + parameters);
		}
	}

	public virtual void AllowDebug(bool allow)
	{
		allowDebug = allow;
	}

	public virtual void Main()
	{
	}
}
