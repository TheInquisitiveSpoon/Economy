using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//  Abstract class of a resource node within the game.
public class ResourceNode : MonoBehaviour
{
    public int MaxResources;
    public int ResourcesLeft;
    public float HarvestingDelay;
    public float RespawnTimer;
    public float RespawnTime;
    public ResourceTypes ResourceType;

    //  Gets the value of resources gained by the player during harvesting.
    virtual public int GetNumberHarvested(int playerSkill)
    {
        return 1;
    }

    //  Gets how long the player must wait between harvesting.
    virtual public float GetHarvestingDelay(int playerSkill)
    {
        return 0;
    }

    //  Slowly decrements the respawn timer.
    public void ReduceRespawnTimer()
    {
        RespawnTimer -= Time.deltaTime;
    }

    // Destroys the resource.
    virtual public void DestroyResource()
    {

    }

    //  Recreates the resource.
    virtual public void RegenerateResource()
    {

    }
}