using GameLab.UnitSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MarketBoard : MonoBehaviour
{
    List<MarketRequest> requests = new();

    void CreateRequest(Unit client, object target, object reward)
    {
        MarketRequest request = new();
        request.SetClient(client);
        request.SetTarget(target);
        request.SetReward(reward);
        requests.Add(request);
    }
}
