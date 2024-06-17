using System.Collections.Generic;
using UnityEngine;


namespace GameLab.InventorySystem
{
    [System.Serializable]
    public class UnitEquipmentWorldHolder : MonoBehaviour
    {
        EquipmentHandler equipmentHandler;
        [SerializeField] GameObject headSlot;
        [SerializeField] GameObject bodySlot;
        [SerializeField] GameObject bootSlot;
        [SerializeField] GameObject mainHand;
        [SerializeField] GameObject offHand;

        Dictionary<EquipmentType, GameObject> equippedItemReferences = new();
        Dictionary<EquipmentType, GameObject> equipmentSlotReferences = new();

        private void Awake()
        {
            equipmentSlotReferences.Add(EquipmentType.Head, headSlot);

            equipmentSlotReferences.Add(EquipmentType.Body, bodySlot);

            equipmentSlotReferences.Add(EquipmentType.Boots, bootSlot);

            equipmentSlotReferences.Add(EquipmentType.Main, mainHand);

            equipmentSlotReferences.Add(EquipmentType.OffHand, offHand);


            equippedItemReferences.Add(EquipmentType.Head, null);

            equippedItemReferences.Add(EquipmentType.Body, null);

            equippedItemReferences.Add(EquipmentType.Boots, null);

            equippedItemReferences.Add(EquipmentType.Main, null);

            equippedItemReferences.Add(EquipmentType.OffHand, null);

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

