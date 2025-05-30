using System;
using UnityEngine;

[Serializable]
public class UpgradeClass
{
	public string Name;

	private int id;

	public string DisplayName;

	public string Description;

	public int currentLevel;

	public int MaxUnlockedLevel;

	public bool HasBeenUnlocked;

	public ShopPages ShopTypeDefault;

	public int[] CostAtLevel;

	public int MaxLevel;

	public Texture2D[] ShopImage;

	public UpgradeClass()
	{
		MaxUnlockedLevel = 4;
		ShopTypeDefault = ShopPages.TrackShop;
		CostAtLevel = new int[3];
		MaxLevel = 3;
	}

	public virtual int getID()
	{
		return id;
	}

	public virtual void setID(int ID)
	{
		id = ID;
	}
}
