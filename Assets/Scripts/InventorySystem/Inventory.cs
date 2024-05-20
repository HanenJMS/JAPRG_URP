using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameLab.InventorySystem
{
    public class Inventory : MonoBehaviour
    {
        [SerializeField] List<InventorySlot> inventorySlots = new List<InventorySlot>(5);

    }
}

