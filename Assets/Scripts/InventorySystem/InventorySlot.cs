using UnityEngine;

namespace GameLab.InventorySystem
{
    [System.Serializable]
    public class InventorySlot
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
        public void SetCurrentQuantity(int quantity)
        {
            this.currentQuantity = quantity;
        }
        public int GetCurrentQuantity()
        {
            return currentQuantity;
        }
        public int GetQuantity(int quantityDesired)
        {
            int quantityPickedup = 0;
            for ( quantityPickedup = 0; quantityPickedup <= quantityDesired && quantityPickedup <= currentQuantity; quantityPickedup++)
            {
                quantityPickedup++;
            }
            currentQuantity -= quantityPickedup;
            return quantityPickedup;
        }
    }
}

