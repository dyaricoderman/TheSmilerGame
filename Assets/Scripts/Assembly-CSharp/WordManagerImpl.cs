using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using UnityEngine;

public class WordManagerImpl : WordManager
{
	private const string TEMPLATE_IDENTIFIER = "Template_ID";

	private readonly Dictionary<int, WordResult> mTrackedWords = new Dictionary<int, WordResult>();

	private readonly List<WordResult> mNewWords = new List<WordResult>();

	private readonly List<Word> mLostWords = new List<Word>();

	private readonly Dictionary<int, WordBehaviour> mActiveWordBehaviours = new Dictionary<int, WordBehaviour>();

	private readonly List<WordBehaviour> mWordBehavioursMarkedForDeletion = new List<WordBehaviour>();

	private readonly List<Word> mWaitingQueue = new List<Word>();

	private readonly Dictionary<string, List<WordBehaviour>> mWordBehaviours = new Dictionary<string, List<WordBehaviour>>();

	private bool mAutomaticTemplate;

	private int mMaxInstances = 1;

	private WordPrefabCreationMode mWordPrefabCreationMode;

	public override IEnumerable<WordResult> GetActiveWordResults()
	{
		return mTrackedWords.Values;
	}

	public override IEnumerable<WordResult> GetNewWords()
	{
		return mNewWords;
	}

	public override IEnumerable<Word> GetLostWords()
	{
		return mLostWords;
	}

	public override bool TryGetWordBehaviour(Word word, out WordBehaviour behaviour)
	{
		return mActiveWordBehaviours.TryGetValue(word.ID, out behaviour);
	}

	public override IEnumerable<WordBehaviour> GetTrackableBehaviours()
	{
		List<WordBehaviour> list = new List<WordBehaviour>();
		foreach (List<WordBehaviour> value in mWordBehaviours.Values)
		{
			list.AddRange(value);
		}
		return list;
	}

	public override void DestroyWordBehaviour(WordBehaviour behaviour, bool destroyGameObject = true)
	{
		string[] array = mWordBehaviours.Keys.ToArray();
		string[] array2 = array;
		foreach (string key in array2)
		{
			if (mWordBehaviours[key].Contains(behaviour))
			{
				mWordBehaviours[key].Remove(behaviour);
				if (mWordBehaviours[key].Count == 0)
				{
					mWordBehaviours.Remove(key);
				}
				if (destroyGameObject)
				{
					Object.Destroy(behaviour.gameObject);
					mWordBehavioursMarkedForDeletion.Add(behaviour);
				}
				else
				{
					((IEditorTrackableBehaviour)behaviour).UnregisterTrackable();
				}
			}
		}
	}

	public void InitializeWordBehaviourTemplates(WordPrefabCreationMode wordPrefabCreationMode, int maxInstances)
	{
		mWordPrefabCreationMode = wordPrefabCreationMode;
		mMaxInstances = maxInstances;
		InitializeWordBehaviourTemplates();
	}

	public void InitializeWordBehaviourTemplates()
	{
		if (mWordPrefabCreationMode == WordPrefabCreationMode.DUPLICATE)
		{
			List<WordBehaviour> list = mWordBehavioursMarkedForDeletion.ToList();
			if (mAutomaticTemplate && mWordBehaviours.ContainsKey("Template_ID"))
			{
				foreach (WordBehaviour item2 in mWordBehaviours["Template_ID"])
				{
					list.Add(item2);
					Object.Destroy(item2.gameObject);
				}
				mWordBehaviours.Remove("Template_ID");
			}
			WordBehaviour[] array = (WordBehaviour[])Object.FindObjectsOfType(typeof(WordBehaviour));
			WordBehaviour[] array2 = array;
			foreach (WordBehaviour wordBehaviour in array2)
			{
				if (list.Contains(wordBehaviour))
				{
					continue;
				}
				IEditorWordBehaviour editorWordBehaviour = wordBehaviour;
				string text = ((!editorWordBehaviour.IsTemplateMode) ? editorWordBehaviour.SpecificWord.ToLowerInvariant() : "Template_ID");
				if (!mWordBehaviours.ContainsKey(text))
				{
					mWordBehaviours[text] = new List<WordBehaviour> { wordBehaviour };
					if (text == "Template_ID")
					{
						mAutomaticTemplate = false;
					}
				}
			}
			if (!mWordBehaviours.ContainsKey("Template_ID"))
			{
				WordBehaviour item = CreateWordBehaviour();
				mWordBehaviours.Add("Template_ID", new List<WordBehaviour> { item });
				mAutomaticTemplate = true;
			}
		}
		mWordBehavioursMarkedForDeletion.Clear();
	}

