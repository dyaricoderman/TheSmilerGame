using System;
using System.Runtime.InteropServices;
using UnityEngine;

public class VirtualButtonImpl : VirtualButton
{
	private string mName;

	private int mID;

	private RectangleData mArea;

	private bool mIsEnabled;

	private ImageTarget mParentImageTarget;

	private DataSetImpl mParentDataSet;

	public override string Name
	{
		get
		{
			return mName;
		}
	}

	public override int ID
	{
		get
		{
			return mID;
		}
	}

	public override bool Enabled
	{
		get
		{
			return mIsEnabled;
		}
	}

	public override RectangleData Area
	{
		get
		{
			return mArea;
		}
	}

	public VirtualButtonImpl(string name, int id, RectangleData area, ImageTarget imageTarget, DataSet dataSet)
	{
		mName = name;
		mID = id;
		mArea = area;
		mIsEnabled = true;
		mParentImageTarget = imageTarget;
		mParentDataSet = (DataSetImpl)dataSet;
	}

	public override bool SetArea(RectangleData area)
	{
		IntPtr intPtr = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(RectangleData)));
		Marshal.StructureToPtr(area, intPtr, false);
		int num = QCARWrapper.Instance.VirtualButtonSetAreaRectangle(mParentDataSet.DataSetPtr, mParentImageTarget.Name, Name, intPtr);
		Marshal.FreeHGlobal(intPtr);
		if (num == 0)
		{
			Debug.LogError("Virtual Button area rectangle could not be set.");
			return false;
		}
		return true;
	}

	public override bool SetSensitivity(Sensitivity sensitivity)
	{
		if (QCARWrapper.Instance.VirtualButtonSetSensitivity(mParentDataSet.DataSetPtr, mParentImageTarget.Name, mName, (int)sensitivity) == 0)
		{
			Debug.LogError("Virtual Button sensitivity could not be set.");
			return false;
		}
		return true;
	}

	public override bool SetEnabled(bool enabled)
	{
		if (QCARWrapper.Instance.VirtualButtonSetEnabled(mParentDataSet.DataSetPtr, mParentImageTarget.Name, mName, enabled ? 1 : 0) == 0)
		{
			Debug.LogError("Virtual Button enabled value could not be set.");
			return false;
		}
		mIsEnabled = enabled;
		return true;
	}
}
