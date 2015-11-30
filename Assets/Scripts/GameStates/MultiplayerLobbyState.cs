using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using UnityEngine.Networking.Match;
using UnityEngine.Assertions;

public class MultiplayerLobbyState : State {

	public GameObject networkSessionPrefab;

	// UI elements
	Text MLS_PlayerJoinedCount;
	Text MLS_PlayerReadyCount;

    Toggle MMS_TutorialToggle; // Toggles between showing tutorial or not
	Button MMS_BackButton;
	Button MMS_ReadyButton;

	//GameObject mmBack;
    GameObject mmsBack;

	protected virtual void Awake()
	{
		// Call the base class's function to initialize all variables
		base.Awake();
        
		if (!gameManager)
			Debug.LogError("AWAKE: CANT find game manager in base");

        Assert.raiseExceptions = true;
		// Get SM_Backdrop and disable renderer
		//mmBack = GameObject.Find("MM_Backdrop");
		//mmBack.SetActive(false);

        mmsBack = GameObject.Find("MMS_Backdrop");
        if (mmsBack != null)
        {
            mmsBack.SetActive(false);
        }
        MLS_PlayerJoinedCount = GameObject.Find("MLS_PlayerJoinedCount").GetComponent<Text>();
        MLS_PlayerReadyCount = GameObject.Find("MLS_PlayerReadyCount").GetComponent<Text>();
    }

	/* Reset the UI
     */
	public override void Initialize()
	{
        //Check if gameManager exists
        Assert.IsNotNull(gameManager, "Cant find game manager");

		// Enable SM_Backdrop renderer
		//mmBack.GetComponent<MeshRenderer>().enabled = true;
        if (mmsBack != null)
        {
            mmsBack.SetActive(true);
        }
        MLS_PlayerJoinedCount.text = "Loading...";
        MLS_PlayerReadyCount.text = "";
    }

    NetworkPlayer[] playerList = null;

    public override void RunState()
    {
        base.RunState();
        playerList = GameObject.FindObjectsOfType<NetworkPlayer>();
        if (playerList.Length == 0)
        {
            return;
        }

        Debug.Log("Name " + MLS_PlayerJoinedCount.text + " ");
        Debug.Log("PlayerList " + playerList + " " );
        Debug.Log("PlayerList " + playerList.Length + " ");
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
            //Make sure the list of players is not null
            Assert.IsNotNull<NetworkPlayer[]>(playerList);

            //This may seem redundant, but check if the readyCount 
            //is equal to the length of the list of players
            Assert.AreEqual<int>(playerList.Length, readyCount);
            PlantBomb();
        }
    }

    public void getReady()
    {
        playerList = GameObject.FindObjectsOfType<NetworkPlayer>();

        //Make sure the list of players is not null
        Assert.IsNotNull<NetworkPlayer[]>(playerList);

        foreach (NetworkPlayer p in playerList)
        {
            if (p.isLocalPlayer)
            {
                p.CmdSetReady(true);
            }
        }
    }

	public override void PlantBomb()
	{
		gameManager.plantTimer = new Timer(45);
		gameManager.defuseTimer = new Timer(60);
		gameManager.passTimer = new Timer(30);
		
		gameManager.SetAR();
		
		//Disable SM_Backdrop renderer, enabled camera plane
		//mmBack.GetComponent<MeshRenderer>().enabled = false;
        if (mmsBack != null)
        {
            mmsBack.SetActive(false);
        }
		GameObject.Find("BackgroundPlane").GetComponent<MeshRenderer>().enabled = true;
		
		gameManager.SetState(gameManager.plantBombState);
	}
}
