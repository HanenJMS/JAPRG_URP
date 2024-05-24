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
        [SerializeField] int castMultiple = 1;
        [SerializeField] float duration = 3f;
        [SerializeField] float time = 0f;
        [SerializeField] ParticleSystem ps;


        [SerializeField] AbilityData abilityData;
        Unit castor;
        Unit target;
        [SerializeField] DamageOnImpact im;
        private void Start()
        {
            StartCoroutine(CastAbility());
        }
        public void SetCastInformation(Unit castor, Unit target, AbilityData ability)
        {
            this.castor = castor;
            this.target = target;
            this.abilityData = ability;
        }
        IEnumerator CastAbility()
        {
            yield return new WaitForSeconds(duration);
            for(int i = 0; i < castMultiple; i++)
            {
                GameObject impact = Instantiate(abilityProjectile, castHolder.position, castHolder.rotation);
                
                impact.GetComponent<AbilityProjectile>().SetDamageOnImpact(castor, target, abilityData);

                yield return new WaitForSeconds(0.37f);
            }
        }
    }
}

