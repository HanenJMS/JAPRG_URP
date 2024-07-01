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
        
        public void Cancel()
        {
            currentTarget = null;
            currentTargetLocation = null;
            
        }
        private void LateUpdate()
        {
            if (currentTarget == null) return;
            if(currentTarget != null && currentTargetLocation == null)
            {
                currentTargetLocation = currentTarget.GetCurrentWorldTransform();
            }
            if(currentTargetLocation != null && currentTarget != null)
            {
                bool inDistance = Vector3.Distance(this.transform.position, currentTargetLocation.position) < 1.5f;
                if(!inDistance)
                {
                    var mover = GetComponent<MoveAction>();
                    mover.MoveToDestination(currentTargetLocation.position);
                    return;
                }
                InteractingAction.Interact(currentTarget);
                Cancel();

            }
        }
    }

}
