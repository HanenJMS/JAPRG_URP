using GameLab.GridSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameLab.Controller
{
    public class HexGridController : MonoBehaviour
    {
        private void LateUpdate()
        {
            if (Input.GetMouseButtonUp(0))
            {
                var gp = LevelHexGridSystem.Instance.GetGridPosition(MouseWorldController.GetMousePosition());
                HexGridVisualSystem.Instance.SetHexCellColor(gp, Color.green);
            }
        }
    }

}
