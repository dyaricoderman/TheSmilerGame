using System;
using System.Collections;
using System.Collections.Generic;
using Boo.Lang;
using UnityEngine;
using UnityScript.Lang;

[Serializable]
public class CartCam : MonoBehaviour
{
	[Serializable]
	internal sealed class _0024SetCamHeight_0024117 : GenericGenerator<object>
	{
		[Serializable]
		internal sealed class _0024 : GenericGeneratorEnumerator<object>, IEnumerator
		{
			internal float _0024startHeight_0024118;

			internal float _0024startTime_0024119;

			internal float _0024h_0024120;

			internal CartCam _0024self__0024121;

			public _0024(float h, CartCam self_)
			{
				_0024h_0024120 = h;
				_0024self__0024121 = self_;
			}

			public override bool MoveNext()
			{
				int result;
				switch (_state)
				{
				default:
					_0024startHeight_0024118 = _0024self__0024121.originalOffset.y;
					_0024startTime_0024119 = Time.time;
					goto case 2;
				case 2:
					if (Time.time - _0024startTime_0024119 < 2f)
					{
						_0024self__0024121.originalOffset.y = Mathf.Lerp(_0024self__0024121.originalOffset.y, _0024h_0024120, (Time.time - _0024startTime_0024119) * 0.5f);
						result = (YieldDefault(2) ? 1 : 0);
						break;
					}
					YieldDefault(1);
					goto case 1;
				case 1:
					result = 0;
					break;
				}
				return (byte)result != 0;
			}
		}

		internal float _0024h_0024122;

		internal CartCam _0024self__0024123;

		public _0024SetCamHeight_0024117(float h, CartCam self_)
		{
			_0024h_0024122 = h;
			_0024self__0024123 = self_;
		}

		public override IEnumerator<object> GetEnumerator()
		{
			return new _0024(_0024h_0024122, _0024self__0024123);
		}
	}

	[Serializable]
	internal sealed class _0024Start_0024124 : GenericGenerator<WaitForSeconds>
	{
		[Serializable]
		internal sealed class _0024 : GenericGeneratorEnumerator<WaitForSeconds>, IEnumerator
		{
			internal CartCam _0024self__0024125;

			public _0024(CartCam self_)
			{
				_0024self__0024125 = self_;
			}

			public override bool MoveNext()
			{
				int result;
				switch (_state)
				{
				default:
					_0024self__0024125.PHud = perspectiveHud.inst;
					result = (Yield(2, new WaitForSeconds(0.2f)) ? 1 : 0);
					break;
				case 2:
					_0024self__0024125.StartCoroutine(_0024self__0024125.UpdateTrackLod());
					YieldDefault(1);
					goto case 1;
				case 1:
					result = 0;
					break;
				}
				return (byte)result != 0;
			}
		}

		internal CartCam _0024self__0024126;

		public _0024Start_0024124(CartCam self_)
		{
			_0024self__0024126 = self_;
		}

		public override IEnumerator<WaitForSeconds> GetEnumerator()
		{
			return new _0024(_0024self__0024126);
		}
	}

	[Serializable]
	internal sealed class _0024UpdateTrackLod_0024127 : GenericGenerator<WaitForSeconds>
	{
		[Serializable]
		internal sealed class _0024 : GenericGeneratorEnumerator<WaitForSeconds>, IEnumerator
		{
			internal CartCam _0024self__0024128;

			public _0024(CartCam self_)
			{
				_0024self__0024128 = self_;
			}

			public override bool MoveNext()
			{
				int result;
				switch (_state)
				{
				default:
					_0024self__0024128.track.SetLod(_0024self__0024128.transform, _0024self__0024128.Cart.currentDistance - _0024self__0024128.ViewLead, Mathf.Max(Performance.GamePerformance * 150, 100));
					result = (Yield(2, new WaitForSeconds(0.6f)) ? 1 : 0);
					break;
				case 1:
					result = 0;
					break;
				}
				return (byte)result != 0;
			}
		}

		internal CartCam _0024self__0024129;

