using System.Collections.Generic;
using UnityEngine;

namespace GameLab.StatSystem
{
    public class StatHandler : MonoBehaviour
    {
        Dictionary<StatType, BaseStat> statDictionary = new();

        private void Awake()
        {
            InitializeStats();
        }
        void InitializeStats()
        {
            AddStat(StatType.Strength);
            AddStat(StatType.Intelligence);
            AddStat(StatType.Wisdom);
        }
        void AddStat(StatType statType)
        {
            if (!statDictionary.ContainsKey(statType))
            {
                statDictionary.Add(statType, new BaseStat(statType));
            }
        }
        public BaseStat GetStatType(StatType statType)
        { return statDictionary[statType]; }
    }
}

