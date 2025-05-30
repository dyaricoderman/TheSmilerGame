using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using UnityEngine;

public class ImageTargetImpl : TrackableImpl, ImageTarget, Trackable
{
	private Vector2 mSize;

	private readonly DataSetImpl mDataSet;

	private readonly ImageTargetType mImageTargetType;

	private readonly Dictionary<int, VirtualButton> mVirtualButtons;

	public ImageTargetType ImageTargetType
	{
		get
		{
			return mImageTargetType;
		}
	}

	public ImageTargetImpl(string name, int id, ImageTargetType imageTargetType, DataSet dataSet)
		: base(name, id)
	{
		base.Type = TrackableType.IMAGE_TARGET;
		mImageTargetType = imageTargetType;
		mDataSet = (DataSetImpl)dataSet;
		IntPtr intPtr = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(Vector2)));
		QCARWrapper.Instance.ImageTargetGetSize(mDataSet.DataSetPtr, Name, intPtr);
		mSize = (Vector2)Marshal.PtrToStructure(intPtr, typeof(Vector2));
		Marshal.FreeHGlobal(intPtr);
		mVirtualButtons = new Dictionary<int, VirtualButton>();
		CreateVirtualButtonsFromNative();
	}

	public Vector2 GetSize()
	{
		return mSize;
	}

	public void SetSize(Vector2 size)
	{
		mSize = size;
		IntPtr intPtr = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(Vector2)));
		Marshal.StructureToPtr(size, intPtr, false);
		QCARWrapper.Instance.ImageTargetSetSize(mDataSet.DataSetPtr, Name, intPtr);
		Marshal.FreeHGlobal(intPtr);
	}

	public VirtualButton CreateVirtualButton(string name, RectangleData area)
	{
		VirtualButton virtualButton = CreateNewVirtualButtonInNative(name, area);
		if (virtualButton == null)
		{
			Debug.LogError("Could not create Virtual Button.");
		}
		else
		{
			Debug.Log("Created Virtual Button successfully.");
		}
		return virtualButton;
	}

	public VirtualButton GetVirtualButtonByName(string name)
	{
		foreach (VirtualButton value in mVirtualButtons.Values)
		{
			if (value.Name == name)
			{
				return value;
			}
		}
		return null;
	}

	public IEnumerable<VirtualButton> GetVirtualButtons()
	{
		return mVirtualButtons.Values;
	}

	public bool DestroyVirtualButton(VirtualButton vb)
	{
		bool result = false;
		ImageTracker imageTracker = (ImageTracker)TrackerManager.Instance.GetTracker(Tracker.Type.IMAGE_TRACKER);
		if (imageTracker != null)
		{
			bool flag = false;
			foreach (DataSet activeDataSet in imageTracker.GetActiveDataSets())
			{
				if (mDataSet == activeDataSet)
				{
					flag = true;
				}
			}
			if (flag)
			{
				imageTracker.DeactivateDataSet(mDataSet);
			}
			if (UnregisterVirtualButtonInNative(vb))
			{
				Debug.Log("Unregistering virtual button successfully");
				result = true;
				mVirtualButtons.Remove(vb.ID);
			}
			else
			{
				Debug.LogError("Failed to unregister virtual button.");
			}
			if (flag)
			{
				imageTracker.ActivateDataSet(mDataSet);
			}
		}
		return result;
	}

	private VirtualButton CreateNewVirtualButtonInNative(string name, RectangleData rectangleData)
	{
		if (ImageTargetType != ImageTargetType.PREDEFINED)
		{
			Debug.LogError("DataSet.RegisterVirtualButton: virtual button '" + name + "' cannot be registered for a user defined target.");
			return null;
		}
		IntPtr intPtr = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(RectangleData)));
		Marshal.StructureToPtr(rectangleData, intPtr, false);
		bool flag = QCARWrapper.Instance.ImageTargetCreateVirtualButton(mDataSet.DataSetPtr, Name, name, intPtr) != 0;
		VirtualButton virtualButton = null;
		if (flag)
		{
			int num = QCARWrapper.Instance.VirtualButtonGetId(mDataSet.DataSetPtr, Name, name);
			if (!mVirtualButtons.ContainsKey(num))
			{
				virtualButton = new VirtualButtonImpl(name, num, rectangleData, this, mDataSet);
				mVirtualButtons.Add(num, virtualButton);
			}
			else
			{
				virtualButton = mVirtualButtons[num];
			}
		}
		return virtualButton;
	}

	private bool UnregisterVirtualButtonInNative(VirtualButton vb)
	{
		int key = QCARWrapper.Instance.VirtualButtonGetId(mDataSet.DataSetPtr, Name, vb.Name);
		bool flag = false;
		if (QCARWrapper.Instance.ImageTargetDestroyVirtualButton(mDataSet.DataSetPtr, Name, vb.Name) != 0 && mVirtualButtons.Remove(key))
		{
			flag = true;
		}
		if (!flag)
		{
			Debug.LogError("UnregisterVirtualButton: Failed to destroy the Virtual Button.");
		}
		return flag;
	}

	private void CreateVirtualButtonsFromNative()
	{
		int num = QCARWrapper.Instance.ImageTargetGetNumVirtualButtons(mDataSet.DataSetPtr, Name);
		if (num <= 0)
		{
			return;
		}
		IntPtr intPtr = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(QCARManagerImpl.VirtualButtonData)) * num);
		IntPtr intPtr2 = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(RectangleData)) * num);
		QCARWrapper.Instance.ImageTargetGetVirtualButtons(intPtr, intPtr2, num, mDataSet.DataSetPtr, Name);
		for (int i = 0; i < num; i++)
		{
			IntPtr ptr = new IntPtr(intPtr.ToInt32() + i * Marshal.SizeOf(typeof(QCARManagerImpl.VirtualButtonData)));
			QCARManagerImpl.VirtualButtonData virtualButtonData = (QCARManagerImpl.VirtualButtonData)Marshal.PtrToStructure(ptr, typeof(QCARManagerImpl.VirtualButtonData));
			if (!mVirtualButtons.ContainsKey(virtualButtonData.id))
			{
				IntPtr ptr2 = new IntPtr(intPtr2.ToInt32() + i * Marshal.SizeOf(typeof(RectangleData)));
				RectangleData area = (RectangleData)Marshal.PtrToStructure(ptr2, typeof(RectangleData));
				int num2 = 128;
				StringBuilder stringBuilder = new StringBuilder(num2);
				if (QCARWrapper.Instance.ImageTargetGetVirtualButtonName(mDataSet.DataSetPtr, Name, i, stringBuilder, num2) == 0)
				{
					Debug.LogError("Failed to get virtual button name.");
					continue;
				}
				VirtualButton value = new VirtualButtonImpl(stringBuilder.ToString(), virtualButtonData.id, area, this, mDataSet);
				mVirtualButtons.Add(virtualButtonData.id, value);
			}
		}
		Marshal.FreeHGlobal(intPtr);
		Marshal.FreeHGlobal(intPtr2);
	}
}
