using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//  Handles the UI for the player level in each resource.
public class LevelBar : MonoBehaviour
{
    public Text LevelText;
    public Text XPText;
    public int MaxNumber;

    //  Sets the new max XP required to increase a level.
    public void BeginProgress(int maxNumber)
    {
        MaxNumber = maxNumber;
    }

    //  Updates the text with a current XP value compared to the max for that level.
    public void UpdateXPText(int currentNumber)
    {
        XPText.text = "XP: " + currentNumber.ToString() + " / " + MaxNumber.ToString();
    }

    //  Updates the level shown in the UI depending on the resource type.
    public void UpdateLevelText(ResourceTypes resource, int Level)
    {
        switch(resource)
        {
            case ResourceTypes.Wood:
                LevelText.text = "Logging Level: " + Level.ToString();
                break;
            case ResourceTypes.Stone:
                LevelText.text = "Mining Level: " + Level.ToString();
                break;
            case ResourceTypes.IronOre:
                LevelText.text = "Smithing Level: " + Level.ToString();
                break;
        }
    }
}
