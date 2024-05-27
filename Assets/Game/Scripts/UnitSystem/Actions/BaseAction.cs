using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseAction : MonoBehaviour, IEquatable<BaseAction>
{
    public bool Equals(BaseAction other)
    {
        Debug.Log("Fix BaseAction Equatable");
        return false;
    }


}
