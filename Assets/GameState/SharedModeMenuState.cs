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

    // To use the List class, use the System.Collections.Generic library
    public List<Player> planters = new List<Player>();
    public List<Player> defusers = new List<Player>();
    public int numOfBombs; 
    public float timeToPlant;
    public float timeToDefuse;

    protected virtual void Awake()
    {
        base.Awake();// Call the base class's function to initialize all variables
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
    }

    /* Reset the UI
     */
    public override void Initialize()
    {
        planters.Clear();
        defusers.Clear();
        if (!gameManager)
            Debug.LogError("Cant find game manager");
    }

    public override void PlantBomb()
    {
        if (SMM_PlanterNameInputField.text == "" || SMM_DefuserNameInputField.text == "")
        {
            // TODO throw a modal panel or some message to the screen/camera
            return;
        }
        planters.Add(new Player(SMM_PlanterNameInputField.text));
        defusers.Add(new Player(SMM_DefuserNameInputField.text));

        numOfBombs = 1;
        timeToPlant = 45f;
        timeToDefuse = 60f;

        // Set GameManager's game info
        gameManager.planters = planters;
        gameManager.defusers = defusers;
        gameManager.numOfBombs = numOfBombs;
        gameManager.timeToPlant = timeToPlant;
        gameManager.timeToDefuse = timeToDefuse;

        gameManager.SetAR();
        gameManager.SetState(gameManager.plantBombState);
    }

}
