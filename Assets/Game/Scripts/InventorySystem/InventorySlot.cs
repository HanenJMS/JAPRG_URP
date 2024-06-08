using System;
using UnityEngine;

namespace GameLab.InventorySystem
{
    [System.Serializable]
    public class InventorySlot : IEquatable<InventorySlot>
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
        public virtual int GetSlotCapacity()
        {
            return slotCapacity;
        }

        public virtual int GetAvailableCapacity()
        {
            return slotCapacity - currentQuantity;
        }

        public void SetQuantity(int quantity)
        {
            currentQuantity = quantity;
            Debug.Log("ChangedQuantity: " + currentQuantity);
        }
        public int GetQuantity()
        {
            return currentQuantity;
        }
        public bool Equals(InventorySlot other)
        {
            return other.inventoryItem = inventoryItem;
        }
        public static InventorySlot operator +(InventorySlot a, int b)
        {
            return new(a.GetItemData(), a.GetQuantity() + b);
        }
        public static InventorySlot operator -(InventorySlot a, int b)
        {
            return new(a.GetItemData(), a.GetQuantity() - b);
        }
    }
}

