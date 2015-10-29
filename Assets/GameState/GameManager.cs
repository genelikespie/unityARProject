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

    // App Manager (Vuforia's GUI object)
    SceneViewManager sceneViewManager;
    AppManager appManager;

    // Camera properties
    // TODO rename vuforia camera's name
    string arCameraName = "ARCamera";
    string vuforiaCameraName = "Camera";
    string menuCameraName = "MenuCamera";
    private GameObject arCamera; // Camera for the gameObject that hols Vuforia's GUI camera
    private Camera vuforiaCamera; // Camera for Vuforia's GUI
    private Camera menuCamera; // Camera for menus

    // Keep track of game information
    public List<Player> planters;
    public List<Player> defusers;
    public int numOfBombs;
    public float timeToPlant;
    public float timeToDefuse;

    public bool bombPlanted;

    // Derived states
    public MainMenuState mainMenuState { get; private set; }
    public SharedModeMenuState sharedModeMenuState { get; private set; }
<<<<<<< HEAD

    public MultiplayerMenuState multiplayerMenuState { get; private set; }
    public PlantBombState plantBombState { get; private set; }
    public State currentState {get; private set;}

    //added this for ease of determining how many bombs
    public static int numOfBombs { get; set; }

    //added these for determining how many wins each team or player has total
    public static int player1Wins { get; set; }
    public static int player2Wins { get; set; }

    private volatile static GameManager gameManager;
=======
    // TODO multiplayerMenuState
    public TutorialMenuState tutorialMenuState { get; private set; }
    public PlantBombState plantBombState { get; private set; }
    public PassingState passingState { get; private set; }
    public DefuseState defuseState { get; private set; }
    public GameOverState gameOverState { get; private set; }
    public State currentState { get; private set; }
    
    // A list of all the states
    public List<State> stateList { get; private set; }

    public static GameManager gameManager;
>>>>>>> 9ebac8121f25b3cf90b02bab4a7cb509f9d80c5f
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
        // Set Screen Size
        stateList = new List<State>();
        screenSize = new Vector2(Screen.width, Screen.height);

        // Set Vuforia's GUI with app manager
        sceneViewManager = FindObjectOfType<SceneViewManager>();
        appManager = FindObjectOfType<AppManager>();
        if (!sceneViewManager)
            Debug.LogError("Cannot find SceneViewManager");
        if (!appManager)
            Debug.LogError("Cannot find AppManager");

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

        // Disable the AR GUI for now and enable the MainMenu
        //SetMenu();

        mainMenuState = GetComponentInChildren<MainMenuState>();
        sharedModeMenuState = GetComponentInChildren<SharedModeMenuState>();
        tutorialMenuState = GetComponentInChildren<TutorialMenuState>();
        // TODO add multiplayer state initialization
        plantBombState = GetComponentInChildren<PlantBombState>();

        // Add all of the states to the stateList to keep track of them
        stateList.Add(mainMenuState);
        stateList.Add(sharedModeMenuState);
        stateList.Add(tutorialMenuState);
        // TODO add multiplayer state
        stateList.Add(plantBombState);

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

    /* This function disables the Vuforia GUI and the AR Camera component and audio listener.
     * It enables the menu camera and its audio listener
     */
    public void SetMenu()
    {
        arCamera.GetComponent<AudioListener>().enabled = false;
        vuforiaCamera.enabled = false;
        sceneViewManager.gameObject.SetActive(false);
        menuCamera.enabled = true;
        menuCamera.GetComponent<AudioListener>().enabled = true;
    }

     /* It enables the vuforia GUI and AR camera and its audio listener
     */
    public void SetAR()
    {
        arCamera.GetComponent<AudioListener>().enabled = true;
        vuforiaCamera.enabled = true;
        sceneViewManager.gameObject.SetActive(true);
        //menuCamera.enabled = false;
        //menuCamera.GetComponent<AudioListener>().enabled = false;
    }
}
