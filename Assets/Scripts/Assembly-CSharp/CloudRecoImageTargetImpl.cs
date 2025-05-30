using System.Collections.Generic;
using UnityEngine;

public class CloudRecoImageTargetImpl : TrackableImpl, ImageTarget, Trackable
{
	private readonly Vector2 mSize;

	public ImageTargetType ImageTargetType
	{
		get
		{
			return ImageTargetType.CLOUD_RECO;
		}
	}

	public CloudRecoImageTargetImpl(string name, int id, Vector2 size)
		: base(name, id)
	{
		base.Type = TrackableType.IMAGE_TARGET;
		mSize = size;
	}

	public Vector2 GetSize()
	{
		return mSize;
	}

	public void SetSize(Vector2 size)
	{
		Debug.LogError("Setting the size of cloud reco targets is currently not supported.");
	}

	public VirtualButton CreateVirtualButton(string name, RectangleData area)
	{
		Debug.LogError("Virtual buttons are currently not supported for cloud reco targets.");
		return null;
	}

	public VirtualButton GetVirtualButtonByName(string name)
	{
		Debug.LogError("Virtual buttons are currently not supported for cloud reco targets.");
		return null;
	}

	public IEnumerable<VirtualButton> GetVirtualButtons()
	{
		Debug.LogError("Virtual buttons are currently not supported for cloud reco targets.");
		return new List<VirtualButton>();
	}

	public bool DestroyVirtualButton(VirtualButton vb)
	{
		Debug.LogError("Virtual buttons are currently not supported for cloud reco targets.");
		return false;
	}
}
