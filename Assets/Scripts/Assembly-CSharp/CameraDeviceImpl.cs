using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class CameraDeviceImpl : CameraDevice
{
	private Dictionary<Image.PIXEL_FORMAT, Image> mCameraImages;

	private static WebCamImpl mWebCam;

	private bool mCameraReady;

	private bool mIsDirty;

	public WebCamImpl WebCam
	{
		get
		{
			return mWebCam;
		}
	}

	public bool CameraReady
	{
		get
		{
			if (QCARRuntimeUtilities.IsPlayMode())
			{
				if (mWebCam != null)
				{
					return mWebCam.IsTextureSizeAvailable;
				}
				return false;
			}
			return mCameraReady;
		}
	}

	public CameraDeviceImpl()
	{
		mCameraImages = new Dictionary<Image.PIXEL_FORMAT, Image>();
	}

	public override bool Init(CameraDirection cameraDirection)
	{
		if (QCARRuntimeUtilities.IsPlayMode())
		{
			cameraDirection = CameraDirection.CAMERA_BACK;
		}
		if (InitCameraDevice((int)cameraDirection) == 0)
		{
			return false;
		}
		mCameraReady = true;
		if (CameraReady)
		{
			QCARBehaviour qCARBehaviour = (QCARBehaviour)UnityEngine.Object.FindObjectOfType(typeof(QCARBehaviour));
			if ((bool)qCARBehaviour)
			{
				qCARBehaviour.ResetClearBuffers();
				qCARBehaviour.ConfigureVideoBackground(true);
			}
		}
		return true;
	}

	public override bool Deinit()
	{
		if (DeinitCameraDevice() == 0)
		{
			return false;
		}
		mCameraReady = false;
		return true;
	}

	public override bool Start()
	{
		mIsDirty = true;
		GL.Clear(false, true, new Color(0f, 0f, 0f, 1f));
		if (StartCameraDevice() == 0)
		{
			return false;
		}
		return true;
	}

	public override bool Stop()
	{
		if (StopCameraDevice() == 0)
		{
			return false;
		}
		return true;
	}

	public override VideoModeData GetVideoMode(CameraDeviceMode mode)
	{
		if (QCARRuntimeUtilities.IsPlayMode())
		{
			return WebCam.GetVideoMode();
		}
		IntPtr intPtr = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(VideoModeData)));
		QCARWrapper.Instance.CameraDeviceGetVideoMode((int)mode, intPtr);
		VideoModeData result = (VideoModeData)Marshal.PtrToStructure(intPtr, typeof(VideoModeData));
		Marshal.FreeHGlobal(intPtr);
		return result;
	}

	public override bool SelectVideoMode(CameraDeviceMode mode)
	{
		if (QCARWrapper.Instance.CameraDeviceSelectVideoMode((int)mode) == 0)
		{
			return false;
		}
		return true;
	}

	public override bool SetFlashTorchMode(bool on)
	{
		bool flag = QCARWrapper.Instance.CameraDeviceSetFlashTorchMode(on ? 1 : 0) != 0;
		Debug.Log("Toggle flash " + ((!on) ? "OFF" : "ON") + " " + ((!flag) ? "FAILED" : "WORKED"));
		return flag;
	}

	public override bool SetFocusMode(FocusMode mode)
	{
		bool flag = QCARWrapper.Instance.CameraDeviceSetFocusMode((int)mode) != 0;
		Debug.Log(string.Concat("Requested Focus mode ", mode, (!flag) ? ".  Not supported on this device." : " successfully."));
		return flag;
	}

	public override bool SetFrameFormat(Image.PIXEL_FORMAT format, bool enabled)
	{
		if (enabled)
		{
			if (!mCameraImages.ContainsKey(format))
			{
				if (QCARWrapper.Instance.QcarSetFrameFormat((int)format, 1) == 0)
				{
					Debug.LogError("Failed to set frame format");
					return false;
				}
				Image image = new ImageImpl();
				image.PixelFormat = format;
				mCameraImages.Add(format, image);
				return true;
			}
		}
		else if (mCameraImages.ContainsKey(format))
		{
			if (QCARWrapper.Instance.QcarSetFrameFormat((int)format, 0) == 0)
			{
				Debug.LogError("Failed to set frame format");
				return false;
			}
			return mCameraImages.Remove(format);
		}
		return true;
	}

	public override Image GetCameraImage(Image.PIXEL_FORMAT format)
	{
		if (mCameraImages.ContainsKey(format))
		{
			Image image = mCameraImages[format];
			if (image.IsValid())
			{
				return image;
			}
		}
		return null;
	}

	public Dictionary<Image.PIXEL_FORMAT, Image> GetAllImages()
	{
		return mCameraImages;
	}

	public bool IsDirty()
	{
		if (QCARRuntimeUtilities.IsPlayMode())
		{
			return mIsDirty || WebCam.IsRendererDirty();
		}
		return mIsDirty;
	}

	public void ResetDirtyFlag()
	{
		mIsDirty = false;
	}

	private int InitCameraDevice(int camera)
	{
		return QCARWrapper.Instance.CameraDeviceInitCamera(camera);
	}

	private int DeinitCameraDevice()
	{
		return QCARWrapper.Instance.CameraDeviceDeinitCamera();
	}

	private int StartCameraDevice()
	{
		return QCARWrapper.Instance.CameraDeviceStartCamera();
	}

	private int StopCameraDevice()
	{
		return QCARWrapper.Instance.CameraDeviceStopCamera();
	}
}
