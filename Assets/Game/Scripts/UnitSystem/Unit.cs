using GameLab.Animation;
using GameLab.CombatSystem;
using GameLab.FactionSystem;
using GameLab.InteractableSystem;
using GameLab.InventorySystem;
using GameLab.PartySystem;
using GameLab.ResourceSystem;
using GameLab.UISystem;
using GameLab.UnitSystem.AbilitySystem;
using GameLab.UnitSystem.ActionSystem;
using UnityEngine;

namespace GameLab.UnitSystem
{
    [System.Serializable]
    public class Unit : Interactable
    {
        ActionHandler actionHandler;
        HealthHandler healthHandler;
        FactionHandler factionHandler;
        CombatHandler combatHander;
        PartyHandler partyHandler;
        EquipmentHandler equipmentHandler;
        InventoryHandler inventoryHandler;
        AbilityHandler abilityHandler;
        WorldSpaceUIHandler worldSpaceUIHandler;
        UnitAnimationHandler unitAnimationHandler;
        private void Awake()
        {
            actionHandler = GetComponent<ActionHandler>();
            healthHandler = GetComponent<HealthHandler>();
            factionHandler = GetComponent<FactionHandler>();
            combatHander = GetComponent<CombatHandler>();
            partyHandler = GetComponent <PartyHandler>();
            equipmentHandler = GetComponent<EquipmentHandler>();
            unitAnimationHandler = GetComponent<UnitAnimationHandler>();
            abilityHandler = GetComponent<AbilityHandler>();
            inventoryHandler = GetComponent<InventoryHandler>();
            worldSpaceUIHandler = GetComponent<WorldSpaceUIHandler>();
            healthHandler.onDead += OnDeath;
        }

        void OnDeath()
        {
            actionHandler.SetCurrentAction(null);
            unitAnimationHandler.SetTrigger("death");
            GetComponent<CapsuleCollider>().enabled = false;
        }
        public ActionHandler GetActionHandler()
        {
            return actionHandler;
        }
        public HealthHandler GetHealthHandler()
        {
            return healthHandler;
        }
        public CombatHandler GetCombatHandler()
        {
            return combatHander;
        }
        public FactionHandler GetFactionHandler()
        {
            return factionHandler;
        }
        public PartyHandler GetPartyHandler()
        {
            return partyHandler;
        }
        public AbilityHandler GetAbilityHandler() => abilityHandler;
        public EquipmentHandler GetEquipmentHandler() => equipmentHandler;
        public InventoryHandler GetInventoryHandler() => inventoryHandler;
        public WorldSpaceUIHandler GetWorldSpaceUIHandler() => worldSpaceUIHandler;
    }
}

