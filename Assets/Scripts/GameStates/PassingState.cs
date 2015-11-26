using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Assertions;
using System.Collections;

public class PassingState : State {

    // UI
    Text P_TimeLeftText;
    Button P_PassedToDefuserButton;
	Text P_Waiting;

    public virtual void Awake()
    {
        // Call the base class's function to initialize all variables
        base.Awake();

        // Find all UI elements in the scene
        P_TimeLeftText = GameObject.Find("P_TimeLeftText").GetComponent<Text>();
        P_PassedToDefuserButton = GameObject.Find("P_PassedToDefuserButton").GetComponent<Button>();
		P_Waiting = GameObject.Find ("P_Waiting").GetComponent<Text>();

        if (!P_TimeLeftText)
            Debug.LogError("P_TimeLeftText");
        if (!P_PassedToDefuserButton)
            Debug.LogError("P_PassedToDefuserButton");
		if (!P_Waiting)
			Debug.LogError("P_Waiting");
    }

	public override void Initialize() {
		// Initialize to 30 seconds to pass phone to defuser
		gameManager.passTimer.StartTimer();

		P_Waiting.gameObject.SetActive(false);

		Assert.IsNotNull<string>(player.getDefuserName());
		Assert.AreEqual(0, player.getLocalBombsDefused());
		Assert.AreEqual(player.getMaxLocalBombs(), player.getLocalBombsPlanted());
		Assert.AreNotEqual(0, player.getMaxLocalBombs());
		Assert.IsNotNull<string>(player.getPlanterName());
		Assert.IsFalse(player.isAllGlobalBombsDefused());
		Assert.IsTrue(player.isAllGlobalBombsPlanted());
		Assert.IsFalse(player.isAllLocalBombsDefused());
		Assert.IsTrue(player.isAllLocalBombsPlanted());
	}

    public override void RunState()
    {
        // Update the timer UI
		P_TimeLeftText.text = string.Format("{0:N1}", gameManager.passTimer.timeLeft);

        // If time runs out and we have not changed state to DefuseBomb(), planter loses
        /////////////////////////////////////////////////
        // TODO implement time expired
        /////////////////////////////////////////////////
		/// 
		if(gameManager.passTimer.TimedOut()) {
			P_Waiting.gameObject.SetActive(false);
			player.setPlayerOneWins(false);
			gameManager.SetState(gameManager.gameOverState);
		}
        else if (player.isPassReady() && !player.isAllPassReady())
        {
            P_Waiting.gameObject.SetActive(true);
        }
		else if (player.isAllPassReady())
        {
			gameManager.passTimer.StopTimer();
			gameManager.SetState(gameManager.defuseState);
        }
    }

    public override void DefuseBomb()
    {
		player.setPassReady(true);
    }
}
