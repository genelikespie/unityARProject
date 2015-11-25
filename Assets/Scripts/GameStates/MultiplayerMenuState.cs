using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using UnityEngine.Networking.Match;
using UnityEngine.Assertions;

public class MultiplayerMenuState : State {

	public GameObject networkSessionPrefab;
    GameObject mmsBack;
    // UI elements
    InputField MMS_PlanterNameInputField;
	InputField MMS_DefuserNameInputField;
    InputField MMS_GameInputField;

    Toggle MMS_TutorialToggle; // Toggles between showing tutorial or not
	Button MMS_BackButton;
	Button MMS_PlayButton;

	Slider MMS_NumOfBombsSlider;
	Text MMS_NumOfBombsText;

	protected virtual void Awake()
	{
		// Call the base class's function to initialize all variables
		base.Awake();
        Assert.raiseExceptions = true;

        mmsBack = GameObject.Find("MMS_Backdrop");
        if (mmsBack != null)
        {
            mmsBack.GetComponent<MeshRenderer>().enabled = false;
        }


        //Make sure all UI components exist
		MMS_NumOfBombsSlider = GameObject.Find("MMS_NumOfBombsSlider").GetComponent<Slider>();
        Assert.IsNotNull(MMS_NumOfBombsSlider, "MMS_NumOfBombsSlider not found");
		MMS_NumOfBombsText = GameObject.Find("MMS_NumOfBombsText").GetComponent<Text>();
        Assert.IsNotNull(MMS_NumOfBombsText, "MMS_NumOfBombsText not found");
		MMS_PlanterNameInputField = GameObject.Find("MMS_PlanterNameInputField").GetComponent<InputField>();
        Assert.IsNotNull(MMS_PlanterNameInputField, "MMS_PlanterNameInputField not found");
		MMS_DefuserNameInputField = GameObject.Find("MMS_DefuserNameInputField").GetComponent<InputField>();
        Assert.IsNotNull(MMS_DefuserNameInputField, "MMS_DefuserNameInputField not found");
	}

	/* Reset the UI
     */
	public override void Initialize()
	{
        //Make sure gameManager exists
        Assert.IsNotNull(gameManager, "Cant find game manager");

        if (mmsBack != null)
        {
            mmsBack.GetComponent<MeshRenderer>().enabled = true;
        }

		gameManager.SetNumOfBombs((int)MMS_NumOfBombsSlider.value);

	}

	public void OnValueChanged()
	{
		gameManager.SetNumOfBombs((int)MMS_NumOfBombsSlider.value);
	}

	public void DisplayNumOfBombs()
	{
		MMS_NumOfBombsText.text = "Number of Bombs: " + gameManager.getMaxBombLimit().ToString();
	}

	public override void RunState() {
		DisplayNumOfBombs();
	}


    public void CreateGame()
    {
        NetworkManager.singleton.StopHost();
        NetworkManager.singleton.StartMatchMaker();
        MMS_GameInputField = GameObject.Find("MMS_GameInputField").GetComponent<InputField>();
        string roomName = MMS_GameInputField.text;
        if (roomName != "" && MMS_PlanterNameInputField.text != "" && MMS_DefuserNameInputField.text != "")
        {
            //Make sure the name of the game isn't null
            Assert.AreNotEqual(roomName, "");
            //Make sure the name of the planter isn't null
            Assert.AreNotEqual(MMS_PlanterNameInputField.text, "");
            //Make sure the name of the defuser isn't null
            Assert.AreNotEqual(MMS_DefuserNameInputField.text, "");
            uint roomSize = 8;
            NetworkManager.singleton.matchMaker.CreateMatch(roomName, roomSize, true, "", NetworkManager.singleton.OnMatchCreate);
            Debug.LogWarning("Creating match [" + roomName + ":" + roomSize + "]");
            gameManager.SetState(gameManager.multiplayerLobbyState);
        }
    }

    

    public void OnMatchList(ListMatchResponse matchList)
    {
        //make sure match list is not null
        Assert.IsNotNull(matchList, "null Match List returned from server");
        /*
        if (matchList == null)
        {
            Debug.Log("null Match List returned from server");
            return;
        }
        */
        // The naming is NOT a bug. The MMS_JoinGameInputField has been removed.
        MMS_GameInputField = GameObject.Find("MMS_GameInputField").GetComponent<InputField>();
        string roomName = MMS_GameInputField.text;
        if (roomName != "" && MMS_PlanterNameInputField.text != "" && MMS_DefuserNameInputField.text != "")
        {
            //Make sure the name of the game isn't null
            Assert.AreNotEqual(roomName, "");
            //Make sure the name of the planter isn't null
            Assert.AreNotEqual(MMS_PlanterNameInputField.text, "");
            //Make sure the name of the defuser isn't null
            Assert.AreNotEqual(MMS_DefuserNameInputField.text, "");

            NetworkManager manager = NetworkManager.singleton;

            foreach (MatchDesc match in matchList.matches)
            {
                if (match.name.Equals(roomName))
                {
                    manager.matchMaker.JoinMatch(match.networkId, "", manager.OnMatchJoined);
                    gameManager.SetState(gameManager.multiplayerLobbyState);
                    Debug.Log("Match " + roomName + "Found");
                    return;
                }
            }
            MMS_GameInputField.text = roomName + " Not Found";
            Debug.Log("Match " + roomName + "Not Found");
        }
    }

    public void OnMatchList2(ListMatchResponse matchList)
    {
        if (matchList == null)
        {
            Debug.Log("null Match List returned from server");
            return;
        }
        // The naming is NOT a bug. The MMS_JoinGameInputField has been removed.
        MMS_GameInputField = GameObject.Find("MMS_GameInputField").GetComponent<InputField>();

        //Make sure the name of the game isn't null
        Assert.AreNotEqual(matchList.matches.Count, 0);

        /*
        if (matchList.matches.Count == 0)
        {
            Debug.Log("Match List Empty");
            MMS_GameInputField.text = "Match List Empty";
            return;
        }
        */
        string roomName = MMS_GameInputField.text;
        NetworkManager manager = NetworkManager.singleton;

        Assert.IsNotNull(manager, "NetworkManager not found");

        int position = Random.Range(0, matchList.matches.Count - 1);
        MMS_GameInputField.text = matchList.matches[position].name;

        Assert.AreNotEqual(MMS_GameInputField.text, "");
    }

    public void Refresh()
    {
        NetworkManager manager = NetworkManager.singleton;
        Assert.IsNotNull(manager, "NetworkManager not found");
        manager.StartMatchMaker();
        manager.matchMaker.ListMatches(0, 100, "", OnMatchList2);
    }

    public void JoinGame()
    {
        NetworkManager.singleton.StopHost();
        NetworkManager manager = NetworkManager.singleton;
        Assert.IsNotNull(manager, "NetworkManager not found");
        manager.StartMatchMaker();
        manager.matchMaker.ListMatches(0, 100, "", OnMatchList);
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
