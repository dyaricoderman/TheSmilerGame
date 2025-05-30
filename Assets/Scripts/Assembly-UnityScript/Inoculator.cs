using System;
using UnityEngine;

[Serializable]
public class Inoculator : MonoBehaviour
{
	public float[] Points;

	public AnimationCurve InocCurve1;

	public AnimationCurve InocCurve2;

	public AnimationCurve InocCurve3;

	public string UpgradeName;

	private int level;

	public virtual void OnTriggerEnter(Collider other)
	{
		if ((bool)GetComponent<AudioSource>())
		{
			GetComponent<AudioSource>().Play();
		}
		other.gameObject.SendMessage("TriggerInoc", getCurve());
		Cart.PointsBoost((int)Points[level]);
	}

	public virtual AnimationCurve getCurve()
	{
		return (level == 1) ? InocCurve1 : ((level == 2) ? InocCurve2 : ((level != 3) ? null : InocCurve3));
	}

	public virtual void Start()
	{
		level = TrackUpgrade.getCurrentLevel(UpgradeName);
	}

	public virtual void Main()
	{
	}
}
