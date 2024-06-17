using GameLab.InteractableSystem;
using GameLab.InventorySystem;
using GameLab.UISystem;
using GameLab.UnitSystem;
using GameLab.UnitSystem.ActionSystem;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


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

        object UIInteraction;
        private void LateUpdate()
        {

            if(EventSystem.current.IsPointerOverGameObject())
            {
                UIInteraction = MouseWorldController.GetRaycastHit().collider.GetComponent<EquipmentSlotUI>();
                if (UIInteraction != null)
                {
                    executableActions = UnitSelectionSystem.Instance.GetSelectedUnit().GetActionHandler().GetExecutableActions(UIInteraction);
                    selectedIndex = 0;
                    MouseWorldController.SetMouseCursor(executableActions[selectedIndex].GetMouseCursorInfo());
                }
                if (Input.GetMouseButtonUp(1))
                {
                    if (executableActions[selectedIndex].CanExecuteOnTarget(UIInteraction))
                    {
                        executableActions[selectedIndex].ExecuteOnTarget(UIInteraction);
                        return;
                    }
                }
            }
            //handling action controller.
            if (playerUnit.GetHealthHandler().IsDead())
            {
                return;
            }
            //when mouse is over an interactable object that is not the player.
            if (MouseWorldController.GetMouseRayCastInteractable() != null && playerUnit != MouseWorldController.GetMouseRayCastInteractable())
            {
                //if a new interactable has been found, set to the new interactable and set mouse cursor based on the action chosen
                if (interactable != MouseWorldController.GetMouseRayCastInteractable())
                {
                    interactable = MouseWorldController.GetMouseRayCastInteractable();
                    selectedIndex = 0;
                    executableActions = playerUnit.GetActionHandler().GetExecutableActions(interactable);
                    MouseWorldController.SetMouseCursor(executableActions[selectedIndex].GetMouseCursorInfo());
                }
                if (Input.mouseScrollDelta.y != 0)
                {
                    if (interactable != null)
                    {
                        selectedIndex = Mathf.Clamp(selectedIndex += (int)Input.mouseScrollDelta.y, 0, executableActions.Count - 1);
                        MouseWorldController.SetMouseCursor(executableActions[selectedIndex].GetMouseCursorInfo());
                    }
                }
                if (Input.GetMouseButtonUp(1))
                {
                    if (executableActions[selectedIndex].CanExecuteOnTarget(interactable))
                    {
                        executableActions[selectedIndex].ExecuteOnTarget(interactable);
                        return;
                    }
                }
            }
            else
            {
                interactable = null;
                MouseWorldController.SetMouseCursor(moveAction.GetMouseCursorInfo());
                if (Input.GetMouseButtonUp(1))
                {
                    moveAction.ExecuteOnTarget(MouseWorldController.GetMousePosition());
                    return;
                }
            }



            //testing ability system

            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                playerUnit.GetAbilityHandler().SetCurrentAbility(0);
            }
            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                playerUnit.GetAbilityHandler().SetCurrentAbility(1);
            }
            if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                playerUnit.GetAbilityHandler().SetCurrentAbility(2);
            }

            //testing equipment handling system
            if (Input.GetKeyDown(KeyCode.X))
            {
                playerUnit.gameObject.GetComponent<EquipmentHandler>().DrawWeapon();
            }
            if (Input.GetKeyDown(KeyCode.Z))
            {
                playerUnit.gameObject.GetComponent<EquipmentHandler>().UndrawWeapon();
            }
            if (Input.GetKeyDown(KeyCode.C))
            {
                playerUnit.gameObject.GetComponent<EquipmentHandler>().WithdrawCombat();
            }
        }
    }

}
