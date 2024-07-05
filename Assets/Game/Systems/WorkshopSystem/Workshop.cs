using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameLab.WorkShopSystem
{
    public class Workshop : MonoBehaviour
    {
        int storageSpace = 10000;
        int currentStorageSpace = 0;
        public int StorageSpace { get { return storageSpace; } private set {  storageSpace = value; } }
    }
}


