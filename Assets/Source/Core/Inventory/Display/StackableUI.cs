using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Core.Interactivity.Combat;
using Core.ObjectPooling;
using System.Threading;

namespace Core.Inventory.Display
{
	public class StackableUI : MonoBehaviour, IInventoryUIItem, IPointerClickHandler
	{
		private Image _selfRenderer;
        private Text _quantity;
        private StackableItemBase _item;

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
            _quantity = GetComponent<Text>();
			_selfRenderer.sprite = InventoryImagesLoader.GetImageForItem (EItemType.Generic, GenericItemID);
		}

        private void OnEnable()
        {
            _item = (StackableItemBase)ItemsData.GetItemById(ItemID);
            _quantity.text = _item.MaxInStack.ToString();
        }

        #region IPointerClickHandler implementation

        public void OnPointerClick (PointerEventData eventData)
		{
			_selfRenderer.color = Color.grey;
            ThrowController.Instance.SetProjectile(_item.ProjectilePrefab.GetComponent<ProjectileBase>().ID);
		}

		#endregion

	}
}
