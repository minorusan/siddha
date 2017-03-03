using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;


namespace Core.Inventory.Display
{
	public class GenericUI : MonoBehaviour, IInventoryUIItem, IPointerClickHandler
	{
		private Image _selfRenderer;

		public string GenericItemID;

		#region IInventoryUIItem implementation

		public string ItemID
		{
			get
			{
				return GenericItemID;
			}
		}

		public void ToggleOutline (bool active)
		{
			_selfRenderer = GetComponent <Image> ();
			_selfRenderer.color = active ? Color.green : Color.white;
		}

		#endregion

		private void Start ()
		{
			_selfRenderer = GetComponent <Image> ();
			_selfRenderer.sprite = InventoryImagesLoader.GetImageForItem (EItemType.Generic, GenericItemID);
		}

		#region IPointerClickHandler implementation

		public void OnPointerClick (PointerEventData eventData)
		{
			var item = ItemsData.GetItemById (ItemID);
			_selfRenderer.color = Color.grey;
			item.PerformAction ();
		}

		#endregion

	}
}
