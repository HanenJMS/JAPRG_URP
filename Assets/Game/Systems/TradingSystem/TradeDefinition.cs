using GameLab.InventorySystem;

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

        public InventorySlot GetBuyerInventory()
        {
            return receiver;
        }

        public InventorySlot GetSellerInventory()
        {
            return giver;
        }

        public int GetQuantity()
        {
            return quantity;
        }

        public int GetCash()
        {
            return buyerCash;
        }
    }
}

