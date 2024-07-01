using GameLab.InteractableSystem;
using GameLab.UnitSystem.ActionSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameLab.UnitSystem
{
    public class InteractionHandler : MonoBehaviour
    {
        Interactable currentTarget;
        public Interactable CurrentTarget { get { return currentTarget; } set { currentTarget = value; } }
        public InteractAction InteractingAction { get; set; }
        Transform currentTargetLocation = null;
        
        private void LateUpdate()
        {
            if(CurrentTarget != null && currentTargetLocation == null)
            {
                currentTargetLocation = currentTarget.GetCurrentWorldTransform();
            }
            if(currentTargetLocation != null)
            {
                bool inDistance = Vector3.Distance(this.transform.position, currentTargetLocation.position) < 2f;
                if(!inDistance)
                {
                    var mover = GetComponent<MoveAction>();
                    mover.MoveToDestination(currentTargetLocation.position);
                    return;
                }
                InteractingAction.Interact(CurrentTarget);
                CurrentTarget = null;
                currentTargetLocation = null;
                GetComponent<ActionHandler>().SetCurrentAction(null);
            }
        }
    }

}
