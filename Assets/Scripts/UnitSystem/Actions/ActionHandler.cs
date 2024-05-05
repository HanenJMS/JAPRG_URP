using System.Collections.Generic;
using UnityEngine;
namespace GameLab.UnitSystem.ActionSystem
{
    public class ActionHandler : MonoBehaviour
    {
        IAction currentAction;
        IAction nextAction;
        IAction selectedAction;
        IAction[] actions;


        private void Awake()
        {
            actions = GetComponents<IAction>();
            foreach(IAction action in actions)
            {
                Debug.Log(action.ToString());
            }
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
        public List<IAction> GetExecutableActions(object target)
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

