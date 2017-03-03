using UnityEngine;
using System.Collections;
using System;
using Utils.UI;


namespace Core.Inventory
{
	public class AReceiptItemBase : AItemBase
	{
		private readonly string[] _requiredItemsIdentifiers;
		private readonly float _requiredTime;
		public readonly string _resultItemId;

		public float RequiredTime
		{
			get
			{
				return _requiredTime;
			}
		}

		public string ResultingItemId
		{
			get
			{
				return _resultItemId;
			}
		}

		public string[] RequiredItems
		{
			get
			{
				return (string[])_requiredItemsIdentifiers.Clone();
			}
		}

		public AReceiptItemBase(string itemId, string name, string resultItemId, float requiredTime, string[] requiredItemsIdentifiers) : base(itemId, name, EItemType.Receipt)
		{
			_requiredItemsIdentifiers = requiredItemsIdentifiers;
			_resultItemId = resultItemId;
			_requiredTime = requiredTime;
			SetAction(DisplayReceipt());
		}

		private Action DisplayReceipt()
		{
			return () =>
			{
				var receiptDisplayer = FindObjectOfType<ItemInfoDisplayer>();
				if(receiptDisplayer != null)
				{
					receiptDisplayer.DisplayReceiptForItem(ItemID);
				}
				else
				{
					Debug.LogError("ReceiptDisplayer was not found.");
				}
			};
		}
	}
}

