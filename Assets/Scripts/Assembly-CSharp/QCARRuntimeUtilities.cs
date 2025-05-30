using System;
using UnityEngine;

public class QCARRuntimeUtilities
{
	private enum WebCamUsed
	{
		UNKNOWN = 0,
		TRUE = 1,
		FALSE = 2
	}

	private static ScreenOrientation sScreenOrientation;

	public static ScreenOrientation ScreenOrientation
	{
		get
		{
			if (IsPlayMode())
			{
				return (sScreenOrientation != ScreenOrientation.Unknown) ? sScreenOrientation : ScreenOrientation.LandscapeLeft;
			}
			if (sScreenOrientation == ScreenOrientation.Unknown)
			{
				sScreenOrientation = (ScreenOrientation)QCARWrapper.Instance.GetSurfaceOrientation();
			}
			return sScreenOrientation;
		}
	}

	public static bool IsLandscapeOrientation
	{
		get
		{
			ScreenOrientation screenOrientation = ScreenOrientation;
			return screenOrientation == ScreenOrientation.LandscapeLeft || screenOrientation == ScreenOrientation.LandscapeLeft || screenOrientation == ScreenOrientation.LandscapeRight;
		}
	}

	public static bool IsPortraitOrientation
	{
		get
		{
			return !IsLandscapeOrientation;
		}
	}

	public static string StripFileNameFromPath(string fullPath)
	{
		string[] array = fullPath.Split('/');
		return array[array.Length - 1];
	}

	public static string StripExtensionFromPath(string fullPath)
	{
		string[] array = fullPath.Split('.');
		if (array.Length <= 1)
		{
			return string.Empty;
		}
		return array[array.Length - 1];
	}

	public static void CacheSurfaceOrientation(ScreenOrientation surfaceOrientation)
	{
		sScreenOrientation = surfaceOrientation;
	}

	public static void ForceDisableTrackables()
	{
		TrackableBehaviour[] array = (TrackableBehaviour[])UnityEngine.Object.FindObjectsOfType(typeof(TrackableBehaviour));
		if (array != null)
		{
			for (int i = 0; i < array.Length; i++)
			{
				array[i].enabled = false;
			}
		}
	}

	public static bool IsPlayMode()
	{
		return false;
	}

	public static bool IsQCAREnabled()
	{
		return true;
	}

	public static void RestartPlayMode()
	{
	}

	public static QCARRenderer.Vec2I ScreenSpaceToCameraFrameCoordinates(Vector2 screenSpaceCoordinate, Rect bgTextureViewPortRect, bool isTextureMirrored, CameraDevice.VideoModeData videoModeData)
	{
		float xMin = bgTextureViewPortRect.xMin;
		float yMin = bgTextureViewPortRect.yMin;
		float width = bgTextureViewPortRect.width;
		float height = bgTextureViewPortRect.height;
		bool isPortrait = false;
		float num = videoModeData.width;
		float num2 = videoModeData.height;
		float prefixX = 0f;
		float prefixY = 0f;
		float inversionMultiplierX = 0f;
		float inversionMultiplierY = 0f;
		PrepareCoordinateConversion(isTextureMirrored, ref prefixX, ref prefixY, ref inversionMultiplierX, ref inversionMultiplierY, ref isPortrait);
		float num3 = (screenSpaceCoordinate.x - xMin) / width;
		float num4 = (screenSpaceCoordinate.y - yMin) / height;
		return (!isPortrait) ? new QCARRenderer.Vec2I(Mathf.RoundToInt((prefixX + inversionMultiplierX * num3) * num), Mathf.RoundToInt((prefixY + inversionMultiplierY * num4) * num2)) : new QCARRenderer.Vec2I(Mathf.RoundToInt((prefixX + inversionMultiplierX * num4) * num), Mathf.RoundToInt((prefixY + inversionMultiplierY * num3) * num2));
	}

