using GameLab.UISystem;
using GameLab.UnitSystem;
using System.Collections.Generic;
using UnityEngine;

public class EquipmentSystemUI : MonoBehaviour
{
    
    Unit selectedUnit;
    EquipmentSlotUI selectedSlot;
    [SerializeField] List<EquipmentSlotUI> equipmentSlots;

    void Start()
    {
        UnitSelectionSystem.Instance.onSelectedUnit += OnSelectedUnit;
        OnSelectedUnit();
    }

    void OnSelectedUnit()
    {
        if (selectedUnit != null) selectedUnit.GetEquipmentHandler().GetInventory().onEquipmentChanged -= UpdateUI;
        selectedUnit = UnitSelectionSystem.Instance.GetSelectedUnit();
        if (selectedUnit == null) return;
        selectedUnit.GetEquipmentHandler().GetInventory().onEquipmentChanged += UpdateUI;
        UpdateUI();
    }
    void UpdateUI()
    {
        if(selectedUnit == null)
        {
            selectedUnit = UnitSelectionSystem.Instance.GetSelectedUnit();
        }
        if (selectedUnit != null)
        {
            foreach (EquipmentSlotUI ui in equipmentSlots)
            {
                ui.gameObject.SetActive(selectedUnit.GetEquipmentHandler().GetInventory().GetSlots()[ui.GetEquipmentType()].GetItemData() != null);
                if (selectedUnit.GetEquipmentHandler().GetInventory().GetSlots()[ui.GetEquipmentType()].GetItemData() != null)
                {
                    ui.SetImage(selectedUnit.GetEquipmentHandler().GetInventory().GetSlots()[ui.GetEquipmentType()].GetItemData().GetItemSprite());
                }
            }
        }
    }
}
