using GameLab.Animation;
using GameLab.UnitSystem;
using System;
using System.Collections.Generic;
using UnityEngine;


namespace GameLab.InventorySystem
{
    public class EquipmentHandler : MonoBehaviour
    {

        [SerializeField]EquipmentInventory equipmentInventory = new();
        public EquipmentInventory GetInventory() => equipmentInventory;
        public void EquipItem(IamSlot equipItem)
        {
            //equipmentInventory.EquipItem(equipItem);
            var itemData = equipItem.GetItemData() as EquipmentData;
            equipmentInventory.EquipItem(itemData);
            if (itemData.GetEquipmentType() == EquipmentType.Head)
            {
                if (equippedOnHeadWorld != null) Destroy(equippedOnHeadWorld.gameObject);
                unit.GetInventoryHandler().AddToQuantity(equipmentInventory.GetSlot(itemData.GetEquipmentType()), 1);
                unit.GetTradeHandler().TradeItem(equipmentInventory.GetSlot(itemData.GetEquipmentType()), equipItem, 1);
                equippedOnHeadWorld = DrawEquipmentOn(itemData as ArmorData, headSlotHolder);
            }
            if (itemData.GetEquipmentType() == EquipmentType.Body)
            {
                bodyEquipped = itemData as ArmorData;
                unit.GetTradeHandler().TradeItem(equipmentInventory.GetSlot(itemData.GetEquipmentType()), equipItem, 1);
                equippedOnBodyWorld = DrawEquipmentOn(itemData as ArmorData, bodySlotHolder);
            }
            if (itemData.GetEquipmentType() == EquipmentType.Boots)
            {
                bootsEquipped = itemData as ArmorData;
                unit.GetTradeHandler().TradeItem(equipmentInventory.GetSlot(itemData.GetEquipmentType()), equipItem, 1);
                equippedOnBootsWorld = DrawEquipmentOn(itemData as ArmorData, bootsSlotHolder);
            }
            if (itemData.GetEquipmentType() == EquipmentType.Main)
            {
                equippedMainWeapon = itemData as WeaponData;
                unit.GetTradeHandler().TradeItem(equipmentInventory.GetSlot(itemData.GetEquipmentType()), equipItem, 1);
                DrawWeapon();
            }
            if (itemData.GetEquipmentType() == EquipmentType.OffHand)
            {
                headEquipped = itemData as ArmorData;
                equippedOnHeadWorld = Instantiate(itemData.GetItemPrefab(), headSlotHolder);
            }
        }

        private GameObject DrawEquipmentOn(ArmorData equipped, Transform slotHolder)
        {
            if(equipped != null)
            {
                 return Instantiate(equipped.GetItemPrefab(), slotHolder);
            }
            return null;
        }

        //Head Slot
        [SerializeField] Transform headSlotHolder;
        [SerializeField] ArmorData headEquipped;
        [SerializeField] GameObject equippedOnHeadWorld;

        //body slot
        [SerializeField] Transform bodySlotHolder;
        [SerializeField] ArmorData bodyEquipped;
        [SerializeField] GameObject equippedOnBodyWorld;

        //Boots Slot
        [SerializeField] Transform bootsSlotHolder;
        [SerializeField] ArmorData bootsEquipped;
        [SerializeField] GameObject equippedOnBootsWorld;

        //weapon
        [SerializeField] Transform LeftHandWeaponHolder;
        [SerializeField] WeaponData defaultWeapon;
        [SerializeField] WeaponData currentWeapon;

        [SerializeField] Transform RightHandWeaponHolder;
        [SerializeField] WeaponData equippedMainWeapon;
        [SerializeField] GameObject MainWeaponDrawn;


        [SerializeField] Unit unit;
        private void Awake()
        {
            unit = GetComponent<Unit>();
        }
        private void Start()
        {
            if (currentWeapon == null)
            {
                currentWeapon = defaultWeapon;
            }
        }
        void EquipWeapon()
        {
            EquipAnimation();
            EquipWeaponAbility();
        }
        public void UndrawGear(EquipmentData equipping, GameObject drawnGear, EquipmentData slot)
        {
            if (drawnGear == null) return;
            Destroy(drawnGear.gameObject);
            drawnGear = null;
            slot = defaultWeapon;
            EquipWeapon();
        }
        public void DrawWeapon()
        {
            if (equippedMainWeapon != null)
            {
                UndrawWeapon();
                MainWeaponDrawn = Instantiate(equippedMainWeapon.GetItemPrefab(), RightHandWeaponHolder);
                currentWeapon = equippedMainWeapon;
            }
            else
            {
                currentWeapon = defaultWeapon;
            }
            EquipWeapon();
        }
        public void EquipWeaponAbility()
        {
            unit.GetAbilityHandler().SetDefaultAbility(currentWeapon.GetDefaultAbility());
        }
        public void UndrawWeapon()
        {
            if (MainWeaponDrawn == null) return;
            Destroy(MainWeaponDrawn.gameObject);
            MainWeaponDrawn = null;
            currentWeapon = defaultWeapon;
            EquipWeapon();
        }
        public void EquipAnimation()
        {
            GetComponent<UnitAnimationHandler>().SetAnimationOverrideController(currentWeapon.AnimatorOverrideController());
        }
        public void WithdrawCombat()
        {
            UndrawWeapon();
            GetComponent<UnitAnimationHandler>().SetDefaultAnimationController();
            EquipWeaponAbility();
        }
        public WeaponData GetMainWeapon()
        {
            return equippedMainWeapon;
        }

    }
}

