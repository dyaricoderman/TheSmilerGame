public class MultiTargetBehaviour : DataSetTrackableBehaviour, IEditorDataSetTrackableBehaviour, IEditorMultiTargetBehaviour, IEditorTrackableBehaviour
{
	private MultiTarget mMultiTarget;

	public MultiTarget MultiTarget
	{
		get
		{
			return mMultiTarget;
		}
	}

	void IEditorMultiTargetBehaviour.InitializeMultiTarget(MultiTarget multiTarget)
	{
		mTrackable = (mMultiTarget = multiTarget);
	}

	protected override void InternalUnregisterTrackable()
	{
		mTrackable = (mMultiTarget = null);
	}
}
