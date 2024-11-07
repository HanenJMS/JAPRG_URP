using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace GameLab.GridSystem
{
    public class HexGridChunk : MonoBehaviour
    {
        HexCell[] hexCells;
        HexMesh hexhMesh;
        
        private void Awake()
        {
            hexCells = new HexCell[HexMetric.chunkSizeX * HexMetric.chunkSizeZ];
            hexhMesh = GetComponentInChildren<HexMesh>();
        }

        private void LateUpdate()
        {
            hexhMesh.Triangulate(hexCells);
            enabled = false;
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