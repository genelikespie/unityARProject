using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;

// TODO: Create NetworkSession and LocalSession

public class Session {
	// Keep track of game information
	private List<Player> playerDevices;
	public bool isMultiplayer {
		get { return playerDevices.Count > 1; }
	}
	
	[SyncVar]
	public bool playerOneWins;

	public Player AddPlayer(string planterName, string defuserName, int numLocalBombs = 1) {
		Player newPlayer = new Player(planterName, defuserName, numLocalBombs);
		playerDevices.Add(newPlayer);
		return newPlayer;
	}

    //if in shared mode, use the following functions to get the names of the planter
    //and defuser to display in the game over menu
    public string GetPlanterName()
    {
        return playerDevices[0].GetPlanterName();
    }

    public string GetDefuserName()
    {
        return playerDevices[0].GetDefuserName();
    }

	public bool AllDonePlanting() {
		bool allDone = true;
		foreach(Player p in playerDevices)
			allDone = allDone && p.allLocalBombsPlanted;
		return allDone;
	}

	public bool AllDoneSearching() {
		bool allDone = true;
		foreach(Player p in playerDevices)
			allDone = allDone && p.allLocalBombsFound;
		return allDone;
	}

	public Session() {
		playerDevices = new List<Player>();
	}
}