using System;
using UnityEngine;
using UnityScript.Lang;

[Serializable]
public class GuiControl : MonoBehaviour
{
	public UnityEngine.UI.Image BalanceMeter;

	public UnityEngine.UI.Image OptionsButton;

	public UnityEngine.UI.Image Stick;

	public UnityEngine.UI.Image StickBackground;

	public RectTransform stickL;

	public RectTransform stickR;

	public Transform tiltPivot;

	public Renderer tiltMarker;

	public BoostGUIControl BoostControl;

	private CartCam CartCamera;

	private Cart Cart;

	public UnityEngine.UI.Image[] AccuracyText;

	public Renderer[] HudRenderers;

	private RectTransform OptionsRectTransform;

	private float TargetTilt;

	private bool Applyingthrotal;

	private float cartSpeed;

	private Vector2 TempVe2;

	private int DotSize;

	private Vector2 DotLoc;

	private bool RightHanded;

	private float controlSize;

	private Vector2 vecAng;

	//[HideInInspector]
	public bool boostFound;

	private Rect boostRect;

	private float UserTilt;

	private Vector2 StickLocation;

	public GuiControl()
	{
		DotSize = (int)((float)Mathf.Min(Screen.width, Screen.height) * 0.2f);
		RightHanded = true;
		controlSize = (float)Screen.height * 0.4f;
		vecAng = new Vector2(1f, 1f);
	}

	public virtual void Awake()
	{
		SetState(false);
	}

	public virtual void OnApplicationPause(bool pause)
	{
		if (pause && (bool)Cart && Cart.enabled)
		{
			OptionsMenu.inst.StartCoroutine(OptionsMenu.inst.ShowOptions());
		}
	}

	public virtual void SetState(bool state)
	{
		enabled = state;
		Stick.gameObject.SetActive(state);
		BalanceMeter.gameObject.SetActive(state);
		StickBackground.gameObject.SetActive(state);
		tiltMarker.enabled = state;
		OptionsButton.gameObject.SetActive(state);
		int i = 0;
		Renderer[] hudRenderers = HudRenderers;
		for (int length = hudRenderers.Length; i < length; i++)
		{
			hudRenderers[i].enabled = state;
		}
		if (!state)
		{
			ShowAccText(-1);
			return;
		}
		MonoBehaviour.print("initText");
		ShowAccText(3);
	}

	public virtual void ShowAccText(int j)
	{
		for (int i = 0; i < Extensions.get_length((System.Array)AccuracyText); i++)
		{
			if (j == i)
			{
				AccuracyText[i].gameObject.SetActive(true);
			}
			else
			{
				AccuracyText[i].gameObject.SetActive(false);
			}
		}
	}

	public virtual void ConfigureSizes()
	{
		float aspect = CartCam.inst.GetComponent<Camera>().aspect;
		
		// Configure BalanceMeter
		Vector2 balanceMeterSize = BalanceMeter.rectTransform.sizeDelta;
		balanceMeterSize.x = balanceMeterSize.y / aspect * 3f;
		BalanceMeter.rectTransform.sizeDelta = balanceMeterSize;

		// Configure Stick
		Vector2 stickSize = Stick.rectTransform.sizeDelta;
		stickSize.x = stickSize.y / aspect;
		Stick.rectTransform.sizeDelta = stickSize;

		// Configure StickBackground
		Vector2 stickBgSize = StickBackground.rectTransform.sizeDelta;
		stickBgSize.x = stickBgSize.y / aspect;
		StickBackground.rectTransform.sizeDelta = stickBgSize;

		// Position StickBackground
		Vector2 stickBgPos = StickBackground.rectTransform.anchoredPosition;
		stickBgPos.x = Screen.width - stickBgSize.x * 0.5f;
		StickBackground.rectTransform.anchoredPosition = stickBgPos;

		// Configure OptionsButton
		Vector2 optionsSize = OptionsButton.rectTransform.sizeDelta;
		optionsSize.x = optionsSize.y / aspect * 1.3f;
		OptionsButton.rectTransform.sizeDelta = optionsSize;

		// Position OptionsButton
		Vector2 optionsPos = OptionsButton.rectTransform.anchoredPosition;
		optionsPos.x = Screen.width - optionsSize.x * 0.5f;
		optionsPos.y = Screen.height - optionsSize.y * 0.5f;
		OptionsButton.rectTransform.anchoredPosition = optionsPos;

		// Store OptionsButton rect for click detection
		OptionsRectTransform = OptionsButton.rectTransform;

		MaintainTextAspect();
	}

	public virtual void MaintainTextAspect()
	{
		for (int i = 0; i < Extensions.get_length((System.Array)AccuracyText); i++)
		{
			float aspect = CartCamera.GetComponent<Camera>().aspect;
			Vector2 textSize = AccuracyText[i].rectTransform.sizeDelta;
			textSize.x = textSize.y * aspect * 0.5f;
			AccuracyText[i].rectTransform.sizeDelta = textSize;
		}
	}

