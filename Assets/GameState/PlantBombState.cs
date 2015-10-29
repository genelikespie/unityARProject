using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class PlantBombState : State {

    // UI
    Text PB_MenuTitle;
    Text PB_TimeLeftText;
    Button PB_PassPhoneButton;

    // Local copy of game information
    List<Player> planters;
    List<Player> defusers;
    int numOfBombs;
    float timeToPlant;

    // time left to plant
    float timeLeft;

    // start time of plant state
    float timeStart;

    // end time to plant the bomb
    float timeEnd;

    protected virtual void Awake()
    {
        // Call the base class's function to initialize all variables
        base.Awake();

        // Find all UI elements in the scene
        PB_MenuTitle = GameObject.Find("PB_MenuTitle").GetComponent<Text>();
        PB_TimeLeftText = GameObject.Find("PB_TimeLeftText").GetComponent<Text>();
        PB_PassPhoneButton = GameObject.Find("PB_PassPhoneButton").GetComponent<Button>();
        if (!PB_MenuTitle)
            Debug.LogError("PB_MenuTitle");
        if (!PB_TimeLeftText)
            Debug.LogError("PB_TimeLeftText");
        if (!PB_PassPhoneButton)
            Debug.LogError("PB_PassPhoneButton");

    }

    public override void Initialize()
    {
        planters = gameManager.planters;
        defusers = gameManager.defusers;
        numOfBombs = gameManager.numOfBombs;
        timeToPlant = gameManager.timeToPlant;

        timeLeft = timeToPlant;
        timeStart = Time.time;
        timeEnd = timeStart + timeToPlant;

        PB_TimeLeftText.text = string.Format("{0:N1}", timeLeft);
        PB_PassPhoneButton.gameObject.SetActive(false);

        Debug.Log("time to plant: " + timeToPlant + " time start: " + timeStart + " time end: " + timeEnd + " timetodefuse: " + gameManager.timeToDefuse);
    }

    // Update the timer to plant the bomb
    void Update()
    {
        // If this is the current state
        // and the bomb has NOT been planted
        if (isCurrentState)
        {
            if (!gameManager.bombPlanted)
            {
                // Decrement the timer
                timeLeft = timeLeft - Time.deltaTime;
                if (timeLeft < 0.0f)
                    timeLeft = 0;

                // Update the timer UI
                PB_TimeLeftText.text = string.Format("{0:N1}", timeLeft);

                // If time runs out, planter loses
                if (timeLeft <= 0 && !gameManager.bombPlanted)
                {
                    //Debug.LogWarning("Time ran out to plant the bomb!");
                    //gameManager.SetState(gameManager.gameOverState);
                }

            }
            // If the pass phone button is NOT active, make it active
            else if (!PB_PassPhoneButton.gameObject.activeSelf)
            {
                PB_PassPhoneButton.gameObject.SetActive(true);
            }
        }

    }
}
