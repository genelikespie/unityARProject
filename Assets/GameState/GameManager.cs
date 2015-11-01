using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/* The GameManager class manages all the game states
 */
public class GameManager : MonoBehaviour {

    // Menu properties
    Vector2 closedMenuPivot = new Vector2(1.5f, 0.5f);
    Vector2 openMenuPivot = new Vector2(0.5f, 0.5f);
    Vector2 screenSize;

    // Camera properties
    ///////////////////////////////////////////////////////
    // TODO rename vuforia camera's name
    ///////////////////////////////////////////////////////

    string arCameraName = "ARCamera";
    string vuforiaCameraName = "Camera";
    string menuCameraName = "MenuCamera";
    private GameObject arCamera; // Camera for the gameObject that hols Vuforia's GUI camera
    private Camera vuforiaCamera; // Camera for Vuforia's GUI
    private Camera menuCamera; // Camera for menus

	// Code refactor
	public Session session;
	public bool bombVisible { get; set; }
	private UserDefinedTargetEventHandler udtHandler;

    // Derived states
    public MainMenuState mainMenuState { get; private set; }
    public SharedModeMenuState sharedModeMenuState { get; private set; }
    ///////////////////////////////////////////////////////
    // TODO multiplayerMenuState
    ///////////////////////////////////////////////////////
	public MultiplayerMenuState multiplayerMenuState {get; private set;}

    public TutorialMenuState tutorialMenuState { get; private set; }
    public PlantBombState plantBombState { get; private set; }
    public PassingState passingState { get; private set; }
    public DefuseState defuseState { get; private set; }
    public GameOverState gameOverState { get; private set; }
    public State currentState { get; private set; }
    
    // A list of all the states
    public List<State> stateList { get; private set; }

    public static GameManager gameManager;
    public static GameManager Instance() {
        if (!gameManager)
        {
            gameManager = FindObjectOfType(typeof(GameManager)) as GameManager;
            if (!gameManager)
                Debug.LogError("There needs to be one game manager!");
        }
        return gameManager;
    }

	// Use this for linking the states with their variables
	void Awake () {

		udtHandler = GameObject.Find ("UserDefinedTargetBuilder")
			.GetComponent<UserDefinedTargetEventHandler>();

        // Set Screen Size
        stateList = new List<State>();
        screenSize = new Vector2(Screen.width, Screen.height);

        // Find the cameras in the game scene
        arCamera = GameObject.Find(arCameraName);
        vuforiaCamera = GameObject.Find(vuforiaCameraName).GetComponent<Camera>();
        menuCamera = GameObject.Find(menuCameraName).GetComponent<Camera>();

        if (!arCamera)
            Debug.LogError("Cannot find" + arCameraName);
        if (!vuforiaCamera)
            Debug.LogError("Cannot find: " + vuforiaCameraName);
        if (!menuCamera)
            Debug.LogError("Cannot find" + menuCameraName);

        mainMenuState = GetComponentInChildren<MainMenuState>();
        sharedModeMenuState = GetComponentInChildren<SharedModeMenuState>();
        tutorialMenuState = GetComponentInChildren<TutorialMenuState>();
        ///////////////////////////////////////////////////////
        // TODO add multiplayer state initialization
        ///////////////////////////////////////////////////////
		multiplayerMenuState = GetComponentInChildren<MultiplayerMenuState>();

        plantBombState = GetComponentInChildren<PlantBombState>();
        passingState = GetComponentInChildren<PassingState>();
        defuseState = GetComponentInChildren<DefuseState>();
        gameOverState = GetComponentInChildren<GameOverState>();


        // Add all of the states to the stateList to keep track of them
        stateList.Add(mainMenuState);
        stateList.Add(sharedModeMenuState);
        stateList.Add(tutorialMenuState);
        ///////////////////////////////////////////////////////
        //// TODO add multiplayer state
        ///////////////////////////////////////////////////////
		stateList.Add (multiplayerMenuState);

        stateList.Add(plantBombState);
        stateList.Add(passingState);
        stateList.Add(defuseState);
        stateList.Add(gameOverState);

        // Check if any of the states are null
        if (!mainMenuState)
            Debug.LogError("MainMenuState not found!");
        foreach (State s in stateList)
        {
            if (!s)
                Debug.LogError("At least one of the states are not found");
        }

    }

    void Start()
    {
        RectTransform menuRectTransform; // temporary variable to reduce overhead
        foreach (State s in stateList)
        {
            menuRectTransform = s.GetComponent<RectTransform>();
            // Set their position to be (0,0). i.e. in the center
            menuRectTransform.localPosition = Vector2.zero;
            // Set the screensize of all menus
            menuRectTransform.sizeDelta = screenSize;
            // Set all menus to be invisible (outside of the screen)
            menuRectTransform.pivot = closedMenuPivot;
        }

        SetState(mainMenuState);
    }
	// Update is called once per frame
	void Update () {
		if(session != null)
			session.updateTimers();
		currentState.RunState();
	}

    public void SetState (State nextState) {
        // Change the pivot of the current menu to set it outside of view
        if (currentState)
            this.currentState.GetComponent<RectTransform>().pivot = closedMenuPivot;

        // Initialize the next state
        nextState.Initialize();
        // Set the current state to be the next state
        this.currentState = nextState;
        // Set the next state to be in view
        this.currentState.GetComponent<RectTransform>().pivot = openMenuPivot;
    }

     /* It enables the vuforia GUI and AR camera and its audio listener
     */
    public void SetAR()
    {
        arCamera.GetComponent<AudioListener>().enabled = true;
        vuforiaCamera.enabled = true;
        //menuCamera.enabled = false;
        //menuCamera.GetComponent<AudioListener>().enabled = false;
    }

	public void CreateBombTarget() {
		if(udtHandler != null)
			udtHandler.CreateTarget();
		else
			Debug.Log ("Could not create new target. UDT Event Handler variable not set in GameManager.");
	}
}
