using GameLab.InteractableSystem;
using Unity.VisualScripting;
using UnityEngine;

namespace GameLab.InventorySystem
{
    public class ItemWorld : Interactable
    {
        [SerializeField] ItemData itemdata; 
        [SerializeField] InventorySlot itemSlot;
        [SerializeField] GameObject itemWorldContainer;

        private void Awake()
        {
            itemWorldContainer = this.transform.parent.gameObject;

        }
        private void Start()
        {
            if(itemdata == null) { return; }
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
                Destroy(itemWorldContainer);
            }
        }
    }

}

