using GameLab.InventorySystem;
using System.Collections;
using UnityEngine;

public class ItemSpawner : MonoBehaviour
{
    [SerializeField] int qtyToSpawn = 0;
    [SerializeField] GameObject[] prefab;


    private void Start()
    {
        if (prefab == null) return;
        StartCoroutine(Spawn());
    }
    IEnumerator Spawn()
    {
        foreach (var item in prefab)
        {
            for (int i = 0; i < qtyToSpawn; i++)
            {
                GameObject ob = Instantiate(item, this.transform.position, Quaternion.identity);
                ob.GetComponentInChildren<ItemWorld>().GetItemSlot().SetQuantity(1);
                yield return new WaitForSeconds(0.5f);
            }
        }
    }
}
