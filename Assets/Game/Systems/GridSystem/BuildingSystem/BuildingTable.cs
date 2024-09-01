using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameLab.BuildingSystem
{
    [CreateAssetMenu(fileName = "BuildingModeTable") ]
    public class BuildingTable : ScriptableObject
    {
        [SerializeField] List<GameObject> buildingPrefabs;

        public List<GameObject> GetBuildingList() => buildingPrefabs;
    }
}


