using UnityEngine;
using System.Collections;

public class State : MonoBehaviour{

    protected GameManager gameManager;

	protected PlayerAdapter player {
		get { return gameManager.player; }
		set { gameManager.player = value; }
	}
	
    /* NOTE: All derived classes must call this base Awake() function in their respective Awake() functions
     */
    protected virtual void Awake() {
        gameManager = GameManager.Instance();
        if (!gameManager)
            Debug.LogError("No GameManager found!");
    }

    /* This function is called for NextState when the state changes.
    */
    public virtual void Initialize()
    {
        // Implement this function in the derived classes
		Debug.Log ("Warning: Initialize() not overridden in subclass of State!");
    }

	// Use in place of Update()
	public virtual void RunState() {
		Debug.Log ("Warning: RunState() not overridden in subclass of State!");
	}

    public virtual void ToMainMenu()
    {
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
		gameManager.SetState(gameManager.multiplayerMenuState);
    }

    public virtual void PlantBomb()
    {

    }

    // Changes the game state between planting bomb and defusing bomb
    public virtual void PassPhone()
    {

    }

    public virtual void DefuseBomb()
    {

    }

    // Time runs out
    public virtual void TimeExpired()
    {
        /////////////////////////////////////////////////
        // TODO implement time expired
        /////////////////////////////////////////////////
		player.setPlayerOneWins(true);
        gameManager.SetState(gameManager.gameOverState);
    }

    // All bombs are defused
    public virtual void AllBombsDefused()
    {

    }




}
