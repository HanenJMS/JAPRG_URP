using GameLab.InteractableSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameLab.UnitSystem.ActionSystem
{
    public abstract class InteractAction : BaseAction
    {
        internal Unit unit;
        private void Awake()
        {
            unit = GetComponent<Unit>();
        }
        //this is a generic 
        public override void Cancel()
        {
            base.Cancel();
            var interactionHandler = GetComponent<InteractionHandler>();
            interactionHandler.Cancel();
        }
        public abstract void Interact(object target);

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

