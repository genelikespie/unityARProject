using UnityEngine;
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
    Slider SMM_NumOfBombsSlider;
    Text SMM_NumOfBombsText;

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
        SMM_NumOfBombsSlider = GameObject.Find("SMM_NumOfBombsSlider").GetComponent<Slider>();
        SMM_NumOfBombsText = GameObject.Find("SMM_NumOfBombsText").GetComponent<Text>();
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
        if (!SMM_NumOfBombsSlider)
            Debug.LogError("SMM_NumOfBombsSlider");
        if (!SMM_NumOfBombsText)
            Debug.LogError("SMM_NumOfBombsText");

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
        gameManager.SetNumOfBombs((int)SMM_NumOfBombsSlider.value);
    }

    public override void RunState()
    {
        DisplayNumOfBombs();
    }

    public override void PlantBomb()
    {
        // If no name is provided, use these default ones
        if (SMM_PlanterNameInputField.text == "")
        {
            SMM_PlanterNameInputField.text = "Planter";
        }
        if (SMM_DefuserNameInputField.text == "")
        {
            SMM_DefuserNameInputField.text = "Defuser";
        }

        // Create the player objects and give them the names and bomb information
		gameManager.player = new LocalPlayerAdapter(
			new LocalPlayer(SMM_PlanterNameInputField.text,
		        	   SMM_DefuserNameInputField.text,
		           gameManager.getMaxBombLimit()));

        // Setup all the timers
        gameManager.plantTimer = new Timer(15 + 15 * gameManager.getMaxBombLimit());
		gameManager.defuseTimer = new Timer(0 + 30 * gameManager.getMaxBombLimit());
		gameManager.passTimer = new Timer(30);

        // Setup the camera
        gameManager.SetAR();

		// Disable SM_Backdrop renderer, enabled camera plane
		smBack.GetComponent<MeshRenderer>().enabled = false;
		GameObject.Find("BackgroundPlane").GetComponent<MeshRenderer>().enabled = true;

        gameManager.SetState(gameManager.plantBombState);
    }

    
    public void OnValueChanged()
    {
        gameManager.SetNumOfBombs((int)SMM_NumOfBombsSlider.value);
    }
    
    public void DisplayNumOfBombs()
    {
        SMM_NumOfBombsText.text = "Number of Bombs: " + gameManager.getMaxBombLimit().ToString();
    }
}
