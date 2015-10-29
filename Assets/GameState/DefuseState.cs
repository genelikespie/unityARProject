using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class DefuseState : State {

    // UI
    Text D_TimeLeftText;
    Button D_DefuseBombButton;

    //
    public float timeToDefuse;
    public float timeLeft;

    public virtual void Awake()
    {
        // Call the base class's function to initialize all variables
        base.Awake();

        // Find all UI elements in the scene
        D_TimeLeftText = GameObject.Find("D_TimeLeftText").GetComponent<Text>();
        D_DefuseBombButton = GameObject.Find("D_DefuseBombButton").GetComponent<Button>();

        if (!D_TimeLeftText)
            Debug.LogError("D_TimeLeftText");
        if (!D_DefuseBombButton)
            Debug.LogError("D_DefuseBombButton");
    }

    public override void Initialize()
    {
        base.Initialize();
        // Set the defuse button to be false
        // Activate it when the bomb is in view
        D_DefuseBombButton.gameObject.SetActive(false);
        timeToDefuse = gameManager.timeToDefuse;
        timeLeft = timeToDefuse;
    }

    void Update()
    {
        // If this is the current state
        // and the bomb has NOT been planted
        if (isCurrentState)
        {
            // Decrement the timer
            timeLeft = timeLeft - Time.deltaTime;
            if (timeLeft < 0.0f)
                timeLeft = 0;

            // Update the timer UI
            D_TimeLeftText.text = string.Format("{0:N1}", timeLeft);

            // If bomb is in view, activate the button
            /////////////////////////////////////////////////
            // TODO implement checking if the bomb is in view
            /////////////////////////////////////////////////

            if (true) {
                D_DefuseBombButton.gameObject.SetActive(true);
            }

            // If time runs out and we have not defused the bomb, defuser loses
            /////////////////////////////////////////////////
            // TODO implement time expired
            /////////////////////////////////////////////////

            if (timeLeft <= 0 && gameManager.bombPlanted)
            {
                //Debug.LogWarning("Time ran out to plant the bomb!");
                //base.TimeExpired();
            }
        }
    }

    public override void AllBombsDefused()
    {
        base.AllBombsDefused();
        /////////////////////////////////////////////////
        // TODO implement game over functionality
        /////////////////////////////////////////////////

        gameManager.bombPlanted = false;
        gameManager.SetState(gameManager.gameOverState);
    }
}
