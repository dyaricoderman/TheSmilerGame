using UnityEngine;

public class WebCamTexAdaptorImpl : WebCamTexAdaptor
{
	private WebCamTexture mWebCamTexture;

	public override bool DidUpdateThisFrame
	{
		get
		{
			return mWebCamTexture.didUpdateThisFrame;
		}
	}

	public override bool IsPlaying
	{
		get
		{
			return mWebCamTexture.isPlaying;
		}
	}

	public override Texture Texture
	{
		get
		{
			return mWebCamTexture;
		}
	}

	public WebCamTexAdaptorImpl(string deviceName, int requestedFPS, QCARRenderer.Vec2I requestedTextureSize)
	{
		mWebCamTexture = new WebCamTexture();
		mWebCamTexture.deviceName = deviceName;
		mWebCamTexture.requestedFPS = requestedFPS;
		mWebCamTexture.requestedWidth = requestedTextureSize.x;
		mWebCamTexture.requestedHeight = requestedTextureSize.y;
	}

	public override void Play()
	{
		mWebCamTexture.Play();
	}

	public override void Stop()
	{
		mWebCamTexture.Stop();
	}
}
