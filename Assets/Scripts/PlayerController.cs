//  PlayerMovement.cs - Script for enabling Player to move, as well as performing gravity checks.

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//  CLASS:
public class PlayerController : MonoBehaviour
{
    //  REFERENCES:
    public CharacterController  Controller;
    public CameraController     CameraController;
    public ProgressBar ProgressBar;
    public LevelBar LoggingBar;
    public LevelBar MiningBar;
    public LevelBar SmithingBar;
    public InventoryUpdater InventoryUpdater;
    public StoreHandler StoreUI;
    public List<Merchant> Merchants;
    public List<ResourceNode>   Resources;
    public List<ResourceNode> HarvestedResources;

    //  VARIABLES:
    private Vector3     Velocity;
    public float        MoveSpeed       = 5.0f;
    public float        JumpPower       = 2.0f;
    public int Gold = 0;
    public int Wood = 0;
    public int Stone = 0;
    public int IronOre = 0;
    public float LoggingXP = 500.0f;
    public float MiningXP = 500.0f;
    public float SmithingXP = 500.0f;
    public int LoggingLevel = 1;
    public int MiningLevel = 1;
    public int SmithingLevel = 1;
    public bool IsBusy = false;
    public float HarvestingDelay = 3;

    //  FUNCTIONS:
    private void Awake()
    {
        LoggingBar.BeginProgress(GetXPRequirement(ResourceTypes.Wood));
        LoggingBar.UpdateXPText((int)LoggingXP);
        LoggingBar.UpdateLevelText(ResourceTypes.Wood, LoggingLevel);
        MiningBar.BeginProgress(GetXPRequirement(ResourceTypes.Stone));
        MiningBar.UpdateXPText((int)MiningXP);
        MiningBar.UpdateLevelText(ResourceTypes.Stone, MiningLevel);
        SmithingBar.BeginProgress(GetXPRequirement(ResourceTypes.IronOre));
        SmithingBar.UpdateXPText((int)SmithingXP);
        SmithingBar.UpdateLevelText(ResourceTypes.IronOre, SmithingLevel);
    }

    //  Function to update the script during runtime.
    void Update()
    {
        //  Terminates program.
        if (Input.GetKeyDown(KeyCode.F4))
        {
            Application.Quit();
        }

        //  Determines which resources require destroying or adding.
        UpdateResources();

        //  Ensures the player is within range of a resource, before allowing them to begin harvesting it.
        foreach (ResourceNode resource in Resources)
        {
            if (Vector3.Distance(transform.position, resource.transform.position) < 3.0f)
            {
                if (Input.GetKeyDown(KeyCode.E) && !IsBusy)
                {
                    IsBusy = true;
                    ProgressBar.BeginProgress(resource.MaxResources);
                    StartCoroutine(HarvestResource(resource));
                }
            }
        }

        //  Ensures the player is within range of a merchant, before allowing them to open the store.
        foreach(Merchant merchant in Merchants)
        {
            if (Vector3.Distance(transform.position, merchant.transform.position) < 4.0f)
            {
                if (Input.GetKeyDown(KeyCode.E) && !IsBusy)
                {
                    StoreUI.UpdateStore(merchant);
                    CameraController.ShowHideCursor();
                }
            }
        }

        //  Updates inventory values.
        InventoryUpdater.UpdateInventory();

        // Set velocity to negative so Player stays touching the ground.
        if (Controller.isGrounded && Velocity.y < 0.0f) { Velocity.y = -2.0f; }
        else
        {
            //  Move Player by current gravity velocity.
            Velocity.y += -20.0f * Time.deltaTime;
            Controller.Move(Velocity * Time.deltaTime);
        }

        //  Enables player movement when the cursor is not locked.
        if (!CameraController.IsCursorHidden && !IsBusy)
        {
            //  Get Player input from axis.
            float xDirection = Input.GetAxis("Horizontal");
            float zDirection = Input.GetAxis("Vertical");

            //  Add jump velocity to enable jumping if grounded.
            if (Input.GetButtonDown("Jump") && Controller.isGrounded)
            {
                Velocity.y = Mathf.Sqrt(JumpPower * -2.0f * -20.0f);
            }

            //  Move Player by movement input.
            Vector3 moveDirection = transform.right * xDirection + transform.forward * zDirection;
                
            //  Adds increased movement speed if player is holding Left Shift.
            if (Input.GetKey(KeyCode.LeftShift))    { Controller.Move(moveDirection * (2 * MoveSpeed) * Time.deltaTime); }
            else                                    { Controller.Move(moveDirection * MoveSpeed * Time.deltaTime); }
        }
    }

    //  Adds resources that have passed their respawn time back into the world.
    public void UpdateResources()
    {
        List<ResourceNode> Regenerate = new List<ResourceNode>();

        //  Determines if resources have went past their respawn timer.
        foreach (ResourceNode resource in HarvestedResources)
        {
            resource.ReduceRespawnTimer();

            if (resource.RespawnTimer < 0)
            {
                Regenerate.Add(resource);
            }
        }

        //  Adds them to the active resources list, and regenerates the resource.
        foreach(ResourceNode resource in Regenerate)
        {
            HarvestedResources.Remove(resource);
            Resources.Add(resource);
            resource.RegenerateResource();
        }
    }

