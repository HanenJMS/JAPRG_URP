namespace GameLab.InventorySystem
{
    public interface IamSlot
    {
        public void SetItemData(ItemData itemData);
        public ItemData GetItemData();

        public int GetSlotCapacity();

        public int GetAvailableCapacity();

        public void SetQuantity(int quantity);
        public int GetQuantity();
    }
}

