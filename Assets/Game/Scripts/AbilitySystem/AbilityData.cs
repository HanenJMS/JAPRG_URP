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

        [SerializeField] AbilityType abilityType;
        [SerializeField] float range;
        [SerializeField] float cooldown;
        [SerializeField] int pow;
        [SerializeField] float projectileSpeed;
        [SerializeField] bool isMelee;
        [SerializeField] bool isContinuous;
        [SerializeField] AnimationClip animationClip;
        [SerializeField] GameObject damageWorldText; 
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
        public GameObject GetDamageWorldText() => damageWorldText;
        public AbilityType GetAbilityType() => abilityType;
        public int GetAbilityPower() => pow;
        public float GetProjectileSpeed() => projectileSpeed;
        public bool GetIsMelee() => isMelee;
        public bool GetIsContinuous() => isContinuous;
        public float GetRange()
        {
            return range;
        }
    }
}

