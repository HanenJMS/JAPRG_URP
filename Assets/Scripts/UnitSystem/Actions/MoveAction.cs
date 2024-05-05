using UnityEngine;
using UnityEngine.AI;

namespace GameLab.UnitSystem.ActionSystem
{
    public class MoveAction : MonoBehaviour, IAction
    {
        NavMeshAgent agent;
        private void Awake()
        {
            agent = GetComponent<NavMeshAgent>();
        }

        Vector3 targetPosition;
        public void ExecuteOnTarget(object target)
        {
            if (target is Vector3)
            {
                targetPosition = (Vector3)target;
                
            }
            if(target is Unit)
            {
                targetPosition = (target as Unit).gameObject.transform.position;
            }
            agent.SetDestination(targetPosition);
        }

        public bool CanExecuteOnTarget(object target)
        {
            if (target is Vector3) return true;
            if (target is Unit) return true;
            return false;
        }

        public bool Equals(IAction other)
        {
            return false;
        }

        public bool IsInRange()
        {
            return Vector3.Distance(targetPosition, this.transform.position) < 1f;
        }

        public string ActionName()
        {
            return this.ToString();
        }
        public override string ToString()
        {
            return "Move";
        }
    }
}

