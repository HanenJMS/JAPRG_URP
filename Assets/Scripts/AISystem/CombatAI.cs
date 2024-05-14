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
            unit.GetCombatHandler().onEnemyAdded += OnEnemyAdded;
            unit.GetCombatHandler().onEnemyRemoved += OnEnemyRemoved;
        }
        void OnEnemyAdded(Unit enemy)
        {
            unit.GetPartyHandler().AddFoe(enemy);
            ActivateState();
        }
        void OnEnemyRemoved(Unit enemy)
        {
            unit.GetPartyHandler().RemoveFoe(enemy);
        }
        void OnDamageTaken()
        {
            ActivateState();
        }
        private void LateUpdate()
        {
            if (unit.GetCombatHandler().GetEnemies().Count <= 0)
            {
                CancelState();
            }
            if (unit.GetHealthHandler().IsDead())
            {
                CancelState();
                return;
            }
            if (!isActive) return;
            if (targetUnit != null)
            {
                if (targetUnit.GetHealthHandler().IsDead())
                {
                    unit.GetCombatHandler().RemoveEnemy(targetUnit);
                    targetUnit = null;
                }
                if (!isActive)
                {
                    attackAction.Cancel();
                    return;
                }
                attackAction.ExecuteOnTarget(targetUnit);
            }

            if (unit.GetCombatHandler().GetEnemies().Count > 0)
            {
                targetUnit = unit.GetCombatHandler().GetNearestEnemy();
                unit.GetCombatHandler().SetEnemy(targetUnit);
                SetAIState();
            }
            
            
        }
        void SetAIState()
        {
            if (aiHandler != null)
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

