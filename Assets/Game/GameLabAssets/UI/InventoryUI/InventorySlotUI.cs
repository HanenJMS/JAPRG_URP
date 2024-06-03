using GameLab.InventorySystem;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


namespace GameLab.UISystem
{
    public class InventorySlotUI : MonoBehaviour
    {
        [SerializeField] Image slotImage;
        [SerializeField] TextMeshProUGUI slotText;
        private void Awake()
        {

        }
        public void SetUI(InventorySlot ui)
        {
            slotImage.sprite = ui.GetItemData().GetItemSprite();
            slotText.text = ui.GetQuantity().ToString();
        }
    }
}


