using UnityEngine;

namespace GameLab.StatSystem
{
    public struct BaseStat
    {


        [SerializeField] StatType statType;
        [SerializeField] int statValue;
        public BaseStat(StatType statType, int statValue = 0)
        {
            this.statType = statType;
            this.statValue = statValue;
        }
        public StatType GetStatType()
        {
            return statType;
        }

        public int GetStatValue()
        {
            return statValue;
        }
    }
}

