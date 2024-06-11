using GameLab.InventorySystem;
using GameLab.UISystem;
using UnityEngine;

namespace GameLab.UnitSystem.ActionSystem
{
    public class EquipAction : MonoBehaviour, IAction
    {
        [SerializeField] MouseCursorData cursorData;
        public MouseCursorData GetMouseCursorInfo()
        {
            return cursorData;
        }
        public string ActionName()
        {
            return "Equip";
        }

        public void Cancel()
        {

        }

        public bool CanExecuteOnTarget(object target)
        {
            if (target is ItemWorld)
            {
                var item = target as ItemWorld;
                if (item.GetItemSlot().GetItemData() is not EquipmentData) return false;
            }
            if (target is IamSlot)
            {
                var item = target as IamSlot;
                if (item.GetItemData() is not EquipmentData) return false;
            }
            return true;
        }

        public void ExecuteOnTarget(object target)
        {
            var unit = GetComponent<Unit>();
            if (target is ItemWorld)
            {
                var item = target as ItemWorld;
                unit.GetEquipmentHandler().EquipItem(item.GetItemSlot());
                item.PickUp();
            }
            if(target is IamSlot)
            {
                var item = target as IamSlot;
                unit.GetEquipmentHandler().EquipItem(item);
            }
        }
        public override string ToString()
        {
            return ActionName();
        }
    }
}

