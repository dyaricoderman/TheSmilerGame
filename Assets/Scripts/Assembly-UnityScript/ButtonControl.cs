using System;
using System.Collections;
using System.Collections.Generic;
using Boo.Lang;
using UnityEngine;

[Serializable]
public class ButtonControl : MonoBehaviour
{
	[Serializable]
	internal sealed class _0024SkipWait_0024206 : GenericGenerator<WaitForSeconds>
	{
		[Serializable]
		internal sealed class _0024 : GenericGeneratorEnumerator<WaitForSeconds>, IEnumerator
		{
			internal ButtonControl _0024self__0024207;

			public _0024(ButtonControl self_)
			{
				_0024self__0024207 = self_;
			}

			public override bool MoveNext()
			{
				int result;
				switch (_state)
				{
				default:
					_0024self__0024207.CanSkip = false;
					result = (Yield(2, new WaitForSeconds(5f)) ? 1 : 0);
					break;
				case 2:
					_0024self__0024207.CanSkip = true;
					_0024self__0024207.UpdateButtons();
					YieldDefault(1);
					goto case 1;
				case 1:
					result = 0;
					break;
				}
				return (byte)result != 0;
			}
		}

		internal ButtonControl _0024self__0024208;

		public _0024SkipWait_0024206(ButtonControl self_)
		{
			_0024self__0024208 = self_;
		}

		public override IEnumerator<WaitForSeconds> GetEnumerator()
		{
			return new _0024(_0024self__0024208);
		}
	}

	public UnityEngine.UI.Button StartButton;

	public UnityEngine.UI.Button SkipButton;

	private bool CanSkip;

	private Rect ClickRect;

	public virtual void Awake()
	{
		SetButtonState(false);
		ClickRect = GUItexToRect(StartButton.transform);
	}

	public virtual void Start()
	{
		//float num = StartButton.rectTransform.sizeDelta.x / StartButton.rectTransform.sizeDelta.y;
		//float aspect = CartCam.inst.GetComponent<Camera>().aspect;
		//float x = StartButton.transform.localScale.y * num * aspect * 0.5f;
		Vector3 localScale = StartButton.transform.localScale;
		//float num2 = (localScale.x = x);
		Vector3 vector = (StartButton.transform.localScale = localScale);
		//num = SkipButton.rectTransform.sizeDelta.x / SkipButton.rectTransform.sizeDelta.y;
		//aspect = CartCam.inst.GetComponent<Camera>().aspect;
		//float x2 = SkipButton.transform.localScale.y * num * aspect * 0.5f;
		Vector3 localScale2 = SkipButton.transform.localScale;
		//float num3 = (localScale2.x = x2);
		Vector3 vector3 = (SkipButton.transform.localScale = localScale2);
	}

	public virtual void SetButtonState(bool state)
	{
		enabled = state;
		UpdateButtons();
	}

	public virtual void ShowButton(bool start, bool skip)
	{
		StartButton.gameObject.SetActive(start);
		SkipButton.gameObject.SetActive(skip);
	}

	public virtual void UpdateButtons()
	{
		if (CanSkip && enabled)
		{
			if (CartCam.SkipStartDisplay)
			{
				ShowButton(false, true);
			}
			else
			{
				ShowButton(true, false);
			}
		}
		else
		{
			ShowButton(false, false);
		}
	}

	public virtual IEnumerator SkipWait()
	{
		return new _0024SkipWait_0024206(this).GetEnumerator();
	}

	public virtual Rect GUItexToRect(Transform tex)
	{
		float num = tex.localScale.x * (float)Screen.width;
		float num2 = tex.localScale.y * (float)Screen.height;
		return new Rect(tex.position.x * (float)Screen.width - num * 0.5f, tex.position.y * (float)Screen.height - num2 * 0.5f, num, num2);
	}

	public virtual void Update()
	{
		UpdateButtons();
		//if (ClickRect.Contains(Input.mousePosition) && Input.GetMouseButtonDown(0))
		//{
			//perspectiveHud.inst.HideText();
			//GameControl.inst.StartCoroutine(GameControl.inst.StartGame());
			//SetButtonState(false);
		//}
	}

	public void StartGameButton()
    {
		perspectiveHud.inst.HideText();
		GameControl.inst.StartCoroutine(GameControl.inst.StartGame());
		SetButtonState(false);
	}

	public virtual void Main()
	{
	}
}
