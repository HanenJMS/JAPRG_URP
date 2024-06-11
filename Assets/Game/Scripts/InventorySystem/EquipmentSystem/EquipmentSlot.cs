using System;

namespace GameLab.InventorySystem
{
    [Serializable]
    public class EquipmentSlot : IamSlot
    {
        const int _CAPACITY = 1;
        int quantity = 0;
        EquipmentData equipmentData;
        EquipmentType equipmentType;

        public EquipmentSlot(EquipmentType type)
        {
            equipmentType = type;
        }
        public int GetAvailableCapacity()
        {
            return _CAPACITY - quantity;
        }

        public ItemData GetItemData()
        {
            return equipmentData;
        }

        public int GetQuantity()
        {
            return quantity;
        }

        public int GetSlotCapacity()
        {
            return _CAPACITY;
        }

        public void SetItemData(ItemData itemData)
        {
            equipmentData = itemData as EquipmentData;
        }

        public void SetQuantity(int quantity)
        {
            this.quantity = quantity;
        }
    }
}
