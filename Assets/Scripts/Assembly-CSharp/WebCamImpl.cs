using System.Collections.Generic;
using UnityEngine;

public class WebCamImpl
{
	private struct BufferedFrame
	{
		public int frameIndex;

		public RenderTexture frame;
	}

	private readonly Camera mARCamera;

	private readonly BGRenderingBehaviour mBgRenderingTexBehaviour;

	private readonly WebCamTexAdaptor mWebCamTexture;

	private CameraDevice.VideoModeData mVideoModeData = default(CameraDevice.VideoModeData);

	private QCARRenderer.VideoTextureInfo mVideoTextureInfo = default(QCARRenderer.VideoTextureInfo);

	private TextureRenderer mTextureRenderer;

	private Texture2D mBufferReadTexture;

	private Rect mReadPixelsRect = default(Rect);

	private readonly WebCamProfile.ProfileData mWebCamProfile = default(WebCamProfile.ProfileData);

	private readonly bool mFlipHorizontally;

	private int mLastScreenWidth;

	private int mLastScreenHeight;

	private readonly Queue<BufferedFrame> mBufferedFrames = new Queue<BufferedFrame>();

	private int mLastFrameIdx = -1;

	private readonly int mRenderTextureLayer;

	private bool mWebcamPlaying;

	public bool DidUpdateThisFrame
	{
		get
		{
			return IsTextureSizeAvailable && mWebCamTexture.DidUpdateThisFrame;
		}
	}

	public bool IsPlaying
	{
		get
		{
			return mWebCamTexture.IsPlaying;
		}
	}

	public int ActualWidth
	{
		get
		{
			return mTextureRenderer.Width;
		}
	}

	public int ActualHeight
	{
		get
		{
			return mTextureRenderer.Height;
		}
	}

	public bool IsTextureSizeAvailable { get; private set; }

	public bool FlipHorizontally
	{
		get
		{
			return mFlipHorizontally;
		}
	}

	public QCARRenderer.Vec2I ResampledTextureSize
	{
		get
		{
			WebCamProfile.ProfileData profileData = mWebCamProfile;
			return profileData.ResampledTextureSize;
		}
	}

	public WebCamImpl(Camera arCamera, Camera backgroundCamera, int renderTextureLayer, string webcamDeviceName, bool flipHorizontally)
	{
	}

	private void RenderFrame(RenderTexture frameToDraw)
	{
		if (QCARRenderer.Instance.DrawVideoBackground)
		{
			mBgRenderingTexBehaviour.SetTexture(frameToDraw);
			return;
		}
		Texture2D videoBackgroundForEmulator = ((QCARRendererImpl)QCARRenderer.Instance).VideoBackgroundForEmulator;
		if (videoBackgroundForEmulator != null)
		{
			if (videoBackgroundForEmulator.width != frameToDraw.width || videoBackgroundForEmulator.height != frameToDraw.height || videoBackgroundForEmulator.format != TextureFormat.ARGB32)
			{
				videoBackgroundForEmulator.Reinitialize(frameToDraw.width, frameToDraw.height, TextureFormat.ARGB32, false);
			}
			RenderTexture.active = frameToDraw;
			videoBackgroundForEmulator.ReadPixels(new Rect(0f, 0f, frameToDraw.width, frameToDraw.height), 0, 0);
			videoBackgroundForEmulator.Apply();
		}
	}

	public void StartCamera()
	{
		mWebcamPlaying = true;
		if (!mWebCamTexture.IsPlaying)
		{
			mWebCamTexture.Play();
		}
	}

	public void StopCamera()
	{
		mWebcamPlaying = false;
		mWebCamTexture.Stop();
	}

	public void ResetPlaying()
	{
		if (mWebcamPlaying)
		{
			StartCamera();
		}
		else
		{
			StopCamera();
		}
	}

	public Color32[] GetPixels32AndBufferFrame(int frameIndex)
	{
		RenderTexture renderTexture = mTextureRenderer.Render();
		mBufferedFrames.Enqueue(new BufferedFrame
		{
			frame = renderTexture,
			frameIndex = frameIndex
		});
		RenderTexture.active = renderTexture;
		mBufferReadTexture.ReadPixels(mReadPixelsRect, 0, 0, false);
		return mBufferReadTexture.GetPixels32();
	}

	public void SetFrameIndex(int frameIndex)
	{
		int frameIndex2 = mLastFrameIdx;
		if (frameIndex2 == frameIndex)
		{
			return;
		}
		while (frameIndex2 != frameIndex && mBufferedFrames.Count != 0)
		{
			BufferedFrame bufferedFrame = mBufferedFrames.Peek();
			frameIndex2 = bufferedFrame.frameIndex;
			if (frameIndex2 == frameIndex)
			{
				RenderFrame(bufferedFrame.frame);
				break;
			}
			mBufferedFrames.Dequeue();
			RenderTexture.ReleaseTemporary(bufferedFrame.frame);
		}
		mLastFrameIdx = frameIndex;
	}

	public CameraDevice.VideoModeData GetVideoMode()
	{
		return mVideoModeData;
	}

	public QCARRenderer.VideoTextureInfo GetVideoTextureInfo()
	{
		return mVideoTextureInfo;
	}

	public bool IsRendererDirty()
	{
		bool flag = IsTextureSizeAvailable && (mLastScreenWidth != Screen.width || mLastScreenHeight != Screen.height);
		if (flag)
		{
			mLastScreenWidth = Screen.width;
			mLastScreenHeight = Screen.height;
		}
		return flag;
	}
}
