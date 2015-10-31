using UnityEngine;
using System.Collections;

public class DEFUSER : MonoBehaviour {

    public Material redtext;
    public float timer;
    public float initialTimer;
    public GameObject[] bombs;
    GameManager gameManager;


    void Start () {
        bombs = GameObject.FindGameObjectsWithTag("Bomb"); //find bomb tag
        gameManager = GameManager.Instance();
    }
    /*
    // Update is called once per frame
    void Update () {
        if (timer > 0) //when the bomb is still able to blow
        {
          //  if (GameObject.Find("DefusingBOMB").GetComponent<BombState>().BOMBSTATE == true && GameObject.Find("defuseState").GetComponent<DefuseState>().isCurrentState == true) //bomb is active
            //    if (GameObject.Find("GameManager").GetComponent<GameManager>().bombVisible == true && GameObject.Find("defuseState").GetComponent<DefuseState>().isCurrentState == true) //bomb is active
            if (gameManager.bombVisible == true && GameObject.Find("defuseState").GetComponent<DefuseState>().isCurrentState == true)
                timer = timer -= Time.deltaTime; //reduce time to kill
            else
            {
                timer = initialTimer; //lost track of bomb reset bomb to initial
            }
        }
        else
        {
            bombs = GameObject.FindGameObjectsWithTag("Bomb"); //find the bombs again
            foreach (GameObject x in bombs) //need to fix this but essentially get all the bombs and change the texture
                x.GetComponent<Renderer>().material = redtext;
        }
    }*/


    
// Update is called once per frame
void Update () {
    if (timer > 0) //when the bomb is still able to blow
    {
      //  if (GameObject.Find("DefusingBOMB").GetComponent<BombState>().BOMBSTATE == true && GameObject.Find("defuseState").GetComponent<DefuseState>().isCurrentState == true) //bomb is active
        //    if (GameObject.Find("GameManager").GetComponent<GameManager>().bombVisible == true && GameObject.Find("defuseState").GetComponent<DefuseState>().isCurrentState == true) //bomb is active
        if (gameManager.bombVisible == true && GameObject.Find("defuseState").GetComponent<DefuseState>().isCurrentState == true)
            timer = timer -= Time.deltaTime; //reduce time to kill
        else
        {
            timer = initialTimer; //lost track of bomb reset bomb to initial
        }
    }
    else
    {
        bombs = GameObject.FindGameObjectsWithTag("Bomb"); //find the bombs again
        foreach (GameObject x in bombs) //need to fix this but essentially get all the bombs and change the texture
            x.GetComponent<Renderer>().material = redtext;
    }
    }
}