	public static Vector2 CameraFrameToScreenSpaceCoordinates(Vector2 cameraFrameCoordinate, Rect bgTextureViewPortRect, bool isTextureMirrored, CameraDevice.VideoModeData videoModeData)
	{
		float xMin = bgTextureViewPortRect.xMin;
		float yMin = bgTextureViewPortRect.yMin;
		float width = bgTextureViewPortRect.width;
		float height = bgTextureViewPortRect.height;
		bool isPortrait = false;
		float num = videoModeData.width;
		float num2 = videoModeData.height;
		float prefixX = 0f;
		float prefixY = 0f;
		float inversionMultiplierX = 0f;
		float inversionMultiplierY = 0f;
		PrepareCoordinateConversion(isTextureMirrored, ref prefixX, ref prefixY, ref inversionMultiplierX, ref inversionMultiplierY, ref isPortrait);
		float num3 = (cameraFrameCoordinate.x / num - prefixX) / inversionMultiplierX;
		float num4 = (cameraFrameCoordinate.y / num2 - prefixY) / inversionMultiplierY;
		return (!isPortrait) ? new Vector2(width * num3 + xMin, height * num4 + yMin) : new Vector2(width * num4 + xMin, height * num3 + yMin);
	}

	public static OrientedBoundingBox CameraFrameToScreenSpaceCoordinates(OrientedBoundingBox cameraFrameObb, Rect bgTextureViewPortRect, bool isTextureMirrored, CameraDevice.VideoModeData videoModeData)
	{
		bool flag = false;
		float num = 0f;
		switch (ScreenOrientation)
		{
		case ScreenOrientation.Portrait:
			num += 90f;
			flag = true;
			break;
		case ScreenOrientation.LandscapeRight:
			num += 180f;
			break;
		case ScreenOrientation.PortraitUpsideDown:
			num += 270f;
			flag = true;
			break;
		}
		float num2 = bgTextureViewPortRect.width / (float)((!flag) ? videoModeData.width : videoModeData.height);
		float num3 = bgTextureViewPortRect.height / (float)((!flag) ? videoModeData.height : videoModeData.width);
		Vector2 center = CameraFrameToScreenSpaceCoordinates(cameraFrameObb.Center, bgTextureViewPortRect, isTextureMirrored, videoModeData);
		Vector2 halfExtents = new Vector2(cameraFrameObb.HalfExtents.x * num2, cameraFrameObb.HalfExtents.y * num3);
		float num4 = cameraFrameObb.Rotation;
		if (isTextureMirrored)
		{
			num4 = 0f - num4;
		}
		num4 = num4 * 180f / (float)Math.PI + num;
		return new OrientedBoundingBox(center, halfExtents, num4);
	}

	public static void SelectRectTopLeftAndBottomRightForLandscapeLeft(Rect screenSpaceRect, bool isMirrored, out Vector2 topLeft, out Vector2 bottomRight)
	{
		if (!isMirrored)
		{
			switch (ScreenOrientation)
			{
			case ScreenOrientation.LandscapeRight:
				topLeft = new Vector2(screenSpaceRect.xMax, screenSpaceRect.yMax);
				bottomRight = new Vector2(screenSpaceRect.xMin, screenSpaceRect.yMin);
				break;
			case ScreenOrientation.Portrait:
				topLeft = new Vector2(screenSpaceRect.xMax, screenSpaceRect.yMin);
				bottomRight = new Vector2(screenSpaceRect.xMin, screenSpaceRect.yMax);
				break;
			case ScreenOrientation.PortraitUpsideDown:
				topLeft = new Vector2(screenSpaceRect.xMin, screenSpaceRect.yMax);
				bottomRight = new Vector2(screenSpaceRect.xMax, screenSpaceRect.yMin);
				break;
			default:
				topLeft = new Vector2(screenSpaceRect.xMin, screenSpaceRect.yMin);
				bottomRight = new Vector2(screenSpaceRect.xMax, screenSpaceRect.yMax);
				break;
			}
		}
		else
		{
			switch (ScreenOrientation)
			{
			case ScreenOrientation.LandscapeRight:
				topLeft = new Vector2(screenSpaceRect.xMin, screenSpaceRect.yMax);
				bottomRight = new Vector2(screenSpaceRect.xMax, screenSpaceRect.yMin);
				break;
			case ScreenOrientation.Portrait:
				topLeft = new Vector2(screenSpaceRect.xMax, screenSpaceRect.yMax);
				bottomRight = new Vector2(screenSpaceRect.xMin, screenSpaceRect.yMin);
				break;
			case ScreenOrientation.PortraitUpsideDown:
				topLeft = new Vector2(screenSpaceRect.xMin, screenSpaceRect.yMin);
				bottomRight = new Vector2(screenSpaceRect.xMax, screenSpaceRect.yMax);
				break;
			default:
				topLeft = new Vector2(screenSpaceRect.xMax, screenSpaceRect.yMin);
				bottomRight = new Vector2(screenSpaceRect.xMin, screenSpaceRect.yMax);
				break;
			}
		}
	}

