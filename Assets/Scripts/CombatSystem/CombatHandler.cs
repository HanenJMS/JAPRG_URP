using GameLab.Animation;
using GameLab.UnitSystem;
using GameLab.UnitSystem.ActionSystem;
using UnityEngine;
namespace GameLab.CombatSystem
{
    public class CombatHandler : MonoBehaviour
    {
        //resides in unit world objects.
        //takes in damage information and processes them into their correct delegates; DamageCalculator(damageInfo)
        Unit unit;
        Unit combatTarget;

        UnitAnimationHandler animationHandler;
        [SerializeField] float range = 1f;
        [SerializeField] int damage = 1;
        bool isRunning = false;

        float currentAttackCD = float.MaxValue;
        [SerializeField] float AttackCD = 2f;
        private void Awake()
        {
            unit = GetComponent<Unit>();
            animationHandler = GetComponent<UnitAnimationHandler>();
        }
        public void SetCombatTarget(Unit combatTarget)
        {
            this.combatTarget = combatTarget;
            isRunning = true;
        }
        public float GetActionRange()
        {
            return range;
        }
        public void Cancel()
        {
            isRunning = false;
            combatTarget = null;
        }


        private void LateUpdate()
        {
            currentAttackCD += Time.deltaTime;
            if (!isRunning) return;
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
                    if(currentAttackCD > AttackCD) 
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
            
            combatTarget.GetHealthHandler().RemoveFromCurrent(damage);
            Debug.Log("HIT!");
        }
    }
}
