using UnityEngine;
using System.Collections;

/* Base class for our players
 * Keeps track of a player's score
 * Loads any saved data for the player
 */
public class Player {

    string planterName;
	string defuserName;
	int score;

    public Player(string pName, string dName)
    {
        planterName = pName;
		defuserName = dName;
        score = 0;
    }
}
