using UnityEngine;

public static class QCARUnity
{
	public enum InitError
	{
		INIT_DEVICE_NOT_SUPPORTED = -2,
		INIT_ERROR = -1,
		INIT_SUCCESS = 0
	}

	public enum QCARHint
	{
		HINT_MAX_SIMULTANEOUS_IMAGE_TARGETS = 0
	}

	public static InitError CheckInitializationError()
	{
		return QCARUnityImpl.CheckInitializationError();
	}

	public static bool IsRendererDirty()
	{
		return QCARUnityImpl.IsRendererDirty();
	}

	public static bool SetHint(QCARHint hint, int value)
	{
		return QCARUnityImpl.SetHint(hint, value);
	}

	public static bool RequiresAlpha()
	{
		return QCARUnityImpl.RequiresAlpha();
	}

	public static Matrix4x4 GetProjectionGL(float nearPlane, float farPlane, ScreenOrientation screenOrientation)
	{
		return QCARUnityImpl.GetProjectionGL(nearPlane, farPlane, screenOrientation);
	}
}
