using System.Collections.Generic;
using UnityEngine;

public class ImageTargetBehaviour : DataSetTrackableBehaviour, IEditorDataSetTrackableBehaviour, IEditorImageTargetBehaviour, IEditorTrackableBehaviour
{
	[HideInInspector]
	[SerializeField]
	private float mAspectRatio;

	[SerializeField]
	[HideInInspector]
	private ImageTargetType mImageTargetType;

	private ImageTarget mImageTarget;

	private Dictionary<int, VirtualButtonBehaviour> mVirtualButtonBehaviours;

	float IEditorImageTargetBehaviour.AspectRatio
	{
		get
		{
			return mAspectRatio;
		}
	}

	ImageTargetType IEditorImageTargetBehaviour.ImageTargetType
	{
		get
		{
			return mImageTargetType;
		}
	}

	public ImageTarget ImageTarget
	{
		get
		{
			return mImageTarget;
		}
	}

	public ImageTargetBehaviour()
	{
		mAspectRatio = 1f;
	}

	bool IEditorImageTargetBehaviour.SetAspectRatio(float aspectRatio)
	{
		if (mTrackable == null)
		{
			mAspectRatio = aspectRatio;
			return true;
		}
		return false;
	}

	bool IEditorImageTargetBehaviour.SetImageTargetType(ImageTargetType imageTargetType)
	{
		if (mTrackable == null)
		{
			mImageTargetType = imageTargetType;
			return true;
		}
		return false;
	}

	void IEditorImageTargetBehaviour.InitializeImageTarget(ImageTarget imageTarget)
	{
		mTrackable = (mImageTarget = imageTarget);
		mVirtualButtonBehaviours = new Dictionary<int, VirtualButtonBehaviour>();
		if (imageTarget.ImageTargetType == ImageTargetType.PREDEFINED)
		{
			Vector2 size = GetSize();
			imageTarget.SetSize(size);
			return;
		}
		Vector2 size2 = imageTarget.GetSize();
		base.transform.localScale = new Vector3(size2.x, size2.x, size2.x);
		((IEditorTrackableBehaviour)this).CorrectScale();
		((IEditorImageTargetBehaviour)this).SetAspectRatio(size2.y / size2.x);
	}

	void IEditorImageTargetBehaviour.AssociateExistingVirtualButtonBehaviour(VirtualButtonBehaviour virtualButtonBehaviour)
	{
		VirtualButton virtualButton = mImageTarget.GetVirtualButtonByName(virtualButtonBehaviour.VirtualButtonName);
		if (virtualButton == null)
		{
			Vector2 topLeft;
			Vector2 bottomRight;
			virtualButtonBehaviour.CalculateButtonArea(out topLeft, out bottomRight);
			RectangleData area = new RectangleData
			{
				leftTopX = topLeft.x,
				leftTopY = topLeft.y,
				rightBottomX = bottomRight.x,
				rightBottomY = bottomRight.y
			};
			virtualButton = mImageTarget.CreateVirtualButton(virtualButtonBehaviour.VirtualButtonName, area);
			if (virtualButton != null)
			{
				Debug.Log("Successfully created virtual button " + virtualButtonBehaviour.VirtualButtonName + " at startup");
				virtualButtonBehaviour.UnregisterOnDestroy = true;
			}
			else
			{
				Debug.LogError("Failed to create virtual button " + virtualButtonBehaviour.VirtualButtonName + " at startup");
			}
		}
		if (virtualButton != null && !mVirtualButtonBehaviours.ContainsKey(virtualButton.ID))
		{
			((IEditorVirtualButtonBehaviour)virtualButtonBehaviour).InitializeVirtualButton(virtualButton);
			mVirtualButtonBehaviours.Add(virtualButton.ID, virtualButtonBehaviour);
			Debug.Log("Found VirtualButton named " + virtualButtonBehaviour.VirtualButton.Name + " with id " + virtualButtonBehaviour.VirtualButton.ID);
			virtualButtonBehaviour.UpdatePose();
			if (!virtualButtonBehaviour.UpdateAreaRectangle() || !virtualButtonBehaviour.UpdateSensitivity())
			{
				Debug.LogError("Failed to update virtual button " + virtualButtonBehaviour.VirtualButton.Name + " at startup");
			}
			else
			{
				Debug.Log("Updated virtual button " + virtualButtonBehaviour.VirtualButton.Name + " at startup");
			}
		}
	}

	void IEditorImageTargetBehaviour.CreateMissingVirtualButtonBehaviours()
	{
		foreach (VirtualButton virtualButton in mImageTarget.GetVirtualButtons())
		{
			CreateVirtualButtonFromNative(virtualButton);
		}
	}

	bool IEditorImageTargetBehaviour.TryGetVirtualButtonBehaviourByID(int id, out VirtualButtonBehaviour virtualButtonBehaviour)
	{
		return mVirtualButtonBehaviours.TryGetValue(id, out virtualButtonBehaviour);
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
		mTrackable = (mImageTarget = null);
	}

	public VirtualButtonBehaviour CreateVirtualButton(string vbName, Vector2 position, Vector2 size)
	{
		GameObject gameObject = new GameObject(vbName);
		VirtualButtonBehaviour virtualButtonBehaviour = gameObject.AddComponent<VirtualButtonBehaviour>();
		gameObject.transform.parent = base.transform;
		IEditorVirtualButtonBehaviour editorVirtualButtonBehaviour = virtualButtonBehaviour;
		editorVirtualButtonBehaviour.SetVirtualButtonName(vbName);
		editorVirtualButtonBehaviour.transform.localScale = new Vector3(size.x, 1f, size.y);
		editorVirtualButtonBehaviour.transform.localPosition = new Vector3(position.x, 1f, position.y);
		if (Application.isPlaying && !CreateNewVirtualButtonFromBehaviour(virtualButtonBehaviour))
		{
			return null;
		}
		virtualButtonBehaviour.UnregisterOnDestroy = true;
		return virtualButtonBehaviour;
	}

