using System;

namespace GameLab.InventorySystem
{
    [Serializable]
    public class EquipmentSlot : InventorySlot
    {
        public EquipmentSlot(ItemData item, int Quantity = 0) : base(item, Quantity)
        {

        }
        public override int GetSlotCapacity()
        {
            return 1;
        }
    }
}