		public _0024UpdateTrackLod_0024127(CartCam self_)
		{
			_0024self__0024129 = self_;
		}

		public override IEnumerator<WaitForSeconds> GetEnumerator()
		{
			return new _0024(_0024self__0024129);
		}
	}

	[Serializable]
	internal sealed class _0024LaughShake_0024130 : GenericGenerator<WaitForSeconds>
	{
		[Serializable]
		internal sealed class _0024 : GenericGeneratorEnumerator<WaitForSeconds>, IEnumerator
		{
			internal AnimationCurve _0024ac_0024131;

			internal CartCam _0024self__0024132;

			public _0024(AnimationCurve ac, CartCam self_)
			{
				_0024ac_0024131 = ac;
				_0024self__0024132 = self_;
			}

			public override bool MoveNext()
			{
				int result;
				switch (_state)
				{
				default:
					_0024self__0024132.laughCurve = _0024ac_0024131;
					_0024self__0024132.laughing = true;
					_0024self__0024132.laughTime = 0f;
					result = (Yield(2, new WaitForSeconds(_0024self__0024132.laughCurve.keys[_0024self__0024132.laughCurve.length - 1].time)) ? 1 : 0);
					break;
				case 2:
					_0024self__0024132.laughing = false;
					YieldDefault(1);
					goto case 1;
				case 1:
					result = 0;
					break;
				}
				return (byte)result != 0;
			}
		}

		internal AnimationCurve _0024ac_0024133;

		internal CartCam _0024self__0024134;

		public _0024LaughShake_0024130(AnimationCurve ac, CartCam self_)
		{
			_0024ac_0024133 = ac;
			_0024self__0024134 = self_;
		}

		public override IEnumerator<WaitForSeconds> GetEnumerator()
		{
			return new _0024(_0024ac_0024133, _0024self__0024134);
		}
	}

	[NonSerialized]
	public static CartCam inst;

	private TrackMaster track;

	public bool FixedLookUp;

	public Cart Cart;

	public Vector3 Offset;

	public float ViewLead;

	public TextMesh Score;

	public TextMesh Multiply;

	public SmileDisplay Smiles;

	public Transform Hud;

	public Camera HudCamera;

	public Transform ViewTrackParent;

	private Camera HudCam;

	public ViewTrack[] viewTracks;

	private perspectiveHud PHud;

	private int CurrentViewTrack;

	public Renderer WhiteOut;

	public Renderer BlackOut;

	public Renderer[] hudRenderers;

	public GuiControl GuiCon;

	public Camera indicatorCam;

	private int CurrentViewPoint;

	private CameraMode CamMode;

	private Rect ControlArea;

	private Vector3 originalOffset;

	private float CamShakeAmount;

	private Vector3 focusOffset;

	private float SmoothedSpeed;

	private float WorldExposureLevel;

	private float EyeExposureLevel;

	private AnimationCurve ExposureAnimation;

	private float ExposureEventTime;

	private float TotalExposureTime;

	private float AmbiantLight;

	private bool shakeing;

	private bool laughing;

	private float laughTime;

	private float laughStrength;

	private AnimationCurve laughCurve;

	private Vector3 LookUp;

	private float LerpFloat;

	private float viewLeadTarget;

	private float CurrentMulti;

	[NonSerialized]
	public static bool SkipStartDisplay;

	[NonSerialized]
	public static bool IntroPlayed;

	public CartCam()
	{
		FixedLookUp = true;
		ViewLead = 100f;
		CamMode = CameraMode.ScenicView;
		CamShakeAmount = 1f;
		focusOffset = Vector3.zero;
		LookUp = Vector3.up;
		viewLeadTarget = 26.5f;
		CurrentMulti = 1f;
	}

	public virtual void SetAmbiantLight(float val)
	{
		AmbiantLight = val;
	}

	public virtual IEnumerator SetCamHeight(float h)
	{
		return new _0024SetCamHeight_0024117(h, this).GetEnumerator();
	}

