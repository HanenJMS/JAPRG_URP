using GameLab.InventorySystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameLab.TradingSystem
{
    public class TradeDefinition
    {
        InventorySlot receiver;
        int buyerCash;

        InventorySlot giver;
        int quantity;


        public TradeDefinition(InventorySlot receiver, InventorySlot giver, int buyerCash = 0, int quantity = 0)
        {
            this.receiver = receiver;
            this.buyerCash = buyerCash;
            this.giver = giver;
            this.quantity = quantity;
        }

        public InventorySlot GetBuyerInventory() => receiver;
        public InventorySlot GetSellerInventory() => giver;
        public int GetQuantity() => quantity;
        public int GetCash() => buyerCash;
    }
}

