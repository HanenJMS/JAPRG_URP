using GameLab.InventorySystem;
using UnityEngine;
using UnityEngine.UI;

namespace GameLab.UISystem
{
    [System.Serializable]
    public class EquipmentSlotUI : MonoBehaviour
    {
        [SerializeField] EquipmentType equipmentType;
        [SerializeField] Image EquippedImage;
        private void Awake()
        {
            if(EquippedImage == null)
            {
                EquippedImage = GetComponentInChildren<Image>();
            }
        }
        public void SetImage(Sprite image)
        {
            EquippedImage.sprite = image;
        }

        public EquipmentType GetEquipmentType() => equipmentType;
    }
}

