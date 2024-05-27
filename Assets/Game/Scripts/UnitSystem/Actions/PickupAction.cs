using GameLab.InventorySystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameLab.UnitSystem.ActionSystem
{
    public class PickupAction : MonoBehaviour, IAction
    {
        public string ActionName()
        {
            return "Pick up";
        }

        public void Cancel()
        {
            
        }

        public bool CanExecuteOnTarget(object target)
        {
            if (target is not ItemWorld) return false;
            return true;
        }

        public void ExecuteOnTarget(object target)
        {
            throw new System.NotImplementedException();
        }
        public override string ToString()
        {
            return ActionName();
        }
    }
}

