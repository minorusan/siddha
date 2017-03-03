using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;


namespace Core.Inventory.Display
{
	public interface IInventoryUIItem
	{
		string ItemID
		{
			get;
		}

		void ToggleOutline (bool active);
	}


	[RequireComponent (typeof(Image))]
	public class ReceiptUI : MonoBehaviour, IPointerClickHandler, IInventoryUIItem
	{
		private Image _selfRenderer;

		public string ReceiptId;

		#region IInventoryUIItem implementation

		public string ItemID
		{
			get
			{
				return ReceiptId;
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
			_selfRenderer.sprite = InventoryImagesLoader.GetImageForItem (EItemType.Receipt, ReceiptId);
		}

		#region IPointerClickHandler implementation

		public void OnPointerClick (PointerEventData eventData)
		{
			var item = ItemsData.GetReceiptById (ReceiptId);
			_selfRenderer.color = Color.grey;
			item.PerformAction ();
		}

		#endregion
	}
}
