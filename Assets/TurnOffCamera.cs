using UnityEngine;
using System.Collections;

public class TurnOffCamera : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        GameObject camPlane = GameObject.Find("BackgroundPlane");
        GameObject mm = GameObject.Find("mainMenuState");
        GameObject sm = GameObject.Find("sharedModeMenuState");
        GameObject tm = GameObject.Find("tutorialMenuState");
        GameObject go = GameObject.Find("gameOverState");

        GameObject mmBack = GameObject.Find("MM_Backdrop");
        GameObject smBack = GameObject.Find("SM_Backdrop");

        bool isMM = mm.GetComponent<MainMenuState>().isCurrentState;
        bool isSM = sm.GetComponent<SharedModeMenuState>().isCurrentState;
        bool isTM = tm.GetComponent<TutorialMenuState>().isCurrentState;
        bool isGO = go.GetComponent<GameOverState>().isCurrentState;

        //bool childrenActive = true;
        mmBack.GetComponent<MeshRenderer>().enabled = true;
        smBack.GetComponent<MeshRenderer>().enabled = true;

        // Turn off camera's background video plane during above states
        if (isMM || isSM || isTM || isGO)
        {
            camPlane.GetComponent<MeshRenderer>().enabled = false;

            if(isMM)
            {
                smBack.GetComponent<MeshRenderer>().enabled = false;
            }
            else if (isSM)
            {
                mmBack.GetComponent<MeshRenderer>().enabled = false;
            }

            //Debug.Log("Deactivated children");
            //childrenActive = false;
        }
        else // if(!childrenActive)
        {
            mmBack.GetComponent<MeshRenderer>().enabled = false;
            smBack.GetComponent<MeshRenderer>().enabled = false;

            camPlane.GetComponent<MeshRenderer>().enabled = true;

            //Debug.Log("Activated children");
            //childrenActive = true;
        }

    }
}
