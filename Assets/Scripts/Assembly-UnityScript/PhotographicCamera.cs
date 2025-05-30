using System;
using System.Collections;
using System.Collections.Generic;
using Boo.Lang;
using UnityEngine;

[Serializable]
public class PhotographicCamera : MonoBehaviour
{
	[Serializable]
	internal sealed class _0024SnapShots_0024135 : GenericGenerator<WaitForSeconds>
	{
		[Serializable]
		internal sealed class _0024 : GenericGeneratorEnumerator<WaitForSeconds>, IEnumerator
		{
			internal PhotographicCamera _0024self__0024136;

			public _0024(PhotographicCamera self_)
			{
				_0024self__0024136 = self_;
			}

			public override bool MoveNext()
			{
				int result;
				switch (_state)
				{
				default:
					_0024self__0024136.GetComponent<Camera>().cullingMask = _0024self__0024136.CullMask;
					_0024self__0024136.GetComponent<Camera>().clearFlags = _0024self__0024136.CamClear;
					result = (YieldDefault(2) ? 1 : 0);
					break;
				case 2:
					_0024self__0024136.GetComponent<Camera>().cullingMask = 0;
					_0024self__0024136.GetComponent<Camera>().clearFlags = CameraClearFlags.Nothing;
					result = (Yield(3, new WaitForSeconds(2f)) ? 1 : 0);
					break;
				case 1:
					result = 0;
					break;
				}
				return (byte)result != 0;
			}
		}

		internal PhotographicCamera _0024self__0024137;

		public _0024SnapShots_0024135(PhotographicCamera self_)
		{
			_0024self__0024137 = self_;
		}

		public override IEnumerator<WaitForSeconds> GetEnumerator()
		{
			return new _0024(_0024self__0024137);
		}
	}

	private LayerMask CullMask;

	private CameraClearFlags CamClear;

	public virtual void Start()
	{
		CullMask = GetComponent<Camera>().cullingMask;
		CamClear = GetComponent<Camera>().clearFlags;
		StartCoroutine(SnapShots());
	}

	public virtual IEnumerator SnapShots()
	{
		return new _0024SnapShots_0024135(this).GetEnumerator();
	}

	public virtual void Main()
	{
	}
}
