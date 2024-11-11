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

        bool applyElevation = true;
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
                Debug.Log(gp.ToString());

                EditCell(HexGridVisualSystem.Instance.GetHexCell(gp));
                Debug.Log(HexGridVisualSystem.Instance.GetHexCell(gp).ToString());
            }
        }
        public void SetApplyElevation(bool toggle)
        {
            applyElevation = toggle;
        }
        private void EditCell(HexCell cell)
        {
            if (applyElevation)
            {
                cell.SetElevation(activeElevation);
            }
            cell.SetColor(activeColor);
            
            HexGridVisualSystem.Instance.SetHexCellElevation(cell.GetGridPosition());
            HexGridVisualSystem.Instance.Refresh(cell.GetCellChunkIndex());


        }
        enum OptionalToggle
        {
            Ignore, Yes, No
        }

        OptionalToggle riverMode;

        public void SetRiverMode(int mode)
        {
            riverMode = (OptionalToggle)mode;
        }
    }

}
