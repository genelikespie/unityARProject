using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Assertions;
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

    public float basePlantTime;
    public float plantTimePerBomb;
    public float baseDefuseTime;
    public float defuseTimePerBomb;
    public float passTime;

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

        // Get SM_Backdrop and disable renderer
        smBack = GameObject.Find("SM_Backdrop");

        Assert.IsNotNull(SMM_PlanterNameInputField, "Cannot find SMM_BackButton");
        Assert.IsNotNull(SMM_PlanterNameInputField, "Cannot find SMM_BackButton");
        Assert.IsNotNull(SMM_TutorialToggle, "Cannot find SMM_BackButton");
        Assert.IsNotNull(SMM_BackButton, "Cannot find SMM_BackButton");
        Assert.IsNotNull(SMM_PlayButton, "Cannot find SMM_PlayButton");
        Assert.IsNotNull(SMM_NumOfBombsSlider, "Cannot find SMM_NumOfBombsSlider");
        Assert.IsNotNull(SMM_NumOfBombsText, "Cannot find SMM_NumOfBombsText");
        Assert.IsNotNull(gameManager, "Cannot find game manager");
        Assert.IsNotNull(smBack, "Cannot find smBack");

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

        int sliderValue = (int)SMM_NumOfBombsSlider.value;
        Assert.IsTrue(sliderValue >= 1);
        gameManager.SetNumOfBombs(sliderValue);
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
        gameManager.plantTimer = new Timer(basePlantTime + plantTimePerBomb * gameManager.getMaxBombLimit());
		gameManager.defuseTimer = new Timer(baseDefuseTime + defuseTimePerBomb * gameManager.getMaxBombLimit());
		gameManager.passTimer = new Timer(passTime);

        // Setup the camera
        gameManager.SetAR();

		// Disable SM_Backdrop renderer, enabled camera plane
		smBack.GetComponent<MeshRenderer>().enabled = false;
        MeshRenderer backgroundPlantMeshRender = GameObject.Find("BackgroundPlane").GetComponent<MeshRenderer>();
        
        Assert.IsNotNull(backgroundPlantMeshRender, "Cannot find BackgroundPlane");
        backgroundPlantMeshRender.enabled = true;

        gameManager.SetState(gameManager.plantBombState);
    }

    
    public void OnValueChanged()
    {
        int sliderValue = (int)SMM_NumOfBombsSlider.value;
        Assert.IsTrue(sliderValue >= 1);
        gameManager.SetNumOfBombs((int)SMM_NumOfBombsSlider.value);
    }
    
    public void DisplayNumOfBombs()
    {
        SMM_NumOfBombsText.text = "Number of Bombs: " + gameManager.getMaxBombLimit().ToString();
    }
}
