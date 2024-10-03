using UnityEngine;

namespace GameLab.InventorySystem.WorldSpace
{
    public class EquipmentWorldSpaceContainer : MonoBehaviour
    {
        [SerializeField] EquipmentType equipmentType;
        public EquipmentType GetEquipmentType()
        {
            return equipmentType;
        }
    }

}
