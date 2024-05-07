using UnityEngine;
using UnityEngine.AI;

namespace GameLab.UnitSystem.ActionSystem
{
    public class MoveAction : MonoBehaviour, IAction
    {
        NavMeshAgent agent;
        ActionHandler actionHander;
        private void Awake()
        {
            agent = GetComponent<NavMeshAgent>();
            actionHander = GetComponent<ActionHandler>();
        }

        Vector3 targetPosition;
        public void ExecuteOnTarget(object target)
        {
            MoveToDestination(target);
            actionHander.SetCurrentAction(this);
        }

        public bool CanExecuteOnTarget(object target)
        {
            if (target is Vector3) return true;
            if (target is Unit) return true;
            return false;
        }

        public void MoveToDestination(object target)
        {
            SetTargetPosition(target);
            agent.SetDestination(targetPosition);
        }
        void SetTargetPosition(object target)
        {
            if (target is Vector3)
            {
                targetPosition = (Vector3)target;

            }
            if (target is Unit)
            {
                targetPosition = (target as Unit).gameObject.transform.position;
            }
        }

        public string ActionName()
        {
            return this.ToString();
        }
        public override string ToString()
        {
            return "Move";
        }

        public void Cancel()
        {
            agent.SetDestination(this.transform.position);
        }
    }
}

