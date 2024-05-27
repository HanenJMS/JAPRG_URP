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
            if(Input.GetKeyDown(KeyCode.Alpha1))
            {
                playerUnit.GetAbilityHandler().SetCurrentAbility(0);
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
            if (Input.GetMouseButtonUp(0))
            {
                Interactable interactable = MouseWorldController.GetMouseRayCastInteractable();
                if (interactable != null)
                {
                    if (interactable is Unit)
                    {
                        if((interactable as Unit).GetFactionHandler().GetFaction() != playerUnit.GetFactionHandler().GetFaction())
                        {
                            Debug.Log("interaction Type: " + interactable.ToString());
                            currentSelectedUnit = interactable as Unit;
                            executableActions = playerUnit.GetActionHandler().GetExecutableActions(currentSelectedUnit);
                            Debug.Log(executableActions[selectedIndex].ToString());
                        }
                    }
                }
            }
            
            if (Input.GetMouseButtonDown(1))
            {
                interactables = MouseWorldController.GetMouseRayCastInteractables();
                if (interactables.Count > 0)
                {
                    selectedIndex = 0;
                    executableActions = playerUnit.GetActionHandler().GetExecutableActions(interactables[0]);
                }
            }
            if(Input.GetMouseButtonUp(1))
            {
                if(executableActions.Count > 0) 
                {
                    foreach (Interactable interactable in interactables)
                    {
                        if (executableActions[selectedIndex].CanExecuteOnTarget(interactable))
                        {
                            executableActions[selectedIndex].ExecuteOnTarget(interactable);
                            return;
                        }
                    }
                }
                moveAction.ExecuteOnTarget(MouseWorldController.GetMousePosition());
            }
        }
    }

}
