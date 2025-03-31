using System;
using System.Collections.Generic;
using Data;
using Data.Item;
using Data.Progress;
using Infrastructure;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Services
{
    public class ItemsPoolService : IGameService
    {
        private Dictionary<ECategory, Transform> _attachmentPoints;
        private Dictionary<ECategory, Queue<GameObject>> _pools;
        private Dictionary<ECategory, (GameObject obj, string id)> _equippedItems;
        private Dictionary<string, ItemData> _itemsLookup;
        private List<ItemData> _allClothing;

        public event Action<string> OnEquip;
        public event Action<string> OnUnEquip;
        
        public void Initialize(ItemsAttachmentPoints attachmentPoints, List<ItemData> clothingData)
        {
            _pools = new Dictionary<ECategory, Queue<GameObject>>();
            _equippedItems = new Dictionary<ECategory, (GameObject, string)>();
            _allClothing = clothingData;

            _attachmentPoints = new Dictionary<ECategory, Transform>
            {
                { ECategory.Top, attachmentPoints.topAnchor },
                { ECategory.Bottom, attachmentPoints.bottomAnchor },
                { ECategory.Hair, attachmentPoints.hairAnchor }
            };
            
            _itemsLookup = new Dictionary<string, ItemData>();
            foreach (var item in clothingData)
            {
                if (!_itemsLookup.ContainsKey(item.itemID))
                {
                    _itemsLookup.Add(item.itemID, item);
                }
                else
                {
                    Debug.LogError($"Duplicate item ID: {item.itemID}");
                }
            }
            
            InitializePools();
            PrewarmPools();
            LoadEquippedItems(GameController.Services.Get<ProgressService>().PlayerProgress);
        }
        
        public void Initialize()
        {
            
        }
        
        private void LoadEquippedItems(PlayerProgress progress)
        {
            foreach (var value in progress.EquippedItems.categories)
            {
                var item = progress.EquippedItems.GetItem(value);
                if(item == null) continue;
                
                EquipItem(item);
            }
        }

        private void InitializePools()
        {
            foreach (ECategory category in System.Enum.GetValues(typeof(ECategory)))
            {
                _pools.Add(category, new Queue<GameObject>());
            }
        }

        private void PrewarmPools()
        {
            if (_allClothing == null || _allClothing.Count == 0)
            {
                Debug.LogError("No clothing data provided for prewarming pools");
                return;
            }

            foreach (var item in _allClothing)
            {
                CreateItem(item);
            }
        }

        private GameObject CreateItem(ItemData item)
        {
            if (!_attachmentPoints.TryGetValue(item.category, out Transform anchor))
            {
                Debug.LogError($"No attachment point found for category: {item.category}");
                return null;
            }

            var newItem = Object.Instantiate(item.prefab, anchor);
            var identifier = newItem.AddComponent<ItemIdentifier>();
            
            newItem.name = item.displayName;
            identifier.itemID = item.itemID;
            newItem.SetActive(false);
            _pools[item.category].Enqueue(newItem);
            return newItem;
        }

        public void EquipItem(string itemID)
        {
            if (string.IsNullOrEmpty(itemID))
            {
                Debug.LogWarning("Tried to equip item with empty ID");
                return;
            }

            if (!_itemsLookup.TryGetValue(itemID, out ItemData item))
            {
                Debug.LogWarning($"Item with ID {itemID} not found");
                return;
            }

            UnequipCategory(item.category);

            GameObject itemObject = GetFromPool(itemID) ?? CreateItem(item);
            if (itemObject != null)
            {
                itemObject.SetActive(true);
                
                var identifier = itemObject.GetComponent<ItemIdentifier>();
                _equippedItems[item.category] = (itemObject, identifier?.itemID ?? "unknown");
                OnEquip?.Invoke(itemID);
            }
        }
        
        private GameObject GetFromPool(string itemID)
        {
            if (!_itemsLookup.TryGetValue(itemID, out ItemData item))
                return null;

            if (!_pools.TryGetValue(item.category, out var pool))
                return null;
            
            var tempList = new List<GameObject>();

            GameObject foundObject = null;
            while (pool.Count > 0)
            {
                var pooledObject = pool.Dequeue();
                var identifier = pooledObject.GetComponent<ItemIdentifier>();
        
                if (identifier.itemID == itemID)
                {
                    foundObject = pooledObject;
                    break;
                }
                else
                {
                    tempList.Add(pooledObject);
                }
            }
            
            foreach (var obj in tempList)
            {
                pool.Enqueue(obj);
            }

            return foundObject;
        }

        public void UnequipCategory(ECategory category)
        {
            if (_equippedItems.TryGetValue(category, out var equipped))
            {
                var (itemObject, itemID) = equipped;
            
                itemObject.SetActive(false);
                _pools[category].Enqueue(itemObject);
                _equippedItems.Remove(category);
            
                OnUnEquip?.Invoke(itemID);
            }
        }

        public void Cleanup()
        {
            if (_pools != null)
            {
                foreach (var pool in _pools.Values)
                {
                    while (pool.Count > 0)
                    {
                        var obj = pool.Dequeue();
                        if (obj != null)
                            Object.Destroy(obj);
                    }
                }
                _pools.Clear();
            }

            _equippedItems?.Clear();
            _allClothing = null;
            _attachmentPoints = null;
        }

        public bool IsItemEquipped(string itemID)
        {
            if (!_itemsLookup.TryGetValue(itemID, out var itemData))
                return false;
            
            if (!_equippedItems.TryGetValue(itemData.category, out var equippedItem))
                return false;
            
            return equippedItem.id == itemID;
        }
    }
}