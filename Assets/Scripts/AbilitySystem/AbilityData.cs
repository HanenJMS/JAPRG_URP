using Unity.VisualScripting;
using UnityEngine;

namespace GameLab.UnitSystem.AbilitySystem
{
    [System.Serializable]
    [CreateAssetMenu(fileName = "Ability", menuName = "Ability/new Ability")]
    public class AbilityData : ScriptableObject
    {
        [SerializeField] AnimatorOverrideController AnimatorOverrideController;

        [SerializeField] GameObject abilityVFX_self;

        [SerializeField] GameObject abilityVFX_target;

        [SerializeField] float range;
        [SerializeField] float cooldown;
        [SerializeField] int pow;
        [SerializeField] AnimationClip animationClip;
        public AnimatorOverrideController GetAnimation()
        {
            return AnimatorOverrideController;
        }

        public GameObject GetSelfCastVFX()
        {
            return abilityVFX_self;
        }

        public GameObject GetTargetCastVFX()
        {
            return abilityVFX_target;
        }

        public AnimationClip GetAnimationClip()
        {
            return animationClip;
        }
        public int GetAbilityPower() => pow;
        public float GetRange()
        {
            return range;
        }
    }
}

