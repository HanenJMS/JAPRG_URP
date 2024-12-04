using UnityEngine;
using UnityEngine.EventSystems;
using System.IO;
namespace GameLab.GridSystem
{
    public class HexMapEditor : MonoBehaviour
    {
        int activeElevation;

        bool isDrag;
        int activeWaterLevel;
        HexCellDirections dragDirection;
        HexCell previousCell;
        bool applyElevation = true;
        bool applyWaterLevel = true;
        bool applyColorToggle = false;

        int activeUrbanLevel, activeFarmLevel, activePlantLevel, activeSpecialIndex;

        bool applyUrbanLevel, applyFarmLevel, applyPlantLevel, applySpecialIndex;

        int activeTerrainTypeIndex;

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
                if (previousCell != null && previousCell != currentCell && currentCell != null)
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
                    Debug.Log("opp direction: " + dragDirection.GetOppositeDirection().ToString());
                    return;
                }
            }
            isDrag = false;
        }

        private void EditCell(HexCell cell)
        {
            if (activeTerrainTypeIndex >= 0)
            {
                cell.TerrainTypeIndex = activeTerrainTypeIndex;
            }
            if (applyElevation)
            {
                cell.SetElevation(activeElevation);
                HexGridVisualSystem.Instance.SetHexCellElevation(cell.GetGridPosition());
            }
            if (applyColorToggle)
            {
            }
            if (applyWaterLevel)
            {
                cell.WaterLevel = activeWaterLevel;
            }
            if (applySpecialIndex)
            {
                cell.SpecialIndex = activeSpecialIndex;
            }
            if (applyUrbanLevel)
            {
                cell.UrbanLevel = activeUrbanLevel;
            }
            if (applyFarmLevel)
            {
                cell.FarmLevel = activeFarmLevel;
            }
            if (applyPlantLevel)
            {
                cell.PlantLevel = activePlantLevel;
            }
            if (riverMode == OptionalToggle.No)
            {
                cell.RemoveRiver();
            }
            if (roadMode == OptionalToggle.No)
            {
                cell.RemoveRoads();
            }
            if (walledMode != OptionalToggle.Ignore)
            {
                cell.Walled = walledMode == OptionalToggle.Yes;
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

        OptionalToggle riverMode, roadMode, walledMode;

        //toggles
        public void SetRiverMode(int mode)
        {
            riverMode = (OptionalToggle)mode;
        }
        public void SetRoadMode(int mode)
        {
            roadMode = (OptionalToggle)mode;
        }
        public void SetWalledMode(int mode)
        {
            walledMode = (OptionalToggle)mode;
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
        public void SetElevation(float elevation)
        {
            activeElevation = ((int)elevation);
        }
        public void SetWaterLevel(float level)
        {
            activeWaterLevel = (int)level;
        }

        public void SetApplyUrbanLevel(bool toggle)
        {
            applyUrbanLevel = toggle;
        }

        public void SetUrbanLevel(float level)
        {
            activeUrbanLevel = (int)level;
        }
        public void SetApplyFarmLevel(bool toggle)
        {
            applyFarmLevel = toggle;
        }

        public void SetFarmLevel(float level)
        {
            activeFarmLevel = (int)level;
        }

        public void SetApplyPlantLevel(bool toggle)
        {
            applyPlantLevel = toggle;
        }

        public void SetPlantLevel(float level)
        {
            activePlantLevel = (int)level;
        }

        public void SetApplySpecialIndex(bool toggle)
        {
            applySpecialIndex = toggle;
        }

        public void SetSpecialIndex(float index)
        {
            activeSpecialIndex = (int)index;
        }
        public void SetTerrainTypeIndex(int index)
        {
            activeTerrainTypeIndex = index;
        }
        public void Save()
        {
            string path = Path.Combine(Application.persistentDataPath, "test.map");
            using (BinaryWriter writer = new BinaryWriter(
                File.Open(path, FileMode.Create)))
            {
                writer.Write(0);
                HexGridVisualSystem.Instance.Save(writer);
            }
            Debug.Log(Application.persistentDataPath);
        }

        public void Load()
        {
            string path = Path.Combine(Application.persistentDataPath, "test.map");
            using (BinaryReader reader = new BinaryReader(File.OpenRead(path)))
            {
                int header = reader.ReadInt32();
                if (header == 0)
                {
                    HexGridVisualSystem.Instance.Load(reader);
                }
                else
                {
                    Debug.LogWarning("Unknown map format " + header);
                }
            }
        }
    }

}
