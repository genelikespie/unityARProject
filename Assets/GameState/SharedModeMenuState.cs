using UnityEngine;
using System.Collections;

public class SharedModeMenuState : State {

    //float time { get; private set; }
    //int numOfBombs;
    public static float timeAllowed { get; private set; }

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
        //do nothing
    }

    public void getTime()
    {

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
