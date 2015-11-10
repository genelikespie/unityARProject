using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using UnityEngine.Networking.Match;

public class MultiplayerLobbyState : State {

	public GameObject networkSessionPrefab;

	// UI elements
	Text MLS_PlayerJoinedCount;
	Text MLS_PlayerReadyCount;

    Toggle MMS_TutorialToggle; // Toggles between showing tutorial or not
	Button MMS_BackButton;
	Button MMS_ReadyButton;

	protected virtual void Awake()
	{
		// Call the base class's function to initialize all variables
		base.Awake();
		if (!gameManager)
			Debug.LogError("AWAKE: CANT find game manager in base");
	}

	/* Reset the UI
     */
	public override void Initialize()
	{
		if (!gameManager)
			Debug.LogError("Cant find game manager");
	}

    NetworkPlayer[] playerList = null;

    public override void RunState()
    {
        base.RunState();
        MLS_PlayerJoinedCount = GameObject.Find("MLS_PlayerJoinedCount").GetComponent<Text>();
        MLS_PlayerReadyCount = GameObject.Find("MLS_PlayerReadyCount").GetComponent<Text>();
        playerList = GameObject.FindObjectsOfType<NetworkPlayer>();
        if (playerList.Length == 0)
        {
            return;
        }

        Debug.LogError("Name " + MLS_PlayerJoinedCount.text + " ");
        Debug.LogError("PlayerList " + playerList + " " );
        Debug.LogError("PlayerList " + playerList.Length + " ");
        MLS_PlayerJoinedCount.text = "Joined Players: " + playerList.Length;

        int readyCount = 0;
        foreach (NetworkPlayer p in playerList)
        {
            if (p.ready)
            {
                ++readyCount;
            }
        }
        MLS_PlayerReadyCount.text = "Ready Players: " + readyCount;
        if (readyCount == playerList.Length)
        {
            PlantBomb();
        }
    }

    public void getReady()
    {
        playerList = GameObject.FindObjectsOfType<NetworkPlayer>();
        foreach (NetworkPlayer p in playerList)
        {
            if (p.isLocalPlayer)
            {
                p.CmdSetReady();
            }
        }
    }

    public override void PlantBomb()
    {
        gameManager.isMultiplayer = true;

       /* if (MMS_PlanterNameInputField.text == "" || MMS_DefuserNameInputField.text == "")
        {
            // TODO throw a modal panel or some message to the screen/camera
            return;
        }*/

        // TODO: Right now the screen results in nonfunctional buttons.
        // Implement matchmaker here.

        gameManager.SetAR();

        gameManager.SetState(gameManager.plantBombState);
    }
}
