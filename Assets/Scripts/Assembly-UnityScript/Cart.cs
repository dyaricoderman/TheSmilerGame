using System;
using System.Collections;
using System.Collections.Generic;
using Boo.Lang;
using UnityEngine;
using UnityScript.Lang;

[Serializable]
public class Cart : MonoBehaviour
{
	[Serializable]
	internal sealed class _0024FirstTutoralTrigger_0024138 : GenericGenerator<object>
	{
		[Serializable]
		internal sealed class _0024 : GenericGeneratorEnumerator<object>, IEnumerator
		{
			internal Cart _0024self__0024139;

			public _0024(Cart self_)
			{
				_0024self__0024139 = self_;
			}

			public override bool MoveNext()
			{
				int result;
				switch (_state)
				{
				default:
					if (OptionsMenu.inst.OptionsActive)
					{
						result = (YieldDefault(2) ? 1 : 0);
						break;
					}
					TutoralDisplay.inst.StartCoroutine(TutoralDisplay.inst.TriggerTutoral(0, _0024self__0024139.gameObject));
					YieldDefault(1);
					goto case 1;
				case 1:
					result = 0;
					break;
				}
				return (byte)result != 0;
			}
		}

		internal Cart _0024self__0024140;

		public _0024FirstTutoralTrigger_0024138(Cart self_)
		{
			_0024self__0024140 = self_;
		}

		public override IEnumerator<object> GetEnumerator()
		{
			return new _0024(_0024self__0024140);
		}
	}

	[Serializable]
	internal sealed class _0024Start_0024141 : GenericGenerator<object>
	{
		[Serializable]
		internal sealed class _0024 : GenericGeneratorEnumerator<object>, IEnumerator
		{
			internal Cart _0024self__0024142;

			public _0024(Cart self_)
			{
				_0024self__0024142 = self_;
			}

			public override bool MoveNext()
			{
				int result;
				switch (_state)
				{
				default:
					currentScore = 0f;
					_0024self__0024142.CartGraphicPosition = _0024self__0024142.CartGraphic.localPosition;
					_0024self__0024142.CartGraphic.parent = null;
					AchievementManager.SetProgressToAchievement("Throttle Thriller", 0f);
					goto case 2;
				case 2:
					if (!_0024self__0024142.track.TrackComputationFinished)
					{
						result = (YieldDefault(2) ? 1 : 0);
						break;
					}
					_0024self__0024142.transform.position = _0024self__0024142.track.PositionAtDist(0.1f);
					_0024self__0024142.transform.rotation = _0024self__0024142.track.RotationAtDist(0.1f);
					YieldDefault(1);
					goto case 1;
				case 1:
					result = 0;
					break;
				}
				return (byte)result != 0;
			}
		}

		internal Cart _0024self__0024143;

		public _0024Start_0024141(Cart self_)
		{
			_0024self__0024143 = self_;
		}

		public override IEnumerator<object> GetEnumerator()
		{
			return new _0024(_0024self__0024143);
		}
	}

	[Serializable]
	internal sealed class _0024SetCartRunning_0024144 : GenericGenerator<object>
	{
		[Serializable]
		internal sealed class _0024 : GenericGeneratorEnumerator<object>, IEnumerator
		{
			internal float _0024fade_0024145;

			internal bool _0024state_0024146;

			internal Cart _0024self__0024147;

			public _0024(bool state, Cart self_)
			{
				_0024state_0024146 = state;
				_0024self__0024147 = self_;
			}

