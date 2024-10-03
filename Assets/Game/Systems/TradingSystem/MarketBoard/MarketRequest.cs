using GameLab.UnitSystem;

[System.Serializable]
public class MarketRequest
{
    Unit client;

    Unit assignedContractor;

    object reward;
    object target;
    bool isComplete = false;
    public void SetClient(Unit client)
    {
        this.client = client;
    }

    public Unit GetClient()
    {
        return client;
    }

    public void SetContractor(Unit assignedContractor)
    {
        this.assignedContractor = assignedContractor;
    }

    public Unit GetContractor()
    {
        return assignedContractor;
    }

    public object SetTarget(object target)
    {
        return this.target = target;
    }

    public object GetTarget()
    {
        return this.target;
    }

    public void SetReward(object reward)
    {
        this.reward = reward;
    }

    public object GetReward()
    {
        return this.reward;
    }

    public bool RequestCompleted()
    {
        return isComplete;
    }
}
