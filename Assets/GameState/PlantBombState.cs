using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlantBombState : State {

    /* Initialize the plant bomb state
     * Pass in the planters, defusers and number of bombs
     */

    List<Player> planters;
    List<Player> defusers;
    int numOfBombs;
    float timeToPlant;

    public void Initialize(List<Player> planters, List<Player> defusers, int numOfBombs, float timeToPlant)
    {

    }
}