			public override bool MoveNext()
			{
				int result;
				switch (_state)
				{
				default:
					if (_0024self__0024147.paused)
					{
						result = (YieldDefault(2) ? 1 : 0);
						break;
					}
					_0024self__0024147.enabled = _0024state_0024146;
					if (_0024state_0024146)
					{
						Man.SetMenState(true);
						AchievementManager.SetProgressToAchievement("Rail Rush", 0f);
					}
					_0024fade_0024145 = 0f;
					if (!_0024state_0024146)
					{
						_0024self__0024147.EndBoost();
						_0024self__0024147.BoostEnd.Stop();
						_0024self__0024147.BoostStart.Stop();
						_0024self__0024147.IndicatorRight.enabled = false;
						_0024self__0024147.IndicatorLeft.enabled = false;
						_0024self__0024147.SparksLeft.enableEmission = false;
						_0024self__0024147.SparksRight.enableEmission = false;
						goto IL_0268;
					}
					goto IL_02bd;
				case 3:
					_0024fade_0024145 += 0.05f;
					goto IL_0268;
				case 1:
					{
						result = 0;
						break;
					}
					IL_02bd:
					if (_0024state_0024146)
					{
						_0024self__0024147.StartCoroutine(_0024self__0024147.FirstTutoralTrigger());
					}
					YieldDefault(1);
					goto case 1;
					IL_0268:
					if (_0024fade_0024145 < 1f)
					{
						_0024self__0024147.speed *= 1f - _0024fade_0024145;
						_0024self__0024147.grindSound.volume = Mathf.Min(_0024self__0024147.grindSound.volume, 1f - _0024fade_0024145);
						_0024self__0024147.cheerSound.volume = Mathf.Min(_0024self__0024147.cheerSound.volume, 1f - _0024fade_0024145);
						_0024self__0024147.CrankUp.volume = Mathf.Min(_0024self__0024147.CrankUp.volume, 1f - _0024fade_0024145);
						_0024self__0024147.RailSound.volume = Mathf.Min(_0024self__0024147.RailSound.volume, 1f - _0024fade_0024145);
						_0024self__0024147.CartGraphic.position = Vector3.Lerp(_0024self__0024147.CartGraphic.position, _0024self__0024147.transform.TransformPoint(_0024self__0024147.CartGraphicPosition), _0024fade_0024145);
						_0024self__0024147.CartGraphic.rotation = Quaternion.Lerp(_0024self__0024147.CartGraphic.localRotation, _0024self__0024147.transform.rotation, _0024fade_0024145);
						result = (YieldDefault(3) ? 1 : 0);
						break;
					}
					_0024self__0024147.grindSound.volume = 0f;
					_0024self__0024147.cheerSound.volume = 0f;
					_0024self__0024147.CrankUp.volume = 0f;
					_0024self__0024147.RailSound.volume = 0f;
					goto IL_02bd;
				}
				return (byte)result != 0;
			}
		}

		internal bool _0024state_0024148;

		internal Cart _0024self__0024149;

		public _0024SetCartRunning_0024144(bool state, Cart self_)
		{
			_0024state_0024148 = state;
			_0024self__0024149 = self_;
		}

		public override IEnumerator<object> GetEnumerator()
		{
			return new _0024(_0024state_0024148, _0024self__0024149);
		}
	}

	public TrackMaster track;

	[NonSerialized]
	public static Cart inst;

	public float speed;

	public float currentDistance;

	[NonSerialized]
	public static float currentScore;

	public Transform CartGraphic;

	public float CartWheelWidth;

	public float MeshTilt;

	public float TrueTilt;

	public ParticleSystem SparksLeft;

	public ParticleSystem SparksRight;

	public float AccuracyRequired;

	public TrackSection currentSection;

	public float AcceRate;

	public float SlowRate;

	public AudioSource grindSound;

	public AudioSource cheerSound;

	public AudioSource CrankUp;

	public AudioSource RailSound;

	public AudioSource BoostStart;

	public AudioSource BoostEnd;

	public float FullMarmalizeScore;

	private int RiderCount;

	public bool RunOver;

	public float BoostForce;

	public ParticleSystem[] BoostFire;

	public float FullScoreSpeed;

	public BoostGUIControl BoostControl;

	public Renderer IndicatorLeft;

	public Renderer IndicatorRight;

	private Vector3 CartGraphicPosition;

	private float TotalTrackDist;

	public float BoostCount;

	private bool paused;

	public bool Boosting;

	public int AccuracyIcon;

	public int WorstAccuracy;

	public int WorstSectionAccuracy;

	public float MeshTiltDeadSpot;

	private int MarCount;

	public bool UserTiltCorrect;

	public Vector3 tilt;

	public float userTilt;

