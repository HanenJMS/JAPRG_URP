using GameLab.InteractableSystem;
using Unity.VisualScripting;
using UnityEngine;

namespace GameLab.InventorySystem
{
    public class ItemWorld : Interactable
    {
        [SerializeField] ItemData itemdata; 
        [SerializeField] InventorySlot itemSlot;
        private void Start()
        {
            itemSlot.SetItemData(itemdata);
        }
        public InventorySlot GetItemSlot()
        {
            return itemSlot;
        }
        public void PickUp()
        {
            if(itemSlot.GetQuantity() <= 0)
            {
                Destroy(this.gameObject);
            }
        }
    }

}

