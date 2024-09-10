using System.Collections.Generic;
using UnityEngine;

namespace GameLab.GridSystem
{
    public class GridPositionVisualCell : MonoBehaviour
    {
        [SerializeField] Dictionary<string, MeshRenderer> meshRenderer = new();
        [SerializeField] Mesh hexMesh;
        List<Vector3> vertices = new();
        List<int> triangles = new();
        private void Awake()
        {

            foreach (var item in GetComponentsInChildren<MeshRenderer>())
            {
                if (item.gameObject.name == "HexImage") continue;
                meshRenderer.Add(item.gameObject.name, item);
            }

            vertices = new();
            triangles = new();
        }
        public void ShowGridPositionVisual(string state)
        {
            if (meshRenderer.ContainsKey(state))
            {
                meshRenderer[state].enabled = true;
            }
        }
        public void HideGridPositionVisual()
        {
            foreach (var item in meshRenderer)
            {
                item.Value.enabled = false;
            }
        }

    }

}

