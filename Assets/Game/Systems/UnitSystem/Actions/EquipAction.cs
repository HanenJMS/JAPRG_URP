using GameLab.InventorySystem;

namespace GameLab.UnitSystem.ActionSystem
{
    public class EquipAction : InteractAction
    {
        public override void Interact(object target)
        {
            var unit = GetComponent<Unit>();
            if (target is ItemWorld)
            {
                var item = target as ItemWorld;
                unit.GetEquipmentHandler().EquipItem(item.GetItemSlot());
                item.PickUp();
            }
            if (target is IamSlot)
            {
                var item = target as IamSlot;
                unit.GetEquipmentHandler().EquipItem(item);
            }
        }
        public override void ExecuteOnTarget(object target)
        {
            base.ExecuteOnTarget(target);
            if (target is IamSlot) Interact(target);
        }
        public override bool CanExecuteOnTarget(object target)
        {
            if (target is ItemWorld)
            {
                var item = target as ItemWorld;
                if (item.GetItemSlot().GetItemData() is EquipmentData) return true;
            }
            if (target is IamSlot)
            {
                var item = target as IamSlot;
                if (item.GetItemData() is EquipmentData) return true;
            }
            return false;
        }
        public override string ToString()
        {
            return "Equip";
        }
    }
}

