using GameLab.Animation;
using System;
using System.Collections.Generic;
using UnityEngine;
namespace GameLab.UnitSystem.ActionSystem
{
    public class AbilityHandler : MonoBehaviour
    {
        [SerializeField] List<AbilityData> abilities = new();
        [SerializeField] AbilityData currentAbility;
        [SerializeField] int selectedAbility = 0;
        [SerializeField] Unit unit;
        private void Awake()
        {
            unit = GetComponent<Unit>();
        }
        public AbilityData GetAbility()
        {
            return abilities[selectedAbility];
        }
        public void SetCurrentAbility(int ability)
        {
            if (abilities.Count < ability) return;
            selectedAbility = ability;
        }
        public void SetDefaultAbility(AbilityData ability)
        {
            List<AbilityData> newAbilityList = new();
            newAbilityList.Add(ability);
            for(int i = 1;  i < abilities.Count; i++)
            {
                newAbilityList.Add(abilities[i]);
            }
            abilities = newAbilityList;
        }
        internal void UpdateAbility()
        {
            GetComponent<UnitAnimationHandler>().SetAbilityAnimationOverrider(abilities[selectedAbility].GetAnimation());
        }
    }
}
