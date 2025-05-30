using System;
using Boo.Lang.Runtime;
using UnityEngine;
using UnityScript.Lang;

[Serializable]
public class SoundMute : MonoBehaviour
{
	[NonSerialized]
	public static UnityScript.Lang.Array MuteInstances = new UnityScript.Lang.Array();

	public virtual void Start()
	{
		CleanAndAdd();
		CheckState();
	}

	public static void UpdatedMuteState(bool mute)
	{
		if (mute)
		{
			PlayerPrefs.SetInt("SoundMute", 1);
		}
		else
		{
			PlayerPrefs.SetInt("SoundMute", 0);
		}
		for (int i = 0; i < MuteInstances.length; i++)
		{
			if (RuntimeServices.ToBool(MuteInstances[i]))
			{
				(MuteInstances[i] as SoundMute).CheckState();
			}
		}
	}

	public virtual void CleanAndAdd()
	{
		for (int i = 0; i < MuteInstances.length; i++)
		{
			if (!RuntimeServices.ToBool(MuteInstances[i]))
			{
				MuteInstances.RemoveAt(i);
				i--;
			}
		}
		MuteInstances.Add(this);
	}

	public virtual void CheckState()
	{
		AudioSource audioSource = (AudioSource)gameObject.GetComponent(typeof(AudioSource));
		if ((bool)audioSource)
		{
			int num = PlayerPrefs.GetInt("SoundMute");
			if (num == 1)
			{
				audioSource.mute = true;
			}
			else
			{
				audioSource.mute = false;
			}
		}
	}

	public virtual void Main()
	{
	}
}
