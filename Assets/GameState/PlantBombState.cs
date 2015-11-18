using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class PlantBombState : State {

    // UI
    Text PB_MenuTitle;
    Text PB_TimeLeftText;
    Button PB_PassPhoneButton;
    InputField PB_HintField;
	Button PB_PlantBomb;
	Text PB_Waiting;

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

    }

    public override void Initialize()
    {
		PB_TimeLeftText.text = string.Format("{0:N1}", gameManager.plantTimer.timeLeft);
        PB_PassPhoneButton.gameObject.SetActive(false);
		PB_PlantBomb.gameObject.SetActive(true);
		PB_Waiting.gameObject.SetActive(false);

		gameManager.plantTimer.StartTimer();
		
		//Debug.Log("time to plant: " + timeToPlant + " time start: " + timeStart + " time end: " + timeEnd + " timetodefuse: " + gameManager.timeToDefuse);
    }

    // Update the timer to plant the bomb
    public override void RunState() 
	{
		// Update the timer UI
		PB_TimeLeftText.text = string.Format("{0:N1}", gameManager.plantTimer.timeLeft);

		if (!player.isAllLocalBombsPlanted()) {
                /////////////////////////////////////////////////
                // TODO implement time expired
                /////////////////////////////////////////////////
			if(gameManager.plantTimer.TimedOut())
            	Debug.LogWarning("Time ran out to plant the bomb!");
		}
		// If not all global bombs (all players) are planted, display the
		// "Waiting for others" text. In singleplayer global and local will
		// have the same value.
		else if(!player.isAllGlobalBombsPlanted()) {
			PB_Waiting.gameObject.SetActive(true);
		}
		else {
			PB_Waiting.gameObject.SetActive(false);
			PB_PassPhoneButton.gameObject.SetActive(true);
		}
	}

	public void OnTappedOnNewTargetButton()
	{
		gameManager.CreateBombTarget();
		player.setLocalBombsPlanted(player.getLocalBombsPlanted() + 1);
        
        if (player.isAllLocalBombsPlanted()) {    
			PB_PlantBomb.gameObject.SetActive(false);
		}
	}
	
	public override void PassPhone()
	{
		gameManager.plantTimer.StopTimer();
        gameManager.SetState(gameManager.passingState);
    }
}
