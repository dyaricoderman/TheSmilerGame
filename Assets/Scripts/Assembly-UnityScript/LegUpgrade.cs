using System;
using UnityEngine;
using UnityScript.Lang;

[Serializable]
public class LegUpgrade : MonoBehaviour
{
	public Transform[] UpgradeLevels;

	public string UpgradeName;

	public LegUpgrade()
	{
		UpgradeName = "NotUpgradeable";
	}

	public virtual void Start()
	{
		int num = TrackUpgrade.UpgradeID(UpgradeName);
		if (num == -1)
		{
			return;
		}
		int num2 = TrackUpgrade.UpgradeCurrentLevel(UpgradeName);
		if (num2 > 0)
		{
			for (int i = 0; i < Extensions.get_length((System.Array)UpgradeLevels); i++)
			{
				if (i > num2)
				{
					UnityEngine.Object.Destroy(UpgradeLevels[i].gameObject);
				}
			}
		}
		else
		{
			UnityEngine.Object.Destroy(gameObject);
		}
	}

	public virtual void Main()
	{
	}
}
