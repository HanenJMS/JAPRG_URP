using GameLab.Animation;
using GameLab.UnitSystem;
using UnityEngine;


namespace GameLab.InventorySystem
{
    public class EquipmentHandler : MonoBehaviour
    {
        [SerializeField] Transform RightHandWeaponHolder;
        [SerializeField] Transform LeftHandWeaponHolder;
        [SerializeField] WeaponData unarmedData;

        [SerializeField] WeaponData rightWeapon;
        [SerializeField] GameObject rightWeaponDrawn;
        [SerializeField] WeaponData currentWeapon;

        [SerializeField] Unit unit;
        private void Awake()
        {
            unit = GetComponent<Unit>();
        }
        void CombatDraw()
        {
            EquipAnimation();
            EquipWeaponAbility();
        }
        public void DrawWeapon()
        {
            if(rightWeapon != null)
            {
                UndrawWeapon();
                rightWeaponDrawn = Instantiate(rightWeapon.GetItemPrefab(), RightHandWeaponHolder);
                currentWeapon = rightWeapon;
            }
            else
            {
                currentWeapon = unarmedData;
            }
            CombatDraw();
        }
        public void EquipWeaponAbility()
        {
            unit.GetAbilityHandler().SetDefaultAbility(currentWeapon.GetDefaultAbility());
        }
        public void UndrawWeapon()
        {
            if (rightWeaponDrawn == null) return;
            Destroy(rightWeaponDrawn.gameObject);
            rightWeaponDrawn = null;
            currentWeapon = unarmedData;
            CombatDraw();
        }
        public void EquipAnimation()
        {
            GetComponent<UnitAnimationHandler>().SetAnimationOverrideController(currentWeapon.AnimatorOverrideController());
        }
        public void WithdrawCombat()
        {
            UndrawWeapon();
            GetComponent<UnitAnimationHandler>().SetDefaultAnimationController();
            EquipWeaponAbility();
        }
        public WeaponData GetRightWeapon()
        {
            return rightWeapon;
        }

    }
}

