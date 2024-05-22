using UnityEngine;

namespace GameLab.UnitSystem.AbilitySystem
{
    [System.Serializable]
    internal class Ability
    {
        AbilityData abilityData;
        [SerializeField] float cooldown;
        [SerializeField] float currentCooldown;



    }
}
