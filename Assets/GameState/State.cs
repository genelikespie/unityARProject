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

<<<<<<< HEAD
    virtual public void ToMultiplayerMenu()
    {

    }

    virtual public void PlantBomb()
=======
    public virtual void PlantBomb()
>>>>>>> 9ebac8121f25b3cf90b02bab4a7cb509f9d80c5f
    {

    }

    // Changes the game state between planting bomb and defusing bomb
<<<<<<< HEAD
    virtual public void PassPhone(Player from, Player to)
=======
    public virtual void PassPhone(Player from, Player to)
>>>>>>> 9ebac8121f25b3cf90b02bab4a7cb509f9d80c5f
    {

    }

    // Time runs out
<<<<<<< HEAD
    virtual public void TimeExpired()
=======
    public virtual void TimeExpired()
>>>>>>> 9ebac8121f25b3cf90b02bab4a7cb509f9d80c5f
    {

    }

    // All bombs are defused
<<<<<<< HEAD
    virtual public void AllBombsDefused ()
=======
    public virtual void AllBombsDefused()
>>>>>>> 9ebac8121f25b3cf90b02bab4a7cb509f9d80c5f
    {

    }




}