    //  Begins the active harvesting process for the player to mine a resource.
    //  This functions runs asynchronously, by waiting by the harvesting delay between each harvest,
    //  as harvesting a resource takes time. Then determining how much the player harvest after each delay
    //  ending the process if all resources are harvested, and beginning a new coroutine if not.
    IEnumerator HarvestResource(ResourceNode resource)
    {
        //  Determines how long the player should wait before beginning the rest of the function,
        //  using the resource type and player level at harvesting that resource.
        switch(resource.ResourceType)
        {
            case ResourceTypes.Wood:
                yield return new WaitForSeconds(resource.GetHarvestingDelay(LoggingLevel));
                break;

            case ResourceTypes.Stone:
                yield return new WaitForSeconds(resource.GetHarvestingDelay(MiningLevel));
                break;

            case ResourceTypes.IronOre:
                yield return new WaitForSeconds(resource.GetHarvestingDelay(SmithingLevel));
                break;
        }

        //  Determines how much the player of the resource the player has obtained, in this pass.
        switch(resource.ResourceType)
        {
            case ResourceTypes.Wood:
                Wood += resource.GetNumberHarvested(LoggingLevel);
                break;

            case ResourceTypes.Stone:
                Stone += resource.GetNumberHarvested(MiningLevel);
                break;

            case ResourceTypes.IronOre:
                IronOre += resource.GetNumberHarvested(SmithingLevel);
                break;
        }

        //  Updates the progress bar UI.
        ProgressBar.UpdateValue(resource.ResourcesLeft);

        //  After the resource is harvested, adds XP, destroys the resource, allows the player to move,
        //  then hides the progress ui and stops and running coroutines.
        if (resource.ResourcesLeft == 0)
        {
            AddXP(resource.ResourceType);
            resource.DestroyResource();
            HarvestedResources.Add(resource);
            Resources.Remove(resource);
            IsBusy = false;
            ProgressBar.ResetProgress();
            StopAllCoroutines();
        }
        else
        {
            //  Starts a new coroutine of this function if all resources aren't harvested.
            StartCoroutine(HarvestResource(resource));
        }
    }

    //  Adds 500XP to the correct player resource XP, updates UI, and determines if the player has levelled up.
    private void AddXP(ResourceTypes resourceType)
    {
        switch(resourceType)
        {
            case ResourceTypes.Wood:
                LoggingXP += 500;
                LoggingBar.UpdateXPText((int)LoggingXP);
                break;
            case ResourceTypes.Stone:
                MiningXP += 500;
                MiningBar.UpdateXPText((int)MiningXP);
                break;
            case ResourceTypes.IronOre:
                SmithingXP += 500;
                SmithingBar.UpdateXPText((int)SmithingXP);
                break;
        }

        CheckLevelIncrease(resourceType);
    }

    //  Determines if the player has increased in level, then adds the level if so and updates UI.
    private void CheckLevelIncrease(ResourceTypes resourceType)
    {
        switch (resourceType)
        {
            case ResourceTypes.Wood:
                if (LoggingXP > GetXPRequirement(resourceType))
                {
                    LoggingXP = LoggingXP - GetXPRequirement(resourceType);
                    LoggingLevel++;
                    LoggingBar.BeginProgress(GetXPRequirement(resourceType));
                    LoggingBar.UpdateXPText((int)LoggingXP);
                    LoggingBar.UpdateLevelText(resourceType, LoggingLevel);
                }
                break;
            case ResourceTypes.Stone:
                if (MiningXP > GetXPRequirement(resourceType))
                {
                    MiningXP = MiningXP - GetXPRequirement(resourceType);
                    MiningLevel++;
                    MiningBar.BeginProgress(GetXPRequirement(resourceType));
                    MiningBar.UpdateXPText((int)MiningXP);
                    MiningBar.UpdateLevelText(resourceType, MiningLevel);
                }
                break;
            case ResourceTypes.IronOre:
                if (SmithingXP > GetXPRequirement(resourceType))
                {
                    SmithingXP = SmithingXP - GetXPRequirement(resourceType);
                    SmithingLevel++;
                    SmithingBar.BeginProgress(GetXPRequirement(resourceType));
                    SmithingBar.UpdateXPText((int)SmithingXP);
                    SmithingBar.UpdateLevelText(resourceType, SmithingLevel);
                }
                break;
        }
    }

    //  Determines how much XP the player requires to level up the next skill.
    //  This function is determines as 1000 * (1.15 * SkillLevel) to add more required XP as skills progress.
    //  Must determine the correct XP for all resources, so the resource type is required.
    public int GetXPRequirement(ResourceTypes resource)
    {
        if (resource == ResourceTypes.Wood) { return Mathf.FloorToInt(1000.0f * (1.15f * LoggingLevel)); }
        else if (resource == ResourceTypes.Stone) { return Mathf.FloorToInt(1000.0f * (1.15f * MiningLevel)); }
        else { return Mathf.FloorToInt(1000.0f * (1.15f * SmithingLevel)); }
    }
}