	public Cart()
	{
		speed = 5f;
		CartWheelWidth = 1f;
		AccuracyRequired = 0.3f;
		AcceRate = 1f;
		SlowRate = 1f;
		FullMarmalizeScore = 4000f;
		RiderCount = 16;
		BoostForce = 20f;
		FullScoreSpeed = 120f;
		AccuracyIcon = 3;
		WorstAccuracy = 3;
		WorstSectionAccuracy = 3;
	}

	public virtual void UpdateIndicators()
	{
		if (AccuracyIcon == 3)
		{
			IndicatorLeft.enabled = false;
			IndicatorRight.enabled = false;
			return;
		}
		Renderer renderer = null;
		if (!(TrueTilt <= 0f))
		{
			IndicatorRight.enabled = false;
			IndicatorLeft.enabled = true;
			renderer = IndicatorLeft;
		}
		else
		{
			IndicatorRight.enabled = true;
			IndicatorLeft.enabled = false;
			renderer = IndicatorRight;
		}
		if (AccuracyIcon == 2)
		{
			renderer.material.SetTextureOffset("_MainTex", new Vector2(0f, 2f / 3f));
		}
		if (AccuracyIcon == 1)
		{
			renderer.material.SetTextureOffset("_MainTex", new Vector2(0f, 1f / 3f));
		}
		if (AccuracyIcon == 0)
		{
			renderer.material.SetTextureOffset("_MainTex", new Vector2(0f, 0f));
		}
	}

	public virtual void Awake()
	{
		inst = this;
		track.SetToStartLoc(transform);
		enabled = false;
		SparksLeft.enableEmission = false;
		SparksRight.enableEmission = false;
		UserTiltCorrect = true;
		IndicatorRight.enabled = false;
		IndicatorLeft.enabled = false;
		Man.SetMenState(false);
	}

	public static void PointsBoost(int points)
	{
		currentScore += (float)points * inst.UserAccuracy() * (inst.speed / inst.FullScoreSpeed);
	}

	public virtual IEnumerator FirstTutoralTrigger()
	{
		return new _0024FirstTutoralTrigger_0024138(this).GetEnumerator();
	}

	public virtual void TutoralStart()
	{
		Pause();
	}

	public virtual void TutoralEnded()
	{
		Unpause();
	}

	public virtual IEnumerator Start()
	{
		return new _0024Start_0024141(this).GetEnumerator();
	}

	public virtual void Pause()
	{
		paused = true;
		enabled = false;
		CartCam.inst.enabled = false;
		CartCam.inst.SetHud(false);
		IndicatorRight.enabled = false;
		IndicatorLeft.enabled = false;
		SparksLeft.enableEmission = false;
		SparksRight.enableEmission = false;
		CartCam.inst.indicatorCam.enabled = false;
		for (int i = 0; i < Extensions.get_length((System.Array)CartDrone.DroneList); i++)
		{
			if ((bool)CartDrone.DroneList[i])
			{
				CartDrone.DroneList[i].enabled = false;
			}
		}
	}

	public virtual void Unpause()
	{
		enabled = true;
		CartCam.inst.enabled = true;
		CartCam.inst.SetHud(true);
		CartCam.inst.indicatorCam.enabled = true;
		SparksLeft.enableEmission = false;
		SparksRight.enableEmission = false;
		for (int i = 0; i < Extensions.get_length((System.Array)CartDrone.DroneList); i++)
		{
			if ((bool)CartDrone.DroneList[i])
			{
				CartDrone.DroneList[i].enabled = true;
			}
		}
		paused = false;
	}

	public virtual IEnumerator SetCartRunning(bool state)
	{
		return new _0024SetCartRunning_0024144(state, this).GetEnumerator();
	}

	public virtual void Boost()
	{
		if (!(BoostCount <= 0f))
		{
			Boosting = true;
			BoostStart.Play();
			AchievementManager.AddProgressToAchievement("Rail Rush", 1f);
			MonoBehaviour.print("startBoost");
		}
	}

	public virtual void EndBoost()
	{
		BoostStart.Stop();
		BoostEnd.Play();
		Boosting = false;
		BoostControl.SetFill(Mathf.Clamp(BoostCount, 0f, RiderCount) / (float)RiderCount);
	}

	public virtual int GetWorstAcc()
	{
		return WorstAccuracy;
	}

