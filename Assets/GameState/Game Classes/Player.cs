using UnityEngine;
using System.Collections;

/* Base class for our players
 * Keeps track of a player's score
 * Loads any saved data for the player
 */
public class Player {

    string playerName;
    int score;

    public Player(string pName)
    {
        playerName = pName;
        score = 0;
    }
}
