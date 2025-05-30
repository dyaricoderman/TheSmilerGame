using System;
using UnityEngine;

public class CylinderTargetBehaviour : DataSetTrackableBehaviour, IEditorCylinderTargetBehaviour, IEditorDataSetTrackableBehaviour, IEditorTrackableBehaviour
{
	private CylinderTarget mCylinderTarget;

	[SerializeField]
	[HideInInspector]
	private float mTopDiameterRatio;

	[HideInInspector]
	[SerializeField]
	private float mBottomDiameterRatio;

	private int mFrameIndex = -1;

	private int mUpdateFrameIndex = -1;

	private float mFutureScale;

	public CylinderTarget CylinderTarget
	{
		get
		{
			return mCylinderTarget;
		}
	}

	public float SideLength
	{
		get
		{
			return GetScale();
		}
	}

	public float TopDiameter
	{
		get
		{
			return mTopDiameterRatio * GetScale();
		}
	}

	public float BottomDiameter
	{
		get
		{
			return mBottomDiameterRatio * GetScale();
		}
	}

	void IEditorCylinderTargetBehaviour.InitializeCylinderTarget(CylinderTarget cylinderTarget)
	{
		mTrackable = (mCylinderTarget = cylinderTarget);
		cylinderTarget.SetSideLength(SideLength);
	}

	void IEditorCylinderTargetBehaviour.SetAspectRatio(float topRatio, float bottomRatio)
	{
		mTopDiameterRatio = topRatio;
		mBottomDiameterRatio = bottomRatio;
	}

	public bool SetSideLength(float value)
	{
		return SetScale(value);
	}

	public bool SetTopDiameter(float value)
	{
		if (Math.Abs(mTopDiameterRatio) > 1E-05f)
		{
			return SetScale(value / mTopDiameterRatio);
		}
		return false;
	}

	public bool SetBottomDiameter(float value)
	{
		if (Math.Abs(mBottomDiameterRatio) > 1E-05f)
		{
			return SetScale(value / mBottomDiameterRatio);
		}
		return false;
	}

	public override void OnFrameIndexUpdate(int newFrameIndex)
	{
		if (mUpdateFrameIndex >= 0 && mUpdateFrameIndex != newFrameIndex)
		{
			ApplyScale(mFutureScale);
			mUpdateFrameIndex = -1;
		}
		mFrameIndex = newFrameIndex;
	}

	protected override bool CorrectScaleImpl()
	{
		bool result = false;
		for (int i = 0; i < 3; i++)
		{
			if (base.transform.localScale[i] != mPreviousScale[i])
			{
				base.transform.localScale = new Vector3(base.transform.localScale[i], base.transform.localScale[i], base.transform.localScale[i]);
				mPreviousScale = base.transform.localScale;
				result = true;
				break;
			}
		}
		return result;
	}

	protected override void InternalUnregisterTrackable()
	{
		mTrackable = (mCylinderTarget = null);
	}

	private float GetScale()
	{
		return base.transform.localScale.x;
	}

	private bool SetScale(float value)
	{
		if (base.transform.localScale.x == value)
		{
			return true;
		}
		if (mCylinderTarget != null)
		{
			if (!mCylinderTarget.SetSideLength(value))
			{
				return false;
			}
			mUpdateFrameIndex = mFrameIndex;
			mFutureScale = value;
		}
		else
		{
			ApplyScale(value);
		}
		return true;
	}

	private void ApplyScale(float value)
	{
		base.transform.localScale = new Vector3(value, value, value);
	}
}