	public virtual float UserAccuracy()
	{
		float result;
		if (!(Mathf.Abs(TrueTilt) >= 0.06f))
		{
			AccuracyIcon = 3;
			result = 1f;
		}
		else if (!(Mathf.Abs(TrueTilt) >= 0.2f))
		{
			if (WorstAccuracy > 2)
			{
				WorstAccuracy = 2;
				WorstSectionAccuracy = 2;
			}
			AccuracyIcon = 2;
			result = 0.6f;
		}
		else if (!(Mathf.Abs(TrueTilt) >= 0.45f))
		{
			if (WorstAccuracy > 1)
			{
				WorstAccuracy = 1;
				WorstSectionAccuracy = 1;
			}
			AccuracyIcon = 1;
			result = 0.3f;
		}
		else
		{
			if (WorstAccuracy > 0)
			{
				WorstAccuracy = 0;
				WorstSectionAccuracy = 0;
			}
			AccuracyIcon = 0;
			result = 0f;
		}
		return result;
	}

	public virtual void Update()
	{
		if (AccuracyIcon == 0)
		{
			grindSound.volume = Mathf.Lerp(grindSound.volume, 1f, Time.deltaTime * 4f);
			RailSound.volume = Mathf.Lerp(RailSound.volume, 0f, Time.deltaTime * 4f);
		}
		else
		{
			grindSound.volume = Mathf.Lerp(grindSound.volume, 0f, Time.deltaTime * 4f);
			RailSound.volume = Mathf.Lerp(RailSound.volume, 1f, Time.deltaTime * 4f);
		}
		if (AccuracyIcon == 3 && (bool)currentSection && !currentSection.CrankUpChain)
		{
			cheerSound.volume = Mathf.Lerp(cheerSound.volume, 0.6f, Time.deltaTime * 2f);
		}
		else
		{
			cheerSound.volume = Mathf.Lerp(cheerSound.volume, 0f, Time.deltaTime * 2f);
		}
		if ((bool)currentSection && currentSection.CrankUpChain)
		{
			CrankUp.volume = 1f;
		}
		else
		{
			CrankUp.volume = 0f;
		}
		RailSound.pitch = 0.5f + Mathf.Clamp01(speed / 100f);
	}

