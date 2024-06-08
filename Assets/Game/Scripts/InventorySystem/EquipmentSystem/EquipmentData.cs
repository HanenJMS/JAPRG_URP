using UnityEngine;
namespace GameLab.InventorySystem
{
    public class EquipmentData : ItemData
    {

        [SerializeField] EquipmentType equipmentType;
        public EquipmentType GetEquipmentType()
        {
            return equipmentType;
        }
    }
}