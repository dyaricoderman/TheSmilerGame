using System.Collections.Generic;
using UnityEngine;

public class TextRecoBehaviour : MonoBehaviour, ITrackerEventHandler, IEditorTextRecoBehaviour
{
	private bool mHasInitializedOnce;

	[HideInInspector]
	[SerializeField]
	private string mWordListFile;

	[HideInInspector]
	[SerializeField]
	private string mCustomWordListFile;

	[HideInInspector]
	[SerializeField]
	private string mAdditionalCustomWords;

	[SerializeField]
	[HideInInspector]
	private WordFilterMode mFilterMode;

	[HideInInspector]
	[SerializeField]
	private string mFilterListFile;

	[SerializeField]
	[HideInInspector]
	private string mAdditionalFilterWords;

	[HideInInspector]
	[SerializeField]
	private WordPrefabCreationMode mWordPrefabCreationMode;

	[SerializeField]
	[HideInInspector]
	private int mMaximumWordInstances;

	private List<ITextRecoEventHandler> mTextRecoEventHandlers = new List<ITextRecoEventHandler>();

	string IEditorTextRecoBehaviour.WordListFile
	{
		get
		{
			return mWordListFile;
		}
		set
		{
			mWordListFile = value;
		}
	}

	string IEditorTextRecoBehaviour.CustomWordListFile
	{
		get
		{
			return mCustomWordListFile;
		}
		set
		{
			mCustomWordListFile = value;
		}
	}

	string IEditorTextRecoBehaviour.AdditionalCustomWords
	{
		get
		{
			return mAdditionalCustomWords;
		}
		set
		{
			mAdditionalCustomWords = value;
		}
	}

	WordFilterMode IEditorTextRecoBehaviour.FilterMode
	{
		get
		{
			return mFilterMode;
		}
		set
		{
			mFilterMode = value;
		}
	}

	string IEditorTextRecoBehaviour.FilterListFile
	{
		get
		{
			return mFilterListFile;
		}
		set
		{
			mFilterListFile = value;
		}
	}

	string IEditorTextRecoBehaviour.AdditionalFilterWords
	{
		get
		{
			return mAdditionalFilterWords;
		}
		set
		{
			mAdditionalFilterWords = value;
		}
	}

	WordPrefabCreationMode IEditorTextRecoBehaviour.WordPrefabCreationMode
	{
		get
		{
			return mWordPrefabCreationMode;
		}
		set
		{
			mWordPrefabCreationMode = value;
		}
	}

	int IEditorTextRecoBehaviour.MaximumWordInstances
	{
		get
		{
			return mMaximumWordInstances;
		}
		set
		{
			mMaximumWordInstances = value;
		}
	}

	public bool IsInitialized
	{
		get
		{
			return mHasInitializedOnce;
		}
	}

	private void Awake()
	{
		if (QCARRuntimeUtilities.IsQCAREnabled())
		{
			if (QCARRuntimeUtilities.IsPlayMode())
			{
				QCARUnity.CheckInitializationError();
			}
			bool flag = false;
			QCARBehaviour qCARBehaviour = (QCARBehaviour)Object.FindObjectOfType(typeof(QCARBehaviour));
			if ((bool)qCARBehaviour && qCARBehaviour.enabled)
			{
				qCARBehaviour.enabled = false;
				flag = true;
			}
			if (TrackerManager.Instance.GetTracker(Tracker.Type.TEXT_TRACKER) == null)
			{
				TrackerManager.Instance.InitTracker(Tracker.Type.TEXT_TRACKER);
			}
			if (flag)
			{
				qCARBehaviour.enabled = true;
			}
		}
	}

	private void Start()
	{
		if (KeepAliveBehaviour.Instance != null && KeepAliveBehaviour.Instance.KeepTextRecoBehaviourAlive)
		{
			Object.DontDestroyOnLoad(base.gameObject);
		}
		QCARBehaviour qCARBehaviour = (QCARBehaviour)Object.FindObjectOfType(typeof(QCARBehaviour));
		if ((bool)qCARBehaviour)
		{
			qCARBehaviour.RegisterTrackerEventHandler(this);
		}
	}

	private void OnEnable()
	{
		if (mHasInitializedOnce)
		{
			StartTextTracker();
		}
	}

	private void OnApplicationPause(bool pause)
	{
		if (pause)
		{
			StopTextTracker();
		}
		else
		{
			StartTextTracker();
		}
	}

	private void OnDisable()
	{
		StopTextTracker();
	}

