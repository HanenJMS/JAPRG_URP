using UnityEngine;

namespace GameLab.UnitSystem.AbilitySystem
{
    [System.Serializable]
    internal class Ability
    {
        [SerializeField] AbilityData abilityData;
        
        [SerializeField] int requiredExperience;

    }
}
