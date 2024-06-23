using GameLab.InventorySystem.WorldSpace;
using System.Collections.Generic;
using UnityEngine;


namespace GameLab.InventorySystem
{
    [System.Serializable]
    public class UnitEquipmentWorldHolder : MonoBehaviour
    {
        EquipmentHandler equipmentHandler;

        Dictionary<EquipmentType, GameObject> equippedItemReferences = new();
        Dictionary<EquipmentType, GameObject> equipmentSlotReferences = new();

        private void Awake()
        {
            foreach(EquipmentWorldSpaceContainer ewsc in GetComponentsInChildren<EquipmentWorldSpaceContainer>())
            {
                equipmentSlotReferences.Add(ewsc.GetEquipmentType(), ewsc.gameObject);
                equippedItemReferences.Add(ewsc.GetEquipmentType(), null);
            }

            equipmentHandler = GetComponent<EquipmentHandler>();
        }


        public void EquipItem(EquipmentData equipment)
        {

            UnequipItem(equipment.GetEquipmentType());
            equippedItemReferences[equipment.GetEquipmentType()] = Instantiate(equipment.GetItemPrefab(), equipmentSlotReferences[equipment.GetEquipmentType()].transform);
        }

        public void UnequipItem(EquipmentType equipmentType)
        {
            if (equippedItemReferences[equipmentType] != null) Destroy(equippedItemReferences[equipmentType]);
        }

        public void DrawWeapon()
        {
            UnequipItem(EquipmentType.Main);
            equippedItemReferences[EquipmentType.Main] = Instantiate(equipmentHandler.GetInventory().GetCurrentWeaponDrawn().GetItemPrefab(), equipmentSlotReferences[EquipmentType.Main].transform);
        }
    }
}

