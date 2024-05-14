using GameLab.Animation;
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
        [SerializeField] Unit enemy;
        [SerializeField] List<Unit> enemies = new();
        UnitAnimationHandler animationHandler;
        [SerializeField] float range = 1f;
        [SerializeField] int damage = 1;
        bool isRunning = false;
        public Action onDamageTaken;
        public Action<Unit> onEnemyAdded;
        public Action<Unit> onEnemyRemoved;
        float currentAttackCD = float.MaxValue;
        [SerializeField] float AttackCD = 2f;
        private void Awake()
        {
            unit = GetComponent<Unit>();
            animationHandler = GetComponent<UnitAnimationHandler>();
        }
        public void AddEnemy(Unit enemy)
        {
            if (!enemies.Contains(enemy) && !enemy.GetHealthHandler().IsDead())
            {
                enemies.Add(enemy);
                onEnemyAdded?.Invoke(enemy);
            }
        }
        public void RemoveEnemy(Unit enemy)
        {
            if (enemies.Contains(enemy))
            {
                enemies.Remove(enemy);
                onEnemyRemoved?.Invoke(enemy);
            }
        }
        public void SetEnemy(Unit enemy)
        {
            this.enemy = enemy;
            AddEnemy(enemy);
        }
        void TakeDamage(Unit enemy, int dmg)
        {
            unit.GetHealthHandler().RemoveFromCurrent(dmg);
            AddEnemy(enemy);
            onDamageTaken?.Invoke();
        }
        public List<Unit> GetEnemies()
        {
            return enemies;
        }
        public void SetEnemies(List<Unit> enemies)
        {
            this.enemies = enemies;
            if(this.enemy == null)
                SetEnemy(this.enemies[0]);
            RunCombat();
        }
        public void RunCombat()
        {
            isRunning = true;
            
        }
        public float GetActionRange()
        {
            return range;
        }
        public Unit GetNearestEnemy()
        {
            Unit enemy = enemies[0];
            Vector3 closestEnemyPosition = enemy.transform.position;
            foreach (Unit potentialEnemy in enemies)
            {
                Vector3 potentialEnemyPosition = potentialEnemy.transform.position;
                float currentEnemyPositionDistance = Vector3.Distance(closestEnemyPosition, this.transform.position);
                float potentialEnemyDistance = Vector3.Distance(potentialEnemyPosition, this.transform.position);
                if (currentEnemyPositionDistance > potentialEnemyDistance)
                {
                    enemy = potentialEnemy;
                }
            }
            return enemy;
        }
        public void Cancel()
        {
            isRunning = false;
            enemy = null;
            enemies.Clear();
        }


        private void LateUpdate()
        {
            currentAttackCD += Time.deltaTime;
            if (!isRunning) return;
            if (unit.GetHealthHandler().IsDead())
            {
                Cancel();
                return;
            }
            if (enemy == unit)
            {
                RemoveEnemy(enemy);
            }
            if (enemy != null)
            {
                if (Vector3.Distance(this.transform.position, enemy.transform.position) > range)
                {
                    unit.GetActionHandler().GetActionType<MoveAction>().MoveToDestination(enemy);
                }
                else
                {
                    unit.GetActionHandler().GetActionType<MoveAction>().Cancel();
                    transform.LookAt(enemy.transform);
                    if (currentAttackCD > AttackCD)
                    {
                        if (enemy.GetHealthHandler().IsDead())
                        {
                            RemoveEnemy(enemy);
                            enemy = null;
                            return;
                        }
                        animationHandler.SetTrigger("attack");
                        currentAttackCD = 0f;
                    }
                }
            }
            if (enemies.Count == 0)
            {
                Cancel();
                return;
            }
        }

        //animation trigger
        void Hit()
        {
            if (enemy == null)
            {
                Debug.Log("Miss");
                return;
            }

            enemy.GetCombatHandler().TakeDamage(unit, damage);

            Debug.Log("HIT!");
        }
    }
}
