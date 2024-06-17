using GameLab.InventorySystem;
using GameLab.UnitSystem;
using UnityEngine;
using UnityEngine.UI;

namespace GameLab.UISystem
{
    [System.Serializable]
    public class EquipmentSlotUI : MonoBehaviour
    {
        [SerializeField] EquipmentType equipmentType;
        [SerializeField] Image EquippedImage;
        [SerializeField] Button button;
        private void Awake()
        {
            if(EquippedImage == null)
            {
                EquippedImage = GetComponentInChildren<Image>();
            }
            button = GetComponent<Button>();
        }

        private void Start()
        {
            button?.onClick.AddListener(() =>
            {
                UnitSelectionSystem.Instance.GetSelectedUnit().GetEquipmentHandler().UnequipItem(equipmentType);
            });
        }
        public void SetImage(Sprite image)
        {
            EquippedImage.sprite = image;
        }

        public EquipmentType GetEquipmentType() => equipmentType;
    }
}

