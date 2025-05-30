using System;
using UnityEngine;

[Serializable]
public class SharingExampleGUI : MonoBehaviour
{
	private bool postComplete;

	private bool loginFailed;

	public virtual void Awake()
	{
	}

	public virtual void OnGUI()
	{
		float left = default(float);
		float num = default(float);
		if (GUI.Button(new Rect(left, num, (float)Screen.width * 0.2f, (float)Screen.height * 0.1f), "Post Facebook"))
		{
			string iconURL = "http://matmiapps.co.uk/pocketwarwick/img/appicon.png";
			string facebookAppLink = "http://www.facebook.com/pages/swseven/320007621443174";
			string linkName = "Pocket Warwick";
			string iconCaption = "Pocket Warwick Logo";
			SharingControl.inst.StartCoroutine(SharingControl.inst.FacebookSharePost("Test share", facebookAppLink, linkName, iconURL, iconCaption));
		}
		num += (float)Screen.height * 0.15f;
		if (GUI.Button(new Rect(left, num, (float)Screen.width * 0.2f, (float)Screen.height * 0.1f), "Post Twitter"))
		{
			SharingControl.inst.StartCoroutine(SharingControl.inst.TwitterSharePost("Test share"));
		}
		num += (float)Screen.height * 0.15f;
		if (GUI.Button(new Rect(left, num, (float)Screen.width * 0.2f, (float)Screen.height * 0.1f), "Logout"))
		{
			SharingControl.inst.SocialNetworksLogout();
		}
		num += (float)Screen.height * 0.15f;
		if (GUI.Button(new Rect(left, num, (float)Screen.width * 0.2f, (float)Screen.height * 0.1f), "Quit Application"))
		{
			Application.Quit();
		}
	}

	public virtual void PostComplete()
	{
		Debug.Log("Social network post complete");
	}

	public virtual void LoginFailed()
	{
		Debug.Log("Social network login failed");
	}

	public virtual void Main()
	{
	}
}
