using System;
using UnityEngine;

namespace GameLab.InventorySystem
{
    [System.Serializable]
    public class InventorySlot : IEquatable<InventorySlot>
    {
        [SerializeField] ItemData inventoryItem;
        [SerializeField] int currentQuantity = 1;
        public void SetItemData(ItemData itemData)
        {
            inventoryItem = itemData;
        }
        public ItemData GetItemData()
        {
            return inventoryItem;
        }

        public void AddQuantity(int quantity)
        {
            currentQuantity = Mathf.Clamp(currentQuantity += quantity, 0, 100);
        }

        public bool Equals(InventorySlot other)
        {
            return other.inventoryItem = inventoryItem;
        }
    }
}

