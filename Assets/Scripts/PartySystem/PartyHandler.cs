using GameLab.UnitSystem;
using System;
using System.Collections.Generic;
using UnityEngine;
namespace GameLab.PartySystem
{
    public class PartyHandler : MonoBehaviour
    {
        [SerializeField]Unit partyLeader;
        [SerializeField]List<Unit> partyMembers = new();
        [SerializeField] List<Unit> partyEnemies = new();
        public Action onPartyLeaderChanged;
        public Action onEnemiesFighting;
        public void AddAlly(Unit partyMember)
        {
            if (!partyMembers.Contains(partyMember))
            {
                partyMembers.Add(partyMember);
                UpdateMemberList();
            }
            
        }
        public void RemoveAlly(Unit partyMember)
        {
            if (partyMembers.Contains(partyMember))
            {
                partyMembers.Remove(partyMember);
                UpdateMemberList();
            }
            
        }
        public void AddEnemy(Unit Enemy)
        {
            if (!partyEnemies.Contains(Enemy))
            {
                partyEnemies.Add(Enemy);
                UpdateEnemyList();
            }
        }
        public void RemoveEnemy(Unit Enemy)
        {
            if (partyEnemies.Contains(Enemy))
            {
                partyEnemies.Remove(Enemy);
                RemoveEnemyFromCombat(Enemy);
            }
        }
        public List<Unit> GetPartyMemberList()  
        {
            return partyMembers;
        }
        public void SetPartyMemberList(List<Unit> partyMembers)
        {
            this.partyMembers = partyMembers;
        }
        public void SetEnemyList(List<Unit> leaderEnemyList)
        {
            partyEnemies = leaderEnemyList;
        }
        public void UpdateMemberList()
        {
            foreach(Unit member in partyMembers)
            {
                member.GetPartyHandler().SetPartyMemberList(partyMembers);
                member.GetFactionHandler().SetFaction(GetLeader().GetFactionHandler().GetFaction());
                member.GetPartyHandler().SetLeader(GetLeader());
            }
            UpdateEnemyList();
        }
        public void UpdateEnemyList()
        {
            foreach (Unit member in partyMembers)
            {
                member.GetPartyHandler().SetEnemyList(partyEnemies);
                if (partyEnemies.Count <= 0) continue;
                foreach (Unit enemy in partyEnemies)
                {
                    member.GetCombatHandler().SetCombatTarget(enemy);
                    
                }
            }
        }
        public void RemoveEnemyFromCombat(Unit target)
        {
            foreach (Unit member in partyMembers)
            {
                member.GetCombatHandler().RemoveTarget(target);
            }
        }
        public Unit GetLeader()
        {
            return partyLeader;
        }
        public void SetLeader(Unit leader)
        {
            if(partyLeader != leader)
            {
                partyLeader = leader;
                onPartyLeaderChanged?.Invoke();
            }
        }
    }
}