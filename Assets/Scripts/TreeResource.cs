using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//  Tree resource node class.
public class TreeResource : ResourceNode
{
    //  Returns a random number within a range, based on the players skill in harvesting that resource.
    //  Corrects the random value if it is greater than the number of resources left.
    public override int GetNumberHarvested(int playerSkill)
    {
        if (ResourcesLeft == 0) { return 0; }

        int random = 1;

        //  Determins the range to allow the random function to return, using player skill.
        //  The skill values 4, 8 are the predetermined ranges for increasings the players
        //  harvesting abilities.
        if (playerSkill < 4) { random = Random.Range(1, 8); }
        else if (playerSkill > 4 && playerSkill < 8) { random = Random.Range(1, 12); }
        else { random = Random.Range(1, 15); }

        //  Corrects the random value if it would return more than what is left.
        random = random > ResourcesLeft ? ResourcesLeft : random;
        ResourcesLeft -= random;
        return random;
    }

    //  Returns the time it should take for the player to harvest from the resource, decrementing
    //  this value based on the players skill harvesting this resource.
    public override float GetHarvestingDelay(int playerSkill)
    {
        //  Default harvesting delay for wood is 3, meaning 2 and 1 should be achieved in the next values.
        if (playerSkill < 4) { return HarvestingDelay; }
        if (playerSkill > 4 && playerSkill < 8) { return HarvestingDelay - 1.0f; }
        else { return HarvestingDelay - 2.0f; }
    }

    //  Hides the resources and initialises a timer to bring it back, default at 5 mins.
    public override void DestroyResource()
    {
        gameObject.SetActive(false);
        RespawnTimer = RespawnTime;
    }

    //  Regenerates the resources after the timer has passed, by setting the resources left, and showing the object.
    public override void RegenerateResource()
    {
        ResourcesLeft = MaxResources;
        gameObject.SetActive(true);
    }
}
