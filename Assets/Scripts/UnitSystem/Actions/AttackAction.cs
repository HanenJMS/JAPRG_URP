using GameLab.CombatSystem;
using UnityEngine;

namespace GameLab.UnitSystem.ActionSystem
{
    public class AttackAction : MonoBehaviour, IAction
    {
        CombatHandler combatHandler;
        ActionHandler actionHandler;
        Unit selfUnit;
        private void Awake()
        {
            combatHandler = GetComponent<CombatHandler>();
            actionHandler = GetComponent<ActionHandler>();
            selfUnit = GetComponent<Unit>();
        }
        int damage = 5;
        public void ExecuteOnTarget(object target)
        {
            if (target is Unit)
            {
                combatHandler.SetCombatTarget(target as Unit);
                actionHandler.SetCurrentAction(this);
            }

        }

        public bool CanExecuteOnTarget(object target)
        {
            if (target is Unit && !(target as Unit).GetHealthHandler().IsDead() ) return true;
            return false;
        }
        public void Cancel()
        {
            combatHandler.Cancel();
        }

        public string ActionName()
        {
            return this.ToString();
        }
        public override string ToString()
        {
            return "Attack";
        }
    }
}

