using UnityEngine;
using System.Collections;
using Core.Characters.Player.Demand;


namespace Core.Inventory
{
	public enum HungerDecreasers
	{
		Meat,
		Banana,
		IceCream
	}

	public enum StressDecreasers
	{
		Vodka,
		Heroin
	}

	public class HungerDecreaser : AConsumableBase
	{
		public HungerDecreaser(string itemId, string name, EDemand affector, int effectValue) : base(itemId, name, affector, effectValue)
		{
		}
	}

	public class StressDecreaser : AConsumableBase
	{
		public StressDecreaser(string itemId, string name, EDemand affector, int effectValue) : base(itemId, name, affector, effectValue)
		{
		}
	}
}

