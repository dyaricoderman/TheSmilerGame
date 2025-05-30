using UnityEngine;

public abstract class DataSetTrackableBehaviour : TrackableBehaviour, IEditorDataSetTrackableBehaviour, IEditorTrackableBehaviour
{
	[HideInInspector]
	[SerializeField]
	protected string mDataSetPath = string.Empty;

	string IEditorDataSetTrackableBehaviour.DataSetName
	{
		get
		{
			string text = QCARRuntimeUtilities.StripFileNameFromPath(mDataSetPath);
			string text2 = QCARRuntimeUtilities.StripExtensionFromPath(mDataSetPath);
			int length = text2.Length;
			if (length > 0)
			{
				length++;
				return text.Remove(text.Length - length);
			}
			return text;
		}
	}

	string IEditorDataSetTrackableBehaviour.DataSetPath
	{
		get
		{
			return mDataSetPath;
		}
	}

	bool IEditorDataSetTrackableBehaviour.SetDataSetPath(string dataSetPath)
	{
		if (mTrackable == null)
		{
			mDataSetPath = dataSetPath;
			return true;
		}
		return false;
	}
}
