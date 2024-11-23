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
            if 
            ( 
                Input.GetMouseButton(0) &&
                !EventSystem.current.IsPointerOverGameObject()
            )
            {
                HandleInput();
            }
            else
            {
                previousCell = null;
            }
        }
        void HandleInput()
        {
            if (MouseWorldController.GetMousePosition(out RaycastHit hit))
            {
                HexCell currentCell = HexGridVisualSystem.Instance.GetHexCell(LevelHexGridSystem.Instance.GetGridPosition(hit.point));
                if (previousCell != null && previousCell != currentCell)
                {
                    ValidateDrag(currentCell);
                    Debug.Log($"{currentCell.GetGridPosition().ToString()}");
                }
                else
                {
                    isDrag = false;
                }
                EditCell(currentCell);
                previousCell = currentCell;
            }
            else
            {
                previousCell = null;
            }
        }
        void ValidateDrag(HexCell currentCell)
        {
            for (dragDirection = HexCellDirections.NE; dragDirection <= HexCellDirections.NW; dragDirection++) 
            {
                if (previousCell.GetHexCellNeighbor(dragDirection) == currentCell)
                {
                    isDrag = true;
                    Debug.Log("direction: " + dragDirection.ToString());
                    Debug.Log("opp direction: "+ dragDirection.GetOppositeDirection().ToString());
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
                HexGridVisualSystem.Instance.SetHexCellElevation(cell.GetGridPosition());
            }
            cell.SetColor(activeColor);
            if (riverMode == OptionalToggle.No)
            {
                cell.RemoveRiver();
            }
            if (roadMode == OptionalToggle.No)
            {
                cell.RemoveRoads();
            }
            else if (isDrag)
            {
                if (riverMode == OptionalToggle.Yes)
                    previousCell.SetOutgoingRiver(dragDirection);

                if (roadMode == OptionalToggle.Yes)
                {
                    previousCell.AddRoad(dragDirection);
                }
            }

            
            HexGridVisualSystem.Instance.Refresh(cell.GetCellChunkIndex());


        }
        enum OptionalToggle
        {
            Ignore, Yes, No
        }

        OptionalToggle riverMode, roadMode;

        public void SetRiverMode(int mode)
        {
            riverMode = (OptionalToggle)mode;
        }
        public void SetRoadMode(int mode)
        {
            roadMode = (OptionalToggle)mode;
        }
    }

}
