public interface IEditorTextRecoBehaviour
{
	string WordListFile { get; set; }

	string CustomWordListFile { get; set; }

	string AdditionalCustomWords { get; set; }

	WordFilterMode FilterMode { get; set; }

	string FilterListFile { get; set; }

	string AdditionalFilterWords { get; set; }

	WordPrefabCreationMode WordPrefabCreationMode { get; set; }

	int MaximumWordInstances { get; set; }
}
