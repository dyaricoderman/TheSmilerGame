using System;
using UnityEngine;

public class NullWebCamTexAdaptor : WebCamTexAdaptor
{
	private const string ERROR_MSG = "No camera connected!\nTo run your application using Play Mode, please connect a webcam to your computer.";

	private readonly Texture2D mTexture;

	private bool mPseudoPlaying = true;

	private readonly double mMsBetweenFrames;

	private DateTime mLastFrame;

	public override bool DidUpdateThisFrame
	{
		get
		{
			if ((DateTime.Now - mLastFrame).TotalMilliseconds > mMsBetweenFrames)
			{
				mLastFrame = DateTime.Now;
				return true;
			}
			return false;
		}
	}

	public override bool IsPlaying
	{
		get
		{
			return mPseudoPlaying;
		}
	}

	public override Texture Texture
	{
		get
		{
			return mTexture;
		}
	}

	public NullWebCamTexAdaptor(int requestedFPS, QCARRenderer.Vec2I requestedTextureSize)
	{
		mTexture = new Texture2D(requestedTextureSize.x, requestedTextureSize.y);
		mMsBetweenFrames = 1000.0 / (double)requestedFPS;
		mLastFrame = DateTime.Now - TimeSpan.FromDays(1.0);
		if (QCARRuntimeUtilities.IsQCAREnabled())
		{
			Debug.LogError("No camera connected!\nTo run your application using Play Mode, please connect a webcam to your computer.");
		}
	}

	public override void Play()
	{
		mPseudoPlaying = true;
	}

	public override void Stop()
	{
		mPseudoPlaying = false;
	}
}
