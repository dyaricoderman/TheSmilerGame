using UnityEngine;

public class Spinner : MonoBehaviour
{
	public GameObject pointer;

	public GameObject wheelLock;

	public TextMesh counter;

	private int remainingSpins = 3;

	public float friction = 1f;

	private float speed;

	private Ray ray;

	private RaycastHit hit;

	public float initAngle;

	public float lastCursorAngle;

	public float cursorAngle;

	public float angleChange;

	private bool playerControl = true;

	public float snapback;

	private int currentSegment;

	public bool prizeLoaded;

	public AudioClip clickSnd;

	public float lastAngle;

	private void Start()
	{
		GameObject.Find("Competition").SendMessage("Begin");
		base.GetComponent<Animation>()["winWheel-loop"].speed = 0.5f;
		counter.text = remainingSpins.ToString();
		currentSegment = Mathf.FloorToInt((base.transform.eulerAngles.z - 15f) / 30f);
		if (remainingSpins > 0)
		{
			wheelLock.GetComponent<Animation>().Play("wheelUnlock");
		}
		else
		{
			wheelLock.GetComponent<Animation>().Play("wheelLock");
		}
	}

	private void Update()
	{
		ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		lastCursorAngle = cursorAngle;
		if (playerControl && Input.GetMouseButton(0) && Physics.Raycast(ray, out hit))
		{
			cursorAngle = Angle(new Vector2(base.transform.position.x, base.transform.position.y), new Vector2(hit.point.x, hit.point.y));
			if (Input.GetMouseButtonDown(0))
			{
				initAngle = cursorAngle;
			}
			cursorAngle = ToAbsAngle(cursorAngle);
			cursorAngle -= initAngle;
			if (cursorAngle - lastCursorAngle > 300f)
			{
				lastCursorAngle = cursorAngle;
			}
			else if (cursorAngle - lastCursorAngle < -300f)
			{
				lastCursorAngle = cursorAngle;
			}
			speed = cursorAngle - lastCursorAngle;
			if (speed < 0f)
			{
				snapback = speed;
				speed *= 0.15f / friction;
			}
		}
		else
		{
			speed = Mathf.Lerp(speed, 0f, Time.deltaTime * friction);
			if (snapback < 0f)
			{
				snapback *= 0.175f / friction;
				speed -= snapback;
				if ((double)snapback < 0.01)
				{
					snapback = 0f;
				}
			}
			if (speed > 50f && playerControl)
			{
				remainingSpins--;
				counter.text = remainingSpins.ToString();
				playerControl = false;
				base.GetComponent<Animation>().Play("winWheel-loop");
			}
		}
		if (Input.GetMouseButtonUp(0))
		{
			cursorAngle = 0f;
		}
		if (playerControl)
		{
			base.transform.eulerAngles = new Vector3(base.transform.eulerAngles.x, base.transform.eulerAngles.y, base.transform.eulerAngles.z + speed);
		}
		else
		{
			speed = base.transform.eulerAngles.z - lastAngle;
			lastAngle = base.transform.eulerAngles.z;
			if (prizeLoaded)
			{
				base.GetComponent<Animation>().Play("winWheel-STOPYELLOW");
			}
		}
		float num = speed;
		if (num > 5f)
		{
			num = 5f;
		}
		pointer.GetComponent<Animation>()["winWheelPointer-forward"].speed = num;
		pointer.GetComponent<Animation>()["winWheelPointer-backward"].speed = 0f - num;
		pointer.GetComponent<Animation>()["winWheelPointer-fullSpin"].speed = 0f - num;
		int num2 = Mathf.FloorToInt((base.transform.eulerAngles.z - 15f) / 30f);
		if (num2 == currentSegment)
		{
			return;
		}
		if (num2 > currentSegment)
		{
			if (speed > 20f)
			{
				pointer.GetComponent<Animation>().Play("winWheelPointer-fullSpin");
				MonoBehaviour.print("Wooo");
			}
			else
			{
				pointer.GetComponent<Animation>().Play("winWheelPointer-fullSpin");
			}
		}
		else if (num2 < currentSegment && !base.GetComponent<Animation>().isPlaying)
		{
			pointer.GetComponent<Animation>().Play("winWheelPointer-backward");
		}
		base.GetComponent<AudioSource>().clip = clickSnd;
		base.GetComponent<AudioSource>().Play();
		currentSegment = num2;
	}

	private float Angle(Vector2 pos1, Vector2 pos2)
	{
		Vector2 vector = pos2 - pos1;
		Vector2 vector2 = new Vector2(1f, 0f);
		float num = Vector2.Angle(vector, vector2);
		if (Vector3.Cross(vector, vector2).z > 0f)
		{
			num = 360f - num;
		}
		return num;
	}

	private float ToAbsAngle(float angle)
	{
		if (angle > 360f)
		{
			return angle -= 360f;
		}
		if (angle < 0f)
		{
			return angle += 360f;
		}
		return angle;
	}
}
