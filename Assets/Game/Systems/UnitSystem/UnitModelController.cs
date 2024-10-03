using System.Collections.Generic;
using UnityEngine;
namespace GameLab.UnitSystem
{
    public class UnitModelController : MonoBehaviour
    {
        [SerializeField] List<UnitModel> modelList = new();
        private void Awake()
        {

        }

        private void Start()
        {
            foreach (var item in GetComponentsInChildren<UnitModel>())
            {
                modelList.Add(item);
            };
            Clear();
            modelList[Random.Range(0, modelList.Count)].gameObject.SetActive(true);
        }
        void Clear()
        {
            foreach (var item in modelList)
            {
                item.gameObject.SetActive(false);
            }

        }
    }
}


