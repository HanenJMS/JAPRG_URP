using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace GameLab.InventorySystem
{
    public class Inventory : MonoBehaviour
    {
        [SerializeField] List<InventorySlot> inventorySlots = new List<InventorySlot>();
        void AddItem(ItemWorld item, int Quantity)
        {
            if (inventorySlots.Contains(item.GetItemSlot()))
            {

            }
        }
    }
}

