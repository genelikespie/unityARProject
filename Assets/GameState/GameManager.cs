using UnityEngine;
using UnityEngine.Assertions;
using System.Collections;
using System.Collections.Generic;
using Vuforia;

/* The GameManager class manages all the game states
 */
public class GameManager : MonoBehaviour {

    // Menu properties
    Vector2 closedMenuPivot = new Vector2(1.5f, 0.5f);
    Vector2 openMenuPivot = new Vector2(0.5f, 0.5f);
    Vector2 screenSize;

    // Camera properties
    string arCameraName = "ARCamera";
    string vuforiaCameraName = "Camera";
    private GameObject arCamera; // Camera for the gameObject that hols Vuforia's GUI camera
    private Camera vuforiaCamera; // Camera for Vuforia's GUI

	// Can either represent NetworkPlayer or Player.
	public PlayerAdapter player;

	//Temporary variables for NetworkPlayer initializing.
	[HideInInspector]
	public string tempDefuserName;

	[HideInInspector]
	public string tempPlanterName;

    private int bombsCount = 2;
    public void SetNumOfBombs(int num)
    {
        bombsCount = num;
    }
	public int getMaxBombLimit() { return bombsCount; }
	public Material DefuseMaterial;
    //private int bombsDefused = 0;
    //private int bombsPlanted = 0;
    //public bool allBombsPlanted() { return bombsPlanted == bombsCount ? true : false; }
    //public bool allBombsDefused () { return bombsDefused == bombsCount ? true : false; }
    //public void defuseBomb() { bombsDefused++; }

	public bool bombVisible { get; set; }
	private UserDefinedTargetEventHandler udtHandler;

	//Hint text
	public string hint;


	// Timer for each state
	public Timer plantTimer;
    public Timer armBombTimer;

	public Timer defuseTimer;
	public Timer passTimer;

    // Derived states
    public MainMenuState mainMenuState { get; private set; }
    public SharedModeMenuState sharedModeMenuState { get; private set; }
	public MultiplayerMenuState multiplayerMenuState {get; private set;}
    public MultiplayerLobbyState multiplayerLobbyState { get; private set; }
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
        // Set exceptions to be raised whenever an Assert statement fails
        Assert.raiseExceptions = true;

		udtHandler = GameObject.Find ("UserDefinedTargetBuilder")
			.GetComponent<UserDefinedTargetEventHandler>();
        Assert.IsNotNull(udtHandler, "Cannot find udtHandler");


        // Set the timers for each state
		plantTimer = new Timer(10);
        armBombTimer = new Timer(4);
        defuseTimer = new Timer(10);
		passTimer = new Timer(10);

        // Set Screen Size
        stateList = new List<State>();
        screenSize = new Vector2(Screen.width, Screen.height);

        // Find the cameras in the game scene
        arCamera = GameObject.Find(arCameraName);
        vuforiaCamera = GameObject.Find(vuforiaCameraName).GetComponent<Camera>();

        Assert.IsNotNull(arCamera, "Cannot find" + arCameraName);
        Assert.IsNotNull(vuforiaCamera, "Cannot find" + vuforiaCameraName);

        // Get references to all states
        mainMenuState = GetComponentInChildren<MainMenuState>();
        sharedModeMenuState = GetComponentInChildren<SharedModeMenuState>();
		multiplayerMenuState = GetComponentInChildren<MultiplayerMenuState>();
        multiplayerLobbyState = GetComponentInChildren<MultiplayerLobbyState>();
        plantBombState = GetComponentInChildren<PlantBombState>();
        passingState = GetComponentInChildren<PassingState>();
        defuseState = GetComponentInChildren<DefuseState>();
        gameOverState = GetComponentInChildren<GameOverState>();


        // Add all of the states to the stateList to keep track of them
        stateList.Add(mainMenuState);
        stateList.Add(sharedModeMenuState);
		stateList.Add (multiplayerMenuState);
        stateList.Add(multiplayerLobbyState);
        stateList.Add(plantBombState);
        stateList.Add(passingState);
        stateList.Add(defuseState);
        stateList.Add(gameOverState);

        // Check if any of the states are null
        foreach (State s in stateList)
        {
            Assert.IsNotNull(s, "One of the states is NOT FOUND");
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
            //menuRectTransform.sizeDelta = screenSize;
            // Set all menus to be invisible (outside of the screen)
            //menuRectTransform.pivot = closedMenuPivot;
			s.gameObject.SetActive(false);
        }

        SetState(mainMenuState);

    }

	public void updateTimers() {
		plantTimer.Run();
		defuseTimer.Run();
		passTimer.Run();
        armBombTimer.Run();
	}

	// Update is called once per frame
	void Update () {
		updateTimers();
		currentState.RunState();
	}

    public void SetState (State nextState) {
        Assert.IsNotNull(nextState, "Next State is NULL");
        Debug.Log("Passing from state " + currentState + " to " + nextState);
        // Change the pivot of the current menu to set it outside of view
        if (currentState)
            //this.currentState.GetComponent<RectTransform>().pivot = closedMenuPivot;
			currentState.gameObject.SetActive(false);

        // Initialize the next state
        nextState.Initialize();

        // Set the current state to be the next state
        currentState = nextState;

        // Set the next state to be in view
        //this.currentState.GetComponent<RectTransform>().pivot = openMenuPivot;
		currentState.gameObject.SetActive(true);
    }

     /* It enables the vuforia GUI and AR camera and its audio listener
     */
    public void SetAR()
    {
        arCamera.GetComponent<AudioListener>().enabled = true;
        vuforiaCamera.enabled = true;
    }

	public void CreateBombTarget() {
        Assert.IsNotNull(udtHandler, "Could not create new target. UDT Event Handler variable not set in GameManager.");
        udtHandler.CreateTarget();
	}

    public void ResetGame()
    {
        
        udtHandler.ReInitialize();
        player.setLocalBombsDefused(0);
        player.setLocalBombsPlanted(0);
        /////////////////////////////////////////////////////////////////
		/// TODO: If we implement a scoring system, reset that here too.
        /////////////////////////////////////////////////////////////////

    }

	// Attempts to defuse one bomb on the screen.
	// If succeeds, returns true. If it doesn't defuse a bomb, returns false.
	public bool AttemptDefuse() {
		StateManager sm = TrackerManager.Instance.GetStateManager();
		IEnumerable<TrackableBehaviour> tbs = sm.GetActiveTrackableBehaviours();
		
		foreach (TrackableBehaviour tb in tbs)
		{
			//find all bombs that are currently in camera view
			string name = tb.TrackableName;
			
			GameObject target = GameObject.Find(name);
            Assert.IsNotNull(target, "Can't find target of " + name);

			Transform child = target.transform.GetChild(0);
            Assert.IsNotNull(child, "Can't find child of " + name);

			if (!child.GetComponent<Renderer>().sharedMaterial.Equals(DefuseMaterial))
			{
				//defuse only 1 bomb at each press on Defuse button
				child.GetComponent<Renderer>().material = DefuseMaterial;
				Debug.Log("Defused " + name);
				return true;
			}
		}
		return false;
	}

}
