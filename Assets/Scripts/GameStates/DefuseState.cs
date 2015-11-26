using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Assertions;

public class DefuseState : State
{

    // UI
    Text D_TimeLeftText;
    Text D_HintLeftBehind;
    Text D_HintLeftBehind2;
    Text D_HintLeftBehind3;
    Button D_DefuseBombButton;
    Text D_Waiting;
    Button D_Tutorial;
	Button D_GiveUp;
	Text D_Penalty;
	Text D_AllDefusedText;

    // Is the tutorial box checked?
    bool tutorialToggleOn;

    //cover Hint Checks 
    bool NextHint1;
    bool NextHint2;
    bool DoOnce1;
    bool DoOnce2;
    public int displayHintCount;

	float timePenalty = 5;
	float fadeRate = .02f;

	IEnumerator fadePenaltyCoroutine;

    //Bomb Texture
    //GameObject[] bombs;
    //public Material DefuseMaterial;

    IEnumerator DelayForDisarmedRoutine()
    {
        yield return new WaitForSeconds(2f);
		gameManager.defuseTimer.StopTimer();
        player.setPlayerOneWins(false);
        gameManager.SetState(gameManager.gameOverState);
    }


    public virtual void Awake()
    {
        base.Awake();

        Assert.raiseExceptions = true;

        // Find all UI elements in the scene
        D_TimeLeftText = GameObject.Find("D_TimeLeftText").GetComponent<Text>();
        D_DefuseBombButton = GameObject.Find("D_DefuseBombButton").GetComponent<Button>();
        D_HintLeftBehind = GameObject.Find("D_HintLeftBehind").GetComponent<Text>();
        D_HintLeftBehind2 = GameObject.Find("D_HintLeftBehind2").GetComponent<Text>();
        D_HintLeftBehind3 = GameObject.Find("D_HintLeftBehind3").GetComponent<Text>();
        D_Waiting = GameObject.Find("D_Waiting").GetComponent<Text>();
        D_Tutorial = GameObject.Find("D_Tutorial").GetComponent<Button>();
		D_GiveUp = GameObject.Find ("D_GiveUp").GetComponent<Button>();
		D_Penalty = GameObject.Find ("D_Penalty").GetComponent<Text>();
		D_AllDefusedText = GameObject.Find ("D_AllDefusedText").GetComponent<Text>();

        //find bomb tag
        //bombs = GameObject.FindGameObjectsWithTag("Bomb");

        Assert.IsNotNull(D_TimeLeftText, "D_TimeLeftText not found");
        Assert.IsNotNull(D_DefuseBombButton, "D_DefuseBombButton not found");
        Assert.IsNotNull(D_HintLeftBehind, "D_HintLeftBehind not found");
        Assert.IsNotNull(D_HintLeftBehind2, "D_HintLeftBehind2 not found");
        Assert.IsNotNull(D_HintLeftBehind3, "D_HintLeftBehind3 not found");
        Assert.IsNotNull(D_Waiting, "D_Waiting not found");

		//Set hint penalty fade coroutine
		fadePenaltyCoroutine = FadeAlphaOut();
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
        gameManager.defuseTimer.StartTimer();
        D_Waiting.gameObject.SetActive(false);
        DoOnce1 = false;
        DoOnce2 = false;
        NextHint1 = false;
        NextHint2 = false;

        //check if gameManager is not null
        Assert.IsNotNull(gameManager, "Cant find game manager");

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

		// Done if player two gives up this state.
		player.setPlayerOneWins(true);

		// To avoid synchronization issues, you can only give up in single player.
		D_GiveUp.gameObject.SetActive(!player.isMultiplayer());

		// Set the penalty text to hidden initially.
		D_Penalty.color = new Color(D_Penalty.color.r, 
		                            D_Penalty.color.g, 
		                            D_Penalty.color.b, 
		                            0);

		D_AllDefusedText.gameObject.SetActive(false);
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

		if(gameManager.defuseTimer.TimedOut()) {
			player.setPlayerOneWins(true);
			TimeExpired();
		}
        if (!player.isAllLocalBombsDefused())
        {
            //check if the player fails to defuse all bombs
            Assert.AreNotEqual<int>(player.getLocalBombsDefused(), player.getLocalBombsPlanted());
        }
        else if (!player.isAllGlobalBombsDefused())
        {
            //check if the player fails to defuse all bombs
            Assert.AreNotEqual<int>(player.getLocalBombsDefused(), player.getLocalBombsPlanted());
            D_Waiting.gameObject.SetActive(true);
        }
        else
        {
			D_Waiting.gameObject.SetActive(false);
			D_AllDefusedText.gameObject.SetActive(true);

            //check if the player successfully defuses all bombs
            Assert.AreEqual<int>(player.getLocalBombsDefused(), player.getLocalBombsPlanted());
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

		// Add hint penalty
		gameManager.defuseTimer.timeLeft -= timePenalty;
		FlashPenalty();
		
    }

	public override void TimeExpired ()
	{
		gameManager.defuseTimer.StopTimer();
		base.TimeExpired ();
	}


    public override void AllBombsDefused()
    {
        if (gameManager.AttemptDefuse())
            player.setLocalBombsDefused(player.getLocalBombsDefused() + 1);
    }

	void FlashPenalty() {
		StopCoroutine(fadePenaltyCoroutine);
		D_Penalty.color = new Color(D_Penalty.color.r, 
		                            D_Penalty.color.g, 
		                            D_Penalty.color.b, 
		                            1);
		StartCoroutine(fadePenaltyCoroutine);
	}

	IEnumerator FadeAlphaOut() {
		while(D_Penalty.color.a != 0) {
			D_Penalty.color = new Color(D_Penalty.color.r, 
			                            D_Penalty.color.g, 
			                            D_Penalty.color.b, 
			                            D_Penalty.color.a - fadeRate);
			yield return null;
		}
	}
}


