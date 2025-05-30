public interface IEditorWordBehaviour : IEditorTrackableBehaviour
{
	string SpecificWord { get; }

	WordTemplateMode Mode { get; }

	bool IsTemplateMode { get; }

	bool IsSpecificWordMode { get; }

	void SetSpecificWord(string word);

	void SetMode(WordTemplateMode mode);

	void InitializeWord(Word word);
}
