using System;
using System.Runtime.InteropServices;

internal class WordListImpl : WordList
{
	public override bool LoadWordListFile(string relativePath)
	{
		return LoadWordListFile(relativePath, DataSet.StorageType.STORAGE_APPRESOURCE);
	}

	public override bool LoadWordListFile(string path, DataSet.StorageType storageType)
	{
		if (storageType == DataSet.StorageType.STORAGE_APPRESOURCE && QCARRuntimeUtilities.IsPlayMode())
		{
			path = "Assets/StreamingAssets/" + path;
		}
		return QCARWrapper.Instance.WordListLoadWordList(path, (int)storageType) == 1;
	}

	public override int AddWordsFromFile(string relativePath)
	{
		return AddWordsFromFile(relativePath, DataSet.StorageType.STORAGE_APPRESOURCE);
	}

	public override int AddWordsFromFile(string path, DataSet.StorageType storageType)
	{
		if (storageType == DataSet.StorageType.STORAGE_APPRESOURCE && QCARRuntimeUtilities.IsPlayMode())
		{
			path = "Assets/StreamingAssets/" + path;
		}
		return QCARWrapper.Instance.WordListAddWordsFromFile(path, (int)storageType);
	}

	public override bool AddWord(string word)
	{
		IntPtr intPtr = Marshal.StringToHGlobalUni(word);
		bool result = QCARWrapper.Instance.WordListAddWordU(intPtr) == 1;
		Marshal.FreeHGlobal(intPtr);
		return result;
	}

	public override bool RemoveWord(string word)
	{
		IntPtr intPtr = Marshal.StringToHGlobalUni(word);
		bool result = QCARWrapper.Instance.WordListRemoveWordU(intPtr) == 1;
		Marshal.FreeHGlobal(intPtr);
		return result;
	}

	public override bool ContainsWord(string word)
	{
		IntPtr intPtr = Marshal.StringToHGlobalUni(word);
		bool result = QCARWrapper.Instance.WordListContainsWordU(intPtr) == 1;
		Marshal.FreeHGlobal(intPtr);
		return result;
	}

	public override bool UnloadAllLists()
	{
		return QCARWrapper.Instance.WordListUnloadAllLists() == 1;
	}

	public override WordFilterMode GetFilterMode()
	{
		return (WordFilterMode)QCARWrapper.Instance.WordListGetFilterMode();
	}

	public override bool SetFilterMode(WordFilterMode mode)
	{
		return QCARWrapper.Instance.WordListSetFilterMode((int)mode) == 1;
	}

	public override bool LoadFilterListFile(string relativePath)
	{
		return LoadFilterListFile(relativePath, DataSet.StorageType.STORAGE_APPRESOURCE);
	}

	public override bool LoadFilterListFile(string path, DataSet.StorageType storageType)
	{
		if (storageType == DataSet.StorageType.STORAGE_APPRESOURCE && QCARRuntimeUtilities.IsPlayMode())
		{
			path = "Assets/StreamingAssets/" + path;
		}
		return QCARWrapper.Instance.WordListLoadFilterList(path, (int)storageType) == 1;
	}

	public override bool AddWordToFilterList(string word)
	{
		IntPtr intPtr = Marshal.StringToHGlobalUni(word);
		bool result = QCARWrapper.Instance.WordListAddWordToFilterListU(intPtr) == 1;
		Marshal.FreeHGlobal(intPtr);
		return result;
	}

	public override bool RemoveWordFromFilterList(string word)
	{
		IntPtr intPtr = Marshal.StringToHGlobalUni(word);
		bool result = QCARWrapper.Instance.WordListRemoveWordFromFilterListU(intPtr) == 1;
		Marshal.FreeHGlobal(intPtr);
		return result;
	}

	public override bool ClearFilterList()
	{
		return QCARWrapper.Instance.WordListClearFilterList() == 1;
	}

	public override int GetFilterListWordCount()
	{
		return QCARWrapper.Instance.WordListGetFilterListWordCount();
	}

	public override string GetFilterListWord(int index)
	{
		IntPtr ptr = QCARWrapper.Instance.WordListGetFilterListWordU(index);
		return Marshal.PtrToStringUni(ptr);
	}
}
