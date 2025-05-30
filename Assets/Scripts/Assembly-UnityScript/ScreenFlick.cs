using System;
using UnityEngine;

[Serializable]
public class ScreenFlick : MonoBehaviour
{
	public Material Mat;

	public float[] offset;

	private int index;

	public virtual void Start()
	{
		if (Performance.GamePerformance < 1)
		{
			UnityEngine.Object.Destroy(gameObject);
		}
		if (Performance.GamePerformance < 1)
		{
			UnityEngine.Object.Destroy(this);
		}
	}

	public virtual void Update()
	{
		index++;
		index = (int)Mathf.Repeat(index, offset.Length);
		Mat.SetTextureOffset("_MainTex", new Vector2(0f, offset[index]));
	}

	public virtual void Main()
	{
	}
}