	public virtual void SetCameraRendering(bool state)
	{
		GetComponent<Camera>().enabled = state;
		HudCamera.enabled = state;
	}

	public virtual void ApplyExposureChange(AnimationCurve AC)
	{
		if (!(ExposureEventTime <= 0f))
		{
			AC = AppendExposure(AC);
		}
		AC = StartEndCurveBlend(AC);
		ExposureAnimation = AC;
		ExposureEventTime = ExposureAnimation.keys[ExposureAnimation.length - 1].time;
		TotalExposureTime = ExposureEventTime;
	}

	public virtual AnimationCurve AppendExposure(AnimationCurve AC)
	{
		int num = 14;
		int num2 = (int)AC.keys[AC.length - 1].time;
		Keyframe[] array = new Keyframe[num];
		for (int i = 0; i < num; i++)
		{
			float num3 = num2 / num * i;
			float time = TotalExposureTime - ExposureEventTime + num3;
			array[i].time = num3;
			array[i].value = Mathf.Max(ExposureAnimation.Evaluate(time), AC.Evaluate(num3));
		}
		AnimationCurve animationCurve = new AnimationCurve(array);
		SmoothCurve(animationCurve);
		return animationCurve;
	}

	public virtual AnimationCurve StartEndCurveBlend(AnimationCurve AC)
	{
		AC.MoveKey(0, new Keyframe(AC.keys[0].time, WorldExposureLevel));
		AC.MoveKey(AC.length - 1, new Keyframe(AC.keys[AC.length - 1].time, AmbiantLight));
		return AC;
	}

	public virtual void Update()
	{
		if (!(ExposureEventTime <= 0f))
		{
			ExposureEventTime -= Time.deltaTime;
			WorldExposureLevel = ExposureAnimation.Evaluate(TotalExposureTime - ExposureEventTime);
		}
		else
		{
			WorldExposureLevel = AmbiantLight;
		}
		EyeExposureLevel = Mathf.Lerp(EyeExposureLevel, Mathf.Clamp(WorldExposureLevel, -0.5f, 0.5f), Time.deltaTime);
		SetBrightness(WorldExposureLevel - EyeExposureLevel);
	}

	public virtual void SetBrightness(float bright)
	{
		if (!(Mathf.Abs(bright) >= 0.01f))
		{
			BlackOut.enabled = false;
			WhiteOut.enabled = false;
		}
		else if (!(bright <= 0f))
		{
			BlackOut.enabled = false;
			WhiteOut.enabled = true;
			WhiteOut.material.SetColor("_TintColor", Color.Lerp(Color.black, Color.white, bright));
		}
		else
		{
			WhiteOut.enabled = false;
			BlackOut.enabled = true;
			BlackOut.material.SetColor("_Color", Color.Lerp(Color.white, Color.black, Mathf.Abs(bright)));
		}
	}

	public virtual void SmoothCurve(AnimationCurve AC)
	{
		for (int i = 0; i < Extensions.get_length((System.Array)AC.keys); i++)
		{
			AC.SmoothTangents(i, 0f);
		}
	}

	public virtual void SetEndCam(Transform trans)
	{
		enabled = false;
		SetHud(false);
		transform.position = trans.position;
		transform.rotation = trans.rotation;
	}

	public virtual void SetCameraMode(CameraMode CM)
	{
		if (CamMode == CameraMode.ScenicView)
		{
			for (int i = 0; i < Extensions.get_length((System.Array)viewTracks); i++)
			{
				if (i != CurrentViewTrack && viewTracks[i].TrackTarget != null && viewTracks[i].TrackTarget.gameObject.active)
				{
					Debug.Log("Caught Problem");
				}
				viewTracks[i].gameObject.SendMessage("DeactivateDrone", SendMessageOptions.DontRequireReceiver);
			}
			PHud.HideText();
		}
		CamMode = CM;
		if (CamMode == CameraMode.PlayGame)
		{
			ViewTrack.priorityViews = (string[])new UnityScript.Lang.Array().ToBuiltin(typeof(string));
			transform.position = track.PositionAtDist(Cart.currentDistance - ViewLead) + Cart.transform.TransformDirection(Offset);
			transform.LookAt(track.PositionAtDist(Cart.currentDistance + SmoothedSpeed) + focusOffset);
			SetHud(true);
		}
		if (CamMode == CameraMode.ScenicView)
		{
			NextCamera();
			SetHud(false);
			GameControl.inst.SkipWait();
		}
	}

