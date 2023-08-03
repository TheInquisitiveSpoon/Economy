using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//  Updates the player inventory UI to the correct values.
public class InventoryUpdater : MonoBehaviour
{
    public Text GoldText;
    public Text WoodText;
    public Text StoneText;
    public Text IronOreText;
    public PlayerController Player;

    //  Gets the correct values for the strings from the player variables.
    public void UpdateInventory()
    {
        GoldText.text = "Gold: " + Player.Gold.ToString();
        WoodText.text = "Wood: " + Player.Wood.ToString();
        StoneText.text = "Stone: " + Player.Stone.ToString();
        IronOreText.text = "Iron Ore: " + Player.IronOre.ToString();
    }
}
