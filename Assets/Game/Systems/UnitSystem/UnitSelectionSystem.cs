using System;
using UnityEngine;

namespace GameLab.UnitSystem
{
    public class UnitSelectionSystem : MonoBehaviour
    {
        [SerializeField] Unit selectedUnit;
        Unit playerUnit;
        public Action onSelectedUnit;
        public Action onRightDoubleClick;
        public Action onPlayerSelected;
        public Action onDeselectedUnit;

        public static UnitSelectionSystem Instance;
        private void Awake()
        {
            if (Instance != null)
            {
                Destroy(this);
                return;
            }
            Instance = this;
        }
        private void OnEnable()
        {
            playerUnit = GameObject.FindGameObjectWithTag("Player").GetComponent<Unit>();
        }
        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (MouseWorldController.GetRaycastHit().transform.TryGetComponent<Unit>(out Unit mouseOverUnit))
                {
                    SetSelectedUnit(mouseOverUnit);
                }
            }
            if (Input.GetMouseButtonUp(1))
            {
                if (MouseWorldController.GetRaycastHit().transform.TryGetComponent<Unit>(out Unit mouseOverUnit))
                {

                    if (selectedUnit != mouseOverUnit)
                    {
                        SetSelectedUnit(mouseOverUnit);
                    }
                    else
                    {
                        onRightDoubleClick?.Invoke();
                    }
                }
            }
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                DeselectUnit();
            }
            if (Input.GetKeyDown(KeyCode.Space))
            {
                SetSelectedUnit(playerUnit);
                onPlayerSelected?.Invoke();

            }
        }

        void SetSelectedUnit(Unit unit)
        {
            selectedUnit = unit;
            onSelectedUnit?.Invoke();
            Debug.Log($"{selectedUnit.name} strength : {selectedUnit.GetStatHandler().GetStatType(StatSystem.StatType.Strength).GetStatValue()}");
        }
        void DeselectUnit()
        {
            selectedUnit = null;
            onDeselectedUnit?.Invoke();
        }
        public Unit GetPlayerUnit()
        {
            return playerUnit;
        }

        public Unit GetSelectedUnit()
        {
            return selectedUnit != null ? selectedUnit : playerUnit;
        }

    }
}

