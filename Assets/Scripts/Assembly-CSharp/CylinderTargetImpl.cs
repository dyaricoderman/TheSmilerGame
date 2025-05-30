using System;
using System.Runtime.InteropServices;

public class CylinderTargetImpl : TrackableImpl, CylinderTarget, Trackable
{
	private float mSideLength;

	private float mTopDiameter;

	private float mBottomDiameter;

	private readonly DataSetImpl mDataSet;

	public CylinderTargetImpl(string name, int id, DataSet dataSet)
		: base(name, id)
	{
		base.Type = TrackableType.CYLINDER_TARGET;
		mDataSet = (DataSetImpl)dataSet;
		float[] array = new float[3];
		IntPtr intPtr = Marshal.AllocHGlobal(3 * Marshal.SizeOf(typeof(float)));
		QCARWrapper.Instance.CylinderTargetGetSize(mDataSet.DataSetPtr, Name, intPtr);
		Marshal.Copy(intPtr, array, 0, 3);
		Marshal.FreeHGlobal(intPtr);
		mSideLength = array[0];
		mTopDiameter = array[1];
		mBottomDiameter = array[2];
	}

	public float GetSideLength()
	{
		return mSideLength;
	}

	public float GetTopDiameter()
	{
		return mTopDiameter;
	}

	public float GetBottomDiameter()
	{
		return mBottomDiameter;
	}

	public bool SetSideLength(float sideLength)
	{
		ScaleCylinder(sideLength / mSideLength);
		return QCARWrapper.Instance.CylinderTargetSetSideLength(mDataSet.DataSetPtr, Name, sideLength) == 1;
	}

	public bool SetTopDiameter(float topDiameter)
	{
		ScaleCylinder(topDiameter / mTopDiameter);
		return QCARWrapper.Instance.CylinderTargetSetTopDiameter(mDataSet.DataSetPtr, Name, topDiameter) == 1;
	}

	public bool SetBottomDiameter(float bottomDiameter)
	{
		ScaleCylinder(bottomDiameter / mBottomDiameter);
		return QCARWrapper.Instance.CylinderTargetSetBottomDiameter(mDataSet.DataSetPtr, Name, bottomDiameter) == 1;
	}

	private void ScaleCylinder(float scale)
	{
		mSideLength *= scale;
		mTopDiameter *= scale;
		mBottomDiameter *= scale;
	}
}
