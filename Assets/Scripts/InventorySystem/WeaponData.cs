using UnityEngine;
namespace GameLab.InventorySystem
{
    [CreateAssetMenu(fileName = "Weapon", menuName = "Item/Weapon/new Weapon")]
    public class WeaponData : ItemData
    {
        [SerializeField] int weaponDamage = 10;
        [SerializeField] AnimatorOverrideController animationOverrider;

        public AnimatorOverrideController AnimatorOverrideController() => animationOverrider;
    }
}

