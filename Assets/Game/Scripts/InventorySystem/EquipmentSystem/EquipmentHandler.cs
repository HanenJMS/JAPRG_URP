using GameLab.Animation;
using GameLab.UnitSystem;
using NUnit.Framework.Interfaces;
using UnityEngine;


namespace GameLab.InventorySystem
{
    public class EquipmentHandler : MonoBehaviour
    {

        [SerializeField] EquipmentInventory equipmentInventory = new();
        [SerializeField] UnitEquipmentWorldHolder worldEquipmentHolder;
        [SerializeField] Unit unit;
        private void Awake()
        {
            unit = GetComponent<Unit>();
            worldEquipmentHolder = GetComponent<UnitEquipmentWorldHolder>();
        }

        public EquipmentInventory GetInventory()
        {
            return equipmentInventory;
        }

        public void EquipItem(IamSlot equipItem)
        {
            var itemData = equipItem.GetItemData() as EquipmentData;
            UnequipItem(itemData.GetEquipmentType());
            equipmentInventory.EquipItem(itemData);
            worldEquipmentHolder.EquipItem(itemData);
            unit.GetTradeHandler().TradeItem(equipmentInventory.GetSlot(itemData.GetEquipmentType()), equipItem, 1);
            unit.GetInventoryHandler().UpdateList();
            if(itemData.GetEquipmentType() == EquipmentType.Main)
            {
                DrawWeapon();
            }
        }

        public void UnequipItem(EquipmentType equipmentType)
        {
            if(equipmentInventory.GetSlot(equipmentType).GetQuantity() > 0)
            {
                unit.GetInventoryHandler().AddToQuantity(equipmentInventory.GetSlot(equipmentType), 1);
                equipmentInventory.UnequipItem(equipmentType);
            }
            worldEquipmentHolder.UnequipItem(equipmentType);
            if (equipmentType == EquipmentType.Main)
            {
                DrawWeapon();
            }
        }
        public void DrawWeapon()
        {
            equipmentInventory.SetWeaponDrawn(true);
            worldEquipmentHolder.DrawWeapon();
            EquipWeapon();
        }

        public void UndrawWeapon()
        {
            equipmentInventory.SetWeaponDrawn(false);
            worldEquipmentHolder.DrawWeapon();
            EquipWeapon();
        }

        void EquipWeapon()
        {
            EquipAnimation();
            EquipWeaponAbility();
        }
        public void EquipAnimation()
        {
            GetComponent<UnitAnimationHandler>().SetAnimationOverrideController(equipmentInventory.GetCurrentWeaponDrawn().AnimatorOverrideController());
        }
        public void EquipWeaponAbility()
        {
            unit.GetAbilityHandler().SetDefaultAbility(equipmentInventory.GetCurrentWeaponDrawn().GetDefaultAbility());
        }
        public void WithdrawCombat()
        {
            UndrawWeapon();
            GetComponent<UnitAnimationHandler>().SetDefaultAnimationController();
            EquipWeaponAbility();
        }

    }
}

