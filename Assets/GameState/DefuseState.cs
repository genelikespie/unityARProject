using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class DefuseState : State
{

    // UI
    Text D_TimeLeftText;
    Text D_HintLeftBehind;
    Button D_DefuseBombButton;
    Text D_Waiting;
    Button D_Tutorial;

    // Is the tutorial box checked?
    bool tutorialToggleOn;

    //Bomb Texture
    //GameObject[] bombs;
    //public Material DefuseMaterial;

    IEnumerator DelayForDisarmedRoutine()
    {
        yield return new WaitForSeconds(2f);
        player.setPlayerOneWins(false);
        gameManager.SetState(gameManager.gameOverState);
    }


    public virtual void Awake()
    {
        base.Awake();

        // Find all UI elements in the scene
        D_TimeLeftText = GameObject.Find("D_TimeLeftText").GetComponent<Text>();
        D_DefuseBombButton = GameObject.Find("D_DefuseBombButton").GetComponent<Button>();
        D_HintLeftBehind = GameObject.Find("D_HintLeftBehind").GetComponent<Text>();
        D_Waiting = GameObject.Find("D_Waiting").GetComponent<Text>();
        D_Tutorial = GameObject.Find("D_Tutorial").GetComponent<Button>();

        //find bomb tag
        //bombs = GameObject.FindGameObjectsWithTag("Bomb");

        if (!D_TimeLeftText)
            Debug.LogError("D_TimeLeftText");
        if (!D_DefuseBombButton)
            Debug.LogError("D_DefuseBombButton");
        if (!D_HintLeftBehind)
            Debug.LogError("D_HintLeftBehind");
        if (!D_Waiting)
            Debug.LogError("D_Waiting");
    }

    public override void Initialize()
    {
        base.Initialize();
        // Set the defuse button to be false
        // Activate it when the bomb is in view
        D_DefuseBombButton.gameObject.SetActive(false);
        gameManager.defuseTimer.StartTimer();
        D_Waiting.gameObject.SetActive(false);

        // init tutorialToggleOn before update()
        tutorialToggleOn = gameManager.tutorialToggleOn;
        if (tutorialToggleOn)
        {
            D_Tutorial.gameObject.SetActive(true);
        }
        else
        {
            D_Tutorial.gameObject.SetActive(false);
        }

    }

    // Need to check if tutorial is TRUE even after everything is initialized b/c can be set during runtime
    public void Update()
    {
        //Display tutorial if tutorial toggle is checked
        tutorialToggleOn = gameManager.tutorialToggleOn;
        Debug.Log("tutorialToggleOn in PlantBombState: " + tutorialToggleOn);
    }

    public override void RunState()
    {

        //update the hint if something was left
        if (gameManager.hint != "")
            D_HintLeftBehind.text = "Hint: " + gameManager.hint;


        // Update the timer UI
        D_TimeLeftText.text = string.Format("{0:N1}", gameManager.defuseTimer.timeLeft);

        // If bomb is in view, activate the button
        /////////////////////////////////////////////////
        // TODO implement checking if the bomb is in view
        /////////////////////////////////////////////////

        if (gameManager.bombVisible)
        {
            D_DefuseBombButton.gameObject.SetActive(true);
            if (tutorialToggleOn)
            {
                D_Tutorial.gameObject.SetActive(false);
            }
        }
        else
        {
            D_DefuseBombButton.gameObject.SetActive(false);
            if (tutorialToggleOn)
            {
                D_Tutorial.gameObject.SetActive(true);
            }
        }

        // If time runs out and we have not defused the bomb, defuser loses
        /////////////////////////////////////////////////
        // TODO implement time expired
        /////////////////////////////////////////////////

        if (!player.isAllLocalBombsDefused())
        {
            if (gameManager.defuseTimer.TimedOut())
            {
                base.TimeExpired();
            }
        }
        else if (!player.isAllGlobalBombsDefused())
        {
            D_Waiting.gameObject.SetActive(true);
            if (player.getPlayerOneWins())
            {
                gameManager.defuseTimer.StopTimer();
                gameManager.SetState(gameManager.gameOverState);
            }
        }
        else
        {
            gameManager.defuseTimer.StopTimer();
            /////////////////////////////////////////////////
            // TODO implement game over functionality
            /////////////////////////////////////////////////

            //TODO: Instead of doing the following, reset/create a new player.

            //player.setPlayerOneWins(false);
            //player.setAllLocalBombsPlanted(false);

            StartCoroutine(DelayForDisarmedRoutine());
        }
    }



    public override void AllBombsDefused()
    {
        if (gameManager.AttemptDefuse())
            player.setLocalBombsDefused(player.getLocalBombsDefused() + 1);
    }
}
