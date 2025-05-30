using System;
using System.Runtime.InteropServices;
using UnityEngine;

public class QCARRendererImpl : QCARRenderer
{
	private VideoBGCfgData mVideoBGConfig;

	private bool mVideoBGConfigSet;

	public override bool DrawVideoBackground
	{
		get
		{
			return QCARManager.Instance.DrawVideoBackground;
		}
		set
		{
			QCARManager.Instance.DrawVideoBackground = value;
		}
	}

	public Texture2D VideoBackgroundForEmulator { get; private set; }

	public override VideoBGCfgData GetVideoBackgroundConfig()
	{
		if (QCARRuntimeUtilities.IsPlayMode())
		{
			return mVideoBGConfig;
		}
		IntPtr intPtr = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(VideoBGCfgData)));
		QCARWrapper.Instance.RendererGetVideoBackgroundCfg(intPtr);
		VideoBGCfgData result = (VideoBGCfgData)Marshal.PtrToStructure(intPtr, typeof(VideoBGCfgData));
		Marshal.FreeHGlobal(intPtr);
		return result;
	}

	public override void ClearVideoBackgroundConfig()
	{
		if (QCARRuntimeUtilities.IsPlayMode())
		{
			mVideoBGConfigSet = false;
		}
	}

	public override void SetVideoBackgroundConfig(VideoBGCfgData config)
	{
		if (QCARRuntimeUtilities.IsPlayMode())
		{
			mVideoBGConfig = config;
			mVideoBGConfigSet = true;
			return;
		}
		IntPtr intPtr = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(VideoBGCfgData)));
		Marshal.StructureToPtr(config, intPtr, true);
		QCARWrapper.Instance.RendererSetVideoBackgroundCfg(intPtr);
		Marshal.FreeHGlobal(intPtr);
	}

	public override bool SetVideoBackgroundTexture(Texture2D texture)
	{
		if (QCARRuntimeUtilities.IsPlayMode())
		{
			VideoBackgroundForEmulator = texture;
			return true;
		}
		if (texture != null)
		{
			return QCARWrapper.Instance.RendererSetVideoBackgroundTextureID(texture.GetNativeTextureID()) != 0;
		}
		return true;
	}

	public override bool IsVideoBackgroundInfoAvailable()
	{
		if (QCARRuntimeUtilities.IsPlayMode())
		{
			return mVideoBGConfigSet;
		}
		return QCARWrapper.Instance.RendererIsVideoBackgroundTextureInfoAvailable() != 0;
	}

	public override VideoTextureInfo GetVideoTextureInfo()
	{
		if (QCARRuntimeUtilities.IsPlayMode())
		{
			CameraDeviceImpl cameraDeviceImpl = (CameraDeviceImpl)CameraDevice.Instance;
			return cameraDeviceImpl.WebCam.GetVideoTextureInfo();
		}
		IntPtr intPtr = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(VideoTextureInfo)));
		QCARWrapper.Instance.RendererGetVideoBackgroundTextureInfo(intPtr);
		VideoTextureInfo result = (VideoTextureInfo)Marshal.PtrToStructure(intPtr, typeof(VideoTextureInfo));
		Marshal.FreeHGlobal(intPtr);
		return result;
	}

	public override void Pause(bool pause)
	{
		((QCARManagerImpl)QCARManager.Instance).Pause(pause);
	}
}
