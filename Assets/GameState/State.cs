using UnityEngine;
using System.Collections;

public class State {
    protected GameManager gameManager;
	
    public State() {
        gameManager = GameManager.Instance();
    }

    virtual public void ToMainMenu()
    {
        gameManager.SetState(gameManager.mainMenuState);
    }

    virtual public void ToSharedModeMenu()
    {
        gameManager.SetState(gameManager.sharedModeMenuState);
    }

    /*
     * Not implemented yet
     */
    virtual public void ToTutorialMenu()
    {
       //gameManager.SetState(gameManager.mainMenuState);
    }

    virtual public void ToMultiplayerMenu()
    {

    }

    virtual public void PlantBomb()
    {

    }

    // Changes the game state between planting bomb and defusing bomb
    virtual public void PassPhone(Player from, Player to)
    {

    }

    // Time runs out
    virtual public void TimeExpired()
    {

    }

    // All bombs are defused
    virtual public void AllBombsDefused ()
    {

    }




}
