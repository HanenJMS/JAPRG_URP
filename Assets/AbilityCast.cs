using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace GameLab.UnitSystem.AbilitySystem
{
    public class AbilityCast : MonoBehaviour
    {
        [SerializeField] GameObject abilityProjectile;
        [SerializeField] Transform castHolder;
        [SerializeField] float duration = 3f;
        [SerializeField] float time = 0f;
        [SerializeField] ParticleSystem ps;
        [SerializeField] AbilityData abilityData;
        private void Start()
        {
            StartCoroutine(CastAbility());
        }
        IEnumerator CastAbility()
        {
            yield return new WaitForSeconds(duration);
            Instantiate(abilityProjectile, castHolder.position, castHolder.rotation);
        }
    }
}

