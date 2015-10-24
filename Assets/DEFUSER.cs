using UnityEngine;
using System.Collections;

public class DEFUSER : MonoBehaviour {

    public Material redtext;
    public float timer;
    public float initialTimer;
    public GameObject[] bombs;

    void Start () {
        bombs = GameObject.FindGameObjectsWithTag("Bomb"); //find bomb tag
    }
	
	// Update is called once per frame
	void Update () {
        if (timer > 0) //when the bomb is still able to blow
        {
            if (GameObject.Find("DefusingBOMB").GetComponent<BombState>().BOMBSTATE == true)
            { //bomb is active
                timer = timer -= Time.deltaTime; //reduce time to kill
                Debug.Log("decrementing timer: " + timer);
            }
            else
            {
                timer = initialTimer; //lost track of bomb reset bomb to initial
                bombs = GameObject.FindGameObjectsWithTag("Bomb"); //find the bombs again
            }
        }
        else
            foreach(GameObject x in bombs) //need to fix this but essentially get all the bombs and change the texture
                x.GetComponent<Renderer>().material = redtext;
    }
}
