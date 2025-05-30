using System;
using UnityEngine;

[Serializable]
public class UVAnim : MonoBehaviour
{
	public int uvAnimationTileX;

	public int uvAnimationTileY;

	public float framesPerSecond;

	public int materialID;

	public UVAnim()
	{
		uvAnimationTileX = 24;
		uvAnimationTileY = 1;
		framesPerSecond = 10f;
	}

	public virtual void FixedUpdate()
	{
		int num = (int)(Time.time * framesPerSecond);
		num %= uvAnimationTileX * uvAnimationTileY;
		Vector2 scale = new Vector2(1f / (float)uvAnimationTileX, 1f / (float)uvAnimationTileY);
		int num2 = num % uvAnimationTileX;
		int num3 = num / uvAnimationTileX;
		Vector2 offset = new Vector2((float)num2 * scale.x, 1f - scale.y - (float)num3 * scale.y);
		GetComponent<Renderer>().materials[materialID].SetTextureOffset("_MainTex", offset);
		GetComponent<Renderer>().materials[materialID].SetTextureScale("_MainTex", scale);
	}

	public virtual void Main()
	{
	}
}
