namespace GameLab.UnitSystem.ActionSystem
{
    public interface IAction
    {
        public string ActionName();
        public void ExecuteOnTarget(object target);
        public bool CanExecuteOnTarget(object target);
        public void Cancel();
    }
}

