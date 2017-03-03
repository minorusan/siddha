using Core.Inventory;
using System.Collections.Generic;
using System.Linq;

namespace Core.Gameplay.Interactivity
{
    public class Vendor
    {
        private readonly string _vendorid;
        private readonly List<string> _itemIds;
        private readonly EItemType _demandedType;
        private List<AItemBase> _items = new List<AItemBase>();

        public string Vendorid
        {
            get
            {
                return _vendorid;
            }
        }

        public List<AItemBase> Items
        {
            get
            {
                return _items;
            }
        }

        public Vendor(string _vendorid, string[] _itemIds, EItemType _demandedType)
        {
            this._vendorid = _vendorid;
            this._itemIds = _itemIds.ToList();
            this._demandedType = _demandedType;
            InitItems();
        }

        public void RemoveItem(string id)
        {
            _items.Remove(_items.First(item => item.ItemID == id));
            _items.ToString();
        }

        public void AddItem(string id)
        {
            _items.Add(ItemsData.GetItemById(id));
        }

        private void InitItems()
        {
            _items.Clear();
            foreach (var itemId in _itemIds)
            {
                _items.Add(ItemsData.GetItemById(itemId));
            }
        }
    }


    internal static class VendorsStorage
    {
        private static List<Vendor> _vendors = new List<Vendor>();

        static VendorsStorage()
        {
            _vendors.Add(new Vendor("vendor.id.sara", new[]{ 
            "hungeritem.id.banana","genericitem.id.chain","genericitem.id.book","genericitem.id.nails","genericitem.id.lockpick"}, EItemType.Trap));
        }

        public static Vendor GetVendor(string vendorId)
        {
            return _vendors.First(v=>v.Vendorid == vendorId);
        }
    }
}