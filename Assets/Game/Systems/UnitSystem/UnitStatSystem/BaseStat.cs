using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameLab.StatSystem
{
    public class BaseStat : MonoBehaviour
    {

        
        [SerializeField] StatType statType;
        public BaseStat(StatType statType)
        {
            this.statType = statType;
        }
        public StatType GetStatType() => statType;
    }
}

