using GameLab.InventorySystem;
using GameLab.UISystem;
using GameLab.UnitSystem;
using System.Collections;
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
        }
        void SetSelected()
        {
            if (selectedInventory != null) selectedInventory.onInventoryChange -= UpdateUI;
            selectedInventory = UnitSelectionSystem.Instance.GetSelectedUnit().GetInventoryHandler();
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
            if(Input.GetKeyDown(KeyCode.I))
            {
                ClearUI();
                UpdateUI(selectedInventory.GetInventory());
            }
        }
        void UpdateUI(List<InventorySlot> slots)
        {
            if (slots == null) return;
            if (slots.Count == 0) return;
            slots.ForEach(slot =>
            {
                GameObject ui = Instantiate(inventorySlotUI, container.transform);
                ui.GetComponent<InventorySlotUI>().SetUI(slot);
                inventorySlotUIs.Add(ui);
            });
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

