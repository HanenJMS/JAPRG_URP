using GameLab.InteractableSystem;
using UnityEngine;

namespace GameLab.InventorySystem
{
    public class ItemWorld : Interactable
    {
        [SerializeField] InventorySlot itemSlot;
        public InventorySlot GetItemSlot()
        {
            return itemSlot;
        }
    }

}

