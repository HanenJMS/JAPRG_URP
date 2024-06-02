using UnityEngine;

namespace GameLab.InventorySystem
{
    public abstract class ItemData : ScriptableObject
    {
        [TextArea]
        [SerializeField] string description;
        [SerializeField] Sprite itemSprite;
        [SerializeField] GameObject itemModelPrefab;
        [SerializeField] GameObject itemPickupPrefab;

        public string GetDescription()
        {
            return description;
        }

        public GameObject GetItemPrefab()
        {
            return itemModelPrefab;
        }
        public GameObject GetItemPickupPrefab()
        {
            return itemPickupPrefab;
        }
        public Sprite GetItemSprite()
        {
            return itemSprite;
        }
    }
}


