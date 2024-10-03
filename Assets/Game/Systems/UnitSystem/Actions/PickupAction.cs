using GameLab.InventorySystem;

namespace GameLab.UnitSystem.ActionSystem
{
    public class PickupAction : InteractAction
    {



        public override void Interact(object target)
        {
            var unit = GetComponent<Unit>();
            unit.GetInventoryHandler().PickupItem(target as ItemWorld);
            (target as ItemWorld).PickUp();
        }
        public override bool CanExecuteOnTarget(object target)
        {
            if (target is not ItemWorld) return false;
            return true;
        }
        public override string ToString()
        {
            return "Pickup";
        }
    }
}

