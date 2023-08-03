using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//  Class for the blacksmith merchant.
public class Blacksmith : Merchant
{
    // Merchants start with 100G, x3 the yield from harvesting their resource, and an initial price for the resource.
    public Blacksmith()
    {
        Gold = 100;
        ResourceAmount = 15;
        ResourcePrice = 10;
        LowResourceThreshold = 10;
        HighResourceThreshold = 25;
        TradeDelay = 600;
        MerchantType = MerchantTypes.Blacksmith;
        ResourceType = ResourceTypes.IronOre;
        TradeResource();
    }
}
