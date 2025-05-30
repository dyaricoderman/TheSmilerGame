using System.Collections.Generic;

public abstract class WordManager
{
	public abstract IEnumerable<WordResult> GetActiveWordResults();

	public abstract IEnumerable<WordResult> GetNewWords();

	public abstract IEnumerable<Word> GetLostWords();

	public abstract bool TryGetWordBehaviour(Word word, out WordBehaviour behaviour);

	public abstract IEnumerable<WordBehaviour> GetTrackableBehaviours();

	public abstract void DestroyWordBehaviour(WordBehaviour behaviour, bool destroyGameObject = true);
}
