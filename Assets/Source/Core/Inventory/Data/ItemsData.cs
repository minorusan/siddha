using UnityEngine;
using System.Collections.Generic;

using System;
using Core.Map;
using Core.Characters.AI;
using JetBrains.Annotations;

namespace Core.Inventory
{
	public static class ItemsData
	{
		#region Private

		private static Dictionary<Type, List<AConsumableBase>> _consumables = new Dictionary<Type, List<AConsumableBase>>();
		private static List<AReceiptItemBase> _receipts = new List<AReceiptItemBase>();
		private static List<AItemBase> _allItems = new List<AItemBase>();

		#endregion

		static ItemsData()
		{
			var items = new string[]{ "stressitem.id.heroin", "hungeritem.id.banana" };
			var items2 = new string[]{ "genericitem.id.nails", "genericitem.id.chain" };
			_receipts.Add(new AReceiptItemBase("receiptitem.testitem", "Ice cream", "hungeritem.id.icecream", 3f, items));
			_receipts.Add(new AReceiptItemBase("receiptitem.id.diary", "Old diary", "trapitem.id.basictrap", 5f, items2));

			var hungerConsumablesList = new List<AConsumableBase>() { 
				new HungerDecreaser("hungeritem.id.meat", "Meat", AConsumableBase.EDemand.Hunger, 20),
				new HungerDecreaser("hungeritem.id.banana", "Banana", AConsumableBase.EDemand.Hunger, 8),
				new HungerDecreaser("hungeritem.id.icecream", "Ice creame", AConsumableBase.EDemand.Hunger, 30),
                new HungerDecreaser("hungeritem.id.berries", "Berries", AConsumableBase.EDemand.Hunger, 5),
            };

			var stressConsumablesList = new List<AConsumableBase>() { 
				new StressDecreaser("stressitem.id.vodka", "Vodka", AConsumableBase.EDemand.Stress, -20),
				new StressDecreaser("stressitem.id.heroin", "Herine", AConsumableBase.EDemand.Stress, -50),
			};

			_consumables.Add(typeof(HungerDecreaser), hungerConsumablesList);
			_consumables.Add(typeof(StressDecreaser), stressConsumablesList);

			foreach(var item in _consumables)
			{
				foreach(var consumable in item.Value)
				{
					_allItems.Add(consumable);
				}
			}

			foreach(var item in _receipts)
			{
				_allItems.Add(item);
			}

			_allItems.Add(new AItemBase("genericitem.id.rope", "Rope", EItemType.Generic));
            _allItems.Add(new AItemBase("genericitem.id.emptybottle", "Empty bottle", EItemType.Generic, 5));
            _allItems.Add(new AItemBase("genericitem.id.lockpick", "Lockpick", EItemType.Generic));
            _allItems.Add(new AItemBase("genericitem.id.book", "Old book", EItemType.Generic));
			_allItems.Add(new AItemBase("genericitem.id.toybear", "Mangled teddy-bear", EItemType.Generic));
			_allItems.Add(new AItemBase("genericitem.id.nails", "Rusty nails pile", EItemType.Generic, 5));
			_allItems.Add(new AItemBase("genericitem.id.chain", "Old handcuffs", EItemType.Generic, 50));
			_allItems.Add(new AItemBase("genericitem.id.nippers", "Nippers", EItemType.Generic));

			InitialiseTraps();
		}

		public static List<AItemBase> GetItems()
		{
			return _allItems;
		}

		public static List<AConsumableBase> GetConsumablesOfType<T>() where T : AConsumableBase
		{
			return _consumables[typeof(T)];
		}

		public static AReceiptItemBase GetReceiptById(string receiptID)
		{
			return _receipts.Find(r => r.ItemID == receiptID);
		}

		public static AItemBase GetItemById(string itemID)
		{
			return _allItems.Find(r => r.ItemID == itemID);
		}

		public static TrapItemBase GetTrapById(string itemID)
		{
			return (TrapItemBase)_allItems.Find(r => r.EItemType == EItemType.Trap && r.ItemID == itemID);
		}

		private static void InitialiseTraps()
		{
			TrapAction action = (GameObject obj) =>
			{
				var blood = Resources.Load <GameObject>("Prefabs/Decorations/blood");
				var instantiatedBlood = (GameObject)GameObject.Instantiate(blood, obj.transform);
				instantiatedBlood.transform.localPosition = Vector3.zero;
				obj.GetComponent <MovableObject>().enabled = false;
				obj.GetComponent <ArtificialIntelligence>().enabled = false;
			};

			_allItems.Add(new TrapItemBase("trapitem.id.basictrap", "Iron trap", 3f, action));
		}
	}

	public static class ItemIDStorage
	{
		private static Dictionary<HungerDecreasers, string> hungerConsumablesIDs = new Dictionary<HungerDecreasers, string>();
		private static Dictionary<StressDecreasers, string> stressConsumablesIDs = new Dictionary<StressDecreasers, string>();
		private static Dictionary<string, string> _descriptionaries = new Dictionary<string, string>();

		static ItemIDStorage()
		{
			_descriptionaries.Add("genericitem.id.rope", "Just a plane rope left by somebody.");
			_descriptionaries.Add("genericitem.id.nails", "Pile of rusty nails.");
			_descriptionaries.Add("genericitem.id.chain", "Guess, that was left by a policeman.");
			_descriptionaries.Add("genericitem.id.toybear", "Guess someone was in such a rush, that forget his or her Teddy-bear...");
			_descriptionaries.Add("genericitem.id.book", "Old book, filled with knowledge and rat poops.");
			_descriptionaries.Add("receiptitem.testitem", "Looks like it teaches how to do an icecream..from heroin and bananas..");
			_descriptionaries.Add("receiptitem.id.diary", "Old diary left by some hunter. Teaches how to create a trap from chain and nails.");
			_descriptionaries.Add("trapitem.id.basictrap", "Oh boy! That thing can byte-off your whole leg!");
			_descriptionaries.Add("genericitem.id.nippers", "Still sharp");


			hungerConsumablesIDs.Add(HungerDecreasers.Meat, "hungeritem.id.meat");
			hungerConsumablesIDs.Add(HungerDecreasers.Banana, "hungeritem.id.banana");
			hungerConsumablesIDs.Add(HungerDecreasers.IceCream, "hungeritem.id.icecream");

			stressConsumablesIDs.Add(StressDecreasers.Heroin, "stressitem.id.heroin");
			stressConsumablesIDs.Add(StressDecreasers.Vodka, "stressitem.id.vodka");
		}

		public static string GetDescriptionForItem(string itemId)
		{
			string description = "";
			_descriptionaries.TryGetValue(itemId, out description);
			return string.IsNullOrEmpty(description) ? "Nothing to say about that" : description;
		}

		public static string GetHungerDecreaserID(HungerDecreasers decreaserType)
		{
			return hungerConsumablesIDs[decreaserType];
		}

		public static string GetStressDecreaserID(StressDecreasers decreaserType)
		{
			return stressConsumablesIDs[decreaserType];
		}
	}
}

