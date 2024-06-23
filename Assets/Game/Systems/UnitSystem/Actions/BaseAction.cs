using GameLab.UISystem;
using GameLab.UnitSystem.ActionSystem;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class BaseAction : MonoBehaviour, IAction
{

    [SerializeField] MouseCursorData cursorData;
    [SerializeField] Sprite actionSprite;

    public string ActionName()
    {
        return ToString();
    }

    public abstract void Cancel();

    public abstract bool CanExecuteOnTarget(object target);

    public abstract void ExecuteOnTarget(object target);

    public MouseCursorData GetMouseCursorInfo()
    {
        return cursorData;
    }
    public Sprite GetActionSprite() => actionSprite;
}
