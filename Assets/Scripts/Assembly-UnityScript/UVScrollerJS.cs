using System;
using UnityEngine;

[Serializable]
public class UVScrollerJS : MonoBehaviour
{
	public float scrollSpeed;

	public int matID;

	public UVScrollerJS()
	{
		scrollSpeed = 0.1f;
	}

	public virtual void FixedUpdate()
	{
		GetComponent<Renderer>().materials[matID].SetTextureOffset("_MainTex", new Vector2(0f, Time.time * scrollSpeed));
	}

	public virtual void OnBecameVisible()
	{
		enabled = true;
	}

	public virtual void OnBecameInvisible()
	{
		enabled = false;
	}

	public virtual void Main()
	{
	}
}
