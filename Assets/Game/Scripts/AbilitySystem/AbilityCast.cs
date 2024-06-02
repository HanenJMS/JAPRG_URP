using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
namespace GameLab.UnitSystem.AbilitySystem
{
    public class AbilityCast : MonoBehaviour
    {
        [SerializeField] GameObject abilityProjectile;
        [SerializeField] Transform castHolder;
        [SerializeField] int castMultipleProjectiles = 1;
        [SerializeField] int multiCast = 1;
        [SerializeField] float duration = 3f;
        [SerializeField] float time = 0f;
        [SerializeField] ParticleSystem ps;


        [SerializeField] AbilityData abilityData;
        Unit castor;
        Unit target;
        [SerializeField] DamageOnImpact im;
        private void Start()
        {
            StartCoroutine(Cast());
            StartCoroutine(ShootProjectile());
        }
        public void SetCastInformation(Unit castor, Unit target, AbilityData ability)
        {
            this.castor = castor;
            this.target = target;
            this.abilityData = ability;
        }
        public void SetProjectile(GameObject projectile)
        {
            abilityProjectile = projectile;
        }
        IEnumerator Cast()
        {
            for (int i = 0; i < multiCast-1; i++)
            {
                yield return new WaitForSeconds(duration);
                multiCast--;
                GameObject cast = Instantiate(gameObject, this.transform.position + (transform.forward * 0.1f), this.transform.rotation);
                cast.GetComponent<AbilityCast>().SetCastInformation(castor, target, abilityData);

                //encircling cast
                //GameObject cast = Instantiate(gameObject, target.transform.position + UnityEngine.Random.insideUnitSphere * Mathf.Sqrt(abilityData.GetRange()) + Vector3.up * Mathf.Sqrt(abilityData.GetRange()), this.transform.rotation);
                //cast.transform.LookAt(target.transform.position);
                //cast.GetComponent<AbilityCast>().SetCastInformation(castor, target, abilityData);

            }
            
        }
        IEnumerator ShootProjectile()
        {
            yield return new WaitForSeconds(duration);
            for(int i = 0; i < castMultipleProjectiles; i++)
            {
                GameObject impact = Instantiate(abilityProjectile, castHolder.position, castHolder.rotation);
                
                impact.GetComponent<AbilityProjectile>().SetDamageOnImpact(castor, target, abilityData);

                yield return new WaitForSeconds(0.37f);
            }
        }
    }
}

