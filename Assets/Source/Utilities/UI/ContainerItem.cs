using Core.Inventory;
using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UI
{
    public class ContainerItem : MonoBehaviour, IPointerClickHandler
    {
        public event Action<AItemBase, ContainerItem> OnItemSelected;
        private AItemBase _item;

        public string Item
        {
            get
            {
                return _item.ItemID;
            }

            set
            {
                _item = ItemsData.GetItemById(value);
                GetComponent<Image>().sprite = InventoryImagesLoader.GetImageForItem(_item.EItemType, _item.ItemID);
                if (_item is StackableItemBase)
                {
                    GetComponentInChildren<Text>().text = ((StackableItemBase)_item).MaxInStack.ToString();
                }
            }
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            OnItemSelected(_item, this);
        }
    }
}