using GameLab.CombatSystem;
using GameLab.UISystem;
using GameLab.UnitSystem.AbilitySystem;
using UnityEngine;

namespace GameLab.UnitSystem.ActionSystem
{
    public class AttackAction : BaseAction
    {
        CombatHandler combatHandler;
        ActionHandler actionHandler;
        AbilityHandler abilityHandler;
        Unit selfUnit;
        private void Awake()
        {
            combatHandler = GetComponent<CombatHandler>();
            actionHandler = GetComponent<ActionHandler>();
            abilityHandler = GetComponent<AbilityHandler>();
            selfUnit = GetComponent<Unit>();
        }
        int damage = 5;
        public override void ExecuteOnTarget(object target)
        {
            if (target is Unit)
            {
                combatHandler.SetEnemy(target as Unit);
                abilityHandler.UpdateAbility();
                actionHandler.SetCurrentAction(this);
            }

        }

        public override bool CanExecuteOnTarget(object target)
        {
            if(target is Unit)
            {
                var unit = target as Unit;
                if (unit.GetHealthHandler().IsDead()) return false;
                if (unit.GetFactionHandler().GetFaction() == selfUnit.GetFactionHandler().GetFaction()) return false;
                return true;
            }
            return false;
        }
        public override void Cancel()
        {
            combatHandler.Cancel();
        }
        public override string ToString()
        {
            return "Attack";
        }
    }
}

