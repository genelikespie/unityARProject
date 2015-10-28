using UnityEngine;
using System.Collections;

public abstract class State {
    protected GameManager gameManager;
	
    public State() {
        gameManager = GameManager.Instance();
    }

    abstract public void ToMainMenu();

    abstract public void ToSharedModeMenu();

    /*
     * Not implemented yet
     */
    abstract public void ToTutorialMenu();

    abstract public void ToMultiplayerMenu();

    abstract public void PlantBomb();

    // Changes the game state between planting bomb and defusing bomb
    abstract public void PassPhone(Player from, Player to);

    // Time runs out
    abstract public void TimeExpired();

    // All bombs are defused
    abstract public void AllBombsDefused ();




}
