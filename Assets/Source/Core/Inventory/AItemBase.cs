using UnityEngine;
using System.Collections;
using System;
using Utils.UI;


namespace Core.Inventory
{
	[Serializable]
	public class AItemBase:UnityEngine.Object
	{
        public static ItemInfoDisplayer Displayer;

		#region Private

		private readonly string _itemID;
		private readonly string _name;
        private readonly int _itemValue;
		private readonly EItemType _itemType;
		private  Action _itemAction;

		#endregion

		public string ItemID
		{
			get
			{
				return _itemID;
			}
		}

		public string Name
		{
			get
			{
				return _name;
			}
		}

		public EItemType EItemType
		{
			get
			{
				return _itemType;
			}
		}

        public int ItemValue
        {
            get
            {
                return _itemValue;
            }
        }

        public AItemBase(string itemId, string itemName, EItemType itemType, int itemValue = 10)
		{
			Debug.Assert(!string.IsNullOrEmpty(itemId), GetType().ToString() + ":: no item ID was assigned");
			SetAction(DisplayItem());
			_name = itemName;
			_itemType = itemType;
			_itemID = itemId;
		    _itemValue = itemValue;
		}

		public void PerformAction()
		{
			Debug.Assert(_itemAction != null, GetType().ToString() + ":: no item action was assigned");
			_itemAction();
		}

		protected void SetAction(Action action)
		{
			_itemAction = action;
		}

		private Action DisplayItem()
		{
			return () =>
			{
                var displayer = Displayer;
				if(displayer != null)
				{
                    displayer.gameObject.SetActive(true);
					displayer.DisplayReceiptForItem(ItemID);
				}
				else
				{
					Debug.LogError("ReceiptDisplayer was not found.");
				}
			};
		}
	}
}

