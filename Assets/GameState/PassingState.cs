using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PassingState : State {

    // UI
    Text P_TimeLeftText;
    Button P_PassedToDefuserButton;

    public virtual void Awake()
    {
        // Call the base class's function to initialize all variables
        base.Awake();

        // Find all UI elements in the scene
        P_TimeLeftText = GameObject.Find("P_TimeLeftText").GetComponent<Text>();
        P_PassedToDefuserButton = GameObject.Find("P_PassedToDefuserButton").GetComponent<Button>();

        if (!P_TimeLeftText)
            Debug.LogError("P_TimeLeftText");
        if (!P_PassedToDefuserButton)
            Debug.LogError("P_PassedToDefuserButton");
    }

	public override void Initialize() {
		// Initialize to 30 seconds to pass phone to defuser
		session.passTimer.StartTimer();
	}

    public override void RunState()
    {
        // Update the timer UI
		P_TimeLeftText.text = string.Format("{0:N1}", session.passTimer.timeLeft);

            // If time runs out and we have not changed state to DefuseBomb(), planter loses
            /////////////////////////////////////////////////
            // TODO implement time expired
            /////////////////////////////////////////////////

		if (session.passTimer.TimedOut())
            {
                //Debug.LogWarning("Time ran out to plant the bomb!");
                //
            }
    }
    public override void DefuseBomb()
    {
        base.DefuseBomb();
		session.passTimer.StopTimer();

		//gameManager.bombVisible = false;
		gameManager.SetState(gameManager.defuseState);

    }
}
