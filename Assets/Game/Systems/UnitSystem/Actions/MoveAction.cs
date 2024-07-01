using GameLab.UISystem;
using UnityEngine;
using UnityEngine.AI;

namespace GameLab.UnitSystem.ActionSystem
{
    public class MoveAction : BaseAction
    {
        NavMeshAgent agent;
        ActionHandler actionHander;
        Unit unit;
        private void Awake()
        {
            agent = GetComponent<NavMeshAgent>();
            actionHander = GetComponent<ActionHandler>();
            unit = GetComponent<Unit>();
        }

        Vector3 targetPosition;
        public override void ExecuteOnTarget(object target)
        {
            
            MoveToDestination(target);
            actionHander.SetCurrentAction(this);
        }

        public override bool CanExecuteOnTarget(object target)
        {
            if (target is Vector3) return true;
            return false;
        }

        public void MoveToDestination(object target)
        {
            if (unit.InBuilding()) unit.ExitBuilding();
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

        public override string ToString()
        {
            return "Move";
        }

        public override void Cancel()
        {
            if(gameObject.activeSelf)
            {
                agent.SetDestination(this.transform.position);
            }
        }
    }
}

