using UnityEngine;
using System.Collections;

public class PlantBombState : State {
    
    public override void ToTutorialMenu()
    {
        //gameManager.SetState(gameManager.mainMenuState);
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

    public void execute()
    {
        SharedModeMenuState.timeAllowed
    }
}
