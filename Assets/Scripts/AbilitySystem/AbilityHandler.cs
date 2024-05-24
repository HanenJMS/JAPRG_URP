using GameLab.Animation;
using System.Collections.Generic;
using UnityEngine;
namespace GameLab.UnitSystem.AbilitySystem
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
        public AbilityData GetAbility(int index)
        {
            return abilities[index];
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
            for (int i = 1; i < abilities.Count; i++)
            {
                newAbilityList.Add(abilities[i]);
            }
            abilities = newAbilityList;
        }
        internal void UpdateAbility()
        {
            GetComponent<UnitAnimationHandler>().SetAbilityAnimationOverrider(abilities[selectedAbility].GetAnimation());
        }


        public void CastAbility(Unit target)
        {
            GameObject selfCastVFX = GetAbility().GetSelfCastVFX();
            CastVFX(selfCastVFX, this.transform.position, target);

            GameObject targetCastVFX = GetAbility().GetTargetCastVFX();
            CastVFX(targetCastVFX, target.gameObject.transform.position, target);

        }
        public void CastVFX(GameObject vfx, Vector3 target, Unit unitTarget = null)
        {
            if (vfx == null) return;
            GameObject impact = Instantiate(vfx, target, this.transform.rotation);
            impact.GetComponent<AbilityCast>().SetCastInformation(unit, unitTarget, GetAbility());
        }
    }
}
