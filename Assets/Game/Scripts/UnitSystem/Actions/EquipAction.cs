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
            if (target is not ItemWorld) return false;
            var item = target as ItemWorld;
            if (item.GetItemSlot().GetItemData() is not EquipmentData) return false;
            return true;
        }

        public void ExecuteOnTarget(object target)
        {
            var unit = GetComponent<Unit>();
            var itemWorld = target as ItemWorld;
            unit.GetEquipmentHandler().EquipItem(itemWorld.GetItemSlot().GetItemData() as EquipmentData);
            //(target as ItemWorld).PickUp();
        }
        public override string ToString()
        {
            return ActionName();
        }
    }
}

