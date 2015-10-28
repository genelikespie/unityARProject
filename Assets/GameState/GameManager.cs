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

    public MainMenuState mainMenuState { get; private set; }
    public SharedModeMenuState sharedModeMenuState { get; private set; }
    // TODO multiplayerMenuState
    public TutorialMenuState tutorialMenuState { get; private set; }
    public PlantBombState plantBombState { get; private set; }
    public PassingState passingState { get; private set; }
    public DefuseState defuseState { get; private set; }
    public GameOverState gameOverState { get; private set; }
    public State currentState { get; private set; }
    
    // A list of all the states
    public List<State> stateList { get; private set; }

    private static GameManager gameManager;
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
        // TODO: get screen size;
        stateList = new List<State>();
        screenSize = new Vector2(Screen.width, Screen.height);

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

    }

    void Start()
    {
        foreach (State s in stateList)
        {
            // Set the screensize of all menus
            s.GetComponent<RectTransform>().sizeDelta = screenSize;
            // Set all menus to be invisible (outside of the screen)
            s.GetComponent<RectTransform>().pivot = closedMenuPivot;
        }

        SetState(mainMenuState);
    }
	// Update is called once per frame
	void Update () {
	
	}

    public void SetState (State nextState) {
        // Change the pivot of the current menu to set it outside of view
        if (currentState != null)
            this.currentState.GetComponent<RectTransform>().pivot = closedMenuPivot;
        // Set the current state to be the next state
        this.currentState = nextState;
        // Set the next state to be in view
        this.currentState.GetComponent<RectTransform>().pivot = openMenuPivot;
    }
}
