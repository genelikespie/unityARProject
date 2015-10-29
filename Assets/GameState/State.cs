using UnityEngine;
using System.Collections;

public class State : MonoBehaviour{
    protected GameManager gameManager;
	
    protected virtual void Awake() {
        Debug.LogError("AWAKE: STATE");
        gameManager = GameManager.Instance();
        if (!gameManager)
            Debug.LogError("No GameManager found!");
        else
            Debug.Log("Found GameManager");
    }

    /* This function is called for NextState when the state changes.
    */
    public virtual void Initialize()
    {
        // Implement this function in the derived classes
    }

    public virtual void ToMainMenu()
    {
        if (!gameManager)
            Debug.LogError("Cannot find static game manager!");
        Debug.Log("GameManager " + gameManager.name);
        Debug.Log("MainMenuState " + gameManager.mainMenuState.name);

        gameManager.SetState(gameManager.mainMenuState);
    }

    public virtual void ToSharedModeMenu()
    {
        Debug.Log("To shared Menu");
        gameManager.SetState(gameManager.sharedModeMenuState);
    }

    /*
     * Not implemented yet
     */
    public virtual void ToTutorialMenu()
    {
        gameManager.SetState(gameManager.tutorialMenuState);
    }

    public virtual void ToMultiplayerMenu()
    {

    }

    public virtual void PlantBomb()
    {

    }

    // Changes the game state between planting bomb and defusing bomb
    public virtual void PassPhone(Player from, Player to)
    {

    }

    // Time runs out
    public virtual void TimeExpired()
    {

    }

    // All bombs are defused
    public virtual void AllBombsDefused()
    {

    }




}
