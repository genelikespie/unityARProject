using UnityEngine;
using System.Collections;

public class PlantBombState : State {
    public override void ToTutorialMenu()
    {
        //gameManager.SetState(gameManager.mainMenuState);
    }

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

    public override void ToMultiplayerMenu()
    {
        //gameManager.SetState(gameManager.MultiplayerMenuState);
    }

    public override void PlantBomb()
    {
        //gameManager.SetState(gameManager.PlantBombState);
    }

    // Changes the game state between planting bomb and defusing bomb
    public override void PassPhone(Player from, Player to)
    {
        //do nothing
    }

    // Time runs out
    public override void TimeExpired()
    {
        //do nothing
    }

    // All bombs are defused
    public override void AllBombsDefused()
    {
        //do nothing
    }

}
