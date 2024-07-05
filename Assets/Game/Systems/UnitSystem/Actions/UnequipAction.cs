using GameLab.InventorySystem;
using GameLab.UISystem;
namespace GameLab.UnitSystem.ActionSystem
{
    public class UnequipAction : BaseAction
    {

        public override void Cancel()
        {

        }

        public override bool CanExecuteOnTarget(object target)
        {
            return target is EquipmentSlotUI;
        }

        public override void ExecuteOnTarget(object target)
        {
            var equipmentType = target as EquipmentSlotUI;
            var equipmentHander = GetComponent<EquipmentHandler>();
            equipmentHander.UnequipItem(equipmentType.GetEquipmentType());
        }
    }
}
