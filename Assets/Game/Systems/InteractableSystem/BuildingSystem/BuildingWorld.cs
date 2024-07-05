using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingWorld : MonoBehaviour
{
    [SerializeField] Transform entrancePosition;
    private void Awake()
    {
        entrancePosition = GetComponentInChildren<Building_World_Entrance>().transform;
    }

    public Transform GetBuildingEntranceLocation() => entrancePosition;
}