	public virtual void SetSmiles(float smile)
	{
		Smiles.UpdateSmileDisplay(smile);
	}

	public virtual IEnumerator Start()
	{
		return new _0024Start_0024124(this).GetEnumerator();
	}

	public virtual IEnumerator UpdateTrackLod()
	{
		return new _0024UpdateTrackLod_0024127(this).GetEnumerator();
	}

	public virtual void SetHud(bool state)
	{
		GuiCon.SetState(state);
	}

	public virtual IEnumerator LaughShake(AnimationCurve ac)
	{
		return new _0024LaughShake_0024130(ac, this).GetEnumerator();
	}

	public virtual void CameraShakeOn()
	{
		shakeing = true;
	}

	public virtual void CameraShakeOff()
	{
		shakeing = false;
	}

	public virtual void SetRightHanded(bool right)
	{
		Cart.BoostControl.SetRightLeft(right);
		GuiCon.SetRightLeft(right);
	}

	public virtual void Awake()
	{
		inst = this;
		Component[] componentsInChildren = ViewTrackParent.gameObject.GetComponentsInChildren(typeof(ViewTrack));
		viewTracks = new ViewTrack[Extensions.get_length((System.Array)componentsInChildren)];
		for (int i = 0; i < Extensions.get_length((System.Array)viewTracks); i++)
		{
			viewTracks[i] = (ViewTrack)componentsInChildren[i];
		}
		CurrentViewPoint = (int)((float)Extensions.get_length((System.Array)viewTracks) * UnityEngine.Random.value);
		if (PlayerPrefs.GetInt("RightHanded") == 1)
		{
			SetRightHanded(true);
		}
		else
		{
			SetRightHanded(false);
		}
		originalOffset = Offset;
		Hud.position = HudCamera.ViewportToWorldPoint(new Vector3(0f, 1f, 10f));
		track = Cart.track;
	}

	public virtual void FixedUpdate()
	{
		if (CamMode == CameraMode.PlayGame && !Cart.RunOver)
		{
			Offset = originalOffset;
			focusOffset = Vector3.zero;
			if (laughing)
			{
				laughStrength = laughCurve.Evaluate(laughTime);
				laughTime += Time.deltaTime;
				Offset += new Vector3(Mathf.Cos(Time.time * 4.5f) * 20f, Mathf.Sin(Time.time * 6f) * 20f, 0f) * laughStrength;
				focusOffset += new Vector3(Mathf.Cos(Time.time * 4.5f) * 20f, Mathf.Sin(Time.time * 6f) * 20f, 0f) * laughStrength;
				if (!(laughCurve.keys[laughCurve.length - 1].time >= laughTime))
				{
					laughing = false;
				}
			}
			if (Cart.UserTiltCorrect)
			{
				CameraShakeOff();
			}
			else
			{
				CameraShakeOn();
			}
			if (shakeing)
			{
				Offset += UnityEngine.Random.insideUnitSphere;
				focusOffset += UnityEngine.Random.insideUnitSphere;
			}
			if (!FixedLookUp)
			{
				LookUp = track.RotationAtDist(Cart.currentDistance - ViewLead) * Vector3.up;
			}
			Score.text = string.Empty + Mathf.Floor(Cart.currentScore);
			if ((bool)Cart.currentSection && CurrentMulti != Cart.currentSection.ScoreMultiplyer)
			{
				CurrentMulti = Cart.currentSection.ScoreMultiplyer;
				Multiply.text = "x" + Cart.currentSection.ScoreMultiplyer;
				if (!(Cart.currentSection.ScoreMultiplyer <= 1f))
				{
					TutoralDisplay.inst.StartCoroutine(TutoralDisplay.inst.TriggerTutoral(2, Cart.gameObject));
				}
			}
			if (Cart.Boosting)
			{
				ViewLead = Mathf.Lerp(ViewLead, 90f, Time.deltaTime);
				SmoothedSpeed = Mathf.Clamp(Mathf.Lerp(SmoothedSpeed, 0f, Time.deltaTime), 0f, 90f);
			}
			else
			{
				ViewLead = Mathf.Lerp(ViewLead, 45f, Time.deltaTime);
				SmoothedSpeed = Mathf.Clamp(Mathf.Lerp(SmoothedSpeed, Cart.speed * 0.6f, Time.deltaTime), 0f, 90f);
			}
			transform.position = Vector3.Lerp(transform.position, track.PositionAtDist(Cart.currentDistance - ViewLead) + Cart.transform.TransformDirection(Offset), 1f);
			transform.LookAt(track.PositionAtDist(Cart.currentDistance + SmoothedSpeed) + focusOffset, LookUp);
		}
		if (CamMode == CameraMode.ScenicView)
		{
			LerpFloat += Time.deltaTime;
			viewTracks[CurrentViewTrack].SetCamera(transform, LerpFloat);
			if (!(LerpFloat <= viewTracks[CurrentViewTrack].GetTotalTime()))
			{
				NextCamera();
			}
		}
	}

