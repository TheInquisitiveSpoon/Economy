using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//  A class for updating and showing the UI for buying, selling and trading with merchants.
public class StoreHandler : MonoBehaviour
{
    public InventoryUpdater InventoryUpdater;
    public PlayerController Player;
    public Merchant Merchant;
    public SliderUpdater SellSlider;
    public SliderUpdater BuySlider;
    public DropdownUpdater TradeDropdown;
    public TradeHandler TradeHandler;
    public Text MerchantNameText;
    public Text PlayerGoldText;
    public Text PlayerResourceText;
    public Text MerchantGoldText;
    public Text MerchantResourceText;
    public Sprite WoodImage;
    public Sprite StoneImage;
    public Sprite IronOreImage;
    public List<Image> ResourceImages;

    //  Updates all of the UI elements of the store page when changes are made.
    public void UpdateStore(Merchant merchant)
    {
        Merchant = merchant;
        PlayerGoldText.text = "Gold: " + Player.Gold.ToString();
        MerchantGoldText.text = "Gold: " + merchant.Gold.ToString();
        BuySlider.Initialise(merchant.ResourceAmount, Merchant, false);
        UpdateMerchantTypeSpecificUI();
        TradeDropdown.InitialiseDropdown(merchant.MerchantType);
        TradeHandler.Initialise(merchant.MerchantType);
        gameObject.SetActive(true);
    }

    //  Stops showing the store and allows the player to move.
    public void CloseStore()
    {
        gameObject.SetActive(false);
        Player.CameraController.ShowHideCursor();
        Player.IsBusy = false;
    }

    //  Handles the gold and resource exchange between the player and the merchant, when the player
    //  sells resources to the merchant, the values are determined by the type of merchant,
    //  and the store is updated after.
    public void PlayerSell()
    {
        switch (Merchant.MerchantType)
        {
            case MerchantTypes.Carpenter:
                Player.Gold += Merchant.GetBuyPrice() * (int)SellSlider.Slider.value;
                Merchant.Gold -= Merchant.GetBuyPrice() * (int)SellSlider.Slider.value;
                Player.Wood -= (int)SellSlider.Slider.value;
                Merchant.ResourceAmount += (int)SellSlider.Slider.value;
                UpdateStore(Merchant);
                break;

            case MerchantTypes.Mason:
                Player.Gold += Merchant.GetBuyPrice() * (int)SellSlider.Slider.value;
                Merchant.Gold -= Merchant.GetBuyPrice() * (int)SellSlider.Slider.value;
                Player.Stone -= (int)SellSlider.Slider.value;
                Merchant.ResourceAmount += (int)SellSlider.Slider.value;
                UpdateStore(Merchant);
                break;

            case MerchantTypes.Blacksmith:
                Player.Gold += Merchant.GetBuyPrice() * (int)SellSlider.Slider.value;
                Merchant.Gold -= Merchant.GetBuyPrice() * (int)SellSlider.Slider.value;
                Player.IronOre -= (int)SellSlider.Slider.value;
                Merchant.ResourceAmount += (int)SellSlider.Slider.value;
                UpdateStore(Merchant);
                break;
        }
    }

    //  Handles the gold and resource exchange between the player and the merchant, when the player
    //  buys resources from the merchant, the values are determined by the type of merchant,
    //  and the store is updated after.
    public void PlayerBuy()
    {
        switch (Merchant.MerchantType)
        {
            case MerchantTypes.Carpenter:
                if (Player.Gold < Merchant.GetSellPrice() * (int)BuySlider.Slider.value) { return; }
                Merchant.Gold += Merchant.GetSellPrice() * (int)BuySlider.Slider.value;
                Player.Gold -= Merchant.GetSellPrice() * (int)BuySlider.Slider.value;
                Merchant.ResourceAmount -= (int)BuySlider.Slider.value;
                Player.Wood += (int)BuySlider.Slider.value;
                UpdateStore(Merchant);
                break;

            case MerchantTypes.Mason:
                if (Player.Gold < Merchant.GetSellPrice() * (int)BuySlider.Slider.value) { return; }
                Merchant.Gold += Merchant.GetSellPrice() * (int)BuySlider.Slider.value;
                Player.Gold -= Merchant.GetSellPrice() * (int)BuySlider.Slider.value;
                Merchant.ResourceAmount -= (int)BuySlider.Slider.value;
                Player.Stone += (int)BuySlider.Slider.value;
                UpdateStore(Merchant);
                break;

            case MerchantTypes.Blacksmith:
                if (Player.Gold < Merchant.GetSellPrice() * (int)BuySlider.Slider.value) { return; }
                Merchant.Gold += Merchant.GetSellPrice() * (int)BuySlider.Slider.value;
                Player.Gold -= Merchant.GetSellPrice() * (int)BuySlider.Slider.value;
                Merchant.ResourceAmount -= (int)BuySlider.Slider.value;
                Player.IronOre += (int)BuySlider.Slider.value;
                UpdateStore(Merchant);
                break;
        }
    }

    //  Updates the trading UI when necessary.
    public void PlayerTrade()
    {
        TradeHandler.Trade();
    }

    //  Some UI elements are not able to be set until the merchant type is determined,
    //  this function sets those UI elements to display the correct information when the
    //  merchant type is determined.
    private void UpdateMerchantTypeSpecificUI()
    {
        switch(Merchant.MerchantType)
        {
            case MerchantTypes.Carpenter:
                MerchantNameText.text = "Carpenter:";
                MerchantResourceText.text = "Wood: " + Merchant.ResourceAmount.ToString();
                PlayerResourceText.text = "Wood: " + Player.Wood.ToString();
                SellSlider.Initialise(Player.Wood, Merchant, true);
                UpdateResourceImages(Merchant.ResourceType);
                break;

            case MerchantTypes.Mason:
                MerchantNameText.text = "Mason:";
                MerchantResourceText.text = "Stone: " + Merchant.ResourceAmount.ToString();
                PlayerResourceText.text = "Stone: " + Player.Stone.ToString();
                SellSlider.Initialise(Player.Stone, Merchant, true);
                UpdateResourceImages(Merchant.ResourceType);
                break;

            case MerchantTypes.Blacksmith:
                MerchantNameText.text = "Blacksmith:";
                MerchantResourceText.text = "Iron Ore: " + Merchant.ResourceAmount.ToString();
                PlayerResourceText.text = "Iron Ore: " + Player.IronOre.ToString();
                SellSlider.Initialise(Player.IronOre, Merchant, true);
                UpdateResourceImages(Merchant.ResourceType);
                break;
        }
    }

    //  Images of the resources are used to show the type of resource being bartered,
    //  this function updates those images to the correct type.
    private void UpdateResourceImages(ResourceTypes resourceType)
    {
        Sprite resourceImage = WoodImage;

        switch(resourceType)
        {
            case ResourceTypes.Wood:
                resourceImage = WoodImage;
                break;

            case ResourceTypes.Stone:
                resourceImage = StoneImage;
                break;

            case ResourceTypes.IronOre:
                resourceImage = IronOreImage;
                break;
        }

        foreach(Image image in ResourceImages)
        {
            image.sprite = resourceImage;
        }
    }
}
