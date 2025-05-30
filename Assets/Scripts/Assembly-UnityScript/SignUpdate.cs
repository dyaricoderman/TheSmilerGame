using System;
using UnityEngine;

[Serializable]
public class SignUpdate : MonoBehaviour
{
	public TextMesh txt;

	public virtual void Start()
	{
		txt.text = Mathf.Floor(TrackUpgrade.UpgradeProgress() * 100f) + "%";
	}

	public virtual void Main()
	{
	}
}
