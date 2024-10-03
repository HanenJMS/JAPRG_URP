using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

namespace GameLab.GridSystem
{
    public class HexMapEditor : MonoBehaviour
    {
        [SerializeField] Color[] colors;
        [SerializeField]
        private Color activeColor;
        int activeElevation;
        [SerializeField] Texture2D noiseSource;
        private void Awake()
        {
            HexMetric.noiseSource = noiseSource;
        }
        void Start()
        {
            SelectColor(0);
        }

        public void SelectColor(int index)
        {
            if (index < colors.Count())
                activeColor = colors[index];
        }
        public void SetElevation(float elevation)
        {
            activeElevation = ((int)elevation);
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
                EditCell(HexGridVisualSystem.Instance.GetHexCell(gp));
            }
        }

        private void EditCell(HexCell cell)
        {
            cell.SetColor(activeColor);
            cell.SetElevation(activeElevation);
            HexGridVisualSystem.Instance.SetHexCellElevation(cell.GetGridPosition());
            HexGridVisualSystem.Instance.Refresh();

        }
    }

}
