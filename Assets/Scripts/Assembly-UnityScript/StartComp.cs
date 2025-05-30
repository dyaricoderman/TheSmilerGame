using System;
using UnityEngine;

[Serializable]
public class StartComp : MonoBehaviour
{
	public virtual void Start()
	{
		GameObject.Find("Competition").SendMessage("Begin");
	}

	public virtual void Flapsy()
	{
		Debug.Log("hoora");
	}

	public virtual void Main()
	{
	}
}
