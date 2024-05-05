using GameLab.UnitSystem;
using GameLab.UnitSystem.ActionSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace GameLab.CombatSystem
{
    public class CombatHandler : MonoBehaviour
    {
        //resides in unit world objects.
        //takes in damage information and processes them into their correct delegates; DamageCalculator(damageInfo)
        Unit unit;
        Unit combatTarget;
        float range = 1f;

        private void Awake()
        {
            unit = GetComponent<Unit>();
        }
        public void SetCombatTarget(Unit combatTarget)
        {
            this.combatTarget = combatTarget;
        }
        public float GetActionRange()
        {
            return range;
        }
        private void LateUpdate()
        {
            if(combatTarget != null)
            {
                if(Vector3.Distance(this.transform.position, combatTarget.transform.position) < range)
                {
                    unit.GetComponent<MoveAction>().ExecuteOnTarget(combatTarget);
                }
                else
                {
                    Debug.Log("Attack!!");
                }
            }
        }
    }
}
