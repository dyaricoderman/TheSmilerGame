using System;
using UnityEngine;

public class SpinWheel : MonoBehaviour
{
	public Competition competition;

	public CompetitionLogic competitionLogic;

	public Transform Wheel;

	public bool playerControl = true;

	public float MinTriggerSpeed = 20f;

	public float friction = 3f;

	private float speed;

	private float initAngle;

	private float lastAngle;

	private float currentAngle;

	public bool WheelBreak;

	public float SpinLeft;

	private bool init;

	public int remainingSpins;

	public GameObject pointer;

	public GameObject wheelLock;

	public AudioClip clickSnd;

	public int currentSegment;

	private void Start()
	{
		if (PlayerPrefs.HasKey("remainingSpins"))
		{
			remainingSpins = PlayerPrefs.GetInt("remainingSpins");
		}
		else
		{
			remainingSpins = 1;
			PlayerPrefs.SetInt("remainingSpins", remainingSpins);
		}
		currentSegment = Mathf.FloorToInt((Wheel.eulerAngles.z - 15f) / 30f);
	}

	public void Lock()
	{
		wheelLock.GetComponent<Animation>().Play("wheelLock");
		playerControl = false;
	}

	public void Unlock()
	{
		wheelLock.GetComponent<Animation>().Play("wheelUnlock");
		playerControl = true;
	}

	private void FixedUpdate()
	{
		if (WheelBreak)
		{
			speed -= Mathf.Sign(speed) * (Mathf.Abs(speed) / SpinLeft * Mathf.Abs(speed));
			if (Mathf.Abs(SpinLeft) < 2f)
			{
				speed = 0f;
				WheelBreak = false;
				if (currentSegment % 4 == 0)
				{
					competition.StartCoroutine(competition.LoadWinScreen(0.5f));
				}
				else
				{
					competition.StartCoroutine(competition.LoadLoseScreen(0.5f));
				}
			}
		}
		else if (playerControl && OnWheel() && Input.GetMouseButton(0))
		{
			if (!init)
			{
				init = true;
				initAngle = AbsouteAngle();
				currentAngle = 0f;
			}
			lastAngle = currentAngle;
			currentAngle = AbsouteAngle() - initAngle;
			float f = Mathf.DeltaAngle(lastAngle, currentAngle);
			if (Mathf.Abs(f) > 0f)
			{
				speed = f;
			}
		}
		else if (playerControl)
		{
			speed -= Time.deltaTime * speed * friction;
			init = false;
			if (Mathf.Abs(speed) > MinTriggerSpeed)
			{
				playerControl = false;
				competitionLogic.StartCoroutine(competitionLogic.Spin());
				competitionLogic.StartCoroutine(competitionLogic.SpinTimeoutHandler());
			}
		}
		Wheel.RotateAround(Wheel.position, base.transform.TransformDirection(Vector3.up), speed);
		SpinLeft -= Mathf.Abs(speed);
		float num = speed;
		if (num > 5f)
		{
			num = 5f;
		}
		pointer.GetComponent<Animation>()["winWheelPointer-forward"].speed = num;
		pointer.GetComponent<Animation>()["winWheelPointer-backward"].speed = 0f - num;
		pointer.GetComponent<Animation>()["winWheelPointer-fullSpin"].speed = 1f;
		int num2 = Mathf.FloorToInt((Wheel.eulerAngles.z - 10f) / 30f);
		if (num2 == currentSegment)
		{
			return;
		}
		if (num2 > currentSegment)
		{
			if (speed > 10f)
			{
				if (!pointer.GetComponent<Animation>().isPlaying)
				{
					pointer.GetComponent<Animation>().Play("winWheelPointer-fullSpin");
				}
			}
			else if (!pointer.GetComponent<Animation>().isPlaying)
			{
				pointer.GetComponent<Animation>().Play("winWheelPointer-forward");
			}
		}
		else if (num2 < currentSegment && !pointer.GetComponent<Animation>().isPlaying)
		{
			pointer.GetComponent<Animation>().Play("winWheelPointer-backward");
		}
		base.GetComponent<AudioSource>().clip = clickSnd;
		if (!base.GetComponent<AudioSource>().isPlaying || (double)base.GetComponent<AudioSource>().time > 0.1)
		{
			base.GetComponent<AudioSource>().Play();
		}
		currentSegment = num2;
	}

	public void StopWheel(bool won, bool successful)
	{
		if (!successful)
		{
			WheelBreak = false;
			return;
		}
		WheelBreak = true;
		float num = 0f;
		num = ((!won) ? (Mathf.Floor(UnityEngine.Random.value * 3f) * -30f + Mathf.Floor(UnityEngine.Random.value * 3f) * 120f) : (30f + Mathf.Floor(UnityEngine.Random.value * 3f) * 120f));
		float num2 = Mathf.Abs((1f + Mathf.Floor(speed / 3f)) * 360f);
		if (speed > 0f)
		{
			SpinLeft = num2 + (360f - Wheel.localRotation.eulerAngles.y) + num;
		}
		else
		{
			SpinLeft = num2 + (Wheel.localRotation.eulerAngles.y - 360f) - num;
		}
	}

	private bool OnWheel()
	{
		if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition)))
		{
			return true;
		}
		return false;
	}

	public void SetPlayerControl(bool control)
	{
		playerControl = control;
		speed = 0f;
	}

	private float AbsouteAngle()
	{
		RaycastHit hitInfo = default(RaycastHit);
		Quaternion identity = Quaternion.identity;
		if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hitInfo))
		{
			Vector3 vector = base.transform.InverseTransformPoint(hitInfo.point);
			return (0f - Mathf.Atan2(vector.z, vector.x)) * (180f / (float)Math.PI);
		}
		return 0f;
	}
}
