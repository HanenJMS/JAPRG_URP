using GameLab.ResourceSystem;
using GameLab.UnitSystem;
using GameLab.UnitSystem.ActionSystem;
using System;
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
        Unit currentSelectedUnit;

        List<IAction> executableActions = new();
        int selectedIndex = 0;
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
            if(Input.GetMouseButtonDown(0))
            {
                if (MouseWorldController.GetMouseRayCastInteractable() != null)
                {
                    if (MouseWorldController.GetMouseRayCastInteractable() is Unit)
                    {
                        currentSelectedUnit = MouseWorldController.GetMouseRayCastInteractable() as Unit;
                        executableActions.Clear();
                        executableActions = actionHandler.ExecutableActions(currentSelectedUnit);
                        Debug.Log("Current selected Action" + executableActions[0].ToString());
                    }
                }
            }
            if(Input.GetMouseButtonUp(1)) 
            {
                if(MouseWorldController.GetMouseRayCastInteractable() != null)
                {
                    if(MouseWorldController.GetMouseRayCastInteractable() is Unit)
                    {
                        Debug.Log("Unit selected");
                    }
                }
                else
                {
                    moveAction.ExecuteOnTarget(MouseWorldController.GetMousePosition());
                }
            }
        }
    }

}
