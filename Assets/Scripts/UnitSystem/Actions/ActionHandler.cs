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
        public T GetActionType<T>() where T : IAction
        {
            return GetComponent<T>();
        }
        public void SetCurrentAction(IAction action)
        {
            if (currentAction == action) return;
            if (currentAction != action && currentAction != null) 
            {
                currentAction.Cancel();
            }
            
            currentAction = action;
        }
        public IAction GetselectedAction(IAction action)
        {
            return currentAction;
        }
        public List<IAction> GetActions()
        {
            List<IAction> actionable = new();
            foreach (IAction action in actions)
            {
                actionable.Add(action);
            }
            return actionable;
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

