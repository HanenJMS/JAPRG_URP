using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace GameLab.GridSystem
{
    public class HexGridChunk : MonoBehaviour
    {
        HexCell[] hexCells;
        public HexMesh terrain;
        
        private void Awake()
        {
            hexCells = new HexCell[HexMetric.chunkSizeX * HexMetric.chunkSizeZ];
        }

        private void LateUpdate()
        {
            Triangulate();
            enabled = false;
        }
        public void Triangulate()
        {
            terrain.Clear();
            terrain.Triangulate(hexCells);
            terrain.Apply();
        }
        public void Refresh()
        {
            enabled = true;
        }
        public void AddCell(int index, HexCell cell)
        {
            hexCells[index] = cell;
            cell.transform.SetParent(transform);
        }

    }
}