	public virtual void Start()
	{
		CartCamera = CartCam.inst;
		Cart = Cart.inst;
		//ConfigureSizes();
		if (PlayerPrefs.GetInt("RightHanded") == 1)
		{
			SetRightLeft(true);
		}
		else
		{
			SetRightLeft(false);
		}
	}

	public virtual void SetRightLeft(bool right)
	{
		RightHanded = right;
		float aspect = CartCam.inst.GetComponent<Camera>().aspect;
		if (right)
		{
			//stickR.gameObject.SetActive(true);
			//stickL.gameObject.SetActive(false);
			StickBackground.rectTransform.anchoredPosition = stickR.anchoredPosition;
			StickBackground.rectTransform.sizeDelta = stickR.sizeDelta;
			StickBackground.rectTransform.localScale = stickR.localScale;
		}
		else
		{
			//stickR.gameObject.SetActive(false);
			//stickL.gameObject.SetActive(true);
			StickBackground.rectTransform.anchoredPosition = stickL.anchoredPosition;
			StickBackground.rectTransform.sizeDelta = stickL.sizeDelta;
			StickBackground.rectTransform.localScale = stickL.localScale;
		}
	}

	public virtual void Update()
	{
		Cart.tilt = Cart.ChangeInTilt();
		TargetTilt = Cart.tilt.x;
		Applyingthrotal = false;
		cartSpeed = Mathf.Clamp01(Cart.speed / 150f);

		for (int i = 0; i < Input.touchCount; i++)
		{
			if (RightHanded)
			{
				TempVe2 = new Vector2(Screen.width, 0f) - Input.touches[i].position;
			}
			else
			{
				TempVe2 = new Vector2(0f, 0f) - Input.touches[i].position;
			}
			TempVe2.y = Mathf.Abs(TempVe2.y);
			if (TempVe2.magnitude >= controlSize * 1.45f)
			{
				continue;
			}
			if (RightHanded)
			{
				if (!(TempVe2.x >= 0f))
				{
					TempVe2.x = 0f;
				}
			}
			else if (!(TempVe2.x <= 0f))
			{
				TempVe2.x = 0f;
			}
			vecAng = TempVe2;
			if (!(vecAng.magnitude >= controlSize * 1.45f))
			{
				if (RightHanded)
				{
					UserTilt = (Mathf.Atan2(vecAng.x, vecAng.y) / ((float)Math.PI / 2f) * 2f - 1f) * -1f;
				}
				else
				{
					UserTilt = (Mathf.Atan2(vecAng.x, vecAng.y) + (float)Math.PI / 2f) / ((float)Math.PI / 2f) * -2f + 1f;
				}
			}
		}

		if (Input.touchCount == 0 || !(vecAng.magnitude >= controlSize * 1.3f))
		{
			UserTilt = Mathf.Lerp(UserTilt, 0f, Time.deltaTime * 2f);
			if (RightHanded)
			{
				vecAng = Vector2.Lerp(vecAng, new Vector2(controlSize * 0.9f, controlSize * 0.9f), Time.deltaTime);
			}
			else
			{
				vecAng = Vector2.Lerp(vecAng, new Vector2(controlSize * -0.9f, controlSize * 0.9f), Time.deltaTime);
			}
		}

		Cart.TrueTilt = UserTilt - TargetTilt;
		if (Input.touchCount == 0 && !Input.GetMouseButton(0))
		{
			Cart.TrueTilt *= 2f;
		}
		Cart.MeshTilt = Mathf.Lerp(Cart.MeshTilt, Cart.TrueTilt, Time.deltaTime * 2f);

		Vector3 vector = new Vector3(Mathf.Clamp((UserTilt - TargetTilt) * 60f, -30f, 30f), 0f, 0f);
		Quaternion rotation = tiltPivot.rotation;
		Vector3 vector2 = (rotation.eulerAngles = vector);
		Quaternion quaternion = (tiltPivot.rotation = rotation);

		ShowAccText(Cart.AccuracyIcon);

		DotLoc = vecAng.normalized * controlSize * 0.9f;
		if (RightHanded)
		{
			StickLocation = new Vector2(Screen.width - DotLoc.x, Screen.height - DotLoc.y);
		}
		else
		{
			StickLocation = new Vector2(-1f * DotLoc.x, Screen.height - DotLoc.y);
		}

		Vector2 stickPos = Stick.rectTransform.anchoredPosition;
		stickPos.x = StickLocation.x - 2500;
		stickPos.y = Screen.height - StickLocation.y;
		Stick.rectTransform.anchoredPosition = stickPos;

		//boostFound = false;
		//for (int i = 0; i < Input.touchCount; i++)
		//{
			//if (boostRect.Contains(Input.touches[i].position))
			//{
				//boostFound = true;
			//}
		//}

		if (Cart.Boosting && !boostFound)
		{
			Cart.EndBoost();
		}
		if (!Cart.Boosting && boostFound && !(Cart.BoostCount <= 0f))
		{
			Cart.Boost();
		}
	}

	public void PauseButton()
    {
		OptionsMenu.inst.StartCoroutine(OptionsMenu.inst.ShowOptions());
	}

	public virtual void Main()
	{
	}
}
