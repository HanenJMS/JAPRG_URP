using GameLab.Animation;
using GameLab.CombatSystem;
using GameLab.FactionSystem;
using GameLab.InteractableSystem;
using GameLab.InventorySystem;
using GameLab.PartySystem;
using GameLab.ResourceSystem;
using GameLab.StatSystem;
using GameLab.TradingSystem;
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
        TradeHandler tradeHandler;
        AbilityHandler abilityHandler;
        WorldSpaceUIHandler worldSpaceUIHandler;
        UnitAnimationHandler unitAnimationHandler;
        StatHandler statHandler;

        Interactable_Building currentBuildingLocation = null;
        private void Awake()
        {
            actionHandler = GetComponent<ActionHandler>();
            healthHandler = GetComponent<HealthHandler>();
            factionHandler = GetComponent<FactionHandler>();
            combatHander = GetComponent<CombatHandler>();
            partyHandler = GetComponent<PartyHandler>();
            equipmentHandler = GetComponent<EquipmentHandler>();
            unitAnimationHandler = GetComponent<UnitAnimationHandler>();
            abilityHandler = GetComponent<AbilityHandler>();
            inventoryHandler = GetComponent<InventoryHandler>();
            worldSpaceUIHandler = GetComponent<WorldSpaceUIHandler>();
            tradeHandler = GetComponent<TradeHandler>();
            statHandler = GetComponent<StatHandler>();
            healthHandler.onDead += OnDeath;
        }
        public override void Interact(object interaction)
        {

        }
        void OnDeath()
        {
            actionHandler.SetCurrentAction(null);
            unitAnimationHandler.SetTrigger("death");
            GetComponent<CapsuleCollider>().enabled = false;
        }
        public void EnterBuilding(Interactable_Building building)
        {
            currentBuildingLocation = building;
        }

        public bool InBuilding()
        {
            return currentBuildingLocation != null;
        }

        public void ExitBuilding()
        {
            currentBuildingLocation.Exit(this);
            currentBuildingLocation = null;
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
        public AbilityHandler GetAbilityHandler()
        {
            return abilityHandler;
        }

        public EquipmentHandler GetEquipmentHandler()
        {
            return equipmentHandler;
        }

        public InventoryHandler GetInventoryHandler()
        {
            return inventoryHandler;
        }

        public TradeHandler GetTradeHandler()
        {
            return tradeHandler;
        }

        public WorldSpaceUIHandler GetWorldSpaceUIHandler()
        {
            return worldSpaceUIHandler;
        }

        public StatHandler GetStatHandler()
        {
            return statHandler;
        }
    }
}

