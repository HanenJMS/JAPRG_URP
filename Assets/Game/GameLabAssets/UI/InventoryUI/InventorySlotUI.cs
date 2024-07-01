using GameLab.InventorySystem;
using GameLab.UnitSystem;
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
        [SerializeField] Button button;
        private void Awake()
        {
            button = GetComponent<Button>();    
        }
        public void SetUI(InventorySlot ui)
        {
            if (ui == null) return;
            if (ui.GetItemData() == null) return;
            slotImage.sprite = ui.GetItemData().GetItemSprite();
            slotText.text = ui.GetQuantity().ToString();
            button?.onClick.AddListener(() =>
            {
                ActionSystemUI.Instance.SpawnActionButtons
                (
                    UnitSelectionSystem.Instance.
                        GetSelectedUnit().
                        GetActionHandler().
                        GetExecutableActions(ui), ui
                );
            });
        }

    }
}


