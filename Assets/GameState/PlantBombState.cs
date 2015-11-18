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

        if (!PB_MenuTitle)
            Debug.LogError("PB_MenuTitle");
        if (!PB_TimeLeftText)
            Debug.LogError("PB_TimeLeftText");
        if (!PB_PassPhoneButton)
            Debug.LogError("PB_PassPhoneButton");
        if (!PB_HintField)
            Debug.LogError("PB_HintField");
        if (!PB_PlantBomb)
            Debug.LogError("PB_PlantBomb");

    }

    public override void Initialize()
    {
		PB_TimeLeftText.text = string.Format("{0:N1}", gameManager.plantTimer.timeLeft);
        PB_PassPhoneButton.gameObject.SetActive(false);
		PB_PlantBomb.gameObject.SetActive(true);

		gameManager.plantTimer.StartTimer();
		
		//Debug.Log("time to plant: " + timeToPlant + " time start: " + timeStart + " time end: " + timeEnd + " timetodefuse: " + gameManager.timeToDefuse);
    }

    // Update the timer to plant the bomb
    public override void RunState() 
	{
		if (!localPlayer.allLocalBombsPlanted)
            {
                // Update the timer UI
			PB_TimeLeftText.text = string.Format("{0:N1}", gameManager.plantTimer.timeLeft);

                // If time runs out, planter loses
                /////////////////////////////////////////////////
                // TODO implement time expired
                /////////////////////////////////////////////////

			if (gameManager.plantTimer.TimedOut())
                {
                    //Debug.LogWarning("Time ran out to plant the bomb!");
                    //
                }

            }
            // If the pass phone button is NOT active, make it active
            else if (!PB_PassPhoneButton.gameObject.activeSelf)
            {
                PB_PassPhoneButton.gameObject.SetActive(true);
            }
	}

	public void OnTappedOnNewTargetButton()
	{

		gameManager.CreateBombTarget();
        
        if (!gameManager.allBombsPlanted())
            return;
        //TODO: Only works when only one bomb is planted
        //done
        localPlayer.allLocalBombsPlanted = true;
		PB_PlantBomb.gameObject.SetActive(false);
	}
	
	public override void PassPhone()
	{
		gameManager.plantTimer.StopTimer();
        gameManager.SetState(gameManager.passingState);
    }
}
