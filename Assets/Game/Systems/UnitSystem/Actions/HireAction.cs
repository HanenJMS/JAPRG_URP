using GameLab.PartySystem;
using GameLab.UISystem;
using UnityEngine;
namespace GameLab.UnitSystem.ActionSystem
{
    public class HireAction : BaseAction
    {
        ActionHandler actionHandler;
        PartyHandler partyHandler;

        private void Awake()
        {
            actionHandler = GetComponent<ActionHandler>();
            partyHandler = GetComponent<PartyHandler>();
        }

        public override void Cancel()
        {
            
        }

        public override bool CanExecuteOnTarget(object target)
        {
            if(target is Unit)
            {
                //eventually, i want to manage checks between enemy factions as well. assuming I keep the action Hiring as a mean of interaction.
                //if((target as Unit).GetFactionHandler().GetFaction() == GetComponent<Unit>().GetFactionHandler().GetFaction())
                {
                    if((target as Unit).GetPartyHandler().GetLeader() == null)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public override void ExecuteOnTarget(object target)
        {
            partyHandler.SetLeader(GetComponent<Unit>());
            actionHandler.SetCurrentAction(this);
            partyHandler.AddAlly((target as Unit));
        }
        public override string ToString()
        {
            return "Hire";
        }


    }
}

