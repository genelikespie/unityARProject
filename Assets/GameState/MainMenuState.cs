using UnityEngine;
using System.Collections;

public class MainMenuState : State {

    abstract public void ToMainMenu()
    {

    }

    abstract public void ToSharedModeMenu()
    {
        gameManager.SetState(gameManager.sharedModeMenuState);
    }

    abstract public void ToTutorialMenu()
    {

    }

    abstract public void ToMultiplayerMenu()
    {

    }

    abstract public void PlantBomb();

    // Changes the game state between planting bomb and defusing bomb
    abstract public void PassPhone(Player from, Player to);

    // Time runs out
    abstract public void TimeExpired();

    // All bombs are defused
    abstract public void AllBombsDefused();
}
