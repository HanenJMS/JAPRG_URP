using GameLab.InteractableSystem;
using GameLab.UnitSystem.ActionSystem;
using UnityEngine;

namespace GameLab.UnitSystem
{
    public class InteractionHandler : MonoBehaviour
    {
        Interactable currentTarget;
        public Interactable CurrentTarget
        {
            get => currentTarget;
            set
            {
                currentTarget = value;
                currentTargetLocation = currentTarget.transform;
            }
        }

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
            if (currentTargetLocation == null) currentTargetLocation = currentTarget.transform;

            if (currentTargetLocation != null && currentTarget != null)
            {
                bool inDistance = Vector3.Distance(this.transform.position, currentTargetLocation.position) < 1.5f;
                if (!inDistance)
                {
                    var mover = GetComponent<MoveAction>();
                    mover.MoveToDestination(currentTargetLocation.position);
                    return;
                }
                if (inDistance)
                {
                    InteractingAction.Interact(currentTarget);
                    Cancel();
                }

            }
        }
    }

}
