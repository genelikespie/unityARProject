using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using UnityEngine.Networking.Match;

public class MultiplayerMenuState : State {

	public GameObject networkSessionPrefab;

	// UI elements
	InputField MMS_PlanterNameInputField;
	InputField MMS_DefuserNameInputField;
    InputField MMS_CreateGameInputField;
    InputField MMS_JoinGameInputField;

    Toggle MMS_TutorialToggle; // Toggles between showing tutorial or not
	Button MMS_BackButton;
	Button MMS_PlayButton;

	GameObject smBack;

	protected virtual void Awake()
	{
		// Call the base class's function to initialize all variables
		base.Awake();
		if (!gameManager)
			Debug.LogError("AWAKE: CANT find game manager in base");

		// Get SM_Backdrop and disable renderer
		smBack = GameObject.Find("SM_Backdrop");
		smBack.GetComponent<MeshRenderer>().enabled = false;
	}

	/* Reset the UI
     */
	public override void Initialize()
	{
		if (!gameManager)
			Debug.LogError("Cant find game manager");
		// Enable SM_Backdrop renderer
		smBack.GetComponent<MeshRenderer>().enabled = true;
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

    public override void PlantBomb()
	{
		if (MMS_PlanterNameInputField.text == "" || MMS_DefuserNameInputField.text == "")
		{
			// TODO throw a modal panel or some message to the screen/camera
			return;
		}

		gameManager.tempDefuserName = MMS_DefuserNameInputField.text;
		gameManager.tempPlanterName = MMS_PlanterNameInputField.text;


		// TODO: Right now the screen results in nonfunctional buttons.
		// Implement matchmaker here.

		gameManager.SetAR();

		//Disable SM_Backdrop renderer, enabled camera plane
		smBack.GetComponent<MeshRenderer>().enabled = false;
		GameObject.Find("BackgroundPlane").GetComponent<MeshRenderer>().enabled = true;
		
		gameManager.SetState(gameManager.plantBombState);
	}

	
}
