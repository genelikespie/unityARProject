using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {

    public MainMenuState mainMenuState { get; private set; }
    public SharedModeMenuState sharedModeMenuState { get; private set; }
    public State currentState {get; private set;}

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
