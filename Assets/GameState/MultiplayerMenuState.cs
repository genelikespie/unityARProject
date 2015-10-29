using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MultiplayerMenuState : State {
    List<Player> team1;
    List<Player> team2;
    public int numOfBombs;
    public float time {get; private set;}
    
    public override void ToTutorialMenu()
    {
        //gameManager.SetState(gameManager.mainMenuState);
    }

    public override void ToMultiplayerMenu()
    {
        gameManager.SetState(gameManager.multiplayerMenuState);
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
    
    public void pickNumOfBombs(int num)
    {
        numOfBombs = num;
    }

    public void setNumOfSeconds()
    {
        time = (numOfBombs * 10);
    }
}