	public void RemoveDestroyedTrackables()
	{
		foreach (List<WordBehaviour> value in mWordBehaviours.Values)
		{
			for (int num = value.Count - 1; num >= 0; num--)
			{
				if (value[num] == null)
				{
					value.RemoveAt(num);
				}
			}
		}
		string[] array = mWordBehaviours.Keys.ToArray();
		string[] array2 = array;
		foreach (string key in array2)
		{
			if (mWordBehaviours[key].Count == 0)
			{
				mWordBehaviours.Remove(key);
			}
		}
	}

	public void UpdateWords(Camera arCamera, QCARManagerImpl.WordData[] newWordData, QCARManagerImpl.WordResultData[] wordResults)
	{
		UpdateWords(newWordData, wordResults);
		UpdateWordResultPoses(arCamera, wordResults);
	}

	public void SetWordBehavioursToNotFound()
	{
		foreach (WordBehaviour value in mActiveWordBehaviours.Values)
		{
			value.OnTrackerUpdate(TrackableBehaviour.Status.NOT_FOUND);
		}
	}

	private void UpdateWords(IEnumerable<QCARManagerImpl.WordData> newWordData, IEnumerable<QCARManagerImpl.WordResultData> wordResults)
	{
		mNewWords.Clear();
		mLostWords.Clear();
		foreach (QCARManagerImpl.WordData newWordDatum in newWordData)
		{
			if (!mTrackedWords.ContainsKey(newWordDatum.id))
			{
				WordImpl word = new WordImpl(newWordDatum.id, Marshal.PtrToStringUni(newWordDatum.stringValue), newWordDatum.size);
				WordResultImpl wordResultImpl = new WordResultImpl(word);
				mTrackedWords.Add(newWordDatum.id, wordResultImpl);
				mNewWords.Add(wordResultImpl);
			}
		}
		List<int> list = new List<int>();
		foreach (QCARManagerImpl.WordResultData wordResult in wordResults)
		{
			list.Add(wordResult.id);
		}
		List<int> list2 = mTrackedWords.Keys.ToList();
		foreach (int item in list2)
		{
			if (!list.Contains(item))
			{
				mLostWords.Add(mTrackedWords[item].Word);
				mTrackedWords.Remove(item);
			}
		}
		if (mWordPrefabCreationMode == WordPrefabCreationMode.DUPLICATE)
		{
			UnregisterLostWords();
			AssociateWordResultsWithBehaviours();
		}
	}

	private void UpdateWordResultPoses(Camera arCamera, IEnumerable<QCARManagerImpl.WordResultData> wordResults)
	{
		QCARBehaviour qCARBehaviour = (QCARBehaviour)Object.FindObjectOfType(typeof(QCARBehaviour));
		if (qCARBehaviour == null)
		{
			Debug.LogError("QCAR Behaviour could not be found");
			return;
		}
		Rect viewportRectangle = qCARBehaviour.GetViewportRectangle();
		bool videoBackGroundMirrored = qCARBehaviour.VideoBackGroundMirrored;
		CameraDevice.VideoModeData videoMode = qCARBehaviour.GetVideoMode();
		foreach (QCARManagerImpl.WordResultData wordResult in wordResults)
		{
			WordResultImpl wordResultImpl = (WordResultImpl)mTrackedWords[wordResult.id];
			Vector3 position = arCamera.transform.TransformPoint(wordResult.pose.position);
			Quaternion orientation = wordResult.pose.orientation;
			Quaternion orientation2 = arCamera.transform.rotation * orientation * Quaternion.AngleAxis(270f, Vector3.left);
			wordResultImpl.SetPose(position, orientation2);
			wordResultImpl.SetStatus(wordResult.status);
			OrientedBoundingBox cameraFrameObb = new OrientedBoundingBox(wordResult.orientedBoundingBox.center, wordResult.orientedBoundingBox.halfExtents, wordResult.orientedBoundingBox.rotation);
			wordResultImpl.SetObb(QCARRuntimeUtilities.CameraFrameToScreenSpaceCoordinates(cameraFrameObb, viewportRectangle, videoBackGroundMirrored, videoMode));
		}
		if (mWordPrefabCreationMode == WordPrefabCreationMode.DUPLICATE)
		{
			UpdateWordBehaviourPoses();
		}
	}

