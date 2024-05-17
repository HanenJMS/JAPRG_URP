using GameLab.Animation;
using UnityEngine;


namespace GameLab.InventorySystem
{
    public class EquipmentHandler : MonoBehaviour
    {
        [SerializeField] Transform RightHandWeaponHolder;
        [SerializeField] Transform LeftHandWeaponHolder;

        [SerializeField] WeaponData rightWeapon;
        GameObject rightWeaponEquipped;
        WeaponData leftWeapon;
        GameObject leftWeaponShown;

        WeaponData GetRightWeapon()
        {
            return rightWeapon;
        }

        WeaponData GetLeftWeapon()
        {
            return leftWeapon;
        }
        void SetRightWeapon(WeaponData weapon)
        {
            rightWeapon = weapon;
            GetComponent<UnitAnimationHandler>().SetAnimationOverrideController(weapon.AnimatorOverrideController());
        }
        public void UnSetRightWeapon()
        {
            HideWeapon();
            rightWeapon = null;
            GetComponent<UnitAnimationHandler>().SetDefaultAnimationController();
        }
        void SetLeftWeapon(WeaponData weapon)
        {
            leftWeapon = weapon;
        }

        public void ShowRightWeapon()
        {
            if (rightWeapon == null) return;
            if (rightWeaponEquipped != null)
            {
                HideWeapon();
            }
            SetRightWeapon(rightWeapon);
            rightWeaponEquipped = Instantiate(rightWeapon.GetItemPrefab(), RightHandWeaponHolder);
        }
        public void HideWeapon()
        {
            if (rightWeaponEquipped != null)
            {
                Destroy(rightWeaponEquipped.gameObject);
                rightWeaponEquipped = null;
                GetComponent<UnitAnimationHandler>().SetDefaultAnimationController();
            }
        }

    }
}

