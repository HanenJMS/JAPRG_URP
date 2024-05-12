using GameLab.UnitSystem;
using GameLab.UnitSystem.ActionSystem;
using System.Collections.Generic;
using UnityEngine;

namespace GameLab.AI
{
    public class CombatAI : MonoBehaviour, IAIState
    {
        IAction attackAction;
        AIHandler aiHandler;
        [SerializeField] bool isActive = false;
        Unit unit;
        Unit targetUnit;
        private void Awake()
        {
            unit = GetComponent<Unit>();
            aiHandler = GetComponent<AIHandler>();
            attackAction = unit.GetComponent<AttackAction>();
        }
        private void Start()
        {
            unit.GetCombatHandler().onDamageTaken += OnDamageTaken;
            unit.GetCombatHandler().onCombatInitiated += ActivateState;
        }
        void OnDamageTaken()
        {
            ActivateState();
        }
        private void LateUpdate()
        {
            if (unit.GetCombatHandler().GetCombatTargets().Count <= 0)
            {
                isActive = false;
            }
            if (unit.GetHealthHandler().IsDead())
            {
                isActive = false;
                return;
            }
            if (!isActive) return;
            if (targetUnit != null)
            {
                if (targetUnit.GetHealthHandler().IsDead())
                {
                    unit.GetCombatHandler().RemoveTarget(targetUnit);
                    targetUnit = null;
                }
                if (!isActive)
                {
                    attackAction.Cancel();
                    return;
                }
                attackAction.ExecuteOnTarget(targetUnit);
                return;
            }

            if (unit.GetCombatHandler().GetCombatTargets().Count > 0 && targetUnit == null)
            {
                Unit closestTarget = unit.GetCombatHandler().GetCombatTargets()[0];
                Vector3 closestTargetPosition = closestTarget.transform.position;
                foreach (Unit unitTarget in unit.GetCombatHandler().GetCombatTargets())
                {
                    Vector3 unitTargetPosition = closestTarget.transform.position;
                    float currentTargetDistance = Vector3.Distance(closestTargetPosition, this.transform.position);
                    float potentialTargetDistance = Vector3.Distance(unitTargetPosition, this.transform.position);
                    if (currentTargetDistance > potentialTargetDistance)
                    {
                        closestTarget = unitTarget;
                    }
                }
                targetUnit = closestTarget;
                SetAIState();
            }

        }
        void SetAIState()
        {
            aiHandler.SetCurrentAIState(this);
        }
        public void ActivateState()
        {
            isActive = true;
        }

        public void CancelState()
        {
            isActive = false;
        }

        public bool IsRunning()
        {
            return isActive;
        }
    }
}