	public virtual void FixedUpdate()
	{
		if (Boosting)
		{
			if (!(BoostCount <= 0f))
			{
				speed += BoostForce * Time.deltaTime;
				BoostCount -= Time.deltaTime;
				AchievementManager.AddProgressToAchievement("Throttle Thriller", Time.deltaTime);
				BoostControl.SetFill(Mathf.Clamp(BoostCount, 0f, RiderCount) / (float)RiderCount);
			}
			else
			{
				EndBoost();
			}
		}
		BoostControl.SetSpeed(Mathf.Clamp01(speed / 140f));
		currentDistance += speed * Time.deltaTime;
		transform.position = track.PositionAtDist(currentDistance);
		transform.rotation = track.RotationAtDist(currentDistance);
		TrackSection trackSection = currentSection;
		currentSection = track.GetCurrentTrackSection(currentDistance);
		if ((bool)trackSection && trackSection != currentSection)
		{
			if (!(trackSection.ScoreMultiplyer <= 1f) && WorstSectionAccuracy == 3)
			{
				AchievementManager.AddProgressToAchievement("Round the Twist", 1f);
			}
			WorstSectionAccuracy = 3;
		}
		if (!currentSection.CrankUpChain)
		{
			speed += transform.TransformDirection(Vector3.forward).y * Physics.gravity.y * Time.deltaTime;
		}
		float num = UserAccuracy();
		currentScore += Mathf.Abs(speed * Time.deltaTime * currentSection.ScoreMultiplyer) * num * (speed / FullScoreSpeed);
		int num2 = (int)(currentScore / FullMarmalizeScore * (float)RiderCount);
		CartCam.inst.SetSmiles(currentScore / FullMarmalizeScore);
		if (num2 > MarCount && MarCount < RiderCount)
		{
			MarCount++;
			(Man.ManList[MarCount - 1] as Man).SetMarmalized();
			BoostCount += 2f;
			BoostControl.SetFill(Mathf.Clamp(BoostCount, 0f, RiderCount) / (float)RiderCount);
			TutoralDisplay.inst.StartCoroutine(TutoralDisplay.inst.TriggerTutoral(1, gameObject));
		}
		if (!UserTiltCorrect)
		{
			speed -= Time.deltaTime * SlowRate;
		}
		CartGraphic.localRotation = transform.rotation;
		CartGraphic.position = transform.TransformPoint(CartGraphicPosition);
		if (!(Mathf.Abs(TrueTilt) >= 0.45f))
		{
			if (!UserTiltCorrect)
			{
				SparksLeft.enableEmission = false;
				SparksRight.enableEmission = false;
				UserTiltCorrect = true;
			}
		}
		else
		{
			UserTiltCorrect = false;
		}
		MeshTiltDeadSpot = MeshTilt - Mathf.Sign(MeshTilt) * Mathf.Min(Mathf.Abs(MeshTilt), 0.05f);
		if (Boosting)
		{
			BoostFire[0].enableEmission = true;
			BoostFire[1].enableEmission = true;
		}
		else
		{
			BoostFire[0].enableEmission = false;
			BoostFire[1].enableEmission = false;
		}
		if (!(MeshTilt <= 0f))
		{
			CartGraphic.RotateAround(transform.TransformPoint(new Vector3(CartWheelWidth, 0f, 0f)), transform.TransformDirection(Vector3.forward), MeshTiltDeadSpot * -180f);
			if (!(Mathf.Abs(TrueTilt) <= 0.25f))
			{
				SparksLeft.enableEmission = false;
				SparksRight.enableEmission = true;
				BoostFire[0].enableEmission = false;
			}
			else
			{
				SparksLeft.enableEmission = false;
				SparksRight.enableEmission = false;
			}
		}
		else
		{
			if (!(Mathf.Abs(TrueTilt) <= 0.25f))
			{
				SparksLeft.enableEmission = true;
				SparksRight.enableEmission = false;
				BoostFire[1].enableEmission = false;
			}
			else
			{
				SparksLeft.enableEmission = false;
				SparksRight.enableEmission = false;
			}
			CartGraphic.RotateAround(transform.TransformPoint(new Vector3(0f - CartWheelWidth, 0f, 0f)), transform.TransformDirection(Vector3.forward), MeshTiltDeadSpot * -180f);
		}
		if (currentSection.Break)
		{
			if (!(speed <= currentSection.BreakSpeed))
			{
				if (!(track.DistanceToSectionEnd(currentDistance) >= currentSection.GetLength() * currentSection.BreakKickin))
				{
					speed -= (speed - currentSection.BreakSpeed) / track.DistanceToSectionEnd(currentDistance) * Time.deltaTime * speed;
				}
			}
			else
			{
				speed = currentSection.BreakSpeed;
			}
		}
		if (currentSection.CrankUpChain && !(speed >= currentSection.CrankUpSpeed))
		{
			speed = Mathf.Max(speed + currentSection.CrankUpSpeed * 0.3333f * Time.deltaTime, 1f);
		}
		if (currentDistance > track.GetTotalTrackDist() || (!(speed >= 10f) && !currentSection.CrankUpChain && !currentSection.Break))
		{
			if (!RunOver)
			{
				RunOver = true;
				GameControl.inst.StartCoroutine(GameControl.inst.FinishLevel(this));
			}
			speed *= 1f - Time.deltaTime;
			if (!(speed >= 0f))
			{
				enabled = false;
			}
		}
		UpdateIndicators();
	}

	public virtual Vector3 ChangeInTilt()
	{
		return transform.InverseTransformDirection(track.RotationAtDist(currentDistance + speed) * Vector3.forward) * 0.5f;
	}

	public virtual void OnDrawGizmos()
	{
		Gizmos.DrawSphere(transform.position, 0.5f);
		Gizmos.color = Color.red;
		Gizmos.DrawSphere(transform.TransformPoint(new Vector3(CartWheelWidth, 0f, 0f)), 0.4f);
		Gizmos.DrawSphere(transform.TransformPoint(new Vector3(0f - CartWheelWidth, 0f, 0f)), 0.4f);
	}

	public virtual void Main()
	{
	}
}