	public virtual void RandomView()
	{
		SetBrightness(0f);
		int num = (int)(UnityEngine.Random.value * (float)Extensions.get_length((System.Array)viewTracks));
		viewTracks[num].SetCamera(transform, viewTracks[num].GetTotalTime() * UnityEngine.Random.value);
	}

	private void NextCamera()
	{
		PHud.HideText();
		LerpFloat = 0f;
		int currentViewTrack = CurrentViewTrack;
		CurrentViewTrack = (int)(UnityEngine.Random.value * (float)Extensions.get_length((System.Array)viewTracks));
		if (CurrentViewTrack == currentViewTrack)
		{
			CurrentViewTrack++;
			CurrentViewTrack = (int)Mathf.Repeat(CurrentViewTrack, Extensions.get_length((System.Array)viewTracks));
		}
		SkipStartDisplay = false;
		if (!IntroPlayed)
		{
			for (int i = 0; i < Extensions.get_length((System.Array)viewTracks); i++)
			{
				if (viewTracks[i].UpgradeAssociation == "Intro")
				{
					CurrentViewTrack = i;
					IntroPlayed = true;
					SkipStartDisplay = true;
				}
			}
		}
		if (Extensions.get_length((System.Array)ViewTrack.priorityViews) > 0)
		{
			string text = ViewTrack.PopPriorityView();
			MonoBehaviour.print("view Priority " + text);
			for (int i = 0; i < Extensions.get_length((System.Array)viewTracks); i++)
			{
				if (viewTracks[i].UpgradeAssociation == text)
				{
					CurrentViewTrack = i;
					PHud.DisplayText("Upgrade|" + TrackUpgrade.getDisplayName(TrackUpgrade.UpgradeID(viewTracks[i].UpgradeAssociation)));
				}
			}
			SkipStartDisplay = true;
		}
		else
		{
			PHud.HideText();
		}
		viewTracks[currentViewTrack].gameObject.SendMessage("DeactivateDrone", SendMessageOptions.DontRequireReceiver);
		viewTracks[CurrentViewTrack].gameObject.SendMessage("ActivateDrone", SendMessageOptions.DontRequireReceiver);
	}

	public virtual Rect fluidRect(float x, float y, float width, float height)
	{
		return new Rect(x * (float)Screen.width, y * (float)Screen.height, width * (float)Screen.width, height * (float)Screen.height);
	}

	public virtual Rect UpdateRect(float x, float y, float width, float height, Rect Update)
	{
		Update.x = x;
		Update.y = y;
		Update.width = width;
		Update.height = height;
		return Update;
	}

	public virtual void Main()
	{
	}
}
