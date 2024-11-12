using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

namespace GameLab.GridSystem
{
    public class HexMapEditor : MonoBehaviour
    {
        [SerializeField] Color[] colors;
        [SerializeField]
        private Color activeColor;
        int activeElevation;
        [SerializeField] Texture2D noiseSource;
        bool isDrag;
        HexCellDirections dragDirection;
        HexCell previousCell;
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
                var currentCellGridPosition = LevelHexGridSystem.Instance.GetGridPosition(MouseWorldController.GetMousePosition());
                var currentHexCell = HexGridVisualSystem.Instance.GetHexCell(currentCellGridPosition);
                Debug.Log(currentCellGridPosition.ToString());

                if (previousCell && previousCell != currentHexCell)
                {
                    ValidateDrag(currentHexCell);
                }
                else
                {
                    isDrag = false;
                }
                EditCell(currentHexCell);
                Debug.Log(currentHexCell.ToString());
                previousCell = currentHexCell;
            }
            else
            {
                previousCell = null;
            }
        }
        void ValidateDrag(HexCell currentCell)
        {
            for (
                dragDirection = HexCellDirections.NE;
                dragDirection <= HexCellDirections.NW;
                dragDirection++
            )
            {
                if (HexGridVisualSystem.Instance.GetHexCell(previousCell.GetNextDirectionHexNeighbor(dragDirection)) == currentCell)
                {
                    isDrag = true;
                    return;
                }
            }
            isDrag = false;
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
            if (riverMode == OptionalToggle.No)
            {
                cell.RemoveRiver();
            }
            else if (isDrag && riverMode == OptionalToggle.Yes)
            {
                HexCell otherCell = HexGridVisualSystem.Instance.GetHexCell(cell.GetHexCellNeighborGridPosition(HexMetric.GetOppositeDirection(dragDirection)));
                if (otherCell)
                {
                    otherCell.SetOutgoingRiver(dragDirection);
                }
            }

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