	private void AssociateWordResultsWithBehaviours()
	{
		List<Word> list = new List<Word>(mWaitingQueue);
		foreach (Word item in list)
		{
			if (mTrackedWords.ContainsKey(item.ID))
			{
				WordResult wordResult = mTrackedWords[item.ID];
				if (AssociateWordBehaviour(wordResult) != null)
				{
					mWaitingQueue.Remove(item);
				}
			}
			else
			{
				mWaitingQueue.Remove(item);
			}
		}
		foreach (WordResult mNewWord in mNewWords)
		{
			WordBehaviour wordBehaviour = AssociateWordBehaviour(mNewWord);
			if (wordBehaviour == null)
			{
				mWaitingQueue.Add(mNewWord.Word);
			}
		}
	}

	private void UnregisterLostWords()
	{
		foreach (Word mLostWord in mLostWords)
		{
			if (mActiveWordBehaviours.ContainsKey(mLostWord.ID))
			{
				WordBehaviour wordBehaviour = mActiveWordBehaviours[mLostWord.ID];
				wordBehaviour.OnTrackerUpdate(TrackableBehaviour.Status.NOT_FOUND);
				((IEditorTrackableBehaviour)wordBehaviour).UnregisterTrackable();
				mActiveWordBehaviours.Remove(mLostWord.ID);
			}
		}
	}

	private void UpdateWordBehaviourPoses()
	{
		foreach (KeyValuePair<int, WordBehaviour> mActiveWordBehaviour in mActiveWordBehaviours)
		{
			if (mTrackedWords.ContainsKey(mActiveWordBehaviour.Key))
			{
				WordResult wordResult = mTrackedWords[mActiveWordBehaviour.Key];
				Vector3 position = wordResult.Position;
				Quaternion orientation = wordResult.Orientation;
				Vector2 size = wordResult.Word.Size;
				mActiveWordBehaviour.Value.transform.rotation = orientation;
				Vector3 vector = mActiveWordBehaviour.Value.transform.rotation * new Vector3((0f - size.x) * 0.5f, 0f, (0f - size.y) * 0.5f);
				mActiveWordBehaviour.Value.transform.position = position + vector;
				mActiveWordBehaviour.Value.OnTrackerUpdate(wordResult.CurrentStatus);
			}
		}
	}

	private WordBehaviour AssociateWordBehaviour(WordResult wordResult)
	{
		string text = wordResult.Word.StringValue.ToLowerInvariant();
		List<WordBehaviour> list;
		if (mWordBehaviours.ContainsKey(text))
		{
			list = mWordBehaviours[text];
		}
		else
		{
			if (!mWordBehaviours.ContainsKey("Template_ID"))
			{
				Debug.Log("No prefab available for string value " + text);
				return null;
			}
			list = mWordBehaviours["Template_ID"];
		}
		foreach (WordBehaviour item in list)
		{
			if (item.Trackable == null)
			{
				return AssociateWordBehaviour(wordResult, item);
			}
		}
		if (list.Count < mMaxInstances)
		{
			WordBehaviour wordBehaviour = InstantiateWordBehaviour(list.First());
			list.Add(wordBehaviour);
			return AssociateWordBehaviour(wordResult, wordBehaviour);
		}
		return null;
	}

	private WordBehaviour AssociateWordBehaviour(WordResult wordResult, WordBehaviour wordBehaviourTemplate)
	{
		if (mActiveWordBehaviours.Count >= mMaxInstances)
		{
			return null;
		}
		Word word = wordResult.Word;
		((IEditorTrackableBehaviour)wordBehaviourTemplate).SetNameForTrackable(word.StringValue);
		((IEditorWordBehaviour)wordBehaviourTemplate).InitializeWord(word);
		mActiveWordBehaviours.Add(word.ID, wordBehaviourTemplate);
		return wordBehaviourTemplate;
	}

	private static WordBehaviour InstantiateWordBehaviour(WordBehaviour input)
	{
		GameObject gameObject = Object.Instantiate(input.gameObject) as GameObject;
		return gameObject.GetComponent<WordBehaviour>();
	}

	private static WordBehaviour CreateWordBehaviour()
	{
		GameObject gameObject = new GameObject("Word-AutoTemplate");
		WordBehaviour result = gameObject.AddComponent<WordBehaviour>();
		Debug.Log("Creating Word Behaviour");
		return result;
	}
}
