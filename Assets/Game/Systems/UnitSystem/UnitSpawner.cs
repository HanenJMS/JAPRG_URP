using GameLab.UnitSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitSpawner : MonoBehaviour
{
    [SerializeField] List<Unit> spawnUnits = new();
    [SerializeField] int spawnCount;
    private void Start()
    {


        if(spawnUnits.Count > 0)
        {
            foreach(var unit in spawnUnits)
            {
                for (int i = 0; i < spawnCount; i++)
                {
                    float randomDistance = Random.Range(2, 5);
                    float radians = Random.Range(0, 360);
                    float x = Mathf.Cos(radians) * randomDistance + transform.position.x;
                    float z = Mathf.Sin(radians) * randomDistance + transform.position.z;
                    Vector3 newVectorPosition = new(x, 1, z);
                    Instantiate(unit.gameObject, newVectorPosition, Quaternion.identity);
                }
            }
            
        }
    }
    
}
