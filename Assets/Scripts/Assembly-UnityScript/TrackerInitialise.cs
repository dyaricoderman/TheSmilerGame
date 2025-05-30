using System;
using System.Collections;
using System.Collections.Generic;
using Boo.Lang;
using UnityEngine;

[Serializable]
public class TrackerInitialise : MonoBehaviour
{
	[Serializable]
	internal sealed class _0024Start_0024114 : GenericGenerator<object>
	{
		[Serializable]
		internal sealed class _0024 : GenericGeneratorEnumerator<object>, IEnumerator
		{
			internal TrackerInitialise _0024self__0024115;

			public _0024(TrackerInitialise self_)
			{
				_0024self__0024115 = self_;
			}

			public override bool MoveNext()
			{
				int result;
				switch (_state)
				{
				default:
					if (_0024self__0024115.allowDebug)
					{
						_0024self__0024115.tracker.AllowDebug(true);
					}
					result = (YieldDefault(2) ? 1 : 0);
					break;
				case 2:
					_0024self__0024115.tracker.Log("Event tracking initialised");
					YieldDefault(1);
					goto case 1;
				case 1:
					result = 0;
					break;
				}
				return (byte)result != 0;
			}
		}

		internal TrackerInitialise _0024self__0024116;

		public _0024Start_0024114(TrackerInitialise self_)
		{
			_0024self__0024116 = self_;
		}

		public override IEnumerator<object> GetEnumerator()
		{
			return new _0024(_0024self__0024116);
		}
	}

	public bool allowDebug;

	private bool initialised;

	private Tracker2 tracker;

	public string flurryKeyApple;

	public string flurryKeyAndroid;

	public virtual void Awake()
	{
		UnityEngine.Object.DontDestroyOnLoad(gameObject);
		if (!Tracker2.instance)
		{
			gameObject.AddComponent(typeof(Tracker2));
			tracker = ((Tracker2)GetComponent(typeof(Tracker2))) as Tracker2;
			//tracker.StartFlurrySession(flurryKeyAndroid);
		}
		else
		{
			UnityEngine.Object.Destroy(gameObject);
		}
	}

	public virtual IEnumerator Start()
	{
		return new _0024Start_0024114(this).GetEnumerator();
	}

	public virtual void Main()
	{
	}
}
