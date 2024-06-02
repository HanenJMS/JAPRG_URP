using GameLab.UISystem;
using GameLab.UnitSystem.ActionSystem;
using System;
using UnityEngine;

namespace GameLab.UnitSystem.AbilitySystem
{
    public class AbilityAction : MonoBehaviour, IAction
    {
        AbilityHandler abilityHandler;
        Unit unit;
        GameObject target;
        private void Awake()
        {
            unit = GetComponent<Unit>();
        }
        public string ActionName()
        {
            throw new NotImplementedException();
        }

        public void Cancel()
        {
            throw new NotImplementedException();
        }

        public bool CanExecuteOnTarget(object target)
        {
            throw new NotImplementedException();
        }

        public void ExecuteOnTarget(object target)
        {
            throw new NotImplementedException();
        }
        public override string ToString()
        {
            return ActionName();
        }

        public MouseCursorData GetMouseCursorInfo()
        {
            throw new NotImplementedException();
        }
    }
}
