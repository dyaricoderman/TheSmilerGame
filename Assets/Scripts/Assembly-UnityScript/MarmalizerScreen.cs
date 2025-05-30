using System;
using System.Collections;
using System.Collections.Generic;
using Boo.Lang;
using UnityEngine;

[Serializable]
public class MarmalizerScreen : MonoBehaviour
{
	[Serializable]
	internal sealed class _0024frameFlic_0024197 : GenericGenerator<WaitForSeconds>
	{
		[Serializable]
		internal sealed class _0024 : GenericGeneratorEnumerator<WaitForSeconds>, IEnumerator
		{
			internal MarmalizerScreen _0024self__0024198;

			public _0024(MarmalizerScreen self_)
			{
				_0024self__0024198 = self_;
			}

			public override bool MoveNext()
			{
				int result;
				switch (_state)
				{
				default:
					_0024self__0024198.frame++;
					_0024self__0024198.frame = (int)Mathf.Repeat(_0024self__0024198.frame, 16f);
					result = (Yield(2, new WaitForSeconds(_0024self__0024198.ChangeTime)) ? 1 : 0);
					break;
				case 1:
					result = 0;
					break;
				}
				return (byte)result != 0;
			}
		}

		internal MarmalizerScreen _0024self__0024199;

		public _0024frameFlic_0024197(MarmalizerScreen self_)
		{
			_0024self__0024199 = self_;
		}

		public override IEnumerator<WaitForSeconds> GetEnumerator()
		{
			return new _0024(_0024self__0024199);
		}
	}

	public Material MarScreen;

	public float ChangeTime;

	public float ScrollSpeed;

	private float Scroll;

	private int frame;

	public MarmalizerScreen()
	{
		ChangeTime = 2f;
		ScrollSpeed = 0.2f;
	}

	public virtual void Start()
	{
		StartCoroutine("frameFlic");
	}

	public virtual void Update()
	{
		Scroll += Time.deltaTime * ScrollSpeed;
		MarScreen.SetTextureOffset("_MainTex", new Vector2(Scroll, (float)frame / 16f));
	}

	public virtual IEnumerator frameFlic()
	{
		return new _0024frameFlic_0024197(this).GetEnumerator();
	}

	public virtual void Main()
	{
	}
}
