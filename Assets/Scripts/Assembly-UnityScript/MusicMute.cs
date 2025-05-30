using System;
using UnityEngine;

[Serializable]
public class MusicMute : MonoBehaviour
{
	[NonSerialized]
	public static MusicMute MusicInstance;

	public virtual void Awake()
	{
		MusicInstance = this;
	}

	public virtual void Start()
	{
		CheckState();
	}

	public static void UpdatedMusicState(bool mute)
	{
		if (mute)
		{
			PlayerPrefs.SetInt("MusicMute", 1);
		}
		else
		{
			PlayerPrefs.SetInt("MusicMute", 0);
		}
		if ((bool)MusicInstance)
		{
			MusicInstance.CheckState();
		}
	}

	public virtual void CheckState()
	{
		AudioSource audioSource = (AudioSource)gameObject.GetComponent(typeof(AudioSource));
		if ((bool)audioSource)
		{
			int num = PlayerPrefs.GetInt("MusicMute");
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
