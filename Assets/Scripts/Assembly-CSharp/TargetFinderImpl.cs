using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using UnityEngine;

public class TargetFinderImpl : TargetFinder
{
	[StructLayout(LayoutKind.Sequential, Pack = 1)]
	private struct TargetFinderState
	{
		public int IsRequesting;

		[MarshalAs(UnmanagedType.SysInt)]
		public UpdateState UpdateState;

		public int ResultCount;
	}

	[StructLayout(LayoutKind.Sequential, Pack = 1)]
	private struct InternalTargetSearchResult
	{
		public IntPtr TargetNamePtr;

		public IntPtr UniqueTargetIdPtr;

		public float TargetSize;

		public IntPtr MetaDataPtr;

		public IntPtr TargetSearchResultPtr;

		public byte TrackingRating;
	}

	private IntPtr mTargetFinderStatePtr;

	private TargetFinderState mTargetFinderState;

	private List<TargetSearchResult> mNewResults;

	private Dictionary<int, ImageTarget> mImageTargets;

	public TargetFinderImpl()
	{
		mTargetFinderState = default(TargetFinderState);
		mTargetFinderStatePtr = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(TargetFinderState)));
		Marshal.StructureToPtr(mTargetFinderState, mTargetFinderStatePtr, false);
		mImageTargets = new Dictionary<int, ImageTarget>();
	}

	~TargetFinderImpl()
	{
		Marshal.FreeHGlobal(mTargetFinderStatePtr);
		mTargetFinderStatePtr = IntPtr.Zero;
	}

	public override bool StartInit(string userAuth, string secretAuth)
	{
		return QCARWrapper.Instance.TargetFinderStartInit(userAuth, secretAuth) == 1;
	}

	public override InitState GetInitState()
	{
		return (InitState)QCARWrapper.Instance.TargetFinderGetInitState();
	}

	public override bool Deinit()
	{
		return QCARWrapper.Instance.TargetFinderDeinit() == 1;
	}

	public override bool StartRecognition()
	{
		return QCARWrapper.Instance.TargetFinderStartRecognition() == 1;
	}

	public override bool Stop()
	{
		return QCARWrapper.Instance.TargetFinderStop() == 1;
	}

	public override void SetUIScanlineColor(Color color)
	{
		QCARWrapper.Instance.TargetFinderSetUIScanlineColor(color.r, color.g, color.b);
	}

	public override void SetUIPointColor(Color color)
	{
		QCARWrapper.Instance.TargetFinderSetUIPointColor(color.r, color.g, color.b);
	}

	public override bool IsRequesting()
	{
		return mTargetFinderState.IsRequesting == 1;
	}

	public override UpdateState Update()
	{
		QCARWrapper.Instance.TargetFinderUpdate(mTargetFinderStatePtr);
		mTargetFinderState = (TargetFinderState)Marshal.PtrToStructure(mTargetFinderStatePtr, typeof(TargetFinderState));
		if (mTargetFinderState.ResultCount > 0)
		{
			IntPtr intPtr = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(InternalTargetSearchResult)) * mTargetFinderState.ResultCount);
			if (QCARWrapper.Instance.TargetFinderGetResults(intPtr, mTargetFinderState.ResultCount) != 1)
			{
				Debug.LogError("TargetFinder: Could not retrieve new results!");
				return UpdateState.UPDATE_NO_MATCH;
			}
			mNewResults = new List<TargetSearchResult>();
			for (int i = 0; i < mTargetFinderState.ResultCount; i++)
			{
				IntPtr ptr = new IntPtr(intPtr.ToInt32() + i * Marshal.SizeOf(typeof(QCARManagerImpl.TrackableResultData)));
				InternalTargetSearchResult internalTargetSearchResult = (InternalTargetSearchResult)Marshal.PtrToStructure(ptr, typeof(InternalTargetSearchResult));
				mNewResults.Add(new TargetSearchResult
				{
					TargetName = Marshal.PtrToStringAnsi(internalTargetSearchResult.TargetNamePtr),
					UniqueTargetId = Marshal.PtrToStringAnsi(internalTargetSearchResult.UniqueTargetIdPtr),
					TargetSize = internalTargetSearchResult.TargetSize,
					MetaData = Marshal.PtrToStringAnsi(internalTargetSearchResult.MetaDataPtr),
					TrackingRating = internalTargetSearchResult.TrackingRating,
					TargetSearchResultPtr = internalTargetSearchResult.TargetSearchResultPtr
				});
			}
			Marshal.FreeHGlobal(intPtr);
		}
		return mTargetFinderState.UpdateState;
	}

	public override IEnumerable<TargetSearchResult> GetResults()
	{
		return mNewResults;
	}

	public override ImageTargetBehaviour EnableTracking(TargetSearchResult result, string gameObjectName)
	{
		GameObject gameObject = new GameObject(gameObjectName);
		return EnableTracking(result, gameObject);
	}

	public override ImageTargetBehaviour EnableTracking(TargetSearchResult result, GameObject gameObject)
	{
		IntPtr intPtr = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(ImageTargetData)));
		int num = QCARWrapper.Instance.TargetFinderEnableTracking(result.TargetSearchResultPtr, intPtr);
		ImageTargetData imageTargetData = (ImageTargetData)Marshal.PtrToStructure(intPtr, typeof(ImageTargetData));
		Marshal.FreeHGlobal(intPtr);
		StateManagerImpl stateManagerImpl = (StateManagerImpl)TrackerManager.Instance.GetStateManager();
		ImageTargetBehaviour result2 = null;
		if (imageTargetData.id == -1)
		{
			Debug.LogError("TargetSearchResult " + result.TargetName + " could not be enabled for tracking.");
		}
		else
		{
			ImageTarget imageTarget = new CloudRecoImageTargetImpl(result.TargetName, imageTargetData.id, imageTargetData.size);
			mImageTargets[imageTargetData.id] = imageTarget;
			result2 = stateManagerImpl.FindOrCreateImageTargetBehaviourForTrackable(imageTarget, gameObject);
		}
		IntPtr intPtr2 = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(int)) * num);
		QCARWrapper.Instance.TargetFinderGetImageTargets(intPtr2, num);
		List<int> list = new List<int>();
		for (int i = 0; i < num; i++)
		{
			IntPtr ptr = new IntPtr(intPtr2.ToInt32() + i * Marshal.SizeOf(typeof(int)));
			int item = Marshal.ReadInt32(ptr);
			list.Add(item);
		}
		Marshal.FreeHGlobal(intPtr2);
		ImageTarget[] array = mImageTargets.Values.ToArray();
		foreach (ImageTarget imageTarget2 in array)
		{
			if (!list.Contains(imageTarget2.ID))
			{
				stateManagerImpl.DestroyTrackableBehavioursForTrackable(imageTarget2);
				mImageTargets.Remove(imageTarget2.ID);
			}
		}
		return result2;
	}

	public override void ClearTrackables(bool destroyGameObjects = true)
	{
		QCARWrapper.Instance.TargetFinderClearTrackables();
		StateManager stateManager = TrackerManager.Instance.GetStateManager();
		foreach (ImageTarget value in mImageTargets.Values)
		{
			stateManager.DestroyTrackableBehavioursForTrackable(value, destroyGameObjects);
		}
		mImageTargets.Clear();
	}

	public override IEnumerable<ImageTarget> GetImageTargets()
	{
		return mImageTargets.Values;
	}
}
