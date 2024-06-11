using System;
using UnityEngine;

namespace GameLab.InventorySystem
{
    [System.Serializable]
    public class InventorySlot : IEquatable<InventorySlot>, IamSlot
    {
        [SerializeField] ItemData inventoryItem;
        [SerializeField] int currentQuantity = 0;
        [SerializeField] int slotCapacity = 50;
        public InventorySlot(ItemData item, int Quantity = 0)
        {
            SetItemData(item);
            SetQuantity(Quantity);
        }
        public void SetItemData(ItemData itemData)
        {
            inventoryItem = itemData;
        }
        public ItemData GetItemData()
        {
            return inventoryItem;
        }
        public int GetSlotCapacity()
        {
            return slotCapacity;
        }

        public int GetAvailableCapacity()
        {
            return slotCapacity - currentQuantity;
        }

        public void SetQuantity(int quantity)
        {
            currentQuantity = quantity;
        }
        public int GetQuantity()
        {
            return currentQuantity;
        }
        public bool Equals(InventorySlot other)
        {
            return other.inventoryItem = inventoryItem;
        }
    }
}

