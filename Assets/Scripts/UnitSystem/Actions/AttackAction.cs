using GameLab.CombatSystem;
using GameLab.ResourceSystem;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

namespace GameLab.UnitSystem.ActionSystem
{
    public class AttackAction : MonoBehaviour,IAction
    {

        Unit targetUnit;
        CombatHandler combatHandler;
        private void Awake()
        {
            combatHandler = GetComponent<CombatHandler>();
        }
        int damage = 5;
        public void ExecuteOnTarget(object target)
        {
            if(target is Unit)
            {
                combatHandler.SetCombatTarget(target as Unit);
            }
           
        }

        public bool CanExecuteOnTarget(object target)
        {
            if(target is Unit)
            {
                return Vector3.Distance(this.transform.position, (target as Unit).transform.position) < combatHandler.GetActionRange();
            }
            return false;
        }

        public bool IsInRange()
        {
            return Vector3.Distance(targetUnit.transform.position, this.transform.position) < 1f;
        }
        public void Cancel()
        {
            
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

