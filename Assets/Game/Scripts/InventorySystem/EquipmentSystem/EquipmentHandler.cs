using GameLab.Animation;
using GameLab.UnitSystem;
using System;
using System.Collections.Generic;
using UnityEngine;


namespace GameLab.InventorySystem
{
    public class EquipmentHandler : MonoBehaviour
    {
        //Equipment
        //headSlotHolder
        //armorSlotHolder
        //bootsSlotHolder

        //EquipmentSlot head
        //EquipmentSlot armor
        //EquipmentSlot boots

        [SerializeField] Dictionary<EquipmentType, EquipmentSlot> equipmentSlots = new();
        [SerializeField] EquipmentSlot[] equipments = new EquipmentSlot[6];
        public void EquipItem(EquipmentData equipItem)
        {
            if (equipItem.GetEquipmentType() == EquipmentType.Head)
            {
                headEquipped = equipItem as ArmorData;
                if (equippedOnHeadWorld != null) Destroy(equippedOnHeadWorld.gameObject);
                equippedOnHeadWorld = DrawEquipmentOn(headEquipped, headSlotHolder);
            }
            if (equipItem.GetEquipmentType() == EquipmentType.Body)
            {
                bodyEquipped = equipItem as ArmorData;
                equippedOnBodyWorld = Instantiate(equipItem.GetItemPrefab(), headSlotHolder);
            }
            if (equipItem.GetEquipmentType() == EquipmentType.Boots)
            {
                bootsEquipped = equipItem as ArmorData;
                equippedOnBootsWorld = Instantiate(equipItem.GetItemPrefab(), headSlotHolder);
            }
            if (equipItem.GetEquipmentType() == EquipmentType.Main)
            {
                equippedMainWeapon = equipItem as WeaponData;
                DrawWeapon();
            }
            if (equipItem.GetEquipmentType() == EquipmentType.OffHand)
            {
                headEquipped = equipItem as ArmorData;
                equippedOnHeadWorld = Instantiate(equipItem.GetItemPrefab(), headSlotHolder);
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

