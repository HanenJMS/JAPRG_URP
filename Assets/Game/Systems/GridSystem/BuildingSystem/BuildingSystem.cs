using GameLab.GridSystem;
using System.Collections.Generic;
using UnityEngine;

namespace GameLab.BuildingSystem
{
    public class BuildingSystem : MonoBehaviour
    {
        [SerializeField] BuildingTable buildingTable;

        public List<GameObject> GetBuildingList()
        {
            return buildingTable.GetBuildingList();
        }
        private void LateUpdate()
        {
            if (Input.GetMouseButtonDown(1))
            {
                GridPosition gp = LevelGridSystem.Instance.GetGridPosition(MouseWorldController.GetMousePosition());
                var building = Instantiate(buildingTable.GetBuildingList()[0], LevelGridSystem.Instance.GetWorldPosition(gp), Quaternion.identity);
                building.transform.localScale *= LevelGridSystem.Instance.GetGridCellSize() * 0.75f;
                LevelGridSystem.Instance.GetGridObject(gp).AddObjectToGrid(building);
            }
        }
    }

}
