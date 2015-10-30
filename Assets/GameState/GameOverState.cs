using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GameOverState : State
{
    Button PlayAgainButton;
    Button QuitButton;
    Text DisplayWinner;
    // Use this for initialization
    protected virtual void Awake()
    {
        base.Awake();
        PlayAgainButton = GameObject.Find("PlayAgainButton").GetComponent<Button>();
        QuitButton = GameObject.Find("QuitButton").GetComponent<Button>();
        DisplayWinner = GameObject.Find("DisplayWinner").GetComponent<Text>();

        if (!PlayAgainButton)
            Debug.LogError("PlayAgainButton");
        if (!QuitButton)
            Debug.LogError("QuitButton");
    }

    public override void Initialize()
    {
        if (!gameManager)
            Debug.LogError("Cant find game manager");
    }

    public override void ToMainMenu()
    {
        //Debug.Log("GameManager " + gameManager.name);
        //Debug.Log("MainMenuState " + gameManager.mainMenuState.name);

        gameManager.SetState(gameManager.mainMenuState);
    }

    public override void ToSharedModeMenu()
    {
        //Debug.Log("To shared Menu");
        gameManager.SetState(gameManager.sharedModeMenuState);
    }


    public void displayWinner()
    {
        if (gameManager.playerOneWins)
        {
            DisplayWinner.text = "Player one wins!";
        }
        else
        {
            DisplayWinner.text = "Player two wins!";
        }
    }
}
