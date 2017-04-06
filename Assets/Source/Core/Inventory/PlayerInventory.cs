using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Core.Inventory;
using System;
using Utilities.UI;
using Core.Utilities.UI;
using Core.Gameplay.Interactivity;
using Core.Interactivity;
using Core.ObjectPooling;
using Core.Interactivity.Combat;

namespace Core.Inventory
{
    public class ProjectilesInfo
    {
        public ProjectileBase Projectile;
        public Sprite Image;
        public int CurrentCount;
        public AItemBase ItemReference;
    }
    public class PlayerInventory
    {

        public const int kMaxInventoryCapacity = 27;
        private List<AItemBase> _items;
        private Dictionary<EProjecileID, ProjectilesInfo> _projectiles = new Dictionary<EProjecileID, ProjectilesInfo>();
        private static PlayerInventory _instance;
        private AudioClip _sound;

        public event Action InventoryChanged;

        public static PlayerInventory Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new PlayerInventory();
                }
                return _instance;
            }
        }

        public bool HasProjectiles
        {
            get
            { return GetProjectiles().Length > 0; }
        }

        private PlayerInventory()
        {
            _items = new List<AItemBase>();
            _sound = Resources.Load<AudioClip>("Sounds/item");
        }

        public bool TryAddItemToInventory(AItemBase item)
        {
            if (_items.Count < kMaxInventoryCapacity)
            {
                _items.Add(item);
                HandleStacableItem(item);
                //ShowDialogueForItem(item);
                AudioSource.PlayClipAtPoint(_sound, Camera.main.transform.position, 0.01f);
                if (InventoryChanged != null)
                {
                    InventoryChanged();
                }

                return true;
            }
            return false;
        }

        private void HandleStacableItem(AItemBase item)
        {
            
            if (item is StackableItemBase)
            {
                var projectile = ((StackableItemBase)item).ProjectilePrefab.GetComponent<ProjectileBase>();
                ProjectilesInfo projectileInfo;
                _projectiles.TryGetValue(projectile.ID, out projectileInfo);
                if (projectileInfo == null)
                {
                    _projectiles.Add(projectile.ID, new ProjectilesInfo { ItemReference = item, Projectile = projectile, Image = InventoryImagesLoader.GetImageForItem(item.EItemType, item.ItemID), CurrentCount = ((StackableItemBase)item).MaxInStack });

                    if (GetProjectiles().Length <= 1)
                    {
                        ThrowController.Instance.SetProjectile(GetProjectiles()[0]);
                    }
                }
                else
                {
                    projectileInfo.CurrentCount += ((StackableItemBase)item).MaxInStack;
                    projectileInfo.CurrentCount = Mathf.Clamp(projectileInfo.CurrentCount, 0, ((StackableItemBase)item).MaxInStack);
                }
                ThrowController.Instance.UpdateInfo();
            }
        }

        public void RemoveItemFromInventory(string item)
        {
            var index = _items.FindIndex(i => i.ItemID == item);
            _items.RemoveAt(index);
            Debug.Log(item + " was removed from inventory.");
            if (InventoryChanged != null)
            {
                InventoryChanged();
            }
            
        }

        public AItemBase[] GetItems()
        {    
            return _items.ToArray();
        }

        public ProjectilesInfo[] GetProjectiles()
        {
            var projectilesInfos = new ProjectilesInfo[_projectiles.Count];
            int i = 0;
            foreach (var item in _projectiles.Values)
            {
                projectilesInfos[i] = item;
                i++;
            }
            return projectilesInfos;
        }

        static void ShowDialogueForItem(AItemBase item)
        {
            switch (item.EItemType)
            {
                case EItemType.Generic:
                    {
                        Tutorial.ShowForIDIfNeeded(ETutorialId.GenericItemPicked);
                        break;
                    }
                case EItemType.Consumable:
                    {
                        if (item is HungerDecreaser)
                        {
                            Tutorial.ShowForIDIfNeeded(ETutorialId.HungerItemPicked);
                        }
                        else
                        if (item is StressDecreaser)
                        {
                            Tutorial.ShowForIDIfNeeded(ETutorialId.StressItemPicked);
                        }
                        break;
                    }
                case EItemType.Receipt:
                    {
                        Tutorial.ShowForIDIfNeeded(ETutorialId.CraftItemPicked);
                        break;
                    }
                default:
                    break;
            }
        }

        public AItemBase TryGetItemAtIndex(int index)
        {
            if (index < _items.Count)
            {
                return _items[index];
            }
            return null;
        }
    }
}

