using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//  Class to handle updating the trade dropdown of the store.
public class DropdownUpdater : MonoBehaviour
{
    public StoreHandler Store;
    public Dropdown Dropdown;
    public Image ResourceImage;

    //  Sets the initial images and options for the dropdown.
    public void InitialiseDropdown(MerchantTypes merchantType)
    {
        UpdateOptions(merchantType);
        UpdateImage();
    }

    //  Changes the image based on what option is selected from the dropdown.
    public void UpdateImage()
    {
        switch(Dropdown.options[Dropdown.value].text)
        {
            case "Wood":
                ResourceImage.sprite = Store.WoodImage;
                break;

            case "Stone":
                ResourceImage.sprite = Store.StoneImage;
                break;

            case "Iron Ore":
                ResourceImage.sprite = Store.IronOreImage;
                break;
        }
    }

    //  Adds a new list of menu options to the dropdown, based on what merchant is being used.
    public void UpdateOptions(MerchantTypes merchantType)
    {
        Dropdown.options.Clear();
        List<string> items = new List<string>();

        //  Adds correct trade options based on the current merchant.
        if (merchantType == MerchantTypes.Carpenter)
        {
            items.Add("Stone");
            items.Add("Iron Ore");
        }
        else if (merchantType == MerchantTypes.Mason)
        {
            items.Add("Wood");
            items.Add("Iron Ore");
        }
        else
        {
            items.Add("Wood");
            items.Add("Stone");
        }

        //  Adds them to the option data text.
        foreach(string item in items)
        {
            Dropdown.options.Add(new Dropdown.OptionData() { text = item });
        }

        //  Initialises the value and text.
        Dropdown.value = 0;
        Dropdown.captionText.text = Dropdown.options[0].text;
    }
}
