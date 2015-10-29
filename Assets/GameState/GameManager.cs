using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {

    public MainMenuState mainMenuState { get; private set; }
    public SharedModeMenuState sharedModeMenuState { get; private set; }

    public MultiplayerMenuState multiplayerMenuState { get; private set; }
    public PlantBombState plantBombState { get; private set; }
    public State currentState {get; private set;}

    //added this for ease of determining how many bombs
    public static int numOfBombs { get; set; }

    //added these for determining how many wins each team or player has total
    public static int player1Wins { get; set; }
    public static int player2Wins { get; set; }

    private volatile static GameManager gameManager;
    public static GameManager Instance() {
        if (gameManager == null)
        {
            gameManager = FindObjectOfType(typeof(GameManager)) as GameManager;
            if (gameManager == null)
                Debug.LogError("There needs to be one game manager!");
        }
        return gameManager;
    }

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void SetState (State nextState) {
        this.currentState = nextState;
    }
}
