using GameLab.TradingSystem;
using GameLab.UISystem;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameLab.InventorySystem
{
    [System.Serializable]
    public class EquipmentInventory
    {

        EquipmentSlot HeadSlot = new(EquipmentType.Head);
        EquipmentSlot BodySlot = new(EquipmentType.Body);
        EquipmentSlot BootsSLot = new(EquipmentType.Boots);

        EquipmentSlot MainWeaponSlot = new(EquipmentType.Main);
        EquipmentSlot OffHandSlot = new(EquipmentType.OffHand);

        Dictionary<EquipmentType, EquipmentSlot> equipmentInventory = new();
        List<EquipmentSlot> equipmentSlots = new();
        public Action onEquipmentChanged;
        public EquipmentInventory()
        {
            equipmentInventory.Add(EquipmentType.Head, HeadSlot);
            equipmentSlots.Add(HeadSlot);

            equipmentInventory.Add(EquipmentType.Body, BodySlot);
            equipmentSlots.Add(BodySlot);

            equipmentInventory.Add(EquipmentType.Boots, BootsSLot);
            equipmentSlots.Add(BootsSLot);

            equipmentInventory.Add(EquipmentType.Main, MainWeaponSlot);
            equipmentSlots.Add(MainWeaponSlot);

            equipmentInventory.Add(EquipmentType.OffHand, OffHandSlot);
            equipmentSlots.Add(OffHandSlot);
        }
        public Dictionary<EquipmentType, EquipmentSlot> GetSlots() => equipmentInventory;
        public IamSlot GetSlot(EquipmentType type)
        {
            return equipmentInventory[type];
        }
        public void EquipItem(EquipmentData equipItem)
        {
            equipmentInventory[equipItem.GetEquipmentType()].SetItemData(equipItem);
            onEquipmentChanged?.Invoke();
        }
        public void UnequipItem(EquipmentType type)
        {
            
        }
    }
}


