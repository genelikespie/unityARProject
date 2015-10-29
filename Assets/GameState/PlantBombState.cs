using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlantBombState : State {

    // Local copy of game information
    List<Player> planters;
    List<Player> defusers;
    int numOfBombs;
    float timeToPlant;

    float timeLeft; // time left to plant
    float timeStart; // start time of plant state
    float timeEnd; // end time to plant the bomb

    protected virtual void Awake()
    {
        base.Awake();// Call the base class's function to initialize all variables
        // Find all UI elements in the scene

        if (!gameManager)
            Debug.LogError("AWAKE: CANT find game manager in base");
    }

    public override void Initialize()
    {
        planters = gameManager.planters;
        defusers = gameManager.defusers;
        numOfBombs = gameManager.numOfBombs;
        timeToPlant = gameManager.timeToPlant;

        timeLeft = timeToPlant;
        timeStart = Time.time;
        timeEnd = timeStart + timeToPlant;
    }

    /*
    void Update()
    {
        // If bomb has been planted
        if (!gameManager.bombPlanted)
        {
            timeLeft = timeEnd - Time.time;
            if (timeLeft < 0.0f)
            {
                timeLeft = 0;
                gameManager.SetState(gameManager.gameOverState);
            }
        }

    }
    */
}
