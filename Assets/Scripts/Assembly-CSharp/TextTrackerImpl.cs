using System;
using System.Runtime.InteropServices;
using UnityEngine;

public class TextTrackerImpl : TextTracker
{
	private enum UpDirection
	{
		TEXTTRACKER_UP_IS_0_HRS = 1,
		TEXTTRACKER_UP_IS_3_HRS = 2,
		TEXTTRACKER_UP_IS_6_HRS = 3,
		TEXTTRACKER_UP_IS_9_HRS = 4
	}

	private readonly WordList mWordList = new WordListImpl();

	public override WordList WordList
	{
		get
		{
			return mWordList;
		}
	}

	private UpDirection CurrentUpDirection
	{
		get
		{
			UpDirection upDirection = UpDirection.TEXTTRACKER_UP_IS_0_HRS;
			switch (QCARRuntimeUtilities.ScreenOrientation)
			{
			case ScreenOrientation.Portrait:
				return UpDirection.TEXTTRACKER_UP_IS_9_HRS;
			case ScreenOrientation.PortraitUpsideDown:
				return UpDirection.TEXTTRACKER_UP_IS_3_HRS;
			case ScreenOrientation.LandscapeRight:
				return UpDirection.TEXTTRACKER_UP_IS_6_HRS;
			default:
				return UpDirection.TEXTTRACKER_UP_IS_0_HRS;
			}
		}
	}

	public override bool Start()
	{
		if (QCARWrapper.Instance.TextTrackerStart() == 0)
		{
			Debug.LogError("Could not start tracker.");
			return false;
		}
		return true;
	}

	public override void Stop()
	{
		QCARWrapper.Instance.TextTrackerStop();
		WordManagerImpl wordManagerImpl = (WordManagerImpl)TrackerManager.Instance.GetStateManager().GetWordManager();
		wordManagerImpl.SetWordBehavioursToNotFound();
	}

	public override bool SetRegionOfInterest(Rect detectionRegion, Rect trackingRegion)
	{
		QCARBehaviour qCARBehaviour = (QCARBehaviour)UnityEngine.Object.FindObjectOfType(typeof(QCARBehaviour));
		if (qCARBehaviour == null)
		{
			Debug.LogError("QCAR Behaviour could not be found");
			return false;
		}
		Rect viewportRectangle = qCARBehaviour.GetViewportRectangle();
		bool videoBackGroundMirrored = qCARBehaviour.VideoBackGroundMirrored;
		CameraDevice.VideoModeData videoMode = CameraDevice.Instance.GetVideoMode(qCARBehaviour.CameraDeviceMode);
		Vector2 topLeft;
		Vector2 bottomRight;
		QCARRuntimeUtilities.SelectRectTopLeftAndBottomRightForLandscapeLeft(detectionRegion, videoBackGroundMirrored, out topLeft, out bottomRight);
		Vector2 topLeft2;
		Vector2 bottomRight2;
		QCARRuntimeUtilities.SelectRectTopLeftAndBottomRightForLandscapeLeft(trackingRegion, videoBackGroundMirrored, out topLeft2, out bottomRight2);
		QCARRenderer.Vec2I vec2I = QCARRuntimeUtilities.ScreenSpaceToCameraFrameCoordinates(topLeft, viewportRectangle, videoBackGroundMirrored, videoMode);
		QCARRenderer.Vec2I vec2I2 = QCARRuntimeUtilities.ScreenSpaceToCameraFrameCoordinates(bottomRight, viewportRectangle, videoBackGroundMirrored, videoMode);
		QCARRenderer.Vec2I vec2I3 = QCARRuntimeUtilities.ScreenSpaceToCameraFrameCoordinates(topLeft2, viewportRectangle, videoBackGroundMirrored, videoMode);
		QCARRenderer.Vec2I vec2I4 = QCARRuntimeUtilities.ScreenSpaceToCameraFrameCoordinates(bottomRight2, viewportRectangle, videoBackGroundMirrored, videoMode);
		if (QCARWrapper.Instance.TextTrackerSetRegionOfInterest(vec2I.x, vec2I.y, vec2I2.x, vec2I2.y, vec2I3.x, vec2I3.y, vec2I4.x, vec2I4.y, (int)CurrentUpDirection) == 0)
		{
			Debug.LogError(string.Format("Could not set region of interest: ({0}, {1}, {2}, {3}) - ({4}, {5}, {6}, {7})", detectionRegion.x, detectionRegion.y, detectionRegion.width, detectionRegion.height, trackingRegion.x, trackingRegion.y, trackingRegion.width, trackingRegion.height));
			return false;
		}
		return true;
	}

	public override bool GetRegionOfInterest(out Rect detectionRegion, out Rect trackingRegion)
	{
		QCARBehaviour qCARBehaviour = (QCARBehaviour)UnityEngine.Object.FindObjectOfType(typeof(QCARBehaviour));
		if (qCARBehaviour == null)
		{
			Debug.LogError("QCAR Behaviour could not be found");
			detectionRegion = default(Rect);
			trackingRegion = default(Rect);
			return false;
		}
		Rect viewportRectangle = qCARBehaviour.GetViewportRectangle();
		bool videoBackGroundMirrored = qCARBehaviour.VideoBackGroundMirrored;
		CameraDevice.VideoModeData videoMode = CameraDevice.Instance.GetVideoMode(qCARBehaviour.CameraDeviceMode);
		IntPtr intPtr = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(RectangleIntData)));
		IntPtr intPtr2 = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(RectangleIntData)));
		QCARWrapper.Instance.TextTrackerGetRegionOfInterest(intPtr, intPtr2);
		RectangleIntData camSpaceRectData = (RectangleIntData)Marshal.PtrToStructure(intPtr, typeof(RectangleIntData));
		RectangleIntData camSpaceRectData2 = (RectangleIntData)Marshal.PtrToStructure(intPtr2, typeof(RectangleIntData));
		Marshal.FreeHGlobal(intPtr);
		Marshal.FreeHGlobal(intPtr2);
		detectionRegion = ScreenSpaceRectFromCamSpaceRectData(camSpaceRectData, viewportRectangle, videoBackGroundMirrored, videoMode);
		trackingRegion = ScreenSpaceRectFromCamSpaceRectData(camSpaceRectData2, viewportRectangle, videoBackGroundMirrored, videoMode);
		return true;
	}

	private Rect ScreenSpaceRectFromCamSpaceRectData(RectangleIntData camSpaceRectData, Rect bgTextureViewPortRect, bool isTextureMirrored, CameraDevice.VideoModeData videoModeData)
	{
		Vector2 topLeft = QCARRuntimeUtilities.CameraFrameToScreenSpaceCoordinates(new Vector2(camSpaceRectData.leftTopX, camSpaceRectData.leftTopY), bgTextureViewPortRect, isTextureMirrored, videoModeData);
		Vector2 bottomRight = QCARRuntimeUtilities.CameraFrameToScreenSpaceCoordinates(new Vector2(camSpaceRectData.rightBottomX, camSpaceRectData.rightBottomY), bgTextureViewPortRect, isTextureMirrored, videoModeData);
		return QCARRuntimeUtilities.CalculateRectFromLandscapeLeftCorners(topLeft, bottomRight, isTextureMirrored);
	}
}
