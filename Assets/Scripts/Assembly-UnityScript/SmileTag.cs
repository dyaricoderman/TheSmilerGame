using System;
using UnityEngine;

[Serializable]
public class SmileTag : MonoBehaviour
{
	public Vector3 startSize;

	public float timer;

	public virtual void Awake()
	{
		startSize = transform.localScale;
		GetComponent<Renderer>().enabled = false;
		enabled = false;
	}

	public virtual void Show()
	{
		GetComponent<Renderer>().enabled = true;
		enabled = true;
	}

	public virtual void Update()
	{
		timer += Time.deltaTime;
		if (!(timer >= 0.5f))
		{
			transform.localScale = Vector3.Lerp(Vector3.zero, startSize, timer * 2f);
		}
		if (!(timer <= 3f))
		{
			transform.localScale = Vector3.Lerp(startSize, Vector3.zero, (timer - 3f) * 2f);
		}
		if (!(timer <= 3.5f))
		{
			UnityEngine.Object.Destroy(gameObject);
		}
	}

	public virtual void Main()
	{
	}
}
