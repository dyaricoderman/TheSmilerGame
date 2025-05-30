using System;
using System.Collections.Generic;
using UnityEngine;

public class VirtualButtonBehaviour : MonoBehaviour, IEditorVirtualButtonBehaviour
{
	public const float TARGET_OFFSET = 0.001f;

	[SerializeField]
	[HideInInspector]
	private string mName;

	[SerializeField]
	[HideInInspector]
	private VirtualButton.Sensitivity mSensitivity;

	[HideInInspector]
	[SerializeField]
	private bool mHasUpdatedPose;

	[SerializeField]
	[HideInInspector]
	private Matrix4x4 mPrevTransform = Matrix4x4.zero;

	[SerializeField]
	[HideInInspector]
	private GameObject mPrevParent;

	private bool mSensitivityDirty;

	private bool mPreviouslyEnabled;

	private bool mPressed;

	private List<IVirtualButtonEventHandler> mHandlers;

	private Vector2 mLeftTop;

	private Vector2 mRightBottom;

	private bool mUnregisterOnDestroy;

	private VirtualButton mVirtualButton;

	VirtualButton.Sensitivity IEditorVirtualButtonBehaviour.SensitivitySetting
	{
		get
		{
			return mSensitivity;
		}
	}

	Matrix4x4 IEditorVirtualButtonBehaviour.PreviousTransform
	{
		get
		{
			return mPrevTransform;
		}
	}

	GameObject IEditorVirtualButtonBehaviour.PreviousParent
	{
		get
		{
			return mPrevParent;
		}
	}

	public string VirtualButtonName
	{
		get
		{
			return mName;
		}
	}

	public bool Pressed
	{
		get
		{
			return mPressed;
		}
	}

	public bool HasUpdatedPose
	{
		get
		{
			return mHasUpdatedPose;
		}
	}

	public bool UnregisterOnDestroy
	{
		get
		{
			return mUnregisterOnDestroy;
		}
		set
		{
			mUnregisterOnDestroy = value;
		}
	}

	public VirtualButton VirtualButton
	{
		get
		{
			return mVirtualButton;
		}
	}

	public bool enabled
	{
		get
		{
			return base.enabled;
		}
		set
		{
			base.enabled = value;
		}
	}

	public Transform transform
	{
		get
		{
			return base.transform;
		}
	}

	public GameObject gameObject
	{
		get
		{
			return base.gameObject;
		}
	}

	public Renderer renderer
	{
		get
		{
			return base.GetComponent<Renderer>();
		}
	}

	public VirtualButtonBehaviour()
	{
		mName = string.Empty;
		mPressed = false;
		mSensitivity = VirtualButton.Sensitivity.LOW;
		mSensitivityDirty = false;
		mHandlers = new List<IVirtualButtonEventHandler>();
		mHasUpdatedPose = false;
	}

	bool IEditorVirtualButtonBehaviour.SetVirtualButtonName(string virtualButtonName)
	{
		if (mVirtualButton == null)
		{
			mName = virtualButtonName;
			return true;
		}
		return false;
	}

	bool IEditorVirtualButtonBehaviour.SetSensitivitySetting(VirtualButton.Sensitivity sensibility)
	{
		if (mVirtualButton == null)
		{
			mSensitivity = sensibility;
			mSensitivityDirty = true;
			return true;
		}
		return false;
	}

	bool IEditorVirtualButtonBehaviour.SetPreviousTransform(Matrix4x4 transform)
	{
		if (mVirtualButton == null)
		{
			mPrevTransform = transform;
			return true;
		}
		return false;
	}

	bool IEditorVirtualButtonBehaviour.SetPreviousParent(GameObject parent)
	{
		if (mVirtualButton == null)
		{
			mPrevParent = parent;
			return true;
		}
		return false;
	}

	void IEditorVirtualButtonBehaviour.InitializeVirtualButton(VirtualButton virtualButton)
	{
		mVirtualButton = virtualButton;
	}

	bool IEditorVirtualButtonBehaviour.SetPosAndScaleFromButtonArea(Vector2 topLeft, Vector2 bottomRight)
	{
		ImageTargetBehaviour imageTargetBehaviour = GetImageTargetBehaviour();
		if (imageTargetBehaviour == null)
		{
			return false;
		}
		float num = imageTargetBehaviour.transform.lossyScale[0];
		Vector2 vector = (topLeft + bottomRight) * 0.5f;
		Vector2 vector2 = new Vector2(bottomRight[0] - topLeft[0], topLeft[1] - bottomRight[1]);
		Vector3 position = new Vector3(vector[0] / num, 0.001f, vector[1] / num);
		Vector3 vector3 = new Vector3(vector2[0], (vector2[0] + vector2[1]) * 0.5f, vector2[1]);
		base.transform.position = imageTargetBehaviour.transform.TransformPoint(position);
		base.transform.localScale = vector3 / base.transform.parent.lossyScale[0];
		return true;
	}

	public void RegisterEventHandler(IVirtualButtonEventHandler eventHandler)
	{
		mHandlers.Add(eventHandler);
	}

	public bool UnregisterEventHandler(IVirtualButtonEventHandler eventHandler)
	{
		return mHandlers.Remove(eventHandler);
	}

