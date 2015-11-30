using UnityEngine;
using System.Collections;

public class MainMenuState : State {
	GameObject mmBack;

	public virtual void Awake() {
		mmBack = GameObject.Find("MM_Backdrop");
        if (mmBack != null)
        {
            mmBack.SetActive(false);
        }
        else
        {
            Debug.Log("MM_Backdrop not found. = NULL");
        }

        base.Awake();
    }

	public override void Initialize ()
	{
        if (mmBack != null)
        {
            Debug.Log("mmBack not null");
            mmBack.SetActive(true);
        }
        else
        {
            Debug.Log("mmBack IS null");
        }
        GameObject.Find("BackgroundPlane").GetComponent<MeshRenderer>().enabled = false;
	}

	public override void RunState() {
        if (mmBack != null)
        {
            mmBack.SetActive(true);
        }
    }

	public override void ToSharedModeMenu()
	{
		Debug.Log("To shared Menu");
        if (mmBack != null)
        {
            mmBack.SetActive(false);
        }
        gameManager.SetState(gameManager.sharedModeMenuState);
	}
}
