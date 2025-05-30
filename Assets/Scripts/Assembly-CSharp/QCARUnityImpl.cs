using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using UnityEngine;

public static class QCARUnityImpl
{
	public static QCARUnity.InitError CheckInitializationError()
	{
		return (QCARUnity.InitError)QCARWrapper.Instance.GetInitErrorCode();
	}

	public static bool IsRendererDirty()
	{
		CameraDeviceImpl cameraDeviceImpl = (CameraDeviceImpl)CameraDevice.Instance;
		if (QCARRuntimeUtilities.IsPlayMode())
		{
			return cameraDeviceImpl.IsDirty();
		}
		return QCARWrapper.Instance.IsRendererDirty() == 1 || cameraDeviceImpl.IsDirty();
	}

	public static bool SetHint(QCARUnity.QCARHint hint, int value)
	{
		Debug.Log("SetHint");
		return QCARWrapper.Instance.QcarSetHint((int)hint, value) == 1;
	}

	public static bool RequiresAlpha()
	{
		return QCARWrapper.Instance.QcarRequiresAlpha() == 1;
	}

	public static Matrix4x4 GetProjectionGL(float nearPlane, float farPlane, ScreenOrientation screenOrientation)
	{
		float[] array = new float[16];
		IntPtr intPtr = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(float)) * array.Length);
		QCARWrapper.Instance.GetProjectionGL(nearPlane, farPlane, intPtr, (int)screenOrientation);
		Marshal.Copy(intPtr, array, 0, array.Length);
		Matrix4x4 identity = Matrix4x4.identity;
		for (int i = 0; i < 16; i++)
		{
			identity[i] = array[i];
		}
		Marshal.FreeHGlobal(intPtr);
		return identity;
	}

	public static void SetUnityVersion(string path, bool setNative = false)
	{
		int num = 0;
		int num2 = 0;
		int num3 = 0;
		string pattern = "[^0-9]";
		string[] array = Regex.Split(Application.unityVersion, pattern);
		if (array.Length >= 3)
		{
			num = int.Parse(array[0]);
			num2 = int.Parse(array[1]);
			num3 = int.Parse(array[2]);
		}
		try
		{
			File.WriteAllText(Path.Combine(path, "unity.txt"), string.Format("{0}.{1}.{2}", num, num2, num3));
		}
		catch (Exception ex)
		{
			Debug.LogError("Writing Unity version to file failed: " + ex.Message);
		}
		if (setNative)
		{
			QCARWrapper.Instance.SetUnityVersion(num, num2, num3);
		}
	}
}
