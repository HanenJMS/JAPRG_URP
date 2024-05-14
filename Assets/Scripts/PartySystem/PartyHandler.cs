using GameLab.UnitSystem;
using System;
using System.Collections.Generic;
using UnityEngine;
namespace GameLab.PartySystem
{
    public class PartyHandler : MonoBehaviour
    {
        [SerializeField] Unit leader;
        [SerializeField] List<Unit> allies = new();
        [SerializeField] List<Unit> foes = new();
        public Action onLeaderChanged;
        public void AddAlly(Unit ally)
        {
            if (leader == null) return;
            if (!leader.GetPartyHandler().GetAllies().Contains(ally))
            {
                leader.GetPartyHandler().GetAllies().Add(ally);
                UpdateAllies();
            }
        }
        public void RemoveAlly(Unit ally)
        {
            if (leader == null) return;
            if (leader.GetPartyHandler().GetAllies().Contains(ally))
            {
                leader.GetPartyHandler().GetAllies().Remove(ally);
                UpdateAllies();
            }
        }
        public void AddFoe(Unit foe)
        {
            if (leader == null) return;
            if (!leader.GetPartyHandler().GetFoes().Contains(foe))
            {
                leader.GetPartyHandler().GetFoes().Add(foe);
                UpdateAllies();
            }
        }
        public void RemoveFoe(Unit foe)
        {
            if (leader == null) return;
            if (leader.GetPartyHandler().GetFoes().Contains(foe))
            {
                leader.GetPartyHandler().GetFoes().Remove(foe);
                UpdateAllies();
            }
        }
        public List<Unit> GetAllies()
        {
            return allies;
        }
        public void SetAllies(List<Unit> allies)
        {
            this.allies = allies;
        }

        public List<Unit> GetFoes()
        {
            return foes;
        }
        public void SetFoes(List<Unit> foes)
        {
            this.foes = foes;
        }




        public void UpdateAllies()
        {
            foreach (Unit member in leader.GetPartyHandler().GetAllies())
            {
                member.GetPartyHandler().SetLeader(leader);
                member.GetPartyHandler().SetAllies(leader.GetPartyHandler().GetAllies());
                member.GetPartyHandler().SetFoes(leader.GetPartyHandler().GetFoes());
                member.GetCombatHandler().SetEnemies(leader.GetPartyHandler().GetFoes());
            }
        }

        public Unit GetLeader()
        {
            return leader;
        }
        public void SetLeader(Unit leader)
        {
            if(this.leader != leader)
            {
                this.leader = leader;
                onLeaderChanged?.Invoke();
            }
        }
    }
}