using GameLab.InventorySystem;
using GameLab.UISystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace GameLab.UnitSystem.ActionSystem
{
    public class UnequipAction : MonoBehaviour, IAction
    {
        [SerializeField] MouseCursorData cursorData;
        public string ActionName()
        {
            return "Unequip";
        }

        public void Cancel()
        {
            
        }

        public bool CanExecuteOnTarget(object target)
        {
            return target is EquipmentSlotUI;
        }

        public void ExecuteOnTarget(object target)
        {
            var equipmentType = target as EquipmentSlotUI;
            var equipmentHander = GetComponent<EquipmentHandler>();
            equipmentHander.UnequipItem(equipmentType.GetEquipmentType());
        }

        public MouseCursorData GetMouseCursorInfo()
        {
            return cursorData;
        }
    }
}
