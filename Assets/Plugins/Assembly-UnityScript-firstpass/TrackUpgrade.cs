using System;
using UnityEngine;
using UnityScript.Lang;

[Serializable]
public class TrackUpgrade : MonoBehaviour
{
	[NonSerialized]
	public static TrackUpgrade inst;

	public UpgradeClass[] InitalUpgradesList;

	[NonSerialized]
	public static UpgradeClass[] UpgradesList;

	[NonSerialized]
	public static int UpgradeCredits;

	public static int GetCredits()
	{
		return UpgradeCredits;
	}

	public static void Upgrade(string TU)
	{
		Upgrade(UpgradeID(TU));
	}

	public static bool CanAfford(string name)
	{
		return (GetCredits() >= UpgradeCost(UpgradeID(name))) ? true : false;
	}

	public static void SpendCredits(int num)
	{
		UpgradeCredits -= num;
		SaveUpgrades();
	}

	public static float UpgradeProgress()
	{
		float num = 0f;
		float num2 = 0f;
		for (int i = 0; i < Extensions.get_length((System.Array)UpgradesList); i++)
		{
			num += (float)MaxLevel(i);
			num2 += (float)getCurrentLevel(i);
		}
		return num2 / num;
	}

	public static void Upgrade(int id)
	{
		UpgradeCredits -= UpgradeCost(id);
		UpgradesList[id].currentLevel = UpgradesList[id].currentLevel + 1;
		UpgradesList[id].currentLevel = Mathf.Min(UpgradesList[id].currentLevel, MaxLevel(id));
		if (UpgradesList[id].currentLevel == UpgradesList[id].MaxLevel)
		{
			UpgradesList[id].HasBeenUnlocked = true;
		}
		SaveUpgrades();
	}

	public static void AddCredits(int creds)
	{
		UpgradeCredits += creds;
		SaveUpgrades();
	}

	public static int MaxLevel(string name)
	{
		return MaxLevel(UpgradeID(name));
	}

	public static int getCurrentLevel(string name)
	{
		return getCurrentLevel(UpgradeID(name));
	}

	public static int getCurrentLevel(int id)
	{
		return UpgradesList[id].currentLevel;
	}

	public static string getDescription(int id)
	{
		return UpgradesList[id].Description;
	}

	public static bool isUnlockedAtLevel(int id, int level)
	{
		return (UpgradesList[id].currentLevel == level) ? true : false;
	}

	public static int MaxLevel(int id)
	{
		return (!IsUnlocked(id)) ? Mathf.Min(UpgradesList[id].MaxLevel, UpgradesList[id].MaxUnlockedLevel) : UpgradesList[id].MaxLevel;
	}

	public static bool isUpgradeType(string name, ShopPages SP)
	{
		return isUpgradeType(UpgradeID(name), SP);
	}

	public static int TotalUpgrades()
	{
		return UpgradesList.Length;
	}

	public static UpgradeClass[] GetShopPage(ShopPages SP)
	{
		UpgradeClass[] array = new UpgradeClass[ShopPageCount(SP)];
		int num = 0;
		for (int i = 0; i < Extensions.get_length((System.Array)UpgradesList); i++)
		{
			if (isUpgradeType(i, SP))
			{
				array[num] = UpgradesList[i];
				num++;
			}
		}
		return array;
	}

	public static bool IsMaxLevel(int id)
	{
		return (MaxLevel(id) == UpgradesList[id].currentLevel) ? true : false;
	}

	public static int ShopPageCount(ShopPages SP)
	{
		int num = 0;
		for (int i = 0; i < Extensions.get_length((System.Array)UpgradesList); i++)
		{
			if (isUpgradeType(i, SP))
			{
				num++;
			}
		}
		return num;
	}

	public static int ShopPageIdToUpgradeID(ShopPages SP, int j)
	{
		int num = 0;
		int num2 = 0;
		int result;
		while (true)
		{
			if (num2 < Extensions.get_length((System.Array)UpgradesList))
			{
				if (isUpgradeType(num2, SP))
				{
					num++;
					if (j == num)
					{
						result = num2;
						break;
					}
				}
				num2++;
				continue;
			}
			result = -1;
			break;
		}
		return result;
	}

	public static bool IsUnlocked(int id)
	{
		return UpgradesList[id].HasBeenUnlocked;
	}

