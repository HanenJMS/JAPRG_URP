using UnityEngine;

namespace GameLab.WorkShopSystem
{
    public class Workshop : MonoBehaviour
    {
        int storageSpace = 10000;
        int currentStorageSpace = 0;
        public int StorageSpace { get => storageSpace; private set => storageSpace = value; }
    }
}


