using Core.Inventory;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System;
using UI;
using Random = UnityEngine.Random;

namespace Core.Gameplay.Interactivity
{
    public class TradeController : MonoBehaviour
    {
        private static TradeController _instance;

        private Vendor _currentVendor;

        private GameObject[] _playerItemBars;
        private GameObject[] _vendorItemBars;

        private string[] _startStrings = {"Lets see what you've got..", "So, what was that?", "Take a look at those", "Hope u'll not dissapoint me.."};
        private string[] _dealStrings = {"Fine, I agree", "Sounds interesting", "Yeah, lets do it", "Okay. But only this time!"};
        private string[] _refuseStrings = {"No way that will happen", "Try more", "Not impressed", "You gotta be kidding.."};
        private string[] _generousStrings = {"Really? Thanks..", "You are very generous", "You either to kind or insane", "Donno what to say..thanks."};

        private int _playerOfferValue;
        private int _vendorOfferValue;

        private List<TradeItemUI> _instantiatedPlayerInventoryItems = new List<TradeItemUI>();
        private List<TradeItemUI> _instantiatedVendorItems = new List<TradeItemUI>();
        private List<AItemBase> _playerOfferItems = new List<AItemBase>();
        private List<AItemBase> _vendorOfferItems = new List<AItemBase>();

        [Header("Containers")]
        public GameObject PlayerItemsContainer;
        public GameObject VendorItemsContainer;

        [Header("Offers")]
        public GameObject PlayerOffer;
        public GameObject VendorOffer;

        public Text VendorReaction;
        public Button PerformTrade;
        public TradeItemUI TradeItemPrefab;

        private void Awake()
        {
            _instance = this;

            _playerItemBars = new GameObject[PlayerItemsContainer.transform.childCount];
            _vendorItemBars = new GameObject[VendorItemsContainer.transform.childCount];

            for (int i = 0; i < PlayerItemsContainer.transform.childCount; i++)
            {
                _playerItemBars[i] = PlayerItemsContainer.transform.GetChild(i).gameObject;
                _vendorItemBars[i] = VendorItemsContainer.transform.GetChild(i).gameObject;
            }

            gameObject.SetActive(false);
        }

        public void ShowForVendor(string vendorId)
        {
            _playerOfferValue = 0;
            _vendorOfferValue = 0;

            _vendorOfferItems.Clear();
            _playerOfferItems.Clear();

            PerformTrade.interactable = false;
            VendorReaction.text = _startStrings[Random.Range(0, _startStrings.Length)];
            
            InitPlayerItems();
            InitVendorItems(vendorId);
            gameObject.SetActive(true);
        }

        private void InitPlayerItems()
        {
            foreach (var item in _instantiatedPlayerInventoryItems)
            {
                Destroy(item.gameObject);
            }

            _instantiatedPlayerInventoryItems.Clear();
            var playerItems = PlayerInventory.Instance.GetItems();
            InstantiatePlayerInventory(playerItems);
        }

        private void InitVendorItems(string vendorID)
        {
            foreach (var item in _instantiatedVendorItems)
            {
                Destroy(item.gameObject);
            }

            _instantiatedVendorItems.Clear();
            _currentVendor = VendorsStorage.GetVendor(vendorID);
            var vendorItems = _currentVendor.Items.ToArray();
            InstantiateVendorInventory(vendorItems);
        }

        private void InstantiatePlayerInventory(AItemBase[] playerItems)
        {
            int counter = 0;
            for (int i = 0; i < playerItems.Length; i++)
            {
                var newItem = Instantiate<TradeItemUI>(TradeItemPrefab);
                newItem.transform.SetParent(_playerItemBars[counter].transform);
                newItem.InitWithItem(playerItems[i]);
                newItem.transform.localScale = Vector2.one;
                newItem.transform.localPosition = Vector2.zero;
                newItem.InventoryBarIndex = counter;
                newItem.OnItemPressed += OnPlayerInventoryItemPressed;
                _instantiatedPlayerInventoryItems.Add(newItem);

                if (i != 0 && i % 6 == 0)
                {
                    counter++;
                }
            }
        }

