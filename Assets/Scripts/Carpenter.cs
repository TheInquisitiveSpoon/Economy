using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//  Class for the carpenter merchant.
public class Carpenter : Merchant
{
    // Merchants start with 100G, x3 the yield from harvesting their resource, and an initial price for the resource.
    public Carpenter()
    {
        Gold = 100;
        ResourceAmount = 60;
        ResourcePrice = 3;
        LowResourceThreshold = 40;
        HighResourceThreshold = 100;
        TradeDelay = 600;
        MerchantType = MerchantTypes.Carpenter;
        ResourceType = ResourceTypes.Wood;
        TradeResource();
    }
}
