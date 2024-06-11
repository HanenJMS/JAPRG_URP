using GameLab.InventorySystem;
using UnityEngine;

namespace GameLab.TradingSystem
{
    public class TradeHandler : MonoBehaviour
    {

        int wealth = 500;
        public void TradeItem(IamSlot receiver, IamSlot giver, int quantity = 0)
        {
            if (!TradeBank.VerifyTrade(receiver, giver, quantity))
            {
                quantity = PlanQuantityTrade(receiver, giver, quantity);
            }
            TradeBank.Trade(receiver, giver, quantity);
        }


        //brokerageAI probably
        int PlanQuantityTrade(IamSlot receiver, IamSlot giver, int quantity = 0)
        {
            if (receiver.GetAvailableCapacity() < quantity)
            {
                quantity = receiver.GetAvailableCapacity();
            }

            if (quantity > giver.GetQuantity())
            {
                quantity = giver.GetQuantity();
            }
            return quantity;
        }
    }

}

