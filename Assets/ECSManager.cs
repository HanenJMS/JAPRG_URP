using Unity.Entities;
using UnityEngine;
public class ECSManager : MonoBehaviour
{
    EntityManager manager;


    private void Start()
    {
        manager = World.DefaultGameObjectInjectionWorld.EntityManager;

    }
}
