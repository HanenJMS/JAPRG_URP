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
        if(targetUnit == null)
        {
            targetUnit = GameObject.FindGameObjectWithTag("Player").GetComponent<Unit>();
        }
        if (targetUnit != null)
        {
            //attackAction.ExecuteOnTarget(targetUnit);
        }
    }
}
