namespace GameLab.AI
{
    public interface IAIState
    {
        public void ActivateState();
        public void CancelState();
        public bool IsRunning();
    }
}

