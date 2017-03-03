using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Utilities.UI;
using Core.Inventory;
using System.Linq;
using Core.Utilities;
using Core.Characters.Player;
using UnityEngine.Events;


namespace Utils.UI
{
	public class ItemInfoDisplayer : MonoBehaviour, IPointerClickHandler
	{
		#region Private

		private const string kItemDisplaySpritesPath = "Sprites/Items/Display/";
		private Image _displayImage;
	    private Image _actionButtonImage;
		private AItemBase _currentItem;
		private Text _descriptionText;

		#endregion

	    public Sprite[] ButtonImages;
	    public Button ActionButton;
	    public Button DeleteButton;
	    public Image DisplayImage;
		public Text ItemName;

		#region Monobehaviour

		public void Start ()
		{
		    _displayImage = DisplayImage;
			_descriptionText = GetComponentInChildren <Text> ();

		    _actionButtonImage = ActionButton.GetComponent<Image>();
            AItemBase.Displayer = this;
			gameObject.SetActive(false);
		}

		public void Reset ()
		{
			_currentItem = null;
			var items = Inventrory.GetInventoryItems ();
		
			foreach (var item in items) {
				item.ToggleOutline (false);
			}
		}

		#endregion

		public void DisplayReceiptForItem (string itemId)
		{
			var image = Resources.Load <Sprite> (string.Format (kItemDisplaySpritesPath + itemId));
			_currentItem = ItemsData.GetItemById (itemId);

			_displayImage.gameObject.SetActive (true);
			_displayImage.sprite = image;

			_descriptionText.gameObject.SetActive (true);
			_descriptionText.text = ItemIDStorage.GetDescriptionForItem (itemId);

			ItemName.gameObject.SetActive (true);
			ItemName.text = _currentItem.Name;
			DeleteButton.gameObject.SetActive (true);
			DehighlightAllItems ();

			var requiredItemsCount = 0;

			switch (_currentItem.EItemType) {
			case EItemType.Receipt:
				{
					var receipt = ItemsData.GetReceiptById (itemId);
				    _actionButtonImage.sprite = ButtonImages[0];
				    ActionButton.onClick.AddListener(delegate { CraftItem(); });
                        HighlightItems (receipt);
					for (int i = 0; i < receipt.RequiredItems.Length; i++) {
						if (Inventrory.GetInventoryItems ().Any (item => item.ItemID == receipt.RequiredItems [i])) {
							requiredItemsCount++;
						}
					}
                        ActionButton.gameObject.SetActive(true);
                        ActionButton.interactable = receipt.RequiredItems.Length <= requiredItemsCount;
					break;
				}
			case EItemType.Trap:
				{
                        _actionButtonImage.sprite = ButtonImages[1];
                        ActionButton.gameObject.SetActive(true);
                        ActionButton.onClick.AddListener(delegate { SetATrap(); });
                        break;
				}

			default:
				{
					ActionButton.gameObject.SetActive (false);
					break;
				}
			}
		}

		public void SetATrap ()
		{
			var trap = ItemsData.GetTrapById (_currentItem.ItemID);
			PlayerInventory.Instance.RemoveItemFromInventory (trap.ItemID);

			BeginSettingTrapWithCallback ();
			_displayImage.gameObject.SetActive (false);
			_descriptionText.gameObject.SetActive (false);

			ActionButton.gameObject.SetActive (false);
			DeleteButton.gameObject.SetActive (false);
		}

		public void CraftItem ()
		{
			var receipt = ItemsData.GetReceiptById (_currentItem.ItemID);
			for (int i = 0; i < receipt.RequiredItems.Length; i++) {
				PlayerInventory.Instance.RemoveItemFromInventory (receipt.RequiredItems [i]);
			}

			_displayImage.gameObject.SetActive (false);
			ActionButton.gameObject.SetActive (false);
			DeleteButton.gameObject.SetActive (false);
			_descriptionText.gameObject.SetActive (false);
			BeginCraftingWithCallback ();
		}

		private void HighlightItems (AReceiptItemBase receipt)
		{
			foreach (var item in Inventrory.GetInventoryItems ()) {
				if (receipt.RequiredItems.Any (i => i == item.ItemID)) {
					item.ToggleOutline (true);
				} else {
					item.ToggleOutline (false);
				}
			}
		}

		private void BeginSettingTrapWithCallback ()
		{
			var trap = ItemsData.GetTrapById (_currentItem.ItemID);
			var sprite = InventoryImagesLoader.GetImageForItem (trap.EItemType, trap.ItemID);
			ProcessBarController.StartProcessWithCompletion (trap.RequiredTime, sprite, () => {
				var instantiatedTrap = Instantiate (trap.TrapPrefab);
				instantiatedTrap.transform.position = PlayerBehaviour.CurrentPlayer.transform.position;
			}, Color.red);
		}

		private void BeginCraftingWithCallback ()
		{
			var receipt = ItemsData.GetReceiptById (_currentItem.ItemID);
			var item = ItemsData.GetItems ().Find (i => i.ItemID == receipt.ResultingItemId);
			var sprite = InventoryImagesLoader.GetImageForItem (item.EItemType, item.ItemID);
			ProcessBarController.StartProcessWithCompletion (receipt.RequiredTime, sprite, () => {
				PlayerInventory.Instance.TryAddItemToInventory (item);
			}, Color.cyan);
		}


		#region IPointerClickHandler implementation

		public void OnPointerClick (PointerEventData eventData)
		{
			_displayImage.gameObject.SetActive (false);
	
			DehighlightAllItems ();
		}

		public void OnDeleteButton ()
		{
			PlayerInventory.Instance.RemoveItemFromInventory (_currentItem.ItemID);
			_displayImage.gameObject.SetActive (false);
			DeleteButton.gameObject.SetActive (false);
			_descriptionText.gameObject.SetActive (false);
			DehighlightAllItems ();
		}

		private void DehighlightAllItems ()
		{
			var items = Inventrory.GetInventoryItems ();

			var itemsExceptCurrent = items.Where (i => i != null && i.ItemID != _currentItem.ItemID).ToList ();
			foreach (var item in itemsExceptCurrent) {
				item.ToggleOutline (false);
			}
		}

		#endregion
	}
}

