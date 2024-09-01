using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
public class ECSManager : MonoBehaviour
{
    EntityManager manager;


    private void Start()
    {
        manager = World.DefaultGameObjectInjectionWorld.EntityManager;
        
    }
}
