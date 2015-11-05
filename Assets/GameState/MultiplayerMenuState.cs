using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class MultiplayerMenuState : State {

	public GameObject networkSessionPrefab;

	// UI elements
	InputField MMS_PlanterNameInputField;
	InputField MMS_DefuserNameInputField;
	Toggle MMS_TutorialToggle; // Toggles between showing tutorial or not
	Button MMS_BackButton;
	Button MMS_PlayButton;

	protected virtual void Awake()
	{
		// Call the base class's function to initialize all variables
		base.Awake();
		if (!gameManager)
			Debug.LogError("AWAKE: CANT find game manager in base");
	}

	/* Reset the UI
     */
	public override void Initialize()
	{
		if (!gameManager)
			Debug.LogError("Cant find game manager");
	}
	
	public override void PlantBomb()
	{
		gameManager.isMultiplayer = true;

		if (MMS_PlanterNameInputField.text == "" || MMS_DefuserNameInputField.text == "")
		{
			// TODO throw a modal panel or some message to the screen/camera
			return;
		}

		// TODO: Right now the screen results in nonfunctional buttons.
		// Implement matchmaker here.

		gameManager.SetAR();
		
		gameManager.SetState(gameManager.plantBombState);
	}

	
}