	private void OnDestroy()
	{
		QCARBehaviour qCARBehaviour = (QCARBehaviour)Object.FindObjectOfType(typeof(QCARBehaviour));
		if ((bool)qCARBehaviour)
		{
			qCARBehaviour.UnregisterTrackerEventHandler(this);
		}
		Tracker tracker = TrackerManager.Instance.GetTracker(Tracker.Type.TEXT_TRACKER);
		if (tracker != null)
		{
			WordList wordList = ((TextTracker)tracker).WordList;
			wordList.UnloadAllLists();
		}
	}

	public void RegisterTextRecoEventHandler(ITextRecoEventHandler trackableEventHandler)
	{
		mTextRecoEventHandlers.Add(trackableEventHandler);
		if (mHasInitializedOnce)
		{
			trackableEventHandler.OnInitialized();
		}
	}

	public bool UnregisterTextRecoEventHandler(ITextRecoEventHandler trackableEventHandler)
	{
		return mTextRecoEventHandlers.Remove(trackableEventHandler);
	}

	private void StartTextTracker()
	{
		Debug.Log("Starting Text Tracker");
		if (TrackerManager.Instance.GetTracker(Tracker.Type.TEXT_TRACKER) != null)
		{
			TrackerManager.Instance.GetTracker(Tracker.Type.TEXT_TRACKER).Start();
		}
	}

	private void StopTextTracker()
	{
		Debug.Log("Stopping Text Tracker");
		if (TrackerManager.Instance.GetTracker(Tracker.Type.TEXT_TRACKER) != null)
		{
			TrackerManager.Instance.GetTracker(Tracker.Type.TEXT_TRACKER).Stop();
		}
	}

	private void SetupWordList()
	{
		Tracker tracker = TrackerManager.Instance.GetTracker(Tracker.Type.TEXT_TRACKER);
		if (tracker == null || !(tracker is TextTracker))
		{
			return;
		}
		WordList wordList = ((TextTracker)tracker).WordList;
		wordList.LoadWordListFile(mWordListFile);
		if (mCustomWordListFile != string.Empty)
		{
			wordList.AddWordsFromFile(mCustomWordListFile);
		}
		if (mAdditionalCustomWords != null)
		{
			string[] array = mAdditionalCustomWords.Split('\r', '\n');
			string[] array2 = array;
			foreach (string text in array2)
			{
				if (text.Length > 0)
				{
					wordList.AddWord(text);
				}
			}
		}
		wordList.SetFilterMode(mFilterMode);
		if (mFilterMode == WordFilterMode.NONE)
		{
			return;
		}
		if (mFilterListFile != string.Empty)
		{
			wordList.LoadFilterListFile(mFilterListFile);
		}
		if (mAdditionalFilterWords == null)
		{
			return;
		}
		string[] array3 = mAdditionalFilterWords.Split('\n');
		string[] array4 = array3;
		foreach (string text2 in array4)
		{
			if (text2.Length > 0)
			{
				wordList.AddWordToFilterList(text2);
			}
		}
	}

	private void NotifyEventHandlersOfChanges(IEnumerable<Word> lostWords, IEnumerable<WordResult> newWords)
	{
		foreach (Word lostWord in lostWords)
		{
			foreach (ITextRecoEventHandler mTextRecoEventHandler in mTextRecoEventHandlers)
			{
				mTextRecoEventHandler.OnWordLost(lostWord);
			}
		}
		foreach (WordResult newWord in newWords)
		{
			foreach (ITextRecoEventHandler mTextRecoEventHandler2 in mTextRecoEventHandlers)
			{
				mTextRecoEventHandler2.OnWordDetected(newWord);
			}
		}
	}

	public void OnInitialized()
	{
		SetupWordList();
		StartTextTracker();
		mHasInitializedOnce = true;
		WordManager wordManager = TrackerManager.Instance.GetStateManager().GetWordManager();
		((WordManagerImpl)wordManager).InitializeWordBehaviourTemplates(mWordPrefabCreationMode, mMaximumWordInstances);
		foreach (ITextRecoEventHandler mTextRecoEventHandler in mTextRecoEventHandlers)
		{
			mTextRecoEventHandler.OnInitialized();
		}
	}

	public void OnTrackablesUpdated()
	{
		WordManagerImpl wordManagerImpl = (WordManagerImpl)TrackerManager.Instance.GetStateManager().GetWordManager();
		IEnumerable<WordResult> newWords = wordManagerImpl.GetNewWords();
		IEnumerable<Word> lostWords = wordManagerImpl.GetLostWords();
		NotifyEventHandlersOfChanges(lostWords, newWords);
	}
}
