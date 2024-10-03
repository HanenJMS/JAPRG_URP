using GameLab.UnitSystem;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace GameLab.InteractableSystem
{
    public class Interactable_Building : Interactable
    {
        List<Unit> residents = new();
        Unit owner;
        [SerializeField] Transform entrance;
        BuildingWorld buildingWorld;
        private void Start()
        {
            buildingWorld = GetComponentInParent<BuildingWorld>();
            NavMeshObstacle obs = GetComponent<NavMeshObstacle>();

        }
        public override Transform GetCurrentWorldTransform()
        {
            return buildingWorld.GetBuildingEntranceLocation();
        }

        public override void Interact(object interaction)
        {
            var unit = interaction as Unit;
            residents.Add(unit);
            unit.EnterBuilding(this);
            Debug.Log($"Inn has :  {residents.Count}");
            unit.gameObject.SetActive(false);
        }
        public void Exit(object Unit)
        {
            var unit = Unit as Unit;
            if (unit == null) return;
            if (residents.Contains(unit))
            {
                unit.gameObject.SetActive(true);
                unit.transform.position = buildingWorld.GetBuildingEntranceLocation().position;
                residents.Remove(unit);
                Debug.Log($"Inn has :  {residents.Count}");
            }
        }
    }
}