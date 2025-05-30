using System.Runtime.InteropServices;

public abstract class CameraDevice
{
	public enum CameraDeviceMode
	{
		MODE_DEFAULT = -1,
		MODE_OPTIMIZE_SPEED = -2,
		MODE_OPTIMIZE_QUALITY = -3
	}

	public enum FocusMode
	{
		FOCUS_MODE_NORMAL = 0,
		FOCUS_MODE_TRIGGERAUTO = 1,
		FOCUS_MODE_CONTINUOUSAUTO = 2,
		FOCUS_MODE_INFINITY = 3,
		FOCUS_MODE_MACRO = 4
	}

	public enum CameraDirection
	{
		CAMERA_DEFAULT = 0,
		CAMERA_BACK = 1,
		CAMERA_FRONT = 2
	}

	[StructLayout(LayoutKind.Sequential, Pack = 1)]
	public struct VideoModeData
	{
		public int width;

		public int height;

		public float frameRate;
	}

	private static CameraDevice mInstance;

	public static CameraDevice Instance
	{
		get
		{
			if (mInstance == null)
			{
				lock (typeof(CameraDevice))
				{
					if (mInstance == null)
					{
						mInstance = new CameraDeviceImpl();
					}
				}
			}
			return mInstance;
		}
	}

	public abstract bool Init(CameraDirection cameraDirection);

	public abstract bool Deinit();

	public abstract bool Start();

	public abstract bool Stop();

	public abstract VideoModeData GetVideoMode(CameraDeviceMode mode);

	public abstract bool SelectVideoMode(CameraDeviceMode mode);

	public abstract bool SetFlashTorchMode(bool on);

	public abstract bool SetFocusMode(FocusMode mode);

	public abstract bool SetFrameFormat(Image.PIXEL_FORMAT format, bool enabled);

	public abstract Image GetCameraImage(Image.PIXEL_FORMAT format);
}
