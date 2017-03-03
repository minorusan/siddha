using UnityEngine;

using Core.Inventory;
using Core.Inventory.Display;

using System.Collections.Generic;


namespace Utilities.UI
{
	public class Inventrory : MonoBehaviour
	{
		#region Private

		private int maxItemsPerRow = 5;
		private Transform[] _rows;
		private static List<IInventoryUIItem> _uiitems = new List<IInventoryUIItem> ();
		private List<GameObject> _instantiatedObjects = new List<GameObject> ();

		#endregion

		public GameObject ConsumablePrefab;
		public GameObject ReceiptPrefab;
		public GameObject GenericPrefab;

		#region Monobehaviour

		private void Start ()
		{
			InitRows ();
			OnPlayerInventoryChanged ();
		}
		private void OnDestroy ()
		{
			PlayerInventory.Instance.InventoryChanged -= OnPlayerInventoryChanged;
		}

		#endregion

		#region Internal

		private void OnPlayerInventoryChanged ()
		{
			ClearInventorylist ();
			var items = PlayerInventory.Instance.GetItems ();
			var rowIndex = 0;

			for (int i = 0; i < items.Length; i++)
			{
				if (_rows [rowIndex].childCount < maxItemsPerRow)
				{
					InstantiateAtRowIndex (rowIndex, items [i]);
				}
				else
				{
					rowIndex++;
					InstantiateAtRowIndex (rowIndex, items [i]);
				}
			}
		}

		private void ClearInventorylist ()
		{
			_uiitems.Clear ();
			foreach (var prefab in _instantiatedObjects)
			{
				Destroy (prefab);
			}

			for (int i = 0; i < _rows.Length; i++)
			{
				_rows [i].DetachChildren ();
			}

			_instantiatedObjects.Clear ();
		}

		private void InstantiateAtRowIndex (int rowIndex, AItemBase item)
		{
			GameObject prefab;
			switch (item.EItemType)
			{
			case EItemType.Consumable:
				{
					prefab = InstantiateConsumable (item);
					break;
				}
			case EItemType.Receipt:
				{
					prefab = InstantiateReceipt (item);
					break;
				}
			default:
				{
					prefab = InstantiateGeneric (item);
				}

				break;
			}

			prefab.transform.SetParent (_rows [rowIndex].transform);
			prefab.transform.localPosition = Vector3.zero;
			prefab.transform.localScale = Vector3.one;
			_instantiatedObjects.Add (prefab);
		}

		private GameObject InstantiateConsumable (AItemBase item)
		{
			var prefab = Instantiate (ConsumablePrefab);
			prefab.GetComponent <ConsumableUI> ().SetItem ((AConsumableBase)item);
			_uiitems.Add (prefab.GetComponent <ConsumableUI> ());
			return prefab;
		}

		private GameObject InstantiateGeneric (AItemBase item)
		{
			var prefab = Instantiate (GenericPrefab);
			prefab.GetComponent <GenericUI> ().GenericItemID = item.ItemID;
			_uiitems.Add (prefab.GetComponent <GenericUI> ());
			return prefab;
		}

		private GameObject InstantiateReceipt (AItemBase item)
		{
			var prefab = Instantiate (ReceiptPrefab);
			prefab.GetComponent <ReceiptUI> ().ReceiptId = item.ItemID;
			_uiitems.Add (prefab.GetComponent <ReceiptUI> ());
			return prefab;
		}

		private void InitRows ()
		{
			_rows = new RectTransform[transform.childCount];
			for (int i = 0; i < transform.childCount; i++)
			{
				_rows [i] = transform.GetChild (i);
			}
			
			PlayerInventory.Instance.InventoryChanged += OnPlayerInventoryChanged;
			transform.parent.gameObject.SetActive (false);
		}

		public static List<IInventoryUIItem> GetInventoryItems ()
		{
			return _uiitems;
		}

		#endregion
	}
}

