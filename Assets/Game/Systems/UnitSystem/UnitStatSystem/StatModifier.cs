using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum StatModifierType
{
    Buff = 1, Debuff = -1
}
public struct StatModifier
{
    public int modifierValue;

    public StatModifierType StatModifierType;
}
