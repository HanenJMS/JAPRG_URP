using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

namespace GameLab.GridSystem
{
    public class HexMapEditor : MonoBehaviour
    {
        [SerializeField] Color[] colors;
        private Color activeColor;

        void Start()
        {
            SelectColor(0);
        }
        public void SelectColor(int index)
        {
            if (index < colors.Count())
            activeColor = colors[index];
        }
        private void LateUpdate()
        {
            HandleInput();
        }
        public void HandleInput()
        {
            if (Input.GetMouseButtonUp(0) &&
            !EventSystem.current.IsPointerOverGameObject())
            {
                var gp = LevelHexGridSystem.Instance.GetGridPosition(MouseWorldController.GetMousePosition());
                HexGridVisualSystem.Instance.SetHexCellColor(gp, activeColor);
            }
        }
    }

}
