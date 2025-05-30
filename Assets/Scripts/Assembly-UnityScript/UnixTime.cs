using System;
using UnityEngine;

[Serializable]
public class UnixTime : MonoBehaviour
{
	public static int GetUnixTime()
	{
		DateTime value = new DateTime(1970, 1, 1, 0, 0, 0);
		return (int)DateTime.Now.Subtract(value).TotalSeconds;
	}

	public static int GetUnixTime(DateTime dateTime)
	{
		DateTime value = new DateTime(1970, 1, 1, 0, 0, 0);
		return (int)dateTime.Subtract(value).TotalSeconds;
	}

	public static string ConvertSecondsToMinutesString(int s)
	{
		string empty = string.Empty;
		int num = (int)Mathf.Floor(s / 86400);
		s -= num * 60 * 60 * 24;
		int num2 = (int)Mathf.Floor(s / 3600);
		s -= num2 * 60 * 60;
		int num3 = (int)Mathf.Floor(s / 60);
		s -= num3 * 60;
		int num4 = s;
		if (num4 < 10)
		{
			return num3 + ":0" + num4;
		}
		return num3 + ":" + num4;
	}

	public static string ConvertToDateFormat(int s)
	{
		string empty = string.Empty;
		int num = (int)Mathf.Floor(s / 86400);
		s -= num * 60 * 60 * 24;
		int num2 = (int)Mathf.Floor(s / 3600);
		s -= num2 * 60 * 60;
		int num3 = (int)Mathf.Floor(s / 60);
		s -= num3 * 60;
		int num4 = s;
		return "[" + num + "D " + num2 + "H " + num3 + "M " + num4 + "S]";
	}

	public virtual void Main()
	{
	}
}
