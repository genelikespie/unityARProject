using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using Vuforia;

public class PlantBombState : State {

    // UI
    Text PB_MenuTitle;
    Text PB_TimeLeftText;
    Button PB_PassPhoneButton;
    InputField PB_HintField;
    InputField PB_HintField2;
    InputField PB_HintField3;
    Button PB_PlantBomb;
    Button PB_InsertHints;
    Button PB_HideHints;
    Text PB_Waiting;
    Text PB_ArmTimeLeftText;
    Button PB_ReplantBomb;

    // Get a reference to the UDTH to get trackables (for deletion)
    UserDefinedTargetEventHandler userDefinedTargetHandler;
    // Reference to the armBombTimer (from GameManager)
    Timer armBombTimer;


    // Arming bomb variables
    // Keep track of current bomb being planted to get the bomb using Find
    int curBombNum;
    // Variable to store our reference to the current bomb
    GameObject curBomb;
    bool isArmingBomb;
    // If we are arming the bomb, check if bomb is in view (handled in ChangeCurBombVisibility)
    bool curBombIsVisible;

    protected virtual void Awake()
    {
        // Call the base class's function to initialize all variables
        base.Awake();

        // Find all UI elements in the scene
        PB_MenuTitle = GameObject.Find("PB_MenuTitle").GetComponent<Text>();
        PB_TimeLeftText = GameObject.Find("PB_TimeLeftText").GetComponent<Text>();
        PB_PassPhoneButton = GameObject.Find("PB_PassPhoneButton").GetComponent<Button>();
        PB_HintField = GameObject.Find("PB_HintField").GetComponent<InputField>();
        PB_HintField2 = GameObject.Find("PB_HintField2").GetComponent<InputField>();
        PB_HintField3 = GameObject.Find("PB_HintField3").GetComponent<InputField>();
        PB_InsertHints = GameObject.Find("PB_InsertHints").GetComponent<Button>();
        PB_HideHints = GameObject.Find("PB_HideHints").GetComponent<Button>();
        PB_PlantBomb = GameObject.Find("PB_PlantBomb").GetComponent<Button>();
		PB_Waiting = GameObject.Find ("PB_Waiting").GetComponent<Text>();
        PB_ArmTimeLeftText = GameObject.Find("PB_ArmTimeLeftText").GetComponent<Text>();
        PB_ReplantBomb = GameObject.Find("PB_ReplantBomb").GetComponent<Button>();
        userDefinedTargetHandler = GameObject.Find("UserDefinedTargetBuilder").GetComponent<UserDefinedTargetEventHandler>();
    }

    public override void Initialize()
    {
		PB_TimeLeftText.text = string.Format("{0:N1}", gameManager.plantTimer.timeLeft);
        PB_PassPhoneButton.gameObject.SetActive(false);
		PB_PlantBomb.gameObject.SetActive(true);
		PB_Waiting.gameObject.SetActive(false);

        //Don't Display the hints until button press
        PB_HintField.gameObject.SetActive(false);
        PB_HintField2.gameObject.SetActive(false);
        PB_HintField3.gameObject.SetActive(false);
        PB_HideHints.gameObject.SetActive(false);

        gameManager.plantTimer.StartTimer();
        // Deactivate arming bomb logic
        gameManager.armBombTimer.ResetTimer();
        armBombTimer = gameManager.armBombTimer;
        PB_ArmTimeLeftText.gameObject.SetActive(false);
        PB_ReplantBomb.gameObject.SetActive(false);
        curBombNum = 0;
        // Set current bomb to null (delete it if it isn't)
        if (curBomb)
            Destroy(curBomb);
        isArmingBomb = false;

		//Debug.Log("time to plant: " + timeToPlant + " time start: " + timeStart + " time end: " + timeEnd + " timetodefuse: " + gameManager.timeToDefuse);
    }

