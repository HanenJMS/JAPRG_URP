using GameLab.InteractableSystem;

namespace GameLab.UnitSystem.ActionSystem
{
    public class EnterBuildingAction : InteractAction
    {

        public override void Interact(object target)
        {
            if (target is Interactable_Building)
            {
                var building = target as Interactable_Building;
                building.Interact(unit);
            }
        }
        public override bool CanExecuteOnTarget(object target)
        {
            if (target is Interactable)
            {
                if (target is Interactable_Building)
                {
                    return true;
                }
            }

            return false;
        }
    }
}


