using UnityEngine;

public class MarkerBehaviour : TrackableBehaviour, IEditorMarkerBehaviour, IEditorTrackableBehaviour
{
	[SerializeField]
	[HideInInspector]
	private int mMarkerID;

	private Marker mMarker;

	int IEditorMarkerBehaviour.MarkerID
	{
		get
		{
			return mMarkerID;
		}
	}

	public Marker Marker
	{
		get
		{
			return mMarker;
		}
	}

	public MarkerBehaviour()
	{
		mMarkerID = -1;
	}

	bool IEditorMarkerBehaviour.SetMarkerID(int markerID)
	{
		if (mTrackable == null)
		{
			mMarkerID = markerID;
			return true;
		}
		return false;
	}

	void IEditorMarkerBehaviour.InitializeMarker(Marker marker)
	{
		mTrackable = (mMarker = marker);
	}

	protected override void InternalUnregisterTrackable()
	{
		mTrackable = (mMarker = null);
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
}
