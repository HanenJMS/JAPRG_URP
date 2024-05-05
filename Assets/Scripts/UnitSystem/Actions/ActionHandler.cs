using System.Collections.Generic;
using UnityEngine;
namespace GameLab.UnitSystem.ActionSystem
{
    public class ActionHandler : MonoBehaviour
    {
        IAction currentAction;
        IAction nextAction;
        IAction selectedAction;
        List<IAction> equippedActions = new();
        IAction[] actions;


        private void Awake()
        {
            actions = GetComponents<IAction>();
        }

        public void SetCurrentAction(IAction action)
        {
            currentAction = action;
        }
        public IAction GetselectedAction(IAction action)
        {
            return currentAction;
        }
        public IAction[] GetActions()
        {
            return actions;
        }
        public List<IAction> ExecutableActions(object target)
        {
            List<IAction> actionable = new();
            foreach (IAction action in actions)
            {
                if (action.CanExecuteOnTarget(target))
                {
                    actionable.Add(action);
                }
            }
            return actionable;
        }
    }
}

