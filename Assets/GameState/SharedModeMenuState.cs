﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class SharedModeMenuState : State {

    // UI elements
    InputField SMM_PlanterNameInputField;
    InputField SMM_DefuserNameInputField;
    Toggle SMM_TutorialToggle; // Toggles between showing tutorial or not
    Button SMM_BackButton;
    Button SMM_PlayButton;

	GameObject smBack;

    protected virtual void Awake()
    {
        // Call the base class's function to initialize all variables
        base.Awake();

        // Find all UI elements in the scene
        SMM_PlanterNameInputField = GameObject.Find("SMM_PlanterNameInputField").GetComponent<InputField>();
        SMM_DefuserNameInputField = GameObject.Find("SMM_DefuserNameInputField").GetComponent<InputField>();
        SMM_TutorialToggle = GameObject.Find("SMM_TutorialToggle").GetComponent<Toggle>();
        SMM_BackButton = GameObject.Find("SMM_BackButton").GetComponent<Button>();
        SMM_PlayButton = GameObject.Find("SMM_PlayButton").GetComponent<Button>();
        if (!SMM_PlanterNameInputField)
            Debug.LogError("SMM_PlanterNameInputField");
        if (!SMM_DefuserNameInputField)
            Debug.LogError("SMM_DefuserNameInputField");
        if (!SMM_TutorialToggle)
            Debug.LogError("SMM_TutorialToggle");
        if (!SMM_BackButton)
            Debug.LogError("SMM_BackButton");
        if (!SMM_PlayButton)
            Debug.LogError("SMM_PlayButton");

        if (!gameManager)
            Debug.LogError("AWAKE: CANT find game manager in base");

		// Get SM_Backdrop and disable renderer
		smBack = GameObject.Find("SM_Backdrop");
		smBack.GetComponent<MeshRenderer>().enabled = false;
    }

    /* Reset the UI
     */
    public override void Initialize()
    {
        if (!gameManager)
            Debug.LogError("Cant find game manager");

		// Enable SM_Backdrop renderer
		smBack.GetComponent<MeshRenderer>().enabled = true;
    }

    public override void PlantBomb()
    {
        if (SMM_PlanterNameInputField.text == "" || SMM_DefuserNameInputField.text == "")
        {
            // TODO throw a modal panel or some message to the screen/camera
            return;
        }

		gameManager.player = new LocalPlayerAdapter(
			new Player(SMM_PlanterNameInputField.text,
		        	   SMM_DefuserNameInputField.text,
		           gameManager.getMaxBombLimit()));

		// Join match here for multiplayer (different state)

		gameManager.plantTimer = new Timer(45);
		gameManager.defuseTimer = new Timer(60);
		gameManager.passTimer = new Timer(30);

        gameManager.SetAR();

		//Disable SM_Backdrop renderer, enabled camera plane
		smBack.GetComponent<MeshRenderer>().enabled = false;
		GameObject.Find("BackgroundPlane").GetComponent<MeshRenderer>().enabled = true;

        gameManager.SetState(gameManager.plantBombState);
    }

}
