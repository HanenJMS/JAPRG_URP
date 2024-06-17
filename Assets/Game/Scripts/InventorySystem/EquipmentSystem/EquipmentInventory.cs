using System;
using System.Collections.Generic;
using UnityEngine;

namespace GameLab.InventorySystem
{
    [System.Serializable]
    public class EquipmentInventory
    {
        [SerializeField] WeaponData defaultWeapon;
        [SerializeField] WeaponData drawnWeapon;
        EquipmentSlot HeadSlot = new(EquipmentType.Head);
        EquipmentSlot BodySlot = new(EquipmentType.Body);
        EquipmentSlot BootsSlot = new(EquipmentType.Boots);

        EquipmentSlot MainWeaponSlot = new(EquipmentType.Main);
        EquipmentSlot OffHandSlot = new(EquipmentType.OffHand);

        Dictionary<EquipmentType, EquipmentSlot> equipmentInventory = new();
        List<EquipmentSlot> equipmentSlots = new();
        bool weaponDrawn = false;
        public Action onEquipmentChanged;
        public EquipmentInventory()
        {
            equipmentInventory.Add(EquipmentType.Head, HeadSlot);
            equipmentSlots.Add(HeadSlot);

            equipmentInventory.Add(EquipmentType.Body, BodySlot);
            equipmentSlots.Add(BodySlot);

            equipmentInventory.Add(EquipmentType.Boots, BootsSlot);
            equipmentSlots.Add(BootsSlot);

            equipmentInventory.Add(EquipmentType.Main, MainWeaponSlot);
            equipmentSlots.Add(MainWeaponSlot);

            equipmentInventory.Add(EquipmentType.OffHand, OffHandSlot);
            equipmentSlots.Add(OffHandSlot);
        }
        public Dictionary<EquipmentType, EquipmentSlot> GetSlots()
        {
            return equipmentInventory;
        }

        public IamSlot GetSlot(EquipmentType type)
        {
            return equipmentInventory[type];
        }
        public EquipmentData GetEquippedItem(EquipmentType type)
        {
            return equipmentInventory[type].GetItemData() as EquipmentData;

        }
        public void EquipItem(EquipmentData equipItem)
        {
            equipmentInventory[equipItem.GetEquipmentType()].SetItemData(equipItem);
            onEquipmentChanged?.Invoke();
        }
        public void UnequipItem(EquipmentType equipmentType)
        {
            equipmentInventory[equipmentType].SetItemData(null);
            onEquipmentChanged?.Invoke();
        }
        public void SetWeaponDrawn(WeaponData weaponData)
        {
            drawnWeapon = weaponData;
        }
        public WeaponData GetCurrentWeaponDrawn()
        {
            if(weaponDrawn && GetEquippedItem(EquipmentType.Main) != null)
            {
                return GetEquippedItem(EquipmentType.Main) as WeaponData;
            }
            return defaultWeapon;
        }

        internal void SetWeaponDrawn(bool weaponDrawn)
        {
            this.weaponDrawn = weaponDrawn;
        }
    }
}


