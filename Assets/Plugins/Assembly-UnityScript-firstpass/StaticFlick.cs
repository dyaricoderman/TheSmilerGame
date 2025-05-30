using System;
using UnityEngine;
using UnityScript.Lang;

[Serializable]
public class StaticFlick : MonoBehaviour
{
	public Texture2D[] Frames;

	public virtual void Start()
	{
	}

	public virtual void Update()
	{
		GetComponent<Renderer>().material.SetTexture("_MainTex", Frames[(int)(UnityEngine.Random.value * (float)Extensions.get_length((System.Array)Frames))]);
	}

	public virtual void Main()
	{
	}
}
