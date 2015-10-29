using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PassingState : State {

    // UI
    Text P_TimeLeftText;
    Button P_PassedToDefuserButton;

    // Total time for planter to pass phone to defuser
    // Set in PlantBombState's PassPhone() function
    public float timeToPass;

    // Time left for the planter
    public float timeLeft;

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

    public override void Initialize()
    {
        timeLeft = timeToPass;
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
            P_TimeLeftText.text = string.Format("{0:N1}", timeLeft);

            // If time runs out and we have not changed state to DefuseBomb(), planter loses
            /////////////////////////////////////////////////
            // TODO implement time expired
            /////////////////////////////////////////////////

            if (timeLeft <= 0 && isCurrentState)
            {
                //Debug.LogWarning("Time ran out to plant the bomb!");
                //
            }
        }

    }
    public override void DefuseBomb()
    {
        base.DefuseBomb();
        gameManager.SetState(gameManager.defuseState);

    }
}
