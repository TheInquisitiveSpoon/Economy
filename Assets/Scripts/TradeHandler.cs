using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//  Handles updating of the trading UI elements of the store, specific to trading resource for another
//  resource type not selling or buying resources.
public class TradeHandler : MonoBehaviour
{
    public StoreHandler Store;
    public Slider Slider;
    public SliderUpdater TradeSlider;
    public Image TradeImage;
    public Text TradeAmountText;
    public Text RecieveText;
    public MerchantTypes MerchantType;

    //  Updates the trading resource images and dropdown options and selection.
    public void Initialise(MerchantTypes merchantType)
    {
        MerchantType = merchantType;
        UpdateImage();
        UpdateDropdownSelection();
    }

    //  Handles the exchange of resources between the player and merchant, updating the UI elements of the store and determining
    //  how many resources the merchant should give the player based on the conversion rate between resources.
    //  Wood costs roughly half the price of stone, and stone half the price of iron, therefore conversion rates reflect this,
    //  with an additional 10% taken off for the merchant to make profit.
    public void Trade()
    {
        switch (Store.TradeDropdown.Dropdown.options[Store.TradeDropdown.Dropdown.value].text)
        {
            case "Wood":
                Store.Player.Wood -= (int)TradeSlider.Slider.value;
                if (MerchantType == MerchantTypes.Mason) { Store.Player.Stone += Mathf.FloorToInt((float)(Slider.value * 0.4));
                    Store.Merchant.ResourceAmount -= Mathf.FloorToInt((float)(Slider.value * 0.4)); }
                else if (MerchantType == MerchantTypes.Blacksmith) { Store.Player.IronOre += Mathf.FloorToInt((float)(Slider.value * 0.2));
                    Store.Merchant.ResourceAmount -= Mathf.FloorToInt((float)(Slider.value * 0.2)); }
                Store.UpdateStore(Store.Merchant);
                break;

            case "Stone":
                Store.Player.Stone -= (int)TradeSlider.Slider.value;
                if (MerchantType == MerchantTypes.Carpenter) { Store.Player.Wood += Mathf.FloorToInt((float)(Slider.value * 1.9));
                    Store.Merchant.ResourceAmount -= Mathf.FloorToInt((float)(Slider.value * 1.9)); }
                else if (MerchantType == MerchantTypes.Blacksmith) { Store.Player.IronOre += Mathf.FloorToInt((float)(Slider.value * 0.4));
                    Store.Merchant.ResourceAmount -= Mathf.FloorToInt((float)(Slider.value * 0.4)); }
                Store.UpdateStore(Store.Merchant);
                break;

            case "Iron Ore":
                Store.Player.IronOre -= (int)TradeSlider.Slider.value;
                if (MerchantType == MerchantTypes.Carpenter) { Store.Player.Wood += Mathf.FloorToInt((float)(Slider.value * 3.9));
                    Store.Merchant.ResourceAmount -= Mathf.FloorToInt((float)(Slider.value * 3.9)); }
                else if (MerchantType == MerchantTypes.Mason) { Store.Player.Stone += Mathf.FloorToInt((float)(Slider.value * 1.9));
                    Store.Merchant.ResourceAmount -= Mathf.FloorToInt((float)(Slider.value * 1.9)); }
                Store.UpdateStore(Store.Merchant);
                break;
        }
    }

    //  Updates the text elements that show how much the player will receive from the merchant, using the conversion rates:
    //  Wood costs roughly half the price of stone, and stone half the price of iron, therefore conversion rates reflect this,
    //  with an additional 10% taken off for the merchant to make profit.
    private void CalculateReceiveAmount(ResourceTypes tradeResource)
    {
        if (MerchantType == MerchantTypes.Carpenter)
        {
            if (tradeResource == ResourceTypes.Stone)
            {
                RecieveText.text = "Receive: " + Mathf.FloorToInt((float)(Slider.value * 1.9));
            }
            else
            {
                RecieveText.text = "Receive: " + Mathf.FloorToInt((float)(Slider.value * 3.9));
            }
        }
        else if (MerchantType == MerchantTypes.Mason)
        {
            if (tradeResource == ResourceTypes.Wood)
            {
                RecieveText.text = "Receive: " + Mathf.FloorToInt((float)(Slider.value * 0.4));
            }
            else
            {
                RecieveText.text = "Receive: " + Mathf.FloorToInt((float)(Slider.value * 1.9));
            }
        }
        else
        {
            if (tradeResource == ResourceTypes.Wood)
            {
                RecieveText.text = "Receive: " + Mathf.FloorToInt((float)(Slider.value * 0.2));
            }
            else
            {
                RecieveText.text = "Receive: " + Mathf.FloorToInt((float)(Slider.value * 0.4));
            }
        }
    }

    //  Updates the receive UI text when the slider is changed.
    public void TradeSliderChanged()
    {
        switch (Store.TradeDropdown.Dropdown.options[Store.TradeDropdown.Dropdown.value].text)
        {
            case "Wood":
                CalculateReceiveAmount(ResourceTypes.Wood);
                break;

            case "Stone":
                CalculateReceiveAmount(ResourceTypes.Stone);
                break;

            case "Iron Ore":
                CalculateReceiveAmount(ResourceTypes.IronOre);
                break;
        }
    }

    //  When the player selects a different resource type from the dropdown, resource image is updated,
    //  the slider is reinitialised to show the right information, and the text UI elements are updated.
    public void UpdateDropdownSelection()
    {
        switch (Store.TradeDropdown.Dropdown.options[Store.TradeDropdown.Dropdown.value].text)
        {
            case "Wood":
                TradeSlider.Initialise(Store.Player.Wood);
                TradeSlider.UpdateTradeValue();
                CalculateReceiveAmount(ResourceTypes.Wood);
                break;

            case "Stone":
                TradeSlider.Initialise(Store.Player.Stone);
                TradeSlider.UpdateTradeValue();
                CalculateReceiveAmount(ResourceTypes.Stone);
                break;

            case "Iron Ore":
                TradeSlider.Initialise(Store.Player.IronOre);
                TradeSlider.UpdateTradeValue();
                CalculateReceiveAmount(ResourceTypes.IronOre);
                break;
        }
    }

    //  Updates the image of the merchant resource type.
    public void UpdateImage()
    {
        switch (MerchantType)
        {
            case MerchantTypes.Carpenter:
                TradeImage.sprite = Store.WoodImage;
                break;

            case MerchantTypes.Mason:
                TradeImage.sprite = Store.StoneImage;
                break;

            case MerchantTypes.Blacksmith:
                TradeImage.sprite = Store.IronOreImage;
                break;
        }
    }
}
