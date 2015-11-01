using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class DefuseState : State {

    // UI
    Text D_TimeLeftText;
    Button D_DefuseBombButton;

    public virtual void Awake()
    {
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
		session.defuseTimer.StartTimer();
	}

	public override void RunState()
    {
		// Update the timer UI
        D_TimeLeftText.text = string.Format("{0:N1}", session.defuseTimer.timeLeft);

        // If bomb is in view, activate the button
        /////////////////////////////////////////////////
        // TODO implement checking if the bomb is in view
        /////////////////////////////////////////////////

        if (gameManager.bombVisible) {
            D_DefuseBombButton.gameObject.SetActive(true);
        }
        else
        {
            D_DefuseBombButton.gameObject.SetActive(false);
        }

        // If time runs out and we have not defused the bomb, defuser loses
        /////////////////////////////////////////////////
        // TODO implement time expired
        /////////////////////////////////////////////////

		if (session.defuseTimer.TimedOut() && session.bombPlanted)
        {
                //Debug.LogWarning("Time ran out to plant the bomb!");
                //base.TimeExpired();
		}
    }

    public override void AllBombsDefused()
    {
        base.AllBombsDefused();
		session.defuseTimer.StopTimer();
        /////////////////////////////////////////////////
        // TODO implement game over functionality
        /////////////////////////////////////////////////

        session.bombPlanted = false;
        gameManager.SetState(gameManager.gameOverState);
    }
}
