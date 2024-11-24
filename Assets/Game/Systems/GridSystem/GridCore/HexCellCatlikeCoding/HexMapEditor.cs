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
        int activeWaterLevel;
        HexCellDirections dragDirection;
        HexCell previousCell;
        bool applyElevation = true;
        bool applyWaterLevel = true;
        bool applyColorToggle = false;
        private void Awake()

        {
            HexMetric.noiseSource = noiseSource;
        }
        void Start()
        {
            SelectColor(0);
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

        private void EditCell(HexCell cell)
        {
            if (applyElevation)
            {
                cell.SetElevation(activeElevation);
                HexGridVisualSystem.Instance.SetHexCellElevation(cell.GetGridPosition());
            }
            if(applyColorToggle)
            {
                cell.SetColor(activeColor);
            }
            if (applyWaterLevel)
            {
                cell.WaterLevel = activeWaterLevel;
            }
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

        //toggles
        public void SetRiverMode(int mode)
        {
            riverMode = (OptionalToggle)mode;
        }
        public void SetRoadMode(int mode)
        {
            roadMode = (OptionalToggle)mode;
        }
        public void SetApplyElevation(bool toggle)
        {
            applyElevation = toggle;
        }
        public void SetApplyColor(bool toggle)
        {
            applyColorToggle = toggle;
        }
        public void SetApplyWaterLevel(bool toggle)
        {
            applyWaterLevel = toggle;
        }

        //sliders and triggers
        public void SelectColor(int index)
        {
            if (index < colors.Count())
                activeColor = colors[index];
        }
        public void SetElevation(float elevation)
        {
            activeElevation = ((int)elevation);
        }
        public void SetWaterLevel(float level)
        {
            activeWaterLevel = (int)level;
        }

    }

}
