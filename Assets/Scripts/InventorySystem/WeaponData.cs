using GameLab.UnitSystem.ActionSystem;
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
        public AnimatorOverrideController AnimatorOverrideController() => animationOverrider;
        public GameObject GetWeaponEffect() => weaponEffect;
        public AbilityData GetDefaultAbility() => defaultAbility;
    }
}

