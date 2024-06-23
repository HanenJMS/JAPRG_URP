using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameLab.InventorySystem
{
    public interface IamInventory
    {
        public void AddToQuantity(IamSlot giverSlot, int quantity = 0);
        public void RemoveFromQuantity(IamSlot receiver, int quantity = 0);
        public IamSlot GetInventorySlot(ItemData itemData);
    }
}

