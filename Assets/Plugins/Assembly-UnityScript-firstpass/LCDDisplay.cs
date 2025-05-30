using System;
using System.Collections;
using System.Collections.Generic;
using Boo.Lang;
using UnityEngine;
using UnityScript.Lang;

[Serializable]
public class LCDDisplay : MonoBehaviour
{
	[Serializable]
	internal sealed class _0024TypeOutText_002484 : GenericGenerator<WaitForSeconds>
	{
		[Serializable]
		internal sealed class _0024 : GenericGeneratorEnumerator<WaitForSeconds>, IEnumerator
		{
			internal int _0024i_002485;

			internal string _0024dis_002486;

			internal LCDDisplay _0024self__002487;

			public _0024(string dis, LCDDisplay self_)
			{
				_0024dis_002486 = dis;
				_0024self__002487 = self_;
			}

			public override bool MoveNext()
			{
				int result;
				switch (_state)
				{
				default:
					_0024dis_002486 = _0024dis_002486.Replace(" ", "_");
					_0024self__002487.StopCoroutine("TypeLoop");
					_0024self__002487.TextLines = _0024dis_002486.Split("|"[0]);
					_0024i_002485 = 0;
					goto IL_0127;
				case 2:
					_0024i_002485++;
					goto IL_0127;
				case 1:
					{
						result = 0;
						break;
					}
					IL_0127:
					if (_0024i_002485 < Extensions.get_length((System.Array)_0024self__002487.TextLines))
					{
						_0024self__002487.background.enabled = true;
						_0024self__002487.txt.GetComponent<Renderer>().enabled = true;
						_0024self__002487.FullText = _0024dis_002486;
						_0024self__002487.StartCoroutine(_0024self__002487.TypeLoop(_0024self__002487.CenterText(_0024self__002487.TextLines[_0024i_002485])));
						result = (Yield(2, new WaitForSeconds(_0024self__002487.TypeWait * (float)_0024self__002487.DisplayLength + _0024self__002487.NewLineWait)) ? 1 : 0);
						break;
					}
					YieldDefault(1);
					goto case 1;
				}
				return (byte)result != 0;
			}
		}

		internal string _0024dis_002488;

		internal LCDDisplay _0024self__002489;

		public _0024TypeOutText_002484(string dis, LCDDisplay self_)
		{
			_0024dis_002488 = dis;
			_0024self__002489 = self_;
		}

		public override IEnumerator<WaitForSeconds> GetEnumerator()
		{
			return new _0024(_0024dis_002488, _0024self__002489);
		}
	}

	[Serializable]
	internal sealed class _0024TypeLoop_002490 : GenericGenerator<WaitForSeconds>
	{
		[Serializable]
		internal sealed class _0024 : GenericGeneratorEnumerator<WaitForSeconds>, IEnumerator
		{
			internal int _0024positionCount_002491;

			internal string _0024display_002492;

			internal string _0024type_002493;

			internal LCDDisplay _0024self__002494;

			public _0024(string type, LCDDisplay self_)
			{
				_0024type_002493 = type;
				_0024self__002494 = self_;
			}

			public override bool MoveNext()
			{
				int result;
				switch (_state)
				{
				default:
					_0024positionCount_002491 = 0;
					goto case 2;
				case 2:
					if (_0024positionCount_002491 < _0024type_002493.Length + 1)
					{
						_0024display_002492 = _0024self__002494.WhiteSpace(_0024self__002494.DisplayLength - _0024positionCount_002491) + _0024type_002493.Substring(0, _0024positionCount_002491);
						_0024self__002494.txt.text = _0024display_002492;
						_0024positionCount_002491++;
						result = (Yield(2, new WaitForSeconds(_0024self__002494.TypeWait)) ? 1 : 0);
						break;
					}
					MonoBehaviour.print("TypeLoopFinished " + _0024positionCount_002491);
					YieldDefault(1);
					goto case 1;
				case 1:
					result = 0;
					break;
				}
				return (byte)result != 0;
			}
		}

		internal string _0024type_002495;

		internal LCDDisplay _0024self__002496;

		public _0024TypeLoop_002490(string type, LCDDisplay self_)
		{
			_0024type_002495 = type;
			_0024self__002496 = self_;
		}

		public override IEnumerator<WaitForSeconds> GetEnumerator()
		{
			return new _0024(_0024type_002495, _0024self__002496);
		}
	}

	public TextMesh txt;

	public int DisplayLength;

	public Renderer background;

	public float TypeWait;

	public float NewLineWait;

	private string FullText;

	private string DisplayText;

	private string[] TextLines;

	public LCDDisplay()
	{
		DisplayLength = 24;
		TypeWait = 0.5f;
		NewLineWait = 3f;
	}

	public virtual void Awake()
	{
		txt.text = WhiteSpace(DisplayLength);
		background.enabled = false;
		txt.GetComponent<Renderer>().enabled = false;
	}

	public virtual IEnumerator TypeOutText(string dis)
	{
		return new _0024TypeOutText_002484(dis, this).GetEnumerator();
	}

	public virtual string CenterText(string txt)
	{
		int num = DisplayLength - Extensions.get_length(txt);
		int chars = (int)Mathf.Floor((float)num * 0.5f);
		int chars2 = (int)Mathf.Ceil((float)num * 0.5f);
		return WhiteSpace(chars) + txt + WhiteSpace(chars2);
	}

	public virtual void Off()
	{
		background.enabled = false;
		txt.GetComponent<Renderer>().enabled = false;
	}

	public virtual string WhiteSpace(int chars)
	{
		string result;
		if (chars > 0)
		{
			string text = string.Empty;
			for (int i = 0; i < chars; i++)
			{
				text += "_";
			}
			result = text;
		}
		else
		{
			result = string.Empty;
		}
		return result;
	}

	public virtual IEnumerator TypeLoop(string type)
	{
		return new _0024TypeLoop_002490(type, this).GetEnumerator();
	}

	public virtual void Main()
	{
	}
}
