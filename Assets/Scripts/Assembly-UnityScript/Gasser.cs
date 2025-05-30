using System;
using UnityEngine;
using UnityScript.Lang;

[Serializable]
public class Gasser : MonoBehaviour
{
	public ParticleSystem[] PS;

	public float time;

	public float[] Points;

	public string UpgradeName;

	private int level;

	public Gasser()
	{
		time = 5f;
	}

	public virtual void Start()
	{
		level = TrackUpgrade.getCurrentLevel(UpgradeName);
		for (int i = 0; i < Extensions.get_length((System.Array)PS); i++)
		{
			if ((bool)PS[i] && i != level)
			{
				PS[i].Stop();
				PS[i].Clear();
			}
		}
	}

	public virtual void OnTriggerEnter(Collider other)
	{
		Cart.PointsBoost((int)Points[level]);
		other.gameObject.SendMessage("TriggerGas");
	}

	public virtual void Main()
	{
	}
}
