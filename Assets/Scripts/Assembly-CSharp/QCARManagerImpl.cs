using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class QCARManagerImpl : QCARManager
{
	[StructLayout(LayoutKind.Sequential, Pack = 1)]
	public struct PoseData
	{
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
		public Vector3 position;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
		public Quaternion orientation;
	}

	[StructLayout(LayoutKind.Sequential, Pack = 1)]
	public struct TrackableResultData
	{
		public PoseData pose;

		public TrackableBehaviour.Status status;

		public int id;
	}

	[StructLayout(LayoutKind.Sequential, Pack = 1)]
	public struct VirtualButtonData
	{
		public int id;

		public int isPressed;
	}

	[StructLayout(LayoutKind.Sequential, Pack = 1)]
	public struct Obb2D
	{
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
		public Vector2 center;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
		public Vector2 halfExtents;

		public float rotation;
	}

	[StructLayout(LayoutKind.Sequential, Pack = 1)]
	public struct WordResultData
	{
		public PoseData pose;

		public TrackableBehaviour.Status status;

		public int id;

		public Obb2D orientedBoundingBox;
	}

	[StructLayout(LayoutKind.Sequential, Pack = 1)]
	public struct WordData
	{
		public int id;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
		public Vector2 size;

		public IntPtr stringValue;
	}

	[StructLayout(LayoutKind.Sequential, Pack = 1)]
	public struct ImageHeaderData
	{
		public int width;

		public int height;

		public int stride;

		public int bufferWidth;

		public int bufferHeight;

		public int format;

		public int reallocate;

		public int updated;

		public IntPtr data;
	}

	[StructLayout(LayoutKind.Sequential, Pack = 1)]
	private struct FrameState
	{
		public int numTrackableResults;

		public int numVirtualButtonResults;

		public int frameIndex;

		public IntPtr trackableDataArray;

		public IntPtr vbDataArray;

		public int numWordResults;

		public IntPtr wordResultArray;

		public int numNewWords;

		public IntPtr newWordDataArray;

		public IntPtr videoModeData;
	}

	private struct AutoRotationState
	{
		public bool setOnPause;

		public bool autorotateToPortrait;

		public bool autorotateToPortraitUpsideDown;

		public bool autorotateToLandscapeLeft;

		public bool autorotateToLandscapeRight;
	}

	private QCARBehaviour.WorldCenterMode mWorldCenterMode;

	private TrackableBehaviour mWorldCenter;

	private Camera mARCamera;

	private TrackableResultData[] mTrackableResultDataArray;

	private WordData[] mWordDataArray;

	private WordResultData[] mWordResultDataArray;

	private LinkedList<int> mTrackableFoundQueue = new LinkedList<int>();

	private IntPtr mImageHeaderData = IntPtr.Zero;

	private int mNumImageHeaders;

	private bool mDrawVideobackground = true;

	private int mInjectedFrameIdx;

	private IntPtr mLastProcessedFrameStatePtr = IntPtr.Zero;

	private bool mInitialized;

	private bool mPaused;

	private FrameState mFrameState;

	private AutoRotationState mAutoRotationState;

	public override QCARBehaviour.WorldCenterMode WorldCenterMode
	{
		get
		{
			return mWorldCenterMode;
		}
		set
		{
			mWorldCenterMode = value;
		}
	}

	public override TrackableBehaviour WorldCenter
	{
		get
		{
			return mWorldCenter;
		}
		set
		{
			mWorldCenter = value;
		}
	}

	public override Camera ARCamera
	{
		get
		{
			return mARCamera;
		}
		set
		{
			mARCamera = value;
		}
	}

	public override bool DrawVideoBackground
	{
		get
		{
			return mDrawVideobackground;
		}
		set
		{
			mDrawVideobackground = value;
		}
	}

	public override bool Initialized
	{
		get
		{
			return mInitialized;
		}
	}

	public override bool Init()
	{
		mTrackableResultDataArray = new TrackableResultData[0];
		mWordDataArray = new WordData[0];
		mWordResultDataArray = new WordResultData[0];
		mTrackableFoundQueue = new LinkedList<int>();
		mImageHeaderData = IntPtr.Zero;
		mNumImageHeaders = 0;
		mLastProcessedFrameStatePtr = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(FrameState)));
		QCARWrapper.Instance.InitFrameState(mLastProcessedFrameStatePtr);
		InitializeTrackableContainer(0);
		mInitialized = true;
		return true;
	}

	public void Update(ScreenOrientation counterRotation, CameraDevice.CameraDeviceMode deviceMode, ref CameraDevice.VideoModeData videoMode)
	{
		if (mPaused)
		{
			QCARWrapper.Instance.PausedUpdateQCAR();
		}
		else
		{
			if (!QCARRuntimeUtilities.IsQCAREnabled())
			{
				UpdateTrackablesEditor();
				return;
			}
			UpdateImageContainer();
			if (QCARRuntimeUtilities.IsPlayMode())
			{
				CameraDeviceImpl cameraDeviceImpl = (CameraDeviceImpl)CameraDevice.Instance;
				if (cameraDeviceImpl.WebCam.DidUpdateThisFrame)
				{
					InjectCameraFrame();
				}
			}
			QCARWrapper.Instance.UpdateQCAR(mImageHeaderData, mNumImageHeaders, mLastProcessedFrameStatePtr, (int)counterRotation, (int)deviceMode);
			mFrameState = (FrameState)Marshal.PtrToStructure(mLastProcessedFrameStatePtr, typeof(FrameState));
		}
		if (QCARRuntimeUtilities.IsPlayMode())
		{
			videoMode = CameraDevice.Instance.GetVideoMode(deviceMode);
		}
		else
		{
			IntPtr videoModeData = mFrameState.videoModeData;
			videoMode = (CameraDevice.VideoModeData)Marshal.PtrToStructure(videoModeData, typeof(CameraDevice.VideoModeData));
		}
		InitializeTrackableContainer(mFrameState.numTrackableResults);
		UpdateCameraFrame();
		UpdateTrackers(mFrameState);
		if (QCARRuntimeUtilities.IsPlayMode())
		{
			CameraDeviceImpl cameraDeviceImpl2 = (CameraDeviceImpl)CameraDevice.Instance;
			cameraDeviceImpl2.WebCam.SetFrameIndex(mFrameState.frameIndex);
		}
		if (!mDrawVideobackground)
		{
			RenderVideoBackgroundOrDrawIntoTextureInNative();
		}
	}

	public void PrepareRendering()
	{
		if (mDrawVideobackground)
		{
			RenderVideoBackgroundOrDrawIntoTextureInNative();
		}
	}

	public void FinishRendering()
	{
		QCARWrapper.Instance.RendererEnd();
	}

	public override void Deinit()
	{
		if (mInitialized)
		{
			Marshal.FreeHGlobal(mImageHeaderData);
			QCARWrapper.Instance.DeinitFrameState(mLastProcessedFrameStatePtr);
			Marshal.FreeHGlobal(mLastProcessedFrameStatePtr);
			mInitialized = false;
			mPaused = false;
		}
	}

	public void Pause(bool pause)
	{
		if (pause)
		{
			mAutoRotationState = new AutoRotationState
			{
				autorotateToLandscapeLeft = Screen.autorotateToLandscapeLeft,
				autorotateToLandscapeRight = Screen.autorotateToLandscapeRight,
				autorotateToPortrait = Screen.autorotateToPortrait,
				autorotateToPortraitUpsideDown = Screen.autorotateToPortraitUpsideDown,
				setOnPause = true
			};
			Screen.autorotateToLandscapeLeft = false;
			Screen.autorotateToLandscapeRight = false;
			Screen.autorotateToPortrait = false;
			Screen.autorotateToPortraitUpsideDown = false;
		}
		else if (mAutoRotationState.setOnPause)
		{
			Screen.autorotateToLandscapeLeft = mAutoRotationState.autorotateToLandscapeLeft;
			Screen.autorotateToLandscapeRight = mAutoRotationState.autorotateToLandscapeRight;
			Screen.autorotateToPortrait = mAutoRotationState.autorotateToPortrait;
			Screen.autorotateToPortraitUpsideDown = mAutoRotationState.autorotateToPortraitUpsideDown;
		}
		mPaused = pause;
	}

	private void InitializeTrackableContainer(int numTrackableResults)
	{
		if (mTrackableResultDataArray.Length != numTrackableResults)
		{
			mTrackableResultDataArray = new TrackableResultData[numTrackableResults];
			Debug.Log("Num trackables detected: " + numTrackableResults);
		}
	}

	private void UpdateTrackers(FrameState frameState)
	{
		for (int i = 0; i < frameState.numTrackableResults; i++)
		{
			IntPtr ptr = new IntPtr(frameState.trackableDataArray.ToInt32() + i * Marshal.SizeOf(typeof(TrackableResultData)));
			TrackableResultData trackableResultData = (TrackableResultData)Marshal.PtrToStructure(ptr, typeof(TrackableResultData));
			mTrackableResultDataArray[i] = trackableResultData;
		}
		TrackableResultData[] array = mTrackableResultDataArray;
		for (int j = 0; j < array.Length; j++)
		{
			TrackableResultData trackableResultData2 = array[j];
			if (trackableResultData2.status == TrackableBehaviour.Status.DETECTED || trackableResultData2.status == TrackableBehaviour.Status.TRACKED)
			{
				if (!mTrackableFoundQueue.Contains(trackableResultData2.id))
				{
					mTrackableFoundQueue.AddLast(trackableResultData2.id);
				}
			}
			else if (mTrackableFoundQueue.Contains(trackableResultData2.id))
			{
				mTrackableFoundQueue.Remove(trackableResultData2.id);
			}
		}
		List<int> list = new List<int>(mTrackableFoundQueue);
		int id;
		foreach (int item in list)
		{
			id = item;
			if (Array.Exists(mTrackableResultDataArray, (TrackableResultData tr) => tr.id == id))
			{
				break;
			}
			mTrackableFoundQueue.Remove(id);
		}
		StateManagerImpl stateManagerImpl = (StateManagerImpl)TrackerManager.Instance.GetStateManager();
		int originTrackableID = -1;
		if (mWorldCenterMode == QCARBehaviour.WorldCenterMode.SPECIFIC_TARGET && mWorldCenter != null)
		{
			originTrackableID = mWorldCenter.Trackable.ID;
		}
		else if (mWorldCenterMode == QCARBehaviour.WorldCenterMode.FIRST_TARGET)
		{
			stateManagerImpl.RemoveDisabledTrackablesFromQueue(ref mTrackableFoundQueue);
			if (mTrackableFoundQueue.Count > 0)
			{
				originTrackableID = mTrackableFoundQueue.First.Value;
			}
		}
		UpdateWordTrackables(frameState);
		stateManagerImpl.UpdateCameraPose(mARCamera, mTrackableResultDataArray, originTrackableID);
		stateManagerImpl.UpdateTrackablePoses(mARCamera, mTrackableResultDataArray, originTrackableID, frameState.frameIndex);
		stateManagerImpl.UpdateWords(mARCamera, mWordDataArray, mWordResultDataArray);
		stateManagerImpl.UpdateVirtualButtons(frameState.numVirtualButtonResults, frameState.vbDataArray);
	}

	private void UpdateTrackablesEditor()
	{
		TrackableBehaviour[] array = (TrackableBehaviour[])UnityEngine.Object.FindObjectsOfType(typeof(TrackableBehaviour));
		TrackableBehaviour[] array2 = array;
		foreach (TrackableBehaviour trackableBehaviour in array2)
		{
			if (trackableBehaviour.enabled)
			{
				if (trackableBehaviour is WordBehaviour)
				{
					IEditorWordBehaviour editorWordBehaviour = (IEditorWordBehaviour)trackableBehaviour;
					editorWordBehaviour.SetNameForTrackable((!editorWordBehaviour.IsSpecificWordMode) ? "AnyWord" : editorWordBehaviour.SpecificWord);
					editorWordBehaviour.InitializeWord(new WordImpl(0, editorWordBehaviour.TrackableName, new Vector2(500f, 100f)));
				}
				trackableBehaviour.OnTrackerUpdate(TrackableBehaviour.Status.TRACKED);
			}
		}
	}

	private void UpdateWordTrackables(FrameState frameState)
	{
		mWordDataArray = new WordData[frameState.numNewWords];
		for (int i = 0; i < frameState.numNewWords; i++)
		{
			IntPtr ptr = new IntPtr(frameState.newWordDataArray.ToInt32() + i * Marshal.SizeOf(typeof(WordData)));
			mWordDataArray[i] = (WordData)Marshal.PtrToStructure(ptr, typeof(WordData));
		}
		mWordResultDataArray = new WordResultData[frameState.numWordResults];
		for (int j = 0; j < frameState.numWordResults; j++)
		{
			IntPtr ptr2 = new IntPtr(frameState.wordResultArray.ToInt32() + j * Marshal.SizeOf(typeof(WordResultData)));
			mWordResultDataArray[j] = (WordResultData)Marshal.PtrToStructure(ptr2, typeof(WordResultData));
		}
	}

	private void UpdateImageContainer()
	{
		CameraDeviceImpl cameraDeviceImpl = (CameraDeviceImpl)CameraDevice.Instance;
		if (mNumImageHeaders != cameraDeviceImpl.GetAllImages().Count || (cameraDeviceImpl.GetAllImages().Count > 0 && mImageHeaderData == IntPtr.Zero))
		{
			mNumImageHeaders = cameraDeviceImpl.GetAllImages().Count;
			Marshal.FreeHGlobal(mImageHeaderData);
			mImageHeaderData = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(ImageHeaderData)) * mNumImageHeaders);
		}
		int num = 0;
		foreach (ImageImpl value in cameraDeviceImpl.GetAllImages().Values)
		{
			Marshal.StructureToPtr(ptr: new IntPtr(mImageHeaderData.ToInt32() + num * Marshal.SizeOf(typeof(ImageHeaderData))), structure: new ImageHeaderData
			{
				width = value.Width,
				height = value.Height,
				stride = value.Stride,
				bufferWidth = value.BufferWidth,
				bufferHeight = value.BufferHeight,
				format = (int)value.PixelFormat,
				reallocate = 0,
				updated = 0,
				data = value.UnmanagedData
			}, fDeleteOld: false);
			num++;
		}
	}

	private void UpdateCameraFrame()
	{
		int num = 0;
		CameraDeviceImpl cameraDeviceImpl = (CameraDeviceImpl)CameraDevice.Instance;
		foreach (ImageImpl value in cameraDeviceImpl.GetAllImages().Values)
		{
			IntPtr ptr = new IntPtr(mImageHeaderData.ToInt32() + num * Marshal.SizeOf(typeof(ImageHeaderData)));
			ImageHeaderData imageHeaderData = (ImageHeaderData)Marshal.PtrToStructure(ptr, typeof(ImageHeaderData));
			value.Width = imageHeaderData.width;
			value.Height = imageHeaderData.height;
			value.Stride = imageHeaderData.stride;
			value.BufferWidth = imageHeaderData.bufferWidth;
			value.BufferHeight = imageHeaderData.bufferHeight;
			value.PixelFormat = (Image.PIXEL_FORMAT)imageHeaderData.format;
			if (imageHeaderData.reallocate == 1)
			{
				value.Pixels = new byte[QCARWrapper.Instance.QcarGetBufferSize(value.BufferWidth, value.BufferHeight, (int)value.PixelFormat)];
				Marshal.FreeHGlobal(value.UnmanagedData);
				value.UnmanagedData = Marshal.AllocHGlobal(QCARWrapper.Instance.QcarGetBufferSize(value.BufferWidth, value.BufferHeight, (int)value.PixelFormat));
			}
			else if (imageHeaderData.updated == 1)
			{
				value.CopyPixelsFromUnmanagedBuffer();
			}
			num++;
		}
	}

	private void InjectCameraFrame()
	{
		CameraDeviceImpl cameraDeviceImpl = (CameraDeviceImpl)CameraDevice.Instance;
		Color32[] pixels32AndBufferFrame = cameraDeviceImpl.WebCam.GetPixels32AndBufferFrame(mInjectedFrameIdx);
		GCHandle gCHandle = GCHandle.Alloc(pixels32AndBufferFrame, GCHandleType.Pinned);
		IntPtr pixels = gCHandle.AddrOfPinnedObject();
		int actualWidth = cameraDeviceImpl.WebCam.ActualWidth;
		int actualHeight = cameraDeviceImpl.WebCam.ActualHeight;
		QCARWrapper.Instance.QcarAddCameraFrame(pixels, actualWidth, actualHeight, 16, 4 * actualWidth, mInjectedFrameIdx, cameraDeviceImpl.WebCam.FlipHorizontally ? 1 : 0);
		mInjectedFrameIdx++;
		pixels = IntPtr.Zero;
		gCHandle.Free();
	}

	private void RenderVideoBackgroundOrDrawIntoTextureInNative()
	{
		QCARWrapper.Instance.RendererRenderVideoBackground((!mDrawVideobackground) ? 1 : 0);
		GL.InvalidateState();
	}
}
