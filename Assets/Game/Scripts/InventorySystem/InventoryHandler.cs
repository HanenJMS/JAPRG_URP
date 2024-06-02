using GameLab.TradingSystem;
using System.Collections.Generic;
using UnityEngine;

namespace GameLab.InventorySystem
{
    public class InventoryHandler : MonoBehaviour
    {
        [SerializeField] Dictionary<ItemData, InventorySlot> inventory = new();

        [SerializeField] List<InventorySlot> inventorySlots = new List<InventorySlot>();

        public InventorySlot GetInventorySlot(int index)
        {
            UpdateList();
            if (index >= inventorySlots.Count)
            {
                return null;
            }
            return inventorySlots[index];
        }
        public void AddToQuantity(InventorySlot giverSlot, int quantity = 0)
        {
            AddItem(giverSlot.GetItemData());
            int qty = quantity;
            if (quantity == 0)
            {
                qty = giverSlot.GetQuantity();
            }
            GetComponent<TradeHandler>().TradeItem(inventory[giverSlot.GetItemData()], giverSlot, qty);

            UpdateList();
        }
        public void RemoveFromQuantity(InventorySlot receiver, int quantity = 0)
        {
            int qty = quantity;
            Debug.Log("Quantity to Drop : " + qty);
            if (quantity == 0)
            {
                qty = inventory[receiver.GetItemData()].GetQuantity();
            }
            Debug.Log("Quantity to Drop : " + qty);
            Debug.Log("inventorySlot : " + inventory[receiver.GetItemData()]);
            GetComponent<TradeHandler>().TradeItem(receiver, inventory[receiver.GetItemData()], qty);
            RemoveItem(receiver.GetItemData());
        }
        public void PickupItem(ItemWorld item, int Quantity = 0)
        {

            AddToQuantity(item.GetItemSlot(), Quantity);
        }
        public void DropItem(ItemData item, int quantity)
        {
            GameObject go = Instantiate(item.GetItemPickupPrefab(), this.transform.position, Quaternion.identity);
            ItemWorld goItemWorld = go.GetComponentInChildren<ItemWorld>();
            goItemWorld.GetItemSlot().SetItemData(item);
            go.transform.position += Vector3.up * 2 + Vector3.forward * 1.5f;
            RemoveFromQuantity(goItemWorld.GetItemSlot(), quantity);
            UpdateList();

        }
        public InventorySlot GetInventorySlot(ItemData itemData)
        {
            if (!inventory.ContainsKey(itemData)) return null;
            return inventory[itemData];
        }

        void AddItem(ItemData item)
        {
            if (inventory.ContainsKey(item)) return;
            inventory.Add(item, new(item));
            UpdateList();
        }
        void RemoveItem(ItemData item)
        {
            if (inventory.ContainsKey(item))
            {
                if (inventory[item].GetQuantity() <= 0)
                {
                    inventory.Remove(item);
                }
            }
            UpdateList();
        }
        void UpdateList()
        {
            inventorySlots.Clear();
            foreach (KeyValuePair<ItemData, InventorySlot> inv in inventory)
            {
                inventorySlots.Add(inv.Value);
            }
        }
    }
}

