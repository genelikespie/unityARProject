using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Vuforia;
using UnityEngine.Networking;
using UnityEngine.Assertions;

public class GameOverState : State
{
    GameObject explosion;
    Button QuitButton;
    Text DisplayWinner;
    GameObject goBack;

    //Keep sound from playing on loop
    bool playedOnce = false;

    // Use this for initialization
    protected virtual void Awake()
    {
        Assert.raiseExceptions = true;
        explosion = GameObject.Find("explosion");
        explosion.SetActive(false);
        goBack = GameObject.Find("GO_Backdrop");
        if (goBack != null)
        {
            goBack.GetComponent<MeshRenderer>().enabled = false;
        }
        base.Awake();
		
        QuitButton = GameObject.Find("QuitButton").GetComponent<Button>();
        DisplayWinner = GameObject.Find("DisplayWinner").GetComponent<Text>();

        //check if quit button is null
        Assert.IsNotNull(QuitButton);
        //if (!QuitButton)
        //    Debug.LogError("QuitButton");
    }

    public override void Initialize()
    {
        //if (!gameManager)
            //Debug.LogError("Cant find game manager");
        //Checks if game manager is null
        Assert.IsNotNull(gameManager, "Cant find game manager");

        playedOnce = false;

        if (goBack != null)
        {
            goBack.GetComponent<MeshRenderer>().enabled = true;
        }
    }

    public override void ToMainMenu()
    {
        //Debug.Log("GameManager " + gameManager.name);
        //Debug.Log("MainMenuState " + gameManager.mainMenuState.name);
        gameManager.SetState(gameManager.mainMenuState);
        gameManager.ResetGame();

        if (goBack != null)
        {
            goBack.GetComponent<MeshRenderer>().enabled = false;
        }
    }

    public override void ToSharedModeMenu()
    {
        //Debug.Log("To shared Menu");
        gameManager.SetState(gameManager.sharedModeMenuState);

        if (goBack != null)
        {
            goBack.GetComponent<MeshRenderer>().enabled = false;
        }
    }

    public override void RunState()
    {
        displayWinner();
    }

    public void displayWinner()
    {
        ObjectTracker imgTracker = TrackerManager.Instance.GetTracker<ObjectTracker>();
        imgTracker.Stop();
        if (!player.isAllGlobalBombsPlanted())
        {
            
            if (player.isMultiplayer())
            {
                DisplayWinner.text = "Team 2 wins!";
                if (!playedOnce)
                {
                    gameManager.playCheer();
                    playedOnce = true;
                }
            }
            else
            {
                DisplayWinner.text = "You ran out of time! " + player.getDefuserName() + " wins!";
                if (!playedOnce)
                {
                    gameManager.playCheer();
                    playedOnce = true;
                }
            }
        }
        else if (!player.isAllGlobalBombsDefused())
        {
            explosion.SetActive(true);
            if (!playedOnce)
            {
                gameManager.playExplode();
                playedOnce = true;
            }

            if (player.isMultiplayer())
            {
                DisplayWinner.text = "Team 1 wins!";
            }
            else
            {
                DisplayWinner.text = player.getPlanterName() + " wins!";
            }
        }
        else
        {
            if (!playedOnce)
            {
                gameManager.playCheer();
                playedOnce = true;
            }
            if (player.isMultiplayer())
            {
                DisplayWinner.text = "Team 2 wins!";
            }
            else
            {
                DisplayWinner.text = player.getDefuserName() + " wins!";
            }
        }
    }
}
