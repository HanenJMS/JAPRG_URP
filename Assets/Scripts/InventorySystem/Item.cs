using GameLab.InteractableSystem;
using UnityEngine;

namespace GameLab.InventorySystem
{
    public class Item : Interactable
    {
        [SerializeField] InventorySlot itemSlot;
        public InventorySlot GetItemData()
        {
            return itemSlot;
        }
    }

}

