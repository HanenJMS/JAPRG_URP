using GameLab.InventorySystem;
using GameLab.UISystem;
using UnityEngine;

namespace GameLab.UnitSystem.ActionSystem
{
    public class PickupAction : BaseAction
    {


        public override void Cancel()
        {

        }

        public override bool CanExecuteOnTarget(object target)
        {
            if (target is not ItemWorld) return false;
            return true;
        }

        public override void ExecuteOnTarget(object target)
        {
            var unit = GetComponent<Unit>();
            unit.GetInventoryHandler().PickupItem(target as ItemWorld);
            (target as ItemWorld).PickUp();
        }
        public override string ToString()
        {
            return "Pickup";
        }
    }
}

