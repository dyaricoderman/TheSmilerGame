using System;
using UnityEngine;

[Serializable]
public class Tickler : MonoBehaviour
{
	public float time;

	public float[] Points;

	public string UpgradeName;

	private int level;

	public Tickler()
	{
		time = 5f;
	}

	public virtual void Start()
	{
		level = TrackUpgrade.getCurrentLevel(UpgradeName);
	}

	public virtual void OnTriggerEnter(Collider other)
	{
		if ((bool)GetComponent<AudioSource>())
		{
			GetComponent<AudioSource>().Play();
		}
		Cart.PointsBoost((int)Points[level]);
		other.gameObject.SendMessage("TriggerTickler");
	}

	public virtual void Main()
	{
	}
}
