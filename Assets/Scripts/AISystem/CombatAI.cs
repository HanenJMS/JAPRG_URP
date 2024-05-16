using GameLab.UnitSystem;
using GameLab.UnitSystem.ActionSystem;
using System.Collections.Generic;
using UnityEngine;

namespace GameLab.AI
{

    //TODO put combat ai on a higher dependency level, so that players can interact with it. Combathandler should only handle single targetting issues. Such as Setting a single target to chase, and attack.
    //Combat AI should handle points of multiple threats and other things.
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
                    unit.GetCombatHandler().Cancel();
                    targetUnit = null;
                }
                if (unit.GetPartyHandler().GetAllies().Contains(targetUnit))
                {
                    unit.GetCombatHandler().RemoveEnemy(targetUnit);
                    unit.GetCombatHandler().Cancel();
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
                if (!unit.GetPartyHandler().GetAllies().Contains(targetUnit))
                {
                    unit.GetCombatHandler().SetEnemy(targetUnit);
                    SetAIState();
                    return;
                }
                unit.GetCombatHandler().RemoveEnemy(targetUnit);
                targetUnit = null;
                
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

