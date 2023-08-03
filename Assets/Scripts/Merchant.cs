using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//  Base class for a merchant in the world
public class Merchant : MonoBehaviour
{
    public MerchantTypes MerchantType;
    public int Gold;
    public int ResourceAmount;
    public ResourceTypes ResourceType;
    public float ResourcePrice;
    public int LowResourceThreshold;
    public int HighResourceThreshold;
    public float TradeDelay;
    public PlayerController Player;

    //  Returns the price the merchant is willing to sell their resource for.
    //  Which is either the base price, or base price x2 if resources are low.
    public int GetSellPrice()
    {
        if (ResourceAmount < LowResourceThreshold) { return (int)(ResourcePrice * 2); }

        return (int)ResourcePrice;
    }

    //  Returns the price the merchant is willing to buy resources for.
    //  If resources are low they will buy for x2 the base price, if resources are abundant they will
    //  buy for half the price, or they will buy for the default price.
    //  All buying prices have 10% deducted for the merchant to make money.
    public int GetBuyPrice()
    {
        if (ResourceAmount < LowResourceThreshold) { return (int)((ResourcePrice * 2) * 0.9); }
        else if (ResourceAmount > HighResourceThreshold) { return (int)(ResourcePrice * 0.4); }

        return (int)(ResourcePrice * 0.9);
    }

    public IEnumerator TradeResource()
    {
        //  Wait for a timer, default 10 minutes.
        yield return new WaitForSeconds(TradeDelay);

        //  Random returns 0 or 1, to determine buying or selling.
        int soldResource = Random.Range(0, 2);

        //  Mimics trader buying or selling from other customers, acting as a sink or source.
        if (soldResource == 0)
        {
            int amountSold = Mathf.FloorToInt((ResourceAmount * Random.Range(0.0f, 1.0f)));
            Gold += GetBuyPrice() * amountSold;
            ResourceAmount -= amountSold;
        }
        else
        {
            int maxPotentialBuy = Mathf.FloorToInt(Gold / GetBuyPrice());
            int amountBought = Random.Range(1, maxPotentialBuy);
            Gold -= GetBuyPrice() * amountBought;
            ResourceAmount += amountBought;
        }

        //  Begins this function again.
        StartCoroutine(TradeResource());
    }
}
