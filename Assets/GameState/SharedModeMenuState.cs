using UnityEngine;
using System.Collections;

public class SharedModeMenuState : State {

    //float time { get; private set; }
    //int numOfBombs;
    public static float timeAllowed { get; private set; }

    // To use the List class, use the System.Collections.Generic library
    public List<Player> planters = new List<Player>();
    public List<Player> defusers = new List<Player>();
    public int numOfBombs = 1; // TODO possibly make this dynamic
    public float timeToPlant = 30.0f; // TODO make this dynamic
    public float timeToDefuse = 60.0f; // TODO make this dynamic

    protected virtual void Awake()
    {
        base.Awake();// Call the base class's function to initialize all variables
        // Find all UI elements in the scene

	}
	
    public override void ToTutorialMenu()
    {
        //gameManager.SetState(gameManager.mainMenuState);
    }

    virtual public void ToMultiplayerMenu()
    {
        //do nothing
    }

    public override void PlantBomb()
    {
        gameManager.SetState(gameManager.plantBombState);
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
        planters.Add(new Player(SMM_PlanterNameInputField.text));
        defusers.Add(new Player(SMM_DefuserNameInputField.text));

        // Set GameManager's game info
        gameManager.planters = planters;
        gameManager.defusers = defusers;
        gameManager.numOfBombs = numOfBombs;
        gameManager.timeToPlant = timeToPlant;
        gameManager.timeToDefuse = timeToDefuse;

        gameManager.SetAR();
        gameManager.SetState(gameManager.plantBombState);
    }

    /*
    public static void main(string[]args)
    {
        //TO DO: Need a button to determine number of bombs 
        GameManager.numOfBombs = 0; //temporary until button is implemented
        timeAllowed = GameManager.numOfBombs * 10;

        //TO DO: Add a button to start playing
        if (GameManager.numOfBombs == 0)
        {
            //you can't play...
        }
        else
        {
            this.PlantBomb();
        }
    }
     * */

    public void execute()
    {
        //TO DO: Need a button to determine number of bombs 
        GameManager.numOfBombs = 0; //temporary until button is implemented
        timeAllowed = GameManager.numOfBombs * 10;

        //TO DO: Add a button to start playing
        if (GameManager.numOfBombs == 0)
        {
            //you can't play...
        }
        else
        {
            this.PlantBomb();
        }
    }
}
