using GameLab.InventorySystem;

namespace GameLab.TradingSystem
{
    public static class TradeBank
    {

        public static bool VerifyTrade(IamSlot receiver, IamSlot giver, int quantity)
        {
            if (receiver.GetAvailableCapacity() < quantity)
            {
                return false;
            }
            return giver.GetQuantity() >= quantity;
        }
        public static void Trade(IamSlot receiver, IamSlot giver, int quantity)
        {
            int i = receiver.GetQuantity() + quantity;
            receiver.SetQuantity(i);
            int p = giver.GetQuantity() - quantity;
            giver.SetQuantity(p);
        }
    }
}

