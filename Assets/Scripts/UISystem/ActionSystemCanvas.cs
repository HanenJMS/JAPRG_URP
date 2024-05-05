using GameLab.Controller;
using GameLab.UnitSystem;
using GameLab.UnitSystem.ActionSystem;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Toolbars;
using UnityEngine;

namespace GameLab.UISystem
{
    public class ActionSystemCanvas : MonoBehaviour
    {
        ActionButtonUI buttonUIPrefab;



        private void DisplayUI()
        {
            UnitSelectionSystem.Instance.GetPlayerUnit().GetActionHandler().ExecutableActions(UnitSelectionSystem.Instance.GetSelectedUnit());
        }
    }

}
