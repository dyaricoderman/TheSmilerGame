using System;
using System.Collections;
using System.Collections.Generic;
using Boo.Lang;
using UnityEngine;
using UnityScript.Lang;

[Serializable]
public class perspectiveHud : MonoBehaviour
{
	[Serializable]
	internal sealed class _0024HidePopUpWindow_0024116 : GenericGenerator<object>
	{
		[Serializable]
		internal sealed class _0024 : GenericGeneratorEnumerator<object>, IEnumerator
		{
			internal Component[] _0024renderers_0024117;

			internal Renderer _0024r_0024118;

			internal Renderer _0024r_0024119;

			internal int _0024_002476_0024120;

			internal Component[] _0024_002477_0024121;

			internal int _0024_002478_0024122;

			internal int _0024_002480_0024123;

			internal Component[] _0024_002481_0024124;

			internal int _0024_002482_0024125;

			internal perspectiveHud _0024self__0024126;

			public _0024(perspectiveHud self_)
			{
				_0024self__0024126 = self_;
			}

			public override bool MoveNext()
			{
				int result;
				switch (_state)
				{
				default:
					_0024self__0024126.windowstate = false;
					if (!_0024self__0024126.Error)
					{
						_0024self__0024126.PopupWindow.Play("popupOut");
					}
					else
					{
						_0024self__0024126.PopupWindowError.Play("popupOut");
					}
					goto case 2;
				case 2:
					if (_0024self__0024126.PopupWindow.isPlaying || _0024self__0024126.PopupWindowError.isPlaying)
					{
						result = (YieldDefault(2) ? 1 : 0);
						break;
					}
					_0024renderers_0024117 = _0024self__0024126.PopupWindow.transform.GetComponentsInChildren(typeof(Renderer));
					_0024_002476_0024120 = 0;
					_0024_002477_0024121 = _0024renderers_0024117;
					for (_0024_002478_0024122 = _0024_002477_0024121.Length; _0024_002476_0024120 < _0024_002478_0024122; _0024_002476_0024120++)
					{
						((Renderer)_0024_002477_0024121[_0024_002476_0024120]).enabled = false;
					}
					_0024renderers_0024117 = _0024self__0024126.PopupWindowError.transform.GetComponentsInChildren(typeof(Renderer));
					_0024_002480_0024123 = 0;
					_0024_002481_0024124 = _0024renderers_0024117;
					for (_0024_002482_0024125 = _0024_002481_0024124.Length; _0024_002480_0024123 < _0024_002482_0024125; _0024_002480_0024123++)
					{
						((Renderer)_0024_002481_0024124[_0024_002480_0024123]).enabled = false;
					}
					_0024self__0024126.background.enabled = false;
					_0024self__0024126.background.GetComponent<Renderer>().enabled = false;
					_0024self__0024126.Cam.enabled = false;
					_0024self__0024126.gameObject.BroadcastMessage("CamState", false);
					_0024self__0024126.updateCameraState();
					YieldDefault(1);
					goto case 1;
				case 1:
					result = 0;
					break;
				}
				return (byte)result != 0;
			}
		}

		internal perspectiveHud _0024self__0024127;

		public _0024HidePopUpWindow_0024116(perspectiveHud self_)
		{
			_0024self__0024127 = self_;
		}

		public override IEnumerator<object> GetEnumerator()
		{
			return new _0024(_0024self__0024127);
		}
	}

	public Animation PopupWindow;

	public Animation PopupWindowError;

	public LCDDisplay LCDdisplay;

	public TextMesh[] PopUpTitles;

	public StaticFlick background;

	private Camera Cam;

	private bool lcdstate;

	private bool windowstate;

	[NonSerialized]
	public static perspectiveHud inst;

	private bool Error;

	public virtual void Awake()
	{
		inst = this;
		Cam = (Camera)gameObject.GetComponent("Camera");
		Cam.enabled = false;
		Component[] componentsInChildren = PopupWindow.transform.GetComponentsInChildren(typeof(Renderer));
		int i = 0;
		Component[] array = componentsInChildren;
		for (int length = array.Length; i < length; i++)
		{
			((Renderer)array[i]).enabled = false;
		}
		componentsInChildren = PopupWindowError.transform.GetComponentsInChildren(typeof(Renderer));
		int j = 0;
		Component[] array2 = componentsInChildren;
		for (int length2 = array2.Length; j < length2; j++)
		{
			((Renderer)array2[j]).enabled = false;
		}
	}

	public virtual void updateCameraState()
	{
		if (lcdstate || windowstate)
		{
			Cam.enabled = true;
		}
		else
		{
			Cam.enabled = false;
		}
	}

	public virtual void DisplayText(string txt)
	{
		lcdstate = true;
		LCDdisplay.StartCoroutine(LCDdisplay.TypeOutText(txt));
		Cam.enabled = true;
		updateCameraState();
	}

	public virtual void HideText()
	{
		lcdstate = false;
		LCDdisplay.Off();
		Cam.enabled = false;
		updateCameraState();
	}

	public virtual void ShowPopUpWindow(string Title, bool error)
	{
		Error = error;
		for (int i = 0; i < Extensions.get_length((System.Array)PopUpTitles); i++)
		{
			PopUpTitles[i].text = Title;
		}
		HideText();
		windowstate = true;
		Cam.enabled = true;
		if (!Error)
		{
			PopupWindow.Play("popupIn");
			Component[] componentsInChildren = PopupWindow.transform.GetComponentsInChildren(typeof(Renderer));
			int j = 0;
			Component[] array = componentsInChildren;
			for (int length = array.Length; j < length; j++)
			{
				((Renderer)array[j]).enabled = true;
			}
		}
		else
		{
			PopupWindowError.Play("popupIn");
			Component[] componentsInChildren = PopupWindowError.transform.GetComponentsInChildren(typeof(Renderer));
			int k = 0;
			Component[] array2 = componentsInChildren;
			for (int length2 = array2.Length; k < length2; k++)
			{
				((Renderer)array2[k]).enabled = true;
			}
		}
		background.enabled = true;
		background.GetComponent<Renderer>().enabled = true;
		gameObject.BroadcastMessage("CamState", true);
		updateCameraState();
	}

	public virtual void HidePopUpWindowC()
	{
		StartCoroutine("HidePopUpWindow");
	}

	public virtual IEnumerator HidePopUpWindow()
	{
		return new _0024HidePopUpWindow_0024116(this).GetEnumerator();
	}

	public virtual void Main()
	{
	}
}
