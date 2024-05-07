using GameLab.InteractableSystem;
using GameLab.UnitSystem;
using GameLab.UnitSystem.ActionSystem;
using System.Collections.Generic;
using UnityEngine;


namespace GameLab.Controller
{
    public class ActionController : MonoBehaviour
    {
        public static ActionController Instance;
        IAction moveAction;
        ActionHandler actionHandler;

        Unit playerUnit;
        [SerializeField] Unit currentSelectedUnit;

        List<IAction> executableActions = new();
        IAction selectedAction;
        [SerializeField] int selectedIndex = 0;
        private void Awake()
        {
            if (Instance != null)
            {
                Destroy(this);
                return;
            }
            Instance = this;
        }
        private void Start()
        {
            playerUnit = GameObject.FindGameObjectWithTag("Player").GetComponent<Unit>();
            actionHandler = playerUnit.GetActionHandler();
            moveAction = playerUnit.GetComponent<MoveAction>();
        }


        private void LateUpdate()
        {
            if (Input.mouseScrollDelta.y != 0)
            {
                if (executableActions.Count > 0)
                {
                    selectedIndex += (int)Input.mouseScrollDelta.y;
                    if (selectedIndex >= executableActions.Count)
                    {
                        selectedIndex = executableActions.Count - 1;
                    }
                    if (selectedIndex < 0)
                    {
                        selectedIndex = 0;
                    }
                    Debug.Log(executableActions[selectedIndex].ToString());
                }

            }
            if (Input.GetMouseButtonUp(0))
            {
                Interactable interactable = MouseWorldController.GetMouseRayCastInteractable();
                if (interactable != null)
                {
                    if (interactable is Unit)
                    {
                        Debug.Log("interaction Type: " + interactable.ToString());
                        currentSelectedUnit = interactable as Unit;
                        executableActions = playerUnit.GetActionHandler().GetExecutableActions(currentSelectedUnit);
                        Debug.Log(executableActions[selectedIndex].ToString());
                    }
                }
            }
            if (Input.GetMouseButtonUp(1))
            {
                Interactable interactable = MouseWorldController.GetMouseRayCastInteractable();
                if (interactable != null)
                {
                    executableActions = playerUnit.GetActionHandler().GetExecutableActions(interactable);
                    executableActions[selectedIndex].ExecuteOnTarget(interactable);
                }
                else
                {
                    executableActions.Clear();
                    moveAction.ExecuteOnTarget(MouseWorldController.GetMousePosition());
                }
            }
        }
    }

}
