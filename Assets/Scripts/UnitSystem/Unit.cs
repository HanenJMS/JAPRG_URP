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
        object target;
        IAction moveAction;
        IAction attackAction;
        private void Awake()
        {
            moveAction = GetComponent<MoveAction>();
            attackAction = GetComponent<AttackAction>();
            actionHandler = GetComponent<ActionHandler>();
            healthHandler = GetComponent<HealthHandler>();
        }
        public void SetTarget(object target)
        {
            this.target = target;
        }
        public ActionHandler GetActionHandler()
        {
            return actionHandler;
        }
        public HealthHandler GetHealthHandler()
        {
            return healthHandler;
        }
    }
}

