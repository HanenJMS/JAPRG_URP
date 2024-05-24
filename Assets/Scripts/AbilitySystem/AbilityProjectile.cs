using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;

namespace GameLab.UnitSystem.AbilitySystem
{
    public class AbilityProjectile : MonoBehaviour
    {
        Unit castor;
        Unit target;
        AbilityData castedAbility;
        [SerializeField] bool destroyOnImpact = false;
        [SerializeField] float projectileSpeed = 20f;
        private void Start()
        {
            Destroy(gameObject, 5f);
        }
        private void LateUpdate()
        {
            transform.position += transform.forward * Time.deltaTime * castedAbility.GetProjectileSpeed();
           
        }

        public void SetDamageOnImpact(Unit castor, Unit onTarget, AbilityData used)
        {
            this.castor = castor;
            this.target = onTarget;
            castedAbility = used;
        }
        private void OnTriggerEnter(Collider other)
        {
            if (other.GetComponent<Unit>() == castor) return;
            if (other.GetComponent<AbilityProjectile>() != null) return;
            if (other.GetComponent<Unit>() == target || other.GetComponent<Unit>() != castor)
            {
                if (other.GetComponent<Unit>() == target)
                {
                    target.GetCombatHandler().TakeDamage(castor, castedAbility.GetAbilityPower());
                }
                GetComponent<AbilityProjectileVFX>().SpawnImpactVFX(other);
            }
            if (destroyOnImpact)
                Destroy(gameObject);
        }
    }
}


