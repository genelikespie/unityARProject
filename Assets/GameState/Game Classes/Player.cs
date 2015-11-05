using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

/* Base class for our players
 * Keeps track of a player's score
 * Loads any saved data for the player
 */

// TODO: SyncVars only work from server to client.
// Use Commands instead to indicate changes in numLocalBombs and other state booleans.

public class Player {
	
    public string planterName;
	public string defuserName;
	
	public int score;
	
	public int numLocalBombs;
	public bool allLocalBombsPlanted;
	public bool allLocalBombsFound;
	
	public bool playerOneWins;


    public Player(string pName, string dName, int numBombs)
    {
        planterName = pName;
		defuserName = dName;
		numLocalBombs = numBombs;
        score = 0;
    }
}
