using GameLab.Animation;
using GameLab.PartySystem;
using GameLab.UnitSystem;
using GameLab.UnitSystem.ActionSystem;
using System;
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
        [SerializeField] List<Unit> combatTargets = new();
        PartyHandler partyHandler;
        UnitAnimationHandler animationHandler;
        [SerializeField] float range = 1f;
        [SerializeField] int damage = 1;
        bool isRunning = false;
        public Action onDamageTaken;
        public Action onCombatInitiated;
        float currentAttackCD = float.MaxValue;
        [SerializeField] float AttackCD = 2f;
        private void Awake()
        {
            unit = GetComponent<Unit>();
            animationHandler = GetComponent<UnitAnimationHandler>();
            partyHandler = GetComponent<PartyHandler>();
        }
        public void SetCombatTarget(Unit combatTarget)
        {
            this.combatTarget = combatTarget;
            if (!combatTargets.Contains(combatTarget))
            {
                combatTargets.Add(combatTarget);
                partyHandler.AddEnemy(combatTarget);
            }
            isRunning = true;
            onCombatInitiated?.Invoke();
        }
        public void TakeDamage(Unit attacker, int dmg)
        {
            unit.GetHealthHandler().RemoveFromCurrent(dmg);
            if (!combatTargets.Contains(attacker) && !attacker.GetHealthHandler().IsDead())
            {
                combatTargets.Add(attacker);
                partyHandler.AddEnemy(attacker);
            }
            onDamageTaken?.Invoke();
        }
        public List<Unit> GetCombatTargets()
        {
            return combatTargets;
        }
        public void RemoveTarget(Unit target)
        {
            if (combatTargets.Contains(target))
            {
                combatTargets.Remove(target);
                partyHandler.RemoveEnemy(target);
            }
        }
        public float GetActionRange()
        {
            return range;
        }
        public void Cancel()
        {
            isRunning = false;
            combatTarget = null;
            combatTargets.Clear();
        }


        private void LateUpdate()
        {
            currentAttackCD += Time.deltaTime;
            if (!isRunning) return;
            if(unit.GetHealthHandler().IsDead())
            {
                isRunning = false;
                return;
            }
            if (combatTarget != null)
            {
                if (Vector3.Distance(this.transform.position, combatTarget.transform.position) > range)
                {
                    unit.GetActionHandler().GetActionType<MoveAction>().MoveToDestination(combatTarget);
                }
                else
                {
                    unit.GetActionHandler().GetActionType<MoveAction>().Cancel();
                    transform.LookAt(combatTarget.transform);
                    if (currentAttackCD > AttackCD)
                    {
                        if (combatTarget.GetHealthHandler().IsDead())
                        {
                            combatTarget = null;
                            isRunning = false;
                            return;
                        }
                        animationHandler.SetTrigger("attack");
                        currentAttackCD = 0f;
                    }
                }
            }

        }

        //animation trigger
        void Hit()
        {
            if (combatTarget == null)
            {
                Debug.Log("Miss");
                return;
            }

            combatTarget.GetCombatHandler().TakeDamage(unit, damage);

            Debug.Log("HIT!");
        }
    }
}
