using System;
using System.Runtime.InteropServices;
using UnityEngine;

public class ImageImpl : Image
{
	private int mWidth;

	private int mHeight;

	private int mStride;

	private int mBufferWidth;

	private int mBufferHeight;

	private PIXEL_FORMAT mPixelFormat;

	private byte[] mData;

	private IntPtr mUnmanagedData;

	private bool mDataSet;

	public override int Width
	{
		get
		{
			return mWidth;
		}
		set
		{
			mWidth = value;
		}
	}

	public override int Height
	{
		get
		{
			return mHeight;
		}
		set
		{
			mHeight = value;
		}
	}

	public override int Stride
	{
		get
		{
			return mStride;
		}
		set
		{
			mStride = value;
		}
	}

	public override int BufferWidth
	{
		get
		{
			return mBufferWidth;
		}
		set
		{
			mBufferWidth = value;
		}
	}

	public override int BufferHeight
	{
		get
		{
			return mBufferHeight;
		}
		set
		{
			mBufferHeight = value;
		}
	}

	public override PIXEL_FORMAT PixelFormat
	{
		get
		{
			return mPixelFormat;
		}
		set
		{
			mPixelFormat = value;
		}
	}

	public override byte[] Pixels
	{
		get
		{
			return mData;
		}
		set
		{
			mData = value;
		}
	}

	public IntPtr UnmanagedData
	{
		get
		{
			return mUnmanagedData;
		}
		set
		{
			mUnmanagedData = value;
		}
	}

	public ImageImpl()
	{
		mWidth = 0;
		mHeight = 0;
		mStride = 0;
		mBufferWidth = 0;
		mBufferHeight = 0;
		mPixelFormat = PIXEL_FORMAT.UNKNOWN_FORMAT;
		mData = null;
		mUnmanagedData = IntPtr.Zero;
		mDataSet = false;
	}

	~ImageImpl()
	{
		Marshal.FreeHGlobal(mUnmanagedData);
		mUnmanagedData = IntPtr.Zero;
	}

	public override bool IsValid()
	{
		return mWidth > 0 && mHeight > 0 && mStride > 0 && mBufferWidth > 0 && mBufferHeight > 0 && mData != null && mDataSet;
	}

	public void CopyPixelsFromUnmanagedBuffer()
	{
		if (mData == null || mUnmanagedData == IntPtr.Zero)
		{
			Debug.LogError("Image: Cannot copy image image data.");
			return;
		}
		int length;
		switch (mPixelFormat)
		{
		case PIXEL_FORMAT.RGBA8888:
			length = mBufferWidth * mBufferHeight * 4;
			break;
		case PIXEL_FORMAT.RGB888:
			length = mBufferWidth * mBufferHeight * 3;
			break;
		case PIXEL_FORMAT.RGB565:
			length = mBufferWidth * mBufferHeight * 2;
			break;
		default:
			length = mBufferWidth * mBufferHeight;
			break;
		}
		Marshal.Copy(mUnmanagedData, mData, 0, length);
		mDataSet = true;
	}

	public override void CopyToTexture(Texture2D texture2D)
	{
		TextureFormat textureFormat = ConvertPixelFormat(mPixelFormat);
		if (texture2D.width != mWidth || texture2D.height != mHeight || textureFormat != texture2D.format)
		{
			texture2D.Reinitialize(mWidth, mHeight, textureFormat, false);
		}
		int num = 1;
		switch (mPixelFormat)
		{
		case PIXEL_FORMAT.RGBA8888:
			num = 4;
			break;
		case PIXEL_FORMAT.RGB565:
		case PIXEL_FORMAT.RGB888:
			num = 3;
			break;
		}
		Color[] pixels = texture2D.GetPixels();
		int num2 = 0;
		for (int i = 0; i < pixels.Length; i++)
		{
			for (int j = 0; j < num; j++)
			{
				pixels[i][j] = (float)(int)mData[num2++] / 255f;
			}
			for (int k = num; k < 4; k++)
			{
				pixels[i][k] = pixels[i][k - 1];
			}
		}
		texture2D.SetPixels(pixels);
	}

	private TextureFormat ConvertPixelFormat(PIXEL_FORMAT input)
	{
		switch (mPixelFormat)
		{
		case PIXEL_FORMAT.RGBA8888:
			return TextureFormat.RGBA32;
		case PIXEL_FORMAT.RGB888:
			return TextureFormat.RGB24;
		case PIXEL_FORMAT.RGB565:
			return TextureFormat.RGB565;
		default:
			return TextureFormat.Alpha8;
		}
	}
}
