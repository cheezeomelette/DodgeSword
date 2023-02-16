using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemSlotType
{
	Coin,
	Item,
	COUNT
}
public class Reward
{
	public ItemSlotType type;
    public string name; 
	public int count;

	public Reward(ItemSlotType type, string name, int count)
	{
		this.type = type;
		this.name = name;
		this.count = count;
	}

}
