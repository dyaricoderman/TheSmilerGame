using System;
using UnityEngine;

[Serializable]
public class SmileDisplay : MonoBehaviour
{
	public TextMesh text;

	public Material mat;

	public virtual void Start()
	{
	}

	public virtual void UpdateSmileDisplay(float progress)
	{
		progress = Mathf.Clamp01(progress);
		int num = (int)(progress * 16f);
		text.text = "x" + num;
		float num2 = Mathf.Clamp01(progress * 16f - (float)num);
		int num3 = (int)(num2 * 16f);
		if (num > 15)
		{
			num3 = 15;
		}
		int num4 = num3 % 4;
		int num5 = 3 - num3 / 4;
		mat.SetTextureOffset("_MainTex", new Vector2(0.25f * (float)num4, 0.25f * (float)num5));
	}

	public virtual void Main()
	{
	}
}
