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
        private void Start()
        {
            Refresh();
        }
        public void Refresh()
        {
            hexhMesh.Triangulate(hexCells);
        }
        public void AddCell(int index, HexCell cell)
        {
            hexCells[index] = cell;
            cell.transform.SetParent(transform);
        }
    }
}