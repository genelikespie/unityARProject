using UnityEngine;
using System.Collections;

public class State : MonoBehaviour{
    protected GameManager gameManager;
	
    void Awake() {
        gameManager = GameManager.Instance();
    }

    virtual public void ToMainMenu()
    {
        gameManager.SetState(gameManager.mainMenuState);
    }

    virtual public void ToSharedModeMenu()
    {
        gameManager.sharedModeMenuState.Initialize();
        gameManager.SetState(gameManager.sharedModeMenuState);
    }

    /*
     * Not implemented yet
     */
    virtual public void ToTutorialMenu()
    {
        gameManager.SetState(gameManager.tutorialMenuState);
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
    virtual public void AllBombsDefused()
    {

    }




}
