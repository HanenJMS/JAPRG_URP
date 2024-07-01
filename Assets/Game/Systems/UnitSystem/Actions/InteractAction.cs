using GameLab.InteractableSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameLab.UnitSystem.ActionSystem
{
    public class InteractAction : BaseAction
    {
        Unit unit;
        private void Awake()
        {
            unit = GetComponent<Unit>();
        }
        //this is a generic 
        public override void Cancel()
        {
        }
        public virtual void Interact(object target)
        {
            var building = target as Interactable_Building;
            building.Interact(unit);
        }
        public override bool CanExecuteOnTarget(object target)
        {
            if(target is Interactable)
            {
                if (target is Interactable_Building)
                {
                    return true;
                }
            }
            
            return false;
        }

        public override void ExecuteOnTarget(object target)
        {
            var interactable = target as Interactable;
            var actionHandler = unit.GetActionHandler();
            var interactionHandler = GetComponent<InteractionHandler>();
            actionHandler.SetCurrentAction(this);
            interactionHandler.CurrentTarget = interactable;
            interactionHandler.InteractingAction = this;
        }
        public override string ToString()
        {
            return "interact";
        }
    }
}

