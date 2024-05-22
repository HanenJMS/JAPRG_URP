using GameLab.UnitSystem;
using GameLab.UnitSystem.AbilitySystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageOnImpact : MonoBehaviour
{
    Unit castor;
    Unit target;
    AbilityData castedAbility;
    public void SetDamageOnImpact(Unit castor, AbilityData used, Unit onTarget)
    {
        this.castor = castor;
        castedAbility = used;
        this.target = onTarget;
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.GetComponent<Unit>() == target)
        {
            target.GetCombatHandler().TakeDamage(castor, castedAbility.GetAbilityPower());
        }
    }
}
