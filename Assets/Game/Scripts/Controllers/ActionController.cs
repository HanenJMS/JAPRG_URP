using GameLab.InteractableSystem;
using GameLab.InventorySystem;
using GameLab.UnitSystem;
using GameLab.UnitSystem.ActionSystem;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Analytics;


namespace GameLab.Controller
{
    public class ActionController : MonoBehaviour
    {
        public static ActionController Instance;
        IAction moveAction;
        ActionHandler actionHandler;

        Unit playerUnit;
        [SerializeField] Unit currentSelectedUnit;
        List<Interactable> interactables = new();
        Interactable interactable = null;
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
            if (playerUnit.GetHealthHandler().IsDead())
            {
                return;
            }
            if(MouseWorldController.GetMouseRayCastInteractable() != null)
            {
                if(interactable != MouseWorldController.GetMouseRayCastInteractable())
                {
                    interactable = MouseWorldController.GetMouseRayCastInteractable();
                    selectedIndex = 0;
                    executableActions = playerUnit.GetActionHandler().GetExecutableActions(interactable);
                    MouseWorldController.SetMouseCursor(executableActions[selectedIndex].GetMouseCursorInfo());
                }
                if (Input.GetMouseButtonUp(1))
                {
                    if (executableActions[selectedIndex].CanExecuteOnTarget(interactable))
                    {
                        executableActions[selectedIndex].ExecuteOnTarget(interactable);
                        return;
                    }
                    moveAction.ExecuteOnTarget(MouseWorldController.GetMousePosition());
                }
            }
            else
            {
                MouseWorldController.SetMouseCursor(moveAction.GetMouseCursorInfo());
            }

            if (Input.mouseScrollDelta.y != 0)
            {
                if (interactable != null)
                {
                    selectedIndex = Mathf.Clamp(selectedIndex += (int)Input.mouseScrollDelta.y, 0, executableActions.Count - 1);
                    MouseWorldController.SetMouseCursor(executableActions[selectedIndex].GetMouseCursorInfo());
                }
            }



            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                playerUnit.GetAbilityHandler().SetCurrentAbility(0);
                if(playerUnit.GetInventoryHandler().GetInventorySlot(0) != null)
                    playerUnit.GetInventoryHandler().DropItem(playerUnit.GetInventoryHandler().GetInventorySlot(0).GetItemData(), 1);
            }
            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                playerUnit.GetAbilityHandler().SetCurrentAbility(1);
            }
            if(Input.GetKeyDown(KeyCode.Alpha3))
            {
                playerUnit.GetAbilityHandler().SetCurrentAbility(2);
            }
            if (Input.GetKeyDown(KeyCode.X))
            {
                playerUnit.gameObject.GetComponent<EquipmentHandler>().DrawWeapon();
            }
            if (Input.GetKeyDown(KeyCode.Z))
            {
                playerUnit.gameObject.GetComponent<EquipmentHandler>().UndrawWeapon();
            }
            if(Input.GetKeyDown(KeyCode.C))
            {
                playerUnit.gameObject.GetComponent<EquipmentHandler>().WithdrawCombat();
            }

           

        }

        private void HandleValidActionSelection()
        {
            if (selectedIndex <= 0)
            {
                selectedIndex = 0;
            }
            if(executableActions.Count == 0)
            {
                selectedIndex = 0;
            }
            if(executableActions.Count > selectedIndex)
            {
                selectedIndex = executableActions.Count - 1;
                
            }
        }
    }

}