    // Update the timer to plant the bomb
    public override void RunState() 
	{
		// Update the timer UI
		PB_TimeLeftText.text = string.Format("Time Left: {0:N1}", gameManager.plantTimer.timeLeft);

        // Player is arming the bomb
        if (isArmingBomb && PB_ArmTimeLeftText.gameObject.activeSelf)
        {
            // If arm bomb timer is done, create the bomb
            if (gameManager.armBombTimer.timeLeft <= 0)
            {
                PB_ArmTimeLeftText.text = "0";
                OnTappedOnNewTargetButton();
            }
            else if (curBombIsVisible)
            {
                PB_ArmTimeLeftText.text = string.Format("Registering Bomb: {0:N1}", armBombTimer.timeLeft);
            }
        }

		if (!player.isAllLocalBombsPlanted() && gameManager.plantTimer.TimedOut()) {
                /////////////////////////////////////////////////
                // TODO implement time expired
                /////////////////////////////////////////////////
            	Debug.LogWarning("Time ran out to plant the bomb!");
                gameManager.SetState(gameManager.gameOverState);

		}
		// If not all global bombs (all players) are planted, display the
		// "Waiting for others" text. In singleplayer global and local will
		// have the same value.
		else if(player.isAllLocalBombsPlanted() && !player.isAllGlobalBombsPlanted()) {
			PB_Waiting.gameObject.SetActive(true);
		}
		else if(player.isAllLocalBombsPlanted() && player.isAllGlobalBombsPlanted()){
			PB_Waiting.gameObject.SetActive(false);
			PB_PassPhoneButton.gameObject.SetActive(true);
		}
	}

    // Successfully created the bomb
	public void OnTappedOnNewTargetButton()
	{
        // Reset arming logic
        isArmingBomb = false;
        gameManager.armBombTimer.ResetTimer();
        PB_ArmTimeLeftText.gameObject.SetActive(false);
        PB_PlantBomb.gameObject.SetActive(true);
        PB_ReplantBomb.gameObject.SetActive(false);

        player.setLocalBombsPlanted(player.getLocalBombsPlanted() + 1);
        // increment current bomb at the end
        curBombNum++;
        if (player.isAllLocalBombsPlanted()) {    
			PB_PlantBomb.gameObject.SetActive(false);
		}

	}


    // Attempt to create the bomb
    public void ArmBomb()
    {
        isArmingBomb = true;
        gameManager.CreateBombTarget();

        // Set the armBombTimer to start
        gameManager.armBombTimer.ResetTimer();
        gameManager.armBombTimer.StartTimer();

        // Activate the armBombText (to show the time)
        PB_ArmTimeLeftText.gameObject.SetActive(true);

        // Keep the plant timer in game manager running
        // Deactivate plantBombButton
        PB_PlantBomb.gameObject.SetActive(false);

        // Activate ReplantBombButton
        PB_ReplantBomb.gameObject.SetActive(true);

        // Find created bomb in the game
        // Keep track of the bomb (make sure we can see it)

    }

    // Let the player replant the bomb
    public void ReplantBomb()
    {
        isArmingBomb = false;
        string bombName = "UserTarget-" + curBombNum;
        Debug.LogWarning("Deleting trackable for replant: " + bombName);

        // Delete the trackable object
        userDefinedTargetHandler.DeleteTrackable(bombName);
        curBomb = null; // make object null

        gameManager.armBombTimer.ResetTimer();
        PB_ArmTimeLeftText.gameObject.SetActive(false);
        PB_PlantBomb.gameObject.SetActive(true);
        PB_ReplantBomb.gameObject.SetActive(false);
    }

    // Let the player insert hints into the game
    public void InsertHints()
    {
        PB_HintField.gameObject.SetActive(true);
        PB_HintField2.gameObject.SetActive(true);
        PB_HintField3.gameObject.SetActive(true);
        PB_InsertHints.gameObject.SetActive(false);
        PB_HideHints.gameObject.SetActive(true);
    }

    // Let the player insert hints into the game
    public void HideHints()
    {
        PB_HintField.gameObject.SetActive(false);
        PB_HintField2.gameObject.SetActive(false);
        PB_HintField3.gameObject.SetActive(false);
        PB_InsertHints.gameObject.SetActive(true);
        PB_HideHints.gameObject.SetActive(false);
    }

    public void ChangeCurBombVisibility(string bombName, bool IsVisible)
    {
        string curBombName = ("UserTarget-" + curBombNum);
        Debug.LogWarning (" curbombName: " + curBombName + " bombName: " + bombName);
        if (bombName == curBombName)
        {
            curBombIsVisible = IsVisible;
            if (isArmingBomb && PB_ArmTimeLeftText.gameObject.activeSelf)
            {
                if (IsVisible)
                {
                    gameManager.armBombTimer.ResetTimer();
                    gameManager.armBombTimer.StartTimer();
                }
                else
                {
                    gameManager.armBombTimer.ResetTimer();
                    PB_ArmTimeLeftText.text = string.Format("Hold bomb still to register!");
                }
            }
        }
    }

	public override void PassPhone()
	{
		gameManager.hint = PB_HintField.text;
        gameManager.hint2 = PB_HintField2.text;
        gameManager.hint3 = PB_HintField3.text;
        gameManager.plantTimer.StopTimer();
        gameManager.SetState(gameManager.passingState);
    }
}
