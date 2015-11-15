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
	public bool passReady = false;

	[SyncVar]
	public bool allPassReady = false;

    [SyncVar]
	public string planterName;
	
	[SyncVar]
	public string defuserName;
	
	[SyncVar]
	public int score;
	
	[SyncVar]
	public int maxLocalBombs = 0;

	[SyncVar]
	public int localBombsDefused = 0;

	[SyncVar]
	public int localBombsPlanted = 0;

	[SyncVar]
	public bool allNetworkBombsPlanted;
	[SyncVar]
	public bool allNetworkBombsDefused;

	public bool allLocalBombsPlanted() {
		return localBombsPlanted == maxLocalBombs;
	}

	public bool allLocalBombsDefused() {
		return localBombsDefused == maxLocalBombs;
	}

	public override void OnStartLocalPlayer() {
		GameManager gm = GameObject.FindObjectOfType<GameManager>();
		if(gm != null)
			gm.player = new NetworkPlayerAdapter(this);
		else
			Debug.Log ("When creating local NetworkPlayer, GameManager could not be Defused.");
	}
    
    // Do not call this, it is called by BombNetworkManager.
    // Used to initialize NetworkPlayer on creation.
	
    public void InitPlayer(string pName, string dName, int numBombs)
	{
		planterName = pName;
		defuserName = dName;
		maxLocalBombs = numBombs;
		score = 0;
		allNetworkBombsDefused = false;
		allNetworkBombsPlanted = false;
	}

    // All [Command] functions are called on the server.

    // Sets the boolean indicating if the player is ready do start plant bombs.
    [Command]
    public void CmdSetReady(bool val)
    {
        ready = val;
    }

	[Command]
	public void CmdSetPassReady(bool val)
	{
		passReady = val;
		if(passReady)
			CmdCheckAllPassReady();
	}

	// Other basic setters.
	[Command]
	public void CmdSetScore(int val) {
		score = val;
	}

	[Command]
	public void CmdSetMaxLocalBombs(int val) {
		maxLocalBombs = val;
	}

	[Command]
	public void CmdSetLocalBombsPlanted(int val) {
		localBombsPlanted = val;
		if(allLocalBombsPlanted())
			CmdCheckDonePlanting();
	}

	[Command]
	public void CmdSetLocalBombsDefused(int val) {
		localBombsDefused = val;
		if(allLocalBombsDefused())
			CmdCheckDoneDefusing();
	}

	[Command]
	public void CmdSetPlanterName(string val) {
		planterName = val;
	}
	
	[Command]
	public void CmdSetDefuserName(string val) {
		defuserName = val;
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
	// allNetworkBombsDefused boolean to true if so.
	// In the state machine, check that variable instead of using this function.
	[Command]
	private void CmdCheckDoneDefusing() {
		if(AllDoneSearching()) {
			foreach(NetworkPlayer p in GameObject.FindObjectsOfType<NetworkPlayer>())
				p.allNetworkBombsDefused = true;
		}
	}

	[Command]
	private void CmdCheckAllPassReady() {
		if(AllPassReady()) {
			foreach(NetworkPlayer p in GameObject.FindObjectsOfType<NetworkPlayer>()) 
				p.allPassReady = true;
		}
	}

	// Checks if all players are done planting.
	private bool AllDonePlanting() {
		bool allDone = true;
		foreach(NetworkPlayer p in GameObject.FindObjectsOfType<NetworkPlayer>())
			allDone = allDone && p.allLocalBombsPlanted();
		return allDone;
	}

	// Checks if all players are done searching.
	private bool AllDoneSearching() {
		bool allDone = true;
		foreach(NetworkPlayer p in GameObject.FindObjectsOfType<NetworkPlayer>())
			allDone = allDone && p.allLocalBombsDefused();
		return allDone;
	}

	private bool AllPassReady() {
		bool allDone = true;
		foreach(NetworkPlayer p in GameObject.FindObjectsOfType<NetworkPlayer>())
			allDone = allDone && p.passReady;
		return allDone;
	}
}