        private void InstantiateVendorInventory(AItemBase[] vendorItems)
        {
            int counter = 0;
            for (int i = 0; i < vendorItems.Length; i++)
            {
                var newItem = Instantiate<TradeItemUI>(TradeItemPrefab);
                newItem.transform.SetParent(_vendorItemBars[counter].transform);
                newItem.InitWithItem(vendorItems[i]);
                newItem.transform.localScale = Vector2.one;
                newItem.transform.localPosition = Vector2.zero;

                newItem.InventoryBarIndex = counter;
                newItem.OnItemPressed += OnVendorInventoryItemPressed;
                _instantiatedVendorItems.Add(newItem);

                if (i != 0 && i % 6 == 0)
                {
                    counter++;
                }
            }
        }

        #region Events

        private void OnVendorInventoryItemPressed(TradeItemUI tradeItemUi)
        {
            tradeItemUi.transform.SetParent(VendorOffer.transform);
            _vendorOfferValue += tradeItemUi.Item.ItemValue;
            _vendorOfferItems.Add(tradeItemUi.Item);
            tradeItemUi.OnItemPressed -= OnVendorInventoryItemPressed;
            tradeItemUi.OnItemPressed += OnVendorOfferItemPressed;

            CheckReaction();
        }

        private void OnVendorOfferItemPressed(TradeItemUI tradeItemUi)
        {
            _vendorOfferValue -= tradeItemUi.Item.ItemValue;
            tradeItemUi.OnItemPressed -= OnVendorOfferItemPressed;
            _vendorOfferItems.Remove(tradeItemUi.Item);
            tradeItemUi.OnItemPressed += OnVendorInventoryItemPressed;
            tradeItemUi.transform.SetParent(_vendorItemBars[tradeItemUi.InventoryBarIndex].transform);

            CheckReaction();
        }

        private void OnPlayerInventoryItemPressed(TradeItemUI tradeItemUi)
        {
            tradeItemUi.transform.SetParent(PlayerOffer.transform);
            _playerOfferValue += tradeItemUi.Item.ItemValue;
            _playerOfferItems.Add(tradeItemUi.Item);
            tradeItemUi.OnItemPressed -= OnPlayerInventoryItemPressed;
            tradeItemUi.OnItemPressed += OnPlayerOfferItemPressed;

            CheckReaction();
        }

        private void OnPlayerOfferItemPressed(TradeItemUI tradeItemUi)
        {
            _playerOfferValue -= tradeItemUi.Item.ItemValue;
            _playerOfferItems.Remove(tradeItemUi.Item);
            tradeItemUi.OnItemPressed -= OnPlayerOfferItemPressed;
            tradeItemUi.OnItemPressed += OnPlayerInventoryItemPressed;
            tradeItemUi.transform.SetParent(_playerItemBars[tradeItemUi.InventoryBarIndex].transform);

            CheckReaction();
        }

        #endregion

        private void CheckReaction()
        {
            if (_playerOfferValue == _vendorOfferValue)
            {
                VendorReaction.text = _dealStrings[Random.Range(0, _dealStrings.Length)];
            }

            if (_playerOfferValue > _vendorOfferValue)
            {
                VendorReaction.text = _generousStrings[Random.Range(0, _generousStrings.Length)];
            }

            if (_playerOfferValue < _vendorOfferValue)
            {
                VendorReaction.text = _refuseStrings[Random.Range(0, _refuseStrings.Length)];
            }

            PerformTrade.interactable = _playerOfferValue >= _vendorOfferValue;
            
        }

        public void OnTrade()
        {
            foreach (var playerOfferItem in _playerOfferItems)
            {
                PlayerInventory.Instance.RemoveItemFromInventory(playerOfferItem.ItemID);
                _currentVendor.AddItem(playerOfferItem.ItemID);
            }

            foreach (var vendorOfferItem in _vendorOfferItems)
            {
                PlayerInventory.Instance.TryAddItemToInventory(vendorOfferItem);
                _currentVendor.RemoveItem(vendorOfferItem.ItemID);
            }

            gameObject.SetActive(false);
        }

        public static void ShowTradeForVendor(string vendorId)
        {
            _instance.ShowForVendor(vendorId);
        }
    }
}