	public static VirtualButtonBehaviour CreateVirtualButton(string vbName, Vector2 localScale, GameObject immediateParent)
	{
		GameObject gameObject = new GameObject(vbName);
		VirtualButtonBehaviour virtualButtonBehaviour = gameObject.AddComponent<VirtualButtonBehaviour>();
		GameObject gameObject2 = immediateParent.transform.root.gameObject;
		ImageTargetBehaviour componentInChildren = gameObject2.GetComponentInChildren<ImageTargetBehaviour>();
		if (componentInChildren == null || componentInChildren.ImageTarget == null)
		{
			Debug.LogError("Could not create Virtual Button. immediateParent\"immediateParent\" object is not an Image Target or a child of one.");
			Object.Destroy(gameObject);
			return null;
		}
		gameObject.transform.parent = immediateParent.transform;
		IEditorVirtualButtonBehaviour editorVirtualButtonBehaviour = virtualButtonBehaviour;
		editorVirtualButtonBehaviour.SetVirtualButtonName(vbName);
		editorVirtualButtonBehaviour.transform.localScale = new Vector3(localScale[0], 1f, localScale[1]);
		if (Application.isPlaying && !componentInChildren.CreateNewVirtualButtonFromBehaviour(virtualButtonBehaviour))
		{
			return null;
		}
		virtualButtonBehaviour.UnregisterOnDestroy = true;
		return virtualButtonBehaviour;
	}

	public IEnumerable<VirtualButtonBehaviour> GetVirtualButtonBehaviours()
	{
		return mVirtualButtonBehaviours.Values;
	}

	public void DestroyVirtualButton(string vbName)
	{
		List<VirtualButtonBehaviour> list = new List<VirtualButtonBehaviour>(mVirtualButtonBehaviours.Values);
		foreach (VirtualButtonBehaviour item in list)
		{
			if (item.VirtualButtonName == vbName)
			{
				mVirtualButtonBehaviours.Remove(item.VirtualButton.ID);
				item.UnregisterOnDestroy = true;
				Object.Destroy(item.gameObject);
				break;
			}
		}
	}

	public Vector2 GetSize()
	{
		if (mAspectRatio <= 1f)
		{
			return new Vector2(base.transform.localScale.x, base.transform.localScale.x * mAspectRatio);
		}
		return new Vector2(base.transform.localScale.x / mAspectRatio, base.transform.localScale.x);
	}

	public void SetWidth(float width)
	{
		float num = ((!(mAspectRatio <= 1f)) ? (width / mAspectRatio) : width);
		base.transform.localScale = new Vector3(num, num, num);
	}

	public void SetHeight(float height)
	{
		float num = ((!(mAspectRatio <= 1f)) ? height : (height / mAspectRatio));
		base.transform.localScale = new Vector3(num, num, num);
	}

	private void CreateVirtualButtonFromNative(VirtualButton virtualButton)
	{
		GameObject gameObject = new GameObject(virtualButton.Name);
		VirtualButtonBehaviour virtualButtonBehaviour = gameObject.AddComponent<VirtualButtonBehaviour>();
		virtualButtonBehaviour.transform.parent = base.transform;
		IEditorVirtualButtonBehaviour editorVirtualButtonBehaviour = virtualButtonBehaviour;
		Debug.Log("Creating Virtual Button with values: \n ID:           " + virtualButton.ID + "\n Name:         " + virtualButton.Name + "\n Rectangle:    " + virtualButton.Area.leftTopX + "," + virtualButton.Area.leftTopY + "," + virtualButton.Area.rightBottomX + "," + virtualButton.Area.rightBottomY);
		editorVirtualButtonBehaviour.SetVirtualButtonName(virtualButton.Name);
		editorVirtualButtonBehaviour.SetPosAndScaleFromButtonArea(new Vector2(virtualButton.Area.leftTopX, virtualButton.Area.leftTopY), new Vector2(virtualButton.Area.rightBottomX, virtualButton.Area.rightBottomY));
		editorVirtualButtonBehaviour.UnregisterOnDestroy = false;
		editorVirtualButtonBehaviour.InitializeVirtualButton(virtualButton);
		mVirtualButtonBehaviours.Add(virtualButton.ID, virtualButtonBehaviour);
	}

	private bool CreateNewVirtualButtonFromBehaviour(VirtualButtonBehaviour newVBB)
	{
		Vector2 topLeft;
		Vector2 bottomRight;
		newVBB.CalculateButtonArea(out topLeft, out bottomRight);
		RectangleData area = new RectangleData
		{
			leftTopX = topLeft.x,
			leftTopY = topLeft.y,
			rightBottomX = bottomRight.x,
			rightBottomY = bottomRight.y
		};
		VirtualButton virtualButton = mImageTarget.CreateVirtualButton(newVBB.VirtualButtonName, area);
		if (virtualButton == null)
		{
			Object.Destroy(newVBB.gameObject);
			return false;
		}
		((IEditorVirtualButtonBehaviour)newVBB).InitializeVirtualButton(virtualButton);
		mVirtualButtonBehaviours.Add(virtualButton.ID, newVBB);
		return true;
	}
}