	public bool CalculateButtonArea(out Vector2 topLeft, out Vector2 bottomRight)
	{
		ImageTargetBehaviour imageTargetBehaviour = GetImageTargetBehaviour();
		if (imageTargetBehaviour == null)
		{
			topLeft = (bottomRight = Vector2.zero);
			return false;
		}
		Vector3 vector = imageTargetBehaviour.transform.InverseTransformPoint(base.transform.position);
		float num = imageTargetBehaviour.transform.lossyScale[0];
		Vector2 vector2 = new Vector2(vector[0] * num, vector[2] * num);
		Vector2 vector3 = new Vector2(base.transform.lossyScale[0], base.transform.lossyScale[2]);
		Vector2 vector4 = Vector2.Scale(vector3 * 0.5f, new Vector2(1f, -1f));
		topLeft = vector2 - vector4;
		bottomRight = vector2 + vector4;
		return true;
	}

	public bool UpdateAreaRectangle()
	{
		RectangleData area = new RectangleData
		{
			leftTopX = mLeftTop.x,
			leftTopY = mLeftTop.y,
			rightBottomX = mRightBottom.x,
			rightBottomY = mRightBottom.y
		};
		if (mVirtualButton == null)
		{
			return false;
		}
		return mVirtualButton.SetArea(area);
	}

	public bool UpdateSensitivity()
	{
		if (mVirtualButton == null)
		{
			return false;
		}
		return mVirtualButton.SetSensitivity(mSensitivity);
	}

	private bool UpdateEnabled()
	{
		return mVirtualButton.SetEnabled(base.enabled);
	}

	public bool UpdatePose()
	{
		ImageTargetBehaviour imageTargetBehaviour = GetImageTargetBehaviour();
		if (imageTargetBehaviour == null)
		{
			return false;
		}
		Transform parent = base.transform.parent;
		while (parent != null)
		{
			if (parent.localScale[0] != parent.localScale[1] || parent.localScale[0] != parent.localScale[2])
			{
				Debug.LogWarning("Detected non-uniform scale in virtual  button object hierarchy. Forcing uniform scaling of object '" + parent.name + "'.");
				parent.localScale = new Vector3(parent.localScale[0], parent.localScale[0], parent.localScale[0]);
			}
			parent = parent.parent;
		}
		mHasUpdatedPose = true;
		if (base.transform.parent != null && base.transform.parent.gameObject != imageTargetBehaviour.gameObject)
		{
			base.transform.localPosition = Vector3.zero;
		}
		Vector3 position = imageTargetBehaviour.transform.InverseTransformPoint(base.transform.position);
		position.y = 0.001f;
		Vector3 position2 = imageTargetBehaviour.transform.TransformPoint(position);
		base.transform.position = position2;
		base.transform.rotation = imageTargetBehaviour.transform.rotation;
		Vector2 topLeft;
		Vector2 bottomRight;
		CalculateButtonArea(out topLeft, out bottomRight);
		float threshold = imageTargetBehaviour.transform.localScale[0] * 0.001f;
		if (!Equals(topLeft, mLeftTop, threshold) || !Equals(bottomRight, mRightBottom, threshold))
		{
			mLeftTop = topLeft;
			mRightBottom = bottomRight;
			return true;
		}
		return false;
	}

	public void OnTrackerUpdated(bool pressed)
	{
		if (mPreviouslyEnabled != base.enabled)
		{
			mPreviouslyEnabled = base.enabled;
			UpdateEnabled();
		}
		if (!base.enabled)
		{
			return;
		}
		if (mPressed != pressed && mHandlers != null)
		{
			if (pressed)
			{
				foreach (IVirtualButtonEventHandler mHandler in mHandlers)
				{
					mHandler.OnButtonPressed(this);
				}
			}
			else
			{
				foreach (IVirtualButtonEventHandler mHandler2 in mHandlers)
				{
					mHandler2.OnButtonReleased(this);
				}
			}
		}
		mPressed = pressed;
	}

	public ImageTargetBehaviour GetImageTargetBehaviour()
	{
		if (base.transform.parent == null)
		{
			return null;
		}
		GameObject gameObject = base.transform.parent.gameObject;
		while (gameObject != null)
		{
			ImageTargetBehaviour component = gameObject.GetComponent<ImageTargetBehaviour>();
			if (component != null)
			{
				return component;
			}
			if (gameObject.transform.parent == null)
			{
				return null;
			}
			gameObject = gameObject.transform.parent.gameObject;
		}
		return null;
	}

	private void LateUpdate()
	{
		if (UpdatePose())
		{
			UpdateAreaRectangle();
		}
		if (mSensitivityDirty && UpdateSensitivity())
		{
			mSensitivityDirty = false;
		}
	}

	private void OnDisable()
	{
		if (!QCARRuntimeUtilities.IsQCAREnabled())
		{
			return;
		}
		if (mPreviouslyEnabled != base.enabled)
		{
			mPreviouslyEnabled = base.enabled;
			UpdateEnabled();
		}
		if (mPressed && mHandlers != null)
		{
			foreach (IVirtualButtonEventHandler mHandler in mHandlers)
			{
				mHandler.OnButtonReleased(this);
			}
		}
		mPressed = false;
	}

	private void OnDestroy()
	{
		if (Application.isPlaying && mUnregisterOnDestroy)
		{
			ImageTargetBehaviour imageTargetBehaviour = GetImageTargetBehaviour();
			if (imageTargetBehaviour != null)
			{
				imageTargetBehaviour.ImageTarget.DestroyVirtualButton(mVirtualButton);
			}
		}
	}

	private static bool Equals(Vector2 vec1, Vector2 vec2, float threshold)
	{
		Vector2 vector = vec1 - vec2;
		return Math.Abs(vector.x) < threshold && Math.Abs(vector.y) < threshold;
	}
}
