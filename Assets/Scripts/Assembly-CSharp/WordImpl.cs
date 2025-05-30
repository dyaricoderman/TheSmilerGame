using System;
using System.Runtime.InteropServices;
using UnityEngine;

public class WordImpl : TrackableImpl, Trackable, Word
{
	private string mText;

	private Vector2 mSize;

	private Image mLetterMask;

	private QCARManagerImpl.ImageHeaderData mLetterImageHeader;

	private RectangleData[] mLetterBoundingBoxes;

	public string StringValue
	{
		get
		{
			return mText;
		}
	}

	public Vector2 Size
	{
		get
		{
			return mSize;
		}
	}

	public WordImpl(int id, string text, Vector2 size)
		: base(text, id)
	{
		base.Type = TrackableType.WORD;
		mText = text;
		mSize = size;
	}

	public Image GetLetterMask()
	{
		if (!QCARRuntimeUtilities.IsQCAREnabled())
		{
			return null;
		}
		if (mLetterMask == null)
		{
			CreateLetterMask();
		}
		return mLetterMask;
	}

	public RectangleData[] GetLetterBoundingBoxes()
	{
		if (!QCARRuntimeUtilities.IsQCAREnabled())
		{
			return new RectangleData[0];
		}
		if (mLetterBoundingBoxes == null)
		{
			int length = mText.Length;
			mLetterBoundingBoxes = new RectangleData[length];
			IntPtr intPtr = Marshal.AllocHGlobal(length * Marshal.SizeOf(typeof(RectangleData)));
			QCARWrapper.Instance.WordGetLetterBoundingBoxes(ID, intPtr);
			IntPtr ptr = new IntPtr(intPtr.ToInt32());
			for (int i = 0; i < length; i++)
			{
				mLetterBoundingBoxes[i] = (RectangleData)Marshal.PtrToStructure(ptr, typeof(RectangleData));
				ptr = new IntPtr(ptr.ToInt32() + Marshal.SizeOf(typeof(RectangleData)));
			}
			Marshal.FreeHGlobal(intPtr);
		}
		return mLetterBoundingBoxes;
	}

	private void InitImageHeader()
	{
		mLetterImageHeader = default(QCARManagerImpl.ImageHeaderData);
		mLetterImageHeader.width = (mLetterImageHeader.bufferWidth = (int)(Size.x + 1f));
		mLetterImageHeader.height = (mLetterImageHeader.bufferHeight = (int)(Size.y + 1f));
		mLetterImageHeader.format = 4;
		mLetterMask = new ImageImpl();
	}

	private void CreateLetterMask()
	{
		InitImageHeader();
		ImageImpl imageImpl = (ImageImpl)mLetterMask;
		SetImageValues(mLetterImageHeader, imageImpl);
		AllocateImage(imageImpl);
		mLetterImageHeader.data = imageImpl.UnmanagedData;
		IntPtr intPtr = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(QCARManagerImpl.ImageHeaderData)));
		Marshal.StructureToPtr(mLetterImageHeader, intPtr, false);
		QCARWrapper.Instance.WordGetLetterMask(ID, intPtr);
		mLetterImageHeader = (QCARManagerImpl.ImageHeaderData)Marshal.PtrToStructure(intPtr, typeof(QCARManagerImpl.ImageHeaderData));
		if (mLetterImageHeader.reallocate == 1)
		{
			Debug.LogWarning("image wasn't allocated correctly");
			return;
		}
		imageImpl.CopyPixelsFromUnmanagedBuffer();
		mLetterMask = imageImpl;
		Marshal.FreeHGlobal(intPtr);
	}

	private static void SetImageValues(QCARManagerImpl.ImageHeaderData imageHeader, ImageImpl image)
	{
		image.Width = imageHeader.width;
		image.Height = imageHeader.height;
		image.Stride = imageHeader.stride;
		image.BufferWidth = imageHeader.bufferWidth;
		image.BufferHeight = imageHeader.bufferHeight;
		image.PixelFormat = (Image.PIXEL_FORMAT)imageHeader.format;
	}

	private static void AllocateImage(ImageImpl image)
	{
		image.Pixels = new byte[QCARWrapper.Instance.QcarGetBufferSize(image.BufferWidth, image.BufferHeight, (int)image.PixelFormat)];
		Marshal.FreeHGlobal(image.UnmanagedData);
		image.UnmanagedData = Marshal.AllocHGlobal(QCARWrapper.Instance.QcarGetBufferSize(image.BufferWidth, image.BufferHeight, (int)image.PixelFormat));
	}
}
