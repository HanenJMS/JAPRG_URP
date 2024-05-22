using GameLab.UnitSystem.AbilitySystem;
using UnityEngine;
namespace GameLab.InventorySystem
{
    [CreateAssetMenu(fileName = "Weapon", menuName = "Item/Weapon/new Weapon")]
    public class WeaponData : ItemData
    {
        [SerializeField] int weaponDamage = 10;
        [SerializeField] AnimatorOverrideController animationOverrider;
        [SerializeField] AbilityData defaultAbility;
        [SerializeField] GameObject weaponEffect;
        public AnimatorOverrideController AnimatorOverrideController()
        {
            return animationOverrider;
        }

        public GameObject GetWeaponEffect()
        {
            return weaponEffect;
        }

        public AbilityData GetDefaultAbility()
        {
            return defaultAbility;
        }
    }
}

