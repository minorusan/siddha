using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;


namespace  Core.Inventory.Display
{
	[RequireComponent (typeof(Image))]
	public class ConsumableUI : ConsumableDisplay, IPointerClickHandler, IInventoryUIItem
	{
		private Image _image;

		#region IInventoryUIItem implementation

		public string ItemID
		{
			get
			{
				return _selectedConsumable.ItemID;
			}
		}

		public void ToggleOutline (bool active)
		{
			_image.color = active ? Color.green : Color.white;
		}

		#endregion

		public override void ApplyImage ()
		{
			_image = GetComponent <Image> ();
			_image.sprite = InventoryImagesLoader.GetImageForItem (EItemType.Consumable, _selectedConsumable.ItemID);
		}

		public void SetItem (AConsumableBase item)
		{
			_selectedConsumable = item;
			ApplyImage ();
		}

		#region IPointerClickHandler implementation

		public void OnPointerClick (PointerEventData eventData)
		{
			AudioSource.PlayClipAtPoint (_pickup, Camera.main.transform.position);
			_selectedConsumable.PerformAction ();
			PlayerInventory.Instance.RemoveItemFromInventory (_selectedConsumable.ItemID);
		}

		#endregion
	}
}

