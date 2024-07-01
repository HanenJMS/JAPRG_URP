using GameLab.UnitSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MarketRequest
{
    Unit client;

    Unit assignedContractor;

    object reward;
    object target;
    bool isComplete = false;
    public void SetClient(Unit client) => this.client = client;
    public Unit GetClient() => client;

    public void SetContractor(Unit assignedContractor) => this.assignedContractor = assignedContractor;
    public Unit GetContractor() => assignedContractor;

    public object SetTarget(object target) => this.target = target;
    public object GetTarget() => this.target;
    public void SetReward(object reward) => this.reward = reward;
    public object GetReward() => this.reward;
    public bool RequestCompleted() => isComplete;

    
}
