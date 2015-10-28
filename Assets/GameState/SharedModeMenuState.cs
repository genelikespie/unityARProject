using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SharedModeMenuState : State {

    // Insert UI elements here
    //
    //

    // To use the List class, use the System.Collections.Generic library
    public List<Player> planters;
    public List<Player> defusers;
    public int numberOfBombs;
    public float timeToPlant;

    public void Initialize()
    {
        //Reset the UI here
    }

    public void PlantBomb()
    {
        gameManager.plantBombState.Initialize(planters, defusers, numberOfBombs, timeToPlant);
        gameManager.SetState(gameManager.plantBombState);
    }

}
