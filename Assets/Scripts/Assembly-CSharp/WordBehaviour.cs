using UnityEngine;

public class WordBehaviour : TrackableBehaviour, IEditorTrackableBehaviour, IEditorWordBehaviour
{
	[HideInInspector]
	[SerializeField]
	private WordTemplateMode mMode;

	[SerializeField]
	[HideInInspector]
	private string mSpecificWord;

	private Word mWord;

	string IEditorWordBehaviour.SpecificWord
	{
		get
		{
			return mSpecificWord;
		}
	}

	WordTemplateMode IEditorWordBehaviour.Mode
	{
		get
		{
			return mMode;
		}
	}

	bool IEditorWordBehaviour.IsTemplateMode
	{
		get
		{
			return mMode == WordTemplateMode.Template;
		}
	}

	bool IEditorWordBehaviour.IsSpecificWordMode
	{
		get
		{
			return mMode == WordTemplateMode.SpecificWord;
		}
	}

	public Word Word
	{
		get
		{
			return mWord;
		}
	}

	void IEditorWordBehaviour.SetSpecificWord(string word)
	{
		mSpecificWord = word;
	}

	void IEditorWordBehaviour.SetMode(WordTemplateMode mode)
	{
		mMode = mode;
	}

	void IEditorWordBehaviour.InitializeWord(Word word)
	{
		mTrackable = (mWord = word);
		Vector2 size = word.Size;
		Vector3 vector = Vector3.one;
		MeshFilter component = GetComponent<MeshFilter>();
		if (component != null)
		{
			vector = component.sharedMesh.bounds.size;
		}
		float num = size.y / vector.z;
		base.transform.localScale = new Vector3(num, num, num);
	}

	protected override void InternalUnregisterTrackable()
	{
		mTrackable = (mWord = null);
	}
}
