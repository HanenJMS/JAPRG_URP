using GameLab.Controller;
using GameLab.UnitSystem;
using GameLab.UnitSystem.ActionSystem;
using System.Collections.Generic;
using UnityEngine;

public class AiTestingScript : MonoBehaviour
{
    public static ActionController Instance;
    IAction moveAction;
    IAction attackAction;
    ActionHandler actionHandler;


    [SerializeField] bool isActive = false;
    Unit unit;
    Unit targetUnit;

    List<IAction> executableActions = new();
    IAction selectedAction;
    private void Start()
    {
        unit = GetComponent<Unit>();
        actionHandler = unit.GetActionHandler();
        moveAction = unit.GetComponent<MoveAction>();
        attackAction = unit.GetComponent<AttackAction>();
    }

    private void LateUpdate()
    {
        //if no target
        if (unit.GetHealthHandler().IsDead())
        {
            isActive = false;
            return;
        }
            if (targetUnit == null)
        {
            targetUnit = GameObject.FindGameObjectWithTag("Player").GetComponent<Unit>();
        }
        
        if (targetUnit != null)
        {
            if (targetUnit.GetHealthHandler().IsDead()) isActive = false;
            if (!isActive)
            {
                attackAction.Cancel();
                return;
            }
            attackAction.ExecuteOnTarget(targetUnit);
        }
    }
}
