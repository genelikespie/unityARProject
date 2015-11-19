using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using UnityEngine.Networking.Match;

public class MultiplayerMenuState : State {

	public GameObject networkSessionPrefab;
    GameObject mmsBack;

	// UI elements
	InputField MMS_PlanterNameInputField;
	InputField MMS_DefuserNameInputField;
    InputField MMS_CreateGameInputField;
    InputField MMS_JoinGameInputField;

    Toggle MMS_TutorialToggle; // Toggles between showing tutorial or not
	Button MMS_BackButton;
	Button MMS_PlayButton;

	protected virtual void Awake()
	{
		// Call the base class's function to initialize all variables
		base.Awake();

        mmsBack = GameObject.Find("MMS_Backdrop");
        if (mmsBack != null)
        {
            mmsBack.GetComponent<MeshRenderer>().enabled = false;
        }

		if (!gameManager)
			Debug.LogError("AWAKE: CANT find game manager in base");



		MMS_PlanterNameInputField = GameObject.Find("MMS_PlanterNameInputField").GetComponent<InputField>();
		MMS_DefuserNameInputField = GameObject.Find("MMS_DefuserNameInputField").GetComponent<InputField>();
	}

	/* Reset the UI
     */
	public override void Initialize()
	{
		if (!gameManager)
			Debug.LogError("Cant find game manager");

        if (mmsBack != null)
        {
            mmsBack.GetComponent<MeshRenderer>().enabled = true;
        }

	}

	public override void RunState() {
	}


    public void CreateGame()
    {
        NetworkManager.singleton.StartMatchMaker();
        MMS_CreateGameInputField = GameObject.Find("MMS_CreateGameInputField").GetComponent<InputField>();
        string roomName = MMS_CreateGameInputField.text;
        uint roomSize = 8;
        NetworkManager.singleton.matchMaker.CreateMatch(roomName, roomSize, true, "", NetworkManager.singleton.OnMatchCreate);
        Debug.LogWarning("Creating match [" + roomName + ":" + roomSize + "]");
        gameManager.SetState(gameManager.multiplayerLobbyState);
    }

    List<MatchDesc> roomList = null;

    public void OnMatchList(ListMatchResponse matchList)
    {
        if (matchList == null)
        {
            Debug.Log("null Match List returned from server");
            return;
        }

        roomList = new List<MatchDesc>();
        roomList.Clear();
        foreach (MatchDesc match in matchList.matches)
        {
            roomList.Add(match);
        }
        Debug.Log("room list after calling " + roomList.Count);
    }

    public void Refresh()
    {
        NetworkManager manager = NetworkManager.singleton;
        manager.StartMatchMaker();
        manager.matchMaker.ListMatches(0, 10, "", OnMatchList);
    }

    public void JoinGame()
    {
        MMS_JoinGameInputField = GameObject.Find("MMS_JoinGameInputField").GetComponent<InputField>();
        string roomName = MMS_JoinGameInputField.text;
        NetworkManager manager = NetworkManager.singleton;
        foreach (MatchDesc match in roomList)
        {
            if (match.name.Equals(roomName))
            {
                manager.matchMaker.JoinMatch(match.networkId, "", manager.OnMatchJoined);
                gameManager.SetState(gameManager.multiplayerLobbyState);
                break;
            }
        }
    }

    public override void ToMainMenu()
    {

        if (mmsBack != null)
        {
            mmsBack.GetComponent<MeshRenderer>().enabled = false;
        }

        gameManager.SetState(gameManager.mainMenuState);
    }
}
