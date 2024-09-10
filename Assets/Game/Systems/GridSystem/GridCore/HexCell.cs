using System.Collections.Generic;
using UnityEngine;

namespace GameLab.GridSystem
{

    public class HexCell : MonoBehaviour
    {
        Vector3[] corners;
        float outerRadius;
        float innerRadiusConstant = 0.866025404f;
        Color color = Color.white;
        GridPosition coordinates;
        public void InitializeCorners()
        {
            outerRadius = LevelHexGridSystem.Instance.GetGridCellSize() / 2;
            float innerRadiusCalculated = outerRadius * innerRadiusConstant;
            Vector3[] corners =
            {
                    new Vector3(0f, 0f, outerRadius),
                    new Vector3(innerRadiusCalculated, 0f, 0.5f * outerRadius),
                    new Vector3(innerRadiusCalculated, 0f, -0.5f * outerRadius),
                    new Vector3(0f, 0f, -outerRadius),
                    new Vector3(-innerRadiusCalculated, 0f, -0.5f * outerRadius),
                    new Vector3(-innerRadiusCalculated, 0f, 0.5f * outerRadius),
                    new Vector3(0f, 0f, outerRadius)
            };
            this.corners = corners;
        }

        public Vector3[] GetCorners()
        {
            return corners;
        }
        public void SetGridPosition(GridPosition gp)
        {
            coordinates = gp;
        }
        public void SetColor(Color color)
        {
            this.color = color;
        }
    }

}

