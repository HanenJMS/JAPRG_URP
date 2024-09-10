using GameLab.GridSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridTesting : MonoBehaviour
{
    [SerializeField] string gridPositionState;
    private void LateUpdate()
    {
        if(Input.GetMouseButtonUp(0))
        {
            var mouseGp = LevelGridSystem.Instance.GetGridPosition(MouseWorldController.GetMousePosition());
            var gridObject = LevelGridSystem.Instance.GetGridObject(mouseGp);
            List<GridPosition> visualizeNeighborGridPositions = new();
            foreach (var item in gridObject.GetNeighborGridPositions())
            {
                Debug.Log(item.ToString());
                visualizeNeighborGridPositions.Add(item);
            }
            GridPositionVisual.Instance.ShowGridPositions(visualizeNeighborGridPositions, gridPositionState);
        }
    }
}
