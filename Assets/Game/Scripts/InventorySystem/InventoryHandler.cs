using GameLab.TradingSystem;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
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
            if(index >= inventorySlots.Count)
            {
                return null;
            }
            return inventorySlots[index];
        }
        public void AddToQuantity(InventorySlot giverSlot, int quantity = 0)
        {
            int qty = quantity;
            if(quantity == 0)
            {
                qty = giverSlot.GetQuantity();
            }
            GetComponent<TradeHandler>().TradeItem(inventory[giverSlot.GetItemData()], giverSlot, qty);
            
            UpdateList();
        }
        public void RemoveFromQuantity(InventorySlot receiver, int quantity = 0)
        {
            int qty = quantity;
            if (quantity == 0)
            {
                qty = inventory[receiver.GetItemData()].GetQuantity();
            }
            Debug.Log("inventorySlot : " + inventory[receiver.GetItemData()]);
            GetComponent<TradeHandler>().TradeItem(receiver, inventory[receiver.GetItemData()], qty);

            UpdateList();
        }
        public void PickupItem(ItemWorld item, int Quantity = 0)
        {
            AddItem(item.GetItemSlot().GetItemData());
            AddToQuantity(item.GetItemSlot(), Quantity);
        }
        public void DropItem(ItemData item, int quantity)
        {
            GameObject go = Instantiate(item.GetItemPrefab(), this.transform.position, Quaternion.identity);
            Debug.Log("DroppedItemInstantiated : " +go.GetComponent<ItemWorld>().GetItemSlot().ToString());
            go.GetComponent<ItemWorld>().GetItemSlot().SetItemData(item);
            RemoveFromQuantity(go.GetComponent<ItemWorld>().GetItemSlot(), quantity);
            RemoveItem(item);
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
        }
        void RemoveItem(ItemData item)
        {
            if(inventory.ContainsKey(item))
            {
                if (inventory[item].GetQuantity() <= 0)
                {
                    inventory.Remove(item);
                }
            }
        }
        void UpdateList()
        {
            inventorySlots.Clear();
            foreach(KeyValuePair<ItemData, InventorySlot> inv in inventory)
            {
                inventorySlots.Add(inv.Value);
            }
        }
    }
}

