using GameLab.Animation;
using GameLab.FactionSystem;
using GameLab.InteractableSystem;
using GameLab.ResourceSystem;
using GameLab.UnitSystem.ActionSystem;

namespace GameLab.UnitSystem
{
    [System.Serializable]
    public class Unit : Interactable
    {
        ActionHandler actionHandler;
        HealthHandler healthHandler;
        FactionHandler factionHandler;
        UnitAnimationHandler unitAnimationHandler;
        object target;
        IAction moveAction;
        IAction attackAction;
        private void Awake()
        {
            moveAction = GetComponent<MoveAction>();
            attackAction = GetComponent<AttackAction>();
            actionHandler = GetComponent<ActionHandler>();
            healthHandler = GetComponent<HealthHandler>();
            factionHandler = GetComponent<FactionHandler>();
            unitAnimationHandler = GetComponent<UnitAnimationHandler>();
            healthHandler.onDead += OnDeath;
        }

        void OnDeath()
        {
            actionHandler.SetCurrentAction(null);
            unitAnimationHandler.SetTrigger("death");
        }
        public ActionHandler GetActionHandler()
        {
            return actionHandler;
        }
        public HealthHandler GetHealthHandler()
        {
            return healthHandler;
        }
        public FactionHandler GetFactionHandler()
        {
            return factionHandler;
        }
    }
}

