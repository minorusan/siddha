using Core.Inventory;
using System;
using System.Collections;
using System.Collections.Generic;
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
            }
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            OnItemSelected(_item, this);
        }
    }

}
