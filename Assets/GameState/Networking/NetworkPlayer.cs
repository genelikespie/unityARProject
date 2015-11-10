using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

// NOTE: Player object should still be created in multiplayer.
// NetworkPlayer pulls data from it (from gameManager.localPlayer) on creation.

public class NetworkPlayer : NetworkBehaviour{

    // All variables with [SyncVar] attribute are synchronized
    // from server to client. (not the other way around)

    // Because of this, these variables CANNOT be set directly.
    // They can only be set through functions marked [Command].
    // However, they can still be read.

    [SyncVar]
    public bool ready = false;

    [SyncVar]
	string planterName;
	
	[SyncVar]
	string defuserName;
	
	[SyncVar]
	int score;
	
	[SyncVar]
	public int numLocalBombs = 0;

	// These are for bombs local to the device.
	[SyncVar]
	public bool allLocalBombsPlanted = false;
	[SyncVar]
	public bool allLocalBombsFound = false;
	
	[SyncVar]
	public bool allNetworkBombsPlanted;
	[SyncVar]
	public bool allNetworkBombsFound;

	void OnStartLocalPlayer() {
		GameManager gm = GameObject.FindObjectOfType<GameManager>();
		if(gm != null)
			gm.networkPlayer = this;
		else
			Debug.Log ("When creating local NetworkPlayer, GameManager could not be found.");
	}
    
    // Do not call this, it is called by BombNetworkManager.
    // Used to initialize NetworkPlayer on creation.
    public void InitPlayer(string pName, string dName, int numBombs)
	{
		planterName = pName;
		defuserName = dName;
		numLocalBombs = numBombs;
		score = 0;
		allNetworkBombsFound = false;
		allNetworkBombsPlanted = false;
	}

    // All [Command] functions are called on the server.
    // TODO: Command functions for modifying score and number of bombs.


    // Sets the boolean indicating if the player is ready do start plant bombs.
    // If true, checks if everyone else is done as well.
    [Command]
    public void CmdSetReady()
    {
        ready = true;
    }

    // Sets the boolean indicating if all local bombs are planted.
    // If true, checks if everyone else is done as well.
    [Command]
	public void CmdSetPlanted(bool val) {
		allLocalBombsPlanted = val;
		if(val)
			CmdCheckDonePlanting();		
	}

	// Sets the boolean indicating if all local bombs are found.
	// If true, checks if everyone else is done as well.
	[Command]
	public void CmdSetFound(bool val) {
		allLocalBombsFound = val;
		if(val)
			CmdCheckDoneDefusing();
	}

	// Checks if everyone is done planting bombs, setting the
	// allNetworkBombsPlanted boolean to true if so.
	// In the state machine, check that variable instead of using this function.
	[Command]
	private void CmdCheckDonePlanting() {
		if(AllDonePlanting()) {
			foreach(NetworkPlayer p in GameObject.FindObjectsOfType<NetworkPlayer>())
				p.allNetworkBombsPlanted = true;
		}
	}

	// Checks if everyone is done defusing bombs, setting the
	// allNetworkBombsFound boolean to true if so.
	// In the state machine, check that variable instead of using this function.
	[Command]
	private void CmdCheckDoneDefusing() {
		if(AllDoneSearching()) {
			foreach(NetworkPlayer p in GameObject.FindObjectsOfType<NetworkPlayer>())
				p.allNetworkBombsFound = true;
		}
	}

	// Checks if all players are done planting.
	private bool AllDonePlanting() {
		bool allDone = true;
		foreach(NetworkPlayer p in GameObject.FindObjectsOfType<NetworkPlayer>())
			allDone = allDone && p.allLocalBombsPlanted;
		return allDone;
	}

	// Checks if all players are done searching.
	private bool AllDoneSearching() {
		bool allDone = true;
		foreach(NetworkPlayer p in GameObject.FindObjectsOfType<NetworkPlayer>())
			allDone = allDone && p.allLocalBombsFound;
		return allDone;
	}
}