	public static string getName(int id)
	{
		return UpgradesList[id].Name;
	}

	public static string getDisplayName(int id)
	{
		return UpgradesList[id].DisplayName;
	}

	public virtual void LoadUpgrades()
	{
		UpgradesList = InitalUpgradesList;
		int[] intArray = PlayerPrefsX.GetIntArray("TrackUpgrades");
		for (int i = 0; i < Extensions.get_length((System.Array)UpgradesList) && i < Extensions.get_length((System.Array)intArray); i++)
		{
			UpgradesList[i].currentLevel = intArray[i];
		}
		bool[] boolArray = PlayerPrefsX.GetBoolArray("UnlockedUpgrades");
		for (int i = 0; i < Extensions.get_length((System.Array)UpgradesList) && i < Extensions.get_length((System.Array)intArray); i++)
		{
			UpgradesList[i].HasBeenUnlocked = boolArray[i];
		}
		UpgradeCredits = PlayerPrefs.GetInt("UpgradeCredits");
	}

	public static void SaveUpgrades()
	{
		int[] array = new int[Extensions.get_length((System.Array)UpgradesList)];
		for (int i = 0; i < Extensions.get_length((System.Array)array); i++)
		{
			array[i] = UpgradesList[i].currentLevel;
		}
		bool[] array2 = new bool[Extensions.get_length((System.Array)UpgradesList)];
		for (int i = 0; i < Extensions.get_length((System.Array)array2); i++)
		{
			array2[i] = UpgradesList[i].HasBeenUnlocked;
		}
		PlayerPrefs.SetInt("UpgradeCredits", UpgradeCredits);
		PlayerPrefsX.SetIntArray("TrackUpgrades", array);
		PlayerPrefsX.SetBoolArray("UnlockedUpgrades", array2);
	}

	public static bool isUpgradeType(int id, ShopPages SP)
	{
		return (SP == ShopPages.TopSecret || MaxLevel(id) != 0) && ((UpgradesList[id].MaxUnlockedLevel > UpgradesList[id].MaxLevel) ? ((SP == UpgradesList[id].ShopTypeDefault) ? true : false) : (SP == ShopPages.TopSecret || ((SP == UpgradesList[id].ShopTypeDefault) ? true : false)));
	}

	public static int UpgradeCost(string name)
	{
		return UpgradeCost(UpgradeID(name));
	}

	public static int UpgradeCost(int id)
	{
		return UpgradesList[id].CostAtLevel[UpgradesList[id].currentLevel];
	}

	public static void UnlockUpgrade(string name)
	{
		UpgradesList[UpgradeID(name)].HasBeenUnlocked = true;
		SaveUpgrades();
	}

	public static int UpgradeID(string name)
	{
		int num = 0;
		int result;
		while (true)
		{
			if (num < Extensions.get_length((System.Array)UpgradesList))
			{
				if (UpgradesList[num].Name == name)
				{
					result = num;
					break;
				}
				num++;
				continue;
			}
			result = -1;
			break;
		}
		return result;
	}

	public static int UpgradeCurrentLevel(string name)
	{
		return UpgradesList[UpgradeID(name)].currentLevel;
	}

	public static bool SaveExists()
	{
		return (PlayerPrefs.HasKey("UpgradeCredits") && PlayerPrefs.HasKey("TrackUpgrades")) ? true : false;
	}

	public virtual void Die()
	{
		UnityEngine.Object.Destroy(this);
	}

	public virtual void Awake()
	{
		if ((bool)inst)
		{
			UnityEngine.Object.Destroy(gameObject);
			return;
		}
		inst = this;
		UnityEngine.Object.DontDestroyOnLoad(gameObject);
		if (UpgradesList == null)
		{
			if (SaveExists())
			{
				LoadUpgrades();
			}
			else
			{
				UpgradesList = InitalUpgradesList;
			}
		}
		AssignIDs();
	}

	public static void AssignIDs()
	{
		for (int i = 0; i < Extensions.get_length((System.Array)UpgradesList); i++)
		{
			UpgradesList[i].setID(i);
		}
	}

	public static string GetDisplayName(int id)
	{
		return UpgradesList[id].DisplayName;
	}

	public virtual void Main()
	{
	}
}
