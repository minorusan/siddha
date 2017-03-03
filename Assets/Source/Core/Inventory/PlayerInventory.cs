using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Core.Inventory;
using System;
using Utilities.UI;
using Core.Utilities.UI;
using Core.Gameplay.Interactivity;
using Core.Interactivity;


namespace Core.Inventory
{

	public class PlayerInventory
	{
		public const int kMaxInventoryCapacity = 27;
		private List<AItemBase> _items;
		private static PlayerInventory _instance;
	    private AudioClip _sound;

		public event Action InventoryChanged;

		public static PlayerInventory Instance
		{
			get
			{
				if(_instance == null)
				{
					_instance = new PlayerInventory();
				}
				return _instance;
			}
		}

		private PlayerInventory()
		{
			_items = new List<AItemBase>();
		    _sound = Resources.Load<AudioClip>("Sounds/item");
		}

		public bool TryAddItemToInventory(AItemBase item)
		{
			if(_items.Count < kMaxInventoryCapacity)
			{
				_items.Add(item);

                //ShowDialogueForItem(item);
                AudioSource.PlayClipAtPoint(_sound, Camera.main.transform.position, 0.01f);
				InventoryChanged();
				return true;
			}
			return false;
		}

		public void RemoveItemFromInventory(string item)
		{
			var index = _items.FindIndex(i => i.ItemID == item);
			_items.RemoveAt(index);
			Debug.Log(item + " was removed from inventory.");
			InventoryChanged();
		}

		public AItemBase[] GetItems()
		{
			return _items.ToArray();
		}

		static void ShowDialogueForItem(AItemBase item)
		{
			switch(item.EItemType)
			{
			case EItemType.Generic:
				{
					Tutorial.ShowForIDIfNeeded(ETutorialId.GenericItemPicked);
					break;
				}
			case EItemType.Consumable:
				{
					if(item is HungerDecreaser)
					{
						Tutorial.ShowForIDIfNeeded(ETutorialId.HungerItemPicked);
					}
					else
					if(item is StressDecreaser)
					{
						Tutorial.ShowForIDIfNeeded(ETutorialId.StressItemPicked);
					}
					break;
				}
			case EItemType.Receipt:
				{
					Tutorial.ShowForIDIfNeeded(ETutorialId.CraftItemPicked);
					break;
				}
			default:
				break;
			}
		}

		public AItemBase TryGetItemAtIndex(int index)
		{
			if(index < _items.Count)
			{
				return _items[index];
			}
			return null;
		}
	}
}

