using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//  Class for the Mason merchant.
public class Mason : Merchant
{
    // Merchants start with 100G, x3 the yield from harvesting their resource, and an initial price for the resource.
    public Mason()
    {
        Gold = 100;
        ResourceAmount = 30;
        ResourcePrice = 5;
        LowResourceThreshold = 20;
        HighResourceThreshold = 50;
        TradeDelay = 600;
        MerchantType = MerchantTypes.Mason;
        ResourceType = ResourceTypes.Stone;
        TradeResource();
    }
}
