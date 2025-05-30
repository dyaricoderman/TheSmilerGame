using System;
using UnityEngine;

[Serializable]
public class BlueprintMouseOrbit : MonoBehaviour
{
	public Transform target;

	public float distance;

	public float minDistance;

	public float maxDistance;

	public float xSpeed;

	public float ySpeed;

	public float autoRotateXSpeed;

	public int autoRotateYSpeed;

	public int yMinLimit;

	public int yMaxLimit;

	private float x;

	private float y;

	public Vector2 angle;

	public Vector2 velocity;

	public Vector2 lastVelocity;

	public BlueprintMouseOrbit()
	{
		distance = 10f;
		minDistance = 5f;
		maxDistance = 20f;
		xSpeed = 250f;
		ySpeed = 120f;
		autoRotateXSpeed = 250f;
		yMinLimit = -20;
		yMaxLimit = 80;
	}

	public virtual void Start()
	{
		Vector3 eulerAngles = transform.eulerAngles;
		angle.x = eulerAngles.y;
		angle.y = eulerAngles.x;
	}

	public virtual void LateUpdate()
	{
		if (!target)
		{
			return;
		}
		lastVelocity += velocity;
		lastVelocity *= 0.7f;
		if (Input.GetMouseButton(0))
		{
			velocity.x = Input.GetAxis("Mouse X") * xSpeed * Time.deltaTime;
			velocity.y = (0f - Input.GetAxis("Mouse Y")) * ySpeed * Time.deltaTime;
			if (Input.GetMouseButtonDown(0))
			{
				velocity.x = (velocity.y = (lastVelocity.x = (lastVelocity.y = 0f)));
			}
		}
		if (Input.GetMouseButtonUp(0))
		{
			velocity.x = (velocity.y = 0f);
			if (!(lastVelocity.x <= xSpeed * Time.deltaTime))
			{
				lastVelocity.x = xSpeed * Time.deltaTime;
			}
			else if (!(lastVelocity.x >= (0f - xSpeed) * Time.deltaTime))
			{
				lastVelocity.x = (0f - xSpeed) * Time.deltaTime;
			}
			if (!(lastVelocity.y <= ySpeed * Time.deltaTime))
			{
				lastVelocity.y = ySpeed * Time.deltaTime;
			}
			else if (!(lastVelocity.y >= (0f - ySpeed) * Time.deltaTime))
			{
				lastVelocity.y = (0f - ySpeed) * Time.deltaTime;
			}
			velocity += lastVelocity;
		}
		velocity *= 0.97f;
		angle += velocity;
		angle.y = ClampAngle(angle.y, yMinLimit, yMaxLimit);
		Quaternion quaternion = Quaternion.Euler(angle.y, angle.x, 0f);
		Vector3 position = quaternion * new Vector3(0f, 0f, 0f - distance) + target.position;
		transform.rotation = quaternion;
		transform.position = position;
	}

	public static float ClampAngle(float angle, float min, float max)
	{
		if (!(angle >= -360f))
		{
			angle += 360f;
		}
		if (!(angle <= 360f))
		{
			angle -= 360f;
		}
		return Mathf.Clamp(angle, min, max);
	}

	public virtual void Main()
	{
	}
}
