using System;
using UnityEngine;

[Serializable]
public class Global : MonoBehaviour
{
	public static Rect AspectRect(float x, float y, float width, float height, PositionType PT)
	{
		int num = Mathf.Min(Screen.width, Screen.height);
		Rect result;
		switch (PT)
		{
		case PositionType.TopLeft:
			result = new Rect(x * (float)num, y * (float)Screen.height, width * (float)num, height * (float)num);
			break;
		case PositionType.TopRight:
			result = new Rect((float)Screen.width - x * (float)num - width * (float)num, y * (float)Screen.height, width * (float)num, height * (float)num);
			break;
		case PositionType.BottemRight:
			result = new Rect((float)Screen.width - x * (float)num - width * (float)num, (1f - y) * (float)Screen.height - height * (float)num, width * (float)num, height * (float)num);
			break;
		case PositionType.BottemLeft:
			result = new Rect(x * (float)Screen.width, (1f - y) * (float)Screen.height - height * (float)num, width * (float)num, height * (float)num);
			break;
		case PositionType.CenterHorizontal:
			result = new Rect(x * (float)Screen.width - width * (float)num * 0.5f, y * (float)Screen.height, width * (float)num, height * (float)num);
			break;
		default:
			result = default(Rect);
			break;
		}
		return result;
	}

	public static Rect AspectfluidRect(float x, float y, float width, float height, float Aspect)
	{
		int num = (int)(Aspect * (float)Screen.height);
		return new Rect(x * (float)num - (float)(num - Screen.width) * 0.5f, y * (float)Screen.height, width * (float)num, height * (float)Screen.height);
	}

	public static Texture2D[] AutoSliceStripForBlitting(Texture2D source, int Sections)
	{
		Texture2D[] array = new Texture2D[Sections];
		for (int i = 0; i < Sections; i++)
		{
			Rect crop = new Rect(source.width / Sections * i, 0f, source.width / Sections, source.height);
			array[i] = CropTexture(source, crop);
		}
		return array;
	}

	public static Texture2D CropTexture(Texture2D source, Rect Crop)
	{
		Texture2D texture2D = new Texture2D((int)Crop.width, (int)Crop.height);
		for (int i = 0; (float)i < Crop.width; i++)
		{
			for (int j = 0; (float)j < Crop.height; j++)
			{
				texture2D.SetPixel(i, j, source.GetPixel((int)(Crop.x + (float)i), (int)(Crop.y + (float)j)));
			}
		}
		texture2D.Apply();
		return texture2D;
	}

	public static Rect fluidRect(float x, float y, float width, float height)
	{
		return new Rect(x * (float)Screen.width, y * (float)Screen.height, width * (float)Screen.width, height * (float)Screen.height);
	}

	public virtual void Main()
	{
	}
}
