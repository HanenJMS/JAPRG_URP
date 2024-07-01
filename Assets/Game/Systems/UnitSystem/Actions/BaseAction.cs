using GameLab.UISystem;
using GameLab.UnitSystem.ActionSystem;
using UnityEngine;

public abstract class BaseAction : MonoBehaviour, IAction
{

    [SerializeField] MouseCursorData cursorData;
    [SerializeField] Sprite actionSprite;
    [SerializeField] internal object target;
    public string ActionName()
    {
        return ToString();
    }
    /// <summary>
    /// base keyword should only be called after overriden function. 
    /// </summary>
    public virtual void Cancel()
    {
        target = null;
    }

    public abstract bool CanExecuteOnTarget(object target);

    public abstract void ExecuteOnTarget(object target);

    public MouseCursorData GetMouseCursorInfo()
    {
        return cursorData;
    }
    public Sprite GetActionSprite()
    {
        return actionSprite;
    }
}
