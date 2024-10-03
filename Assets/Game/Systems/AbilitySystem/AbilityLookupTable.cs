using System.Collections.Generic;
using UnityEngine;

namespace GameLab.UnitSystem.AbilitySystem
{
    [CreateAssetMenu(fileName = "AbilityTable", menuName = "AbilityTable/new Table")]
    public class AbilityLookupTable : ScriptableObject
    {
        [SerializeField] List<Ability> abilityTree = new();

    }
}

