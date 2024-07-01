using GameLab.UnitSystem;
using System.Collections.Generic;
using UnityEngine;

namespace GameLab.InteractableSystem
{
    public class Interactable_Building : Interactable
    {
        List<Unit> residents = new();
        Unit owner;
        [SerializeField] Transform entrance;
        public override Transform GetCurrentWorldTransform() => entrance;
        public override void Interact(object interaction)
        {
            var unit = interaction as Unit;
            residents.Add(unit);
            unit.EnterBuilding(this);
            Debug.Log($"Inn has :  { residents.Count}");
            unit.gameObject.SetActive(false);
        }
        public void Exit(object Unit)
        {
            var unit = Unit as Unit;
            if (unit == null) return;
            if(residents.Contains(unit))
            {
                unit.gameObject.SetActive(true);
                unit.transform.position = entrance.position;
                residents.Remove(unit);
            }
        }
    }
}