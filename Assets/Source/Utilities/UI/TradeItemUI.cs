using System;
using Core.Inventory;

using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UI
{
    [RequireComponent(typeof(Image))]
    public class TradeItemUI : MonoBehaviour, IPointerClickHandler
    {
        private Image _image;
        public event Action<TradeItemUI> OnItemPressed;
        private AItemBase _item;
        public int InventoryBarIndex;

        public AItemBase Item
        {
            get
            {
                return _item;
            }
        }

        private void OnEnable()
        {
            _image = GetComponent<Image>();
        }

        public void InitWithItem(AItemBase item)
        {
            _item = item;
            _image.sprite = InventoryImagesLoader.GetImageForItem(item.EItemType, item.ItemID);
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (OnItemPressed != null) OnItemPressed(this);
        }
    }

}
