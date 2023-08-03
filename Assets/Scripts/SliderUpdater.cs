using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//  Updates a slider type UI.
public class SliderUpdater : MonoBehaviour
{
    public Text AmountText;
    public Text GoldText;
    public Slider Slider;
    public bool Selling;
    private Merchant Merchant;

    //  Sets the maximum value of the slider to initialise it.
    public void Initialise(int maxValue)
    {
        Slider.maxValue = maxValue;
    }

    //  Initialises sliders within the store, with important values for updating the UI.
    public void Initialise(int maxValue, Merchant merchant, bool selling)
    {
        Slider.maxValue = maxValue;
        Merchant = merchant;
        Selling = selling;
        Slider.value = 0;
        UpdateValue();
    }

    //  Updates the amount text of the trade slider.
    public void UpdateTradeValue()
    {
        AmountText.text = "Amount: " + Slider.value.ToString();
    }

    //  Updates the amount text and gold requirements text of other slider types.
    public void UpdateValue()
    {
        AmountText.text = "Amount: " + Slider.value.ToString();

        if (Selling) { GoldText.text = "= " + (Merchant.GetBuyPrice() * Slider.value).ToString() +"G"; }
        else { GoldText.text = "= " + (Merchant.GetSellPrice() * Slider.value).ToString() + "G"; }
    }
}
