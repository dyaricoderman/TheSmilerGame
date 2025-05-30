using System;
using UnityEngine;

[Serializable]
public class Hypno : MonoBehaviour
{
	public float[] Points;

	public AnimationCurve HypnoCurve1;

	public AnimationCurve HypnoCurve2;

	public AnimationCurve HypnoCurve3;

	public string UpgradeName;

	private int level;

	public virtual void OnTriggerEnter(Collider other)
	{
		if ((bool)GetComponent<AudioSource>())
		{
			GetComponent<AudioSource>().Play();
		}
		other.gameObject.SendMessage("TriggerHypno", getCurve());
		Cart.PointsBoost((int)Points[level]);
	}

	public virtual AnimationCurve getCurve()
	{
		return (level == 1) ? HypnoCurve1 : ((level == 2) ? HypnoCurve2 : ((level != 3) ? null : HypnoCurve3));
	}

	public virtual void Start()
	{
		level = TrackUpgrade.getCurrentLevel(UpgradeName);
	}

	public virtual void Main()
	{
	}
}
