using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class MultiplayerMenuState : State {
	
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
		if (MMS_PlanterNameInputField.text == "" || MMS_DefuserNameInputField.text == "")
		{
			// TODO throw a modal panel or some message to the screen/camera
			return;
		}

		// TODO: Right now the screen results in nonfunctional buttons.

		session.playerDevices.Add(new Player(MMS_PlanterNameInputField.text,
		                                                 MMS_DefuserNameInputField.text));

		session = new Session();

		session.plantTimer = new Timer(45);
		session.defuseTimer = new Timer(60);
		session.passTimer = new Timer(30);
		session.numOfBombs = 1;
		
		gameManager.SetAR();
		
		gameManager.SetState(gameManager.plantBombState);
	}

	
}