	public static Rect CalculateRectFromLandscapeLeftCorners(Vector2 topLeft, Vector2 bottomRight, bool isMirrored)
	{
		Rect result;
		if (!isMirrored)
		{
			switch (ScreenOrientation)
			{
			case ScreenOrientation.LandscapeRight:
				result = new Rect(bottomRight.x, bottomRight.y, topLeft.x - bottomRight.x, topLeft.y - bottomRight.y);
				break;
			case ScreenOrientation.Portrait:
				result = new Rect(bottomRight.x, topLeft.y, topLeft.x - bottomRight.x, bottomRight.y - topLeft.y);
				break;
			case ScreenOrientation.PortraitUpsideDown:
				result = new Rect(topLeft.x, bottomRight.y, bottomRight.x - topLeft.x, topLeft.y - bottomRight.y);
				break;
			default:
				result = new Rect(topLeft.x, topLeft.y, bottomRight.x - topLeft.x, bottomRight.y - topLeft.y);
				break;
			}
		}
		else
		{
			switch (ScreenOrientation)
			{
			case ScreenOrientation.LandscapeRight:
				result = new Rect(topLeft.x, bottomRight.y, bottomRight.x - topLeft.x, topLeft.y - bottomRight.y);
				break;
			case ScreenOrientation.Portrait:
				result = new Rect(bottomRight.x, bottomRight.y, topLeft.x - bottomRight.x, topLeft.y - bottomRight.y);
				break;
			case ScreenOrientation.PortraitUpsideDown:
				result = new Rect(topLeft.x, topLeft.y, bottomRight.x - topLeft.x, bottomRight.y - topLeft.y);
				break;
			default:
				result = new Rect(bottomRight.x, topLeft.y, topLeft.x - bottomRight.x, bottomRight.y - topLeft.y);
				break;
			}
		}
		return result;
	}

	public static void DisableSleepMode()
	{
		Screen.sleepTimeout = -1;
	}

	public static void ResetSleepMode()
	{
		Screen.sleepTimeout = -2;
	}

	private static void PrepareCoordinateConversion(bool isTextureMirrored, ref float prefixX, ref float prefixY, ref float inversionMultiplierX, ref float inversionMultiplierY, ref bool isPortrait)
	{
		switch (ScreenOrientation)
		{
		case ScreenOrientation.Portrait:
			isPortrait = true;
			if (!isTextureMirrored)
			{
				prefixX = 0f;
				prefixY = 1f;
				inversionMultiplierX = 1f;
				inversionMultiplierY = -1f;
			}
			else
			{
				prefixX = 1f;
				prefixY = 1f;
				inversionMultiplierX = -1f;
				inversionMultiplierY = -1f;
			}
			break;
		case ScreenOrientation.PortraitUpsideDown:
			isPortrait = true;
			if (!isTextureMirrored)
			{
				prefixX = 1f;
				prefixY = 0f;
				inversionMultiplierX = -1f;
				inversionMultiplierY = 1f;
			}
			else
			{
				prefixX = 0f;
				prefixY = 0f;
				inversionMultiplierX = 1f;
				inversionMultiplierY = 1f;
			}
			break;
		case ScreenOrientation.LandscapeRight:
			isPortrait = false;
			if (!isTextureMirrored)
			{
				prefixX = 1f;
				prefixY = 1f;
				inversionMultiplierX = -1f;
				inversionMultiplierY = -1f;
			}
			else
			{
				prefixX = 0f;
				prefixY = 1f;
				inversionMultiplierX = 1f;
				inversionMultiplierY = -1f;
			}
			break;
		default:
			isPortrait = false;
			if (!isTextureMirrored)
			{
				prefixX = 0f;
				prefixY = 0f;
				inversionMultiplierX = 1f;
				inversionMultiplierY = 1f;
			}
			else
			{
				prefixX = 1f;
				prefixY = 0f;
				inversionMultiplierX = -1f;
				inversionMultiplierY = 1f;
			}
			break;
		}
	}
}
