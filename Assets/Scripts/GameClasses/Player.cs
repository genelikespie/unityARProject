using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

/* Base class for our players
 * Keeps track of a player's score
 * Loads any saved data for the player
 */

// TODO: SyncVars only work from server to client.
// Use Commands instead to indicate changes in numLocalBombs and other state booleans.

public class LocalPlayer {
	
    public string planterName;
	public string defuserName;
	
	public int score;
	
	public int maxLocalBombs;
	public int localBombsDefused = 0;
	public int localBombsPlanted = 0;

	public bool playerOneWins;
	public bool passReady = false;


    public LocalPlayer(string pName, string dName, int maxBombs)
    {
        planterName = pName;
		defuserName = dName;
		maxLocalBombs = maxBombs;
        score = 0;
    }
}

public interface PlayerAdapter {

	// Getters (common)
	string getPlanterName();
	string getDefuserName();

	int getScore();
	int getMaxLocalBombs();
	int getLocalBombsPlanted();
	int getLocalBombsDefused();

	bool isAllLocalBombsPlanted();
	bool isAllLocalBombsDefused();
	bool getPlayerOneWins();

	// Getters (relevant for networking)
	bool isReady();
	bool isPassReady();
	bool isAllPassReady();
	bool isAllGlobalBombsPlanted();
	bool isAllGlobalBombsDefused();

	// Setters (common)
	void setPlanterName(string val);
	void setDefuserName(string val);

	void setScore(int val);
	void setMaxLocalBombs(int val);
	void setLocalBombsPlanted(int val);
	void setLocalBombsDefused(int val);
	
	void setPlayerOneWins(bool val);

	// Setters (relevant to networking)
	void setReady(bool val);
	void setPassReady(bool val);

	// Note: No setters for AllGlobalBombs booleans.
	// This is because they only really factors into network games,
	// where these variables are handled behind the scenes.

	bool isMultiplayer();
}

public class LocalPlayerAdapter : PlayerAdapter {
	LocalPlayer player;

	public LocalPlayerAdapter(LocalPlayer p) {
		player = p;
	}

	public string getPlanterName() { return player.planterName; }
	public string getDefuserName() { return player.defuserName; }

	public int getScore() { return player.score; }
	public int getMaxLocalBombs() { return player.maxLocalBombs; }
	public int getLocalBombsPlanted() { return player.localBombsPlanted; }
	public int getLocalBombsDefused() { return player.localBombsDefused; }

	public bool isAllLocalBombsPlanted() { 
		return player.localBombsPlanted == player.maxLocalBombs; }
	public bool isAllLocalBombsDefused() { 
		return player.localBombsDefused == player.maxLocalBombs; }
	public bool getPlayerOneWins() { return player.playerOneWins; }

	// is this correct for singleplayer?
	public bool isReady() { return true; }
	public bool isPassReady() { return player.passReady; }
	public bool isAllPassReady() { return isPassReady(); }
	// for singleplayer, local/global mean the same thing.
	public bool isAllGlobalBombsPlanted() { return isAllLocalBombsPlanted(); } 
	public bool isAllGlobalBombsDefused() { return isAllLocalBombsDefused(); }

	public void setPlanterName(string val) { player.planterName = val; }
	public void setDefuserName(string val) { player.defuserName = val; }

	public void setScore(int val) { player.score = val; }
	public void setMaxLocalBombs(int val) { player.maxLocalBombs = val; }
	public void setLocalBombsPlanted(int val) { player.localBombsPlanted = val; }
	public void setLocalBombsDefused(int val) { player.localBombsDefused = val; }
	
	public void setPlayerOneWins(bool val) { player.playerOneWins = val; }

	// is this correct for singeplayer?
	public void setReady(bool val) { return; } 
	public void setPassReady(bool val) { player.passReady = val; }

	public bool isMultiplayer() { return false; }
}

public class NetworkPlayerAdapter : PlayerAdapter {
	NetworkPlayer player;
	public NetworkPlayerAdapter(NetworkPlayer p) {
		player = p;
	}
	
	public string getPlanterName() { return player.planterName; }
	public string getDefuserName() { return player.defuserName; }

	public int getScore() { return player.score; }
	public int getMaxLocalBombs() { return player.maxLocalBombs; }
	public int getLocalBombsPlanted() { return player.localBombsPlanted; }
	public int getLocalBombsDefused() { return player.localBombsDefused; }

	public bool isAllLocalBombsPlanted() { return player.allLocalBombsPlanted(); }
	public bool isAllLocalBombsDefused() { return player.allLocalBombsDefused(); }
	public bool getPlayerOneWins() { return player.getPlayerOneWins(); }

	public bool isReady() { return player.ready; }
	public bool isPassReady() { return player.passReady; }
	public bool isAllPassReady() { return player.allPassReady; }
	public bool isAllGlobalBombsPlanted() { return player.allNetworkBombsPlanted; } 
	public bool isAllGlobalBombsDefused() { return player.allNetworkBombsDefused; }
	
	public void setPlanterName(string val) { player.CmdSetPlanterName(val); }
	public void setDefuserName(string val) { player.CmdSetDefuserName(val); }

	public void setScore(int val) { player.CmdSetScore(val); }
	public void setMaxLocalBombs(int val) { player.CmdSetMaxLocalBombs(val); }
	public void setLocalBombsPlanted(int val) { player.CmdSetLocalBombsPlanted(val); }
	public void setLocalBombsDefused(int val) { player.CmdSetLocalBombsDefused(val); }
	
	public void setPlayerOneWins(bool val) {
		player.CmdSetTeamOneWins(val);
	}

	public void setReady(bool val) { player.CmdSetReady(val); } 
	public void setPassReady(bool val) { player.CmdSetPassReady(val); }

	public bool isMultiplayer() { return true; }
}

