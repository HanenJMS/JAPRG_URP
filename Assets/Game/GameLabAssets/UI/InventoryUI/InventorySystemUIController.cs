using GameLab.InventorySystem;
using GameLab.UISystem;
using GameLab.UnitSystem;
using System.Collections.Generic;
using UnityEngine;

namespace GameLab.Controller
{
    public class InventorySystemUIController : MonoBehaviour
    {
        [SerializeField] List<GameObject> inventorySlotUIs = new();
        [SerializeField] GameObject inventorySlotUI;
        [SerializeField] GameObject container;
        InventoryHandler selectedInventory;
        private void Start()
        {
            UnitSelectionSystem.Instance.onSelectedUnit += SetSelected;
            SetSelected();
        }
        void SetSelected()
        {
            if (selectedInventory != null) selectedInventory.onInventoryChange -= UpdateUI;
            var unit = UnitSelectionSystem.Instance.GetSelectedUnit();
            if (unit == null) return;
            selectedInventory = unit.GetInventoryHandler();
            if (selectedInventory == null) return;
            selectedInventory.onInventoryChange += UpdateUI;
            UpdateUI();
        }
        void UpdateUI()
        {
            ClearUI();
            UpdateUI(selectedInventory.GetInventory());
        }
        private void LateUpdate()
        {
            if (!container.gameObject.activeSelf) return;

            if (Input.GetKeyDown(KeyCode.I))
            {
                ClearUI();
                if (selectedInventory == null)
                {
                    selectedInventory = UnitSelectionSystem.Instance.GetPlayerUnit().GetInventoryHandler();
                }
                if (selectedInventory != null)
                {
                    UpdateUI(selectedInventory.GetInventory());
                }
            }
        }
        void UpdateUI(Dictionary<ItemData, InventorySlot> slots)
        {
            if (slots == null) return;
            if (slots.Count == 0) return;
            ClearUI();
            foreach (KeyValuePair<ItemData, InventorySlot> slot in slots)
            {
                GameObject ui = Instantiate(inventorySlotUI, container.transform);
                ui.GetComponent<InventorySlotUI>().SetUI(slot.Value);
                inventorySlotUIs.Add(ui);
            }
        }
        void ClearUI()
        {
            inventorySlotUIs.ForEach(slot =>
            {
                Destroy(slot.gameObject);
            });
            inventorySlotUIs.Clear();
        }
    }
}

