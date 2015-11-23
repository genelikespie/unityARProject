using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class DefuseState : State
{

    // UI
    Text D_TimeLeftText;
    Text D_HintLeftBehind;
    Text D_HintLeftBehind2;
    Text D_HintLeftBehind3;
    Button D_DefuseBombButton;
    Text D_Waiting;

    //cover Hint Checks 
    bool NextHint1;
    bool NextHint2;
    bool DoOnce1;
    bool DoOnce2;
    public int displayHintCount;

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
        D_HintLeftBehind2 = GameObject.Find("D_HintLeftBehind2").GetComponent<Text>();
        D_HintLeftBehind3 = GameObject.Find("D_HintLeftBehind3").GetComponent<Text>();
        D_Waiting = GameObject.Find("D_Waiting").GetComponent<Text>();

        //find bomb tag
        //bombs = GameObject.FindGameObjectsWithTag("Bomb");

        if (!D_TimeLeftText)
            Debug.LogError("D_TimeLeftText");
        if (!D_DefuseBombButton)
            Debug.LogError("D_DefuseBombButton");
        if (!D_HintLeftBehind)
            Debug.LogError("D_HintLeftBehind");
        if (!D_HintLeftBehind2)
            Debug.LogError("D_HintLeftBehind2");
        if (!D_HintLeftBehind3)
            Debug.LogError("D_HintLeftBehind3");
        if (!D_Waiting)
            Debug.LogError("D_Waiting");
    }

    public override void Initialize()
    {
        base.Initialize();
        // Set the defuse button to be false
        // Activate it when the bomb is in view
        D_DefuseBombButton.gameObject.SetActive(false);
        displayHintCount = 0;
        D_HintLeftBehind.gameObject.SetActive(false);
        D_HintLeftBehind2.gameObject.SetActive(false);
        D_HintLeftBehind3.gameObject.SetActive(false);
        NextHint1 = false;
        NextHint2 = false;
        DoOnce1 = false;
        DoOnce2 = false;
        gameManager.defuseTimer.StartTimer();
        D_Waiting.gameObject.SetActive(false);

    }

    public override void RunState()
    {

        // Update the timer UI
        D_TimeLeftText.text = string.Format("{0:N1}", gameManager.defuseTimer.timeLeft);

        // If bomb is in view, activate the button
        /////////////////////////////////////////////////
        // TODO implement checking if the bomb is in view
        /////////////////////////////////////////////////

        if (gameManager.bombVisible)
        {
            D_DefuseBombButton.gameObject.SetActive(true);
        }
        else
        {
            D_DefuseBombButton.gameObject.SetActive(false);
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

    public void HintButton() {

        if (gameManager.hint2 == "" && gameManager.hint3 != "" && NextHint2 == false && (displayHintCount >= 1 || gameManager.hint == ""))
        {
            NextHint2 = true;
            displayHintCount++;
        }

        if (gameManager.hint == "" && (gameManager.hint2 != "" || gameManager.hint3 != "") && NextHint1 == false)
        {
            NextHint1 = true;
            displayHintCount++; 
        }

        //update the hint if something was left        
        if (gameManager.hint3 != "" && displayHintCount >= 2)
        {
            print("DEBUG");
            D_HintLeftBehind3.text = "Hint: " + gameManager.hint3;
            D_HintLeftBehind3.gameObject.SetActive(true);
            displayHintCount++;
        }
        if (gameManager.hint2 != "" && displayHintCount >= 1 && DoOnce2 == false)
        {
            DoOnce2 = true;
            D_HintLeftBehind2.text = "Hint: " + gameManager.hint2;
            D_HintLeftBehind2.gameObject.SetActive(true);
            displayHintCount++;

        }
        if (gameManager.hint != "" && DoOnce1 == false)
        {
            DoOnce1 = true;
            D_HintLeftBehind.text = "Hint: " + gameManager.hint;
            D_HintLeftBehind.gameObject.SetActive(true);
            displayHintCount++;
        }


    }


    public override void AllBombsDefused()
    {
        if (gameManager.AttemptDefuse())
            player.setLocalBombsDefused(player.getLocalBombsDefused() + 1);
    }
}
