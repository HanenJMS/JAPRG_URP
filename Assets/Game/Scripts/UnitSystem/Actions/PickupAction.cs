using GameLab.InventorySystem;
using GameLab.UISystem;
using UnityEngine;

namespace GameLab.UnitSystem.ActionSystem
{
    public class PickupAction : MonoBehaviour, IAction
    {
        [SerializeField] MouseCursorData cursorData;
        public MouseCursorData GetMouseCursorInfo()
        {
            return cursorData;
        }
        public string ActionName()
        {
            return "Pick up";
        }

        public void Cancel()
        {

        }

        public bool CanExecuteOnTarget(object target)
        {
            if (target is not ItemWorld) return false;
            return true;
        }

        public void ExecuteOnTarget(object target)
        {
            var unit = GetComponent<Unit>();
            unit.GetInventoryHandler().PickupItem(target as ItemWorld);
            (target as ItemWorld).PickUp();
        }
        public override string ToString()
        {
            return ActionName();
        }
    }
}

