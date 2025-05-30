using System;
using UnityEngine;

[Serializable]
public class TargetDef
{
	public int ID;

	public string Name = string.Empty;

	public string Discription = string.Empty;

	public Texture2D Icon;

	public string Unlocks = string.Empty;

	public int Credits;

	public bool ShowPopup = true;
}
