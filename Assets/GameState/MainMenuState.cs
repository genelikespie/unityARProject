using UnityEngine;
using System.Collections;

public class MainMenuState : State {
	GameObject mmBack;

	public virtual void Awake() {
		base.Awake();

		mmBack = GameObject.Find("MM_Backdrop");
		mmBack.GetComponent<MeshRenderer>().enabled = false;
	}

	public override void Initialize ()
	{
		mmBack.GetComponent<MeshRenderer>().enabled = true;
		GameObject.Find("BackgroundPlane").GetComponent<MeshRenderer>().enabled = false;
	}

	public override void ToSharedModeMenu()
	{
		Debug.Log("To shared Menu");
		mmBack.GetComponent<MeshRenderer>().enabled = false;
		gameManager.SetState(gameManager.sharedModeMenuState);
	}
}
