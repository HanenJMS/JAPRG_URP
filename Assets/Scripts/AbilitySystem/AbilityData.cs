using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameLab.UnitSystem.ActionSystem
{
    [System.Serializable]
    [CreateAssetMenu(fileName = "Ability", menuName = "Ability/new Ability")]
    public class AbilityData : ScriptableObject
    {
        [SerializeField] AnimatorOverrideController AnimatorOverrideController;

        [SerializeField] GameObject abilityVFX;

        [SerializeField] int pow;

        [SerializeField] AnimationClip animationClip;
        public AnimatorOverrideController GetAnimation() => AnimatorOverrideController;
        public GameObject GetAbilityVFX() => abilityVFX;
        public AnimationClip GetAnimationClip() => animationClip;

    }
}

