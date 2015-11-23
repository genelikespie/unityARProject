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
	Button PB_PlantBomb;
	Text PB_Waiting;
    Text PB_ArmTimeLeftText;
    Button PB_ReplantBomb;
    Button PB_TutorialPlant;
    Button PB_TutorialReplant;

    // Is the tutorial box checked?
    bool tutorialToggleOn;

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
        PB_PlantBomb = GameObject.Find("PB_PlantBomb").GetComponent<Button>();
		PB_Waiting = GameObject.Find ("PB_Waiting").GetComponent<Text>();
        PB_ArmTimeLeftText = GameObject.Find("PB_ArmTimeLeftText").GetComponent<Text>();
        PB_ReplantBomb = GameObject.Find("PB_ReplantBomb").GetComponent<Button>();
        PB_TutorialPlant = GameObject.Find("PB_TutorialPlant").GetComponent<Button>();
        PB_TutorialReplant = GameObject.Find("PB_TutorialReplant").GetComponent<Button>();

        userDefinedTargetHandler = GameObject.Find("UserDefinedTargetBuilder").GetComponent<UserDefinedTargetEventHandler>();
    }

    // Need to check if tutorial is TRUE even after everything is initialized b/c can be set during runtime
    public void Update()
    {
        //Display tutorial if tutorial toggle is checked
        tutorialToggleOn = gameManager.tutorialToggleOn;
        Debug.Log("tutorialToggleOn in PlantBombState: " + tutorialToggleOn);
    }

    public override void Initialize()
    {
		PB_TimeLeftText.text = string.Format("{0:N1}", gameManager.plantTimer.timeLeft);
        PB_PassPhoneButton.gameObject.SetActive(false);
		PB_PlantBomb.gameObject.SetActive(true);
		PB_Waiting.gameObject.SetActive(false);

        // init tutorialToggleOn before update()
        tutorialToggleOn = gameManager.tutorialToggleOn;
        if (tutorialToggleOn)
        {
            PB_TutorialPlant.gameObject.SetActive(true);
            PB_TutorialReplant.gameObject.SetActive(false);
            //Debug.Log("PB_TutorialPlant is TRUE");
        }
        else
        {
            PB_TutorialPlant.gameObject.SetActive(false);
            PB_TutorialReplant.gameObject.SetActive(false);
            //Debug.Log("PB_TutorialPlant is FALSE");
        }

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
            PB_TutorialPlant.gameObject.SetActive(false);

        }

        // turn off re-plant tutorial if bomb successfully planted
        if (tutorialToggleOn)
        {
            PB_TutorialReplant.gameObject.SetActive(false);
        }

    }


    // Attempt to create the bomb on user-selected location
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

        // Turn off tutorial Bubble for planting bomb & turn on tutorial for re-planting bomb
        if (tutorialToggleOn)
        {
            PB_TutorialPlant.gameObject.SetActive(false);
            PB_TutorialReplant.gameObject.SetActive(true);
        }

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
        if(tutorialToggleOn)
        {
            PB_TutorialPlant.gameObject.SetActive(true);
            PB_TutorialReplant.gameObject.SetActive(false);
        }
        else //Tutorial is not on, turn off all tutorials
        {
            PB_TutorialPlant.gameObject.SetActive(false);
            PB_TutorialReplant.gameObject.SetActive(false);
        }
        PB_ReplantBomb.gameObject.SetActive(false);
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
		gameManager.plantTimer.StopTimer();
        gameManager.SetState(gameManager.passingState);
    }
}
