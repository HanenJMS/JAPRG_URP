using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace GameLab.GridSystem
{
    public class HexGridFeatureManager : MonoBehaviour
    {
        public HexFeatureCollection[] urbanCollections;
        Transform container;
        public void Clear()
        {
            if (container)
            {
                Destroy(container.gameObject);
            }
            container = new GameObject("Features Container").transform;
            container.SetParent(transform, false);
        }

        public void Apply() { }

        public void AddFeature(HexCell cell, Vector3 position)
        {
            HexHash hash = HexMetric.SampleHashGrid(position);
            Transform prefab = PickPrefab(cell.UrbanLevel, hash.a, hash.b);
            if (!prefab)
            {
                return;
            }
            Transform instance = Instantiate(prefab);
            position.y += instance.localScale.y * 0.5f;
            instance.localPosition = HexMetric.Perturb(position);
            instance.localRotation = Quaternion.Euler(0f, 360f * hash.c, 0f);
            instance.SetParent(container, false);
        }
        Transform PickPrefab(int level, float hash, float choice)
        {
            if (level > 0)
            {
                float[] thresholds = HexMetric.GetFeatureThresholds(level - 1);
                for (int i = 0; i < thresholds.Length; i++)
                {
                    if (hash < thresholds[i])
                    {
                        return urbanCollections[i].Pick(choice);
                    }
                }
            }
            return null;
        }
    }
}

