using UnityEngine;
using System.Collections;

public class Timer {

	public float timeLeft;

	private bool isRunning = false;

	public Timer(float val) {
		timeLeft = val;
	}

	// Starts the timer.
	public void StartTimer() {
		isRunning = true;
	}

	// Stops the timer.
	public void StopTimer() {
		isRunning = false;
	}

	// Toggles the timer on/off.
	public void ToggleTimer() {
		if(isRunning)
			StopTimer();
		else
			StartTimer();
	}

	// Checks if time has run out.
	public bool TimedOut() {
		return timeLeft == 0;
	}
	
	// Update is called once per frame
	// Runs timer.
	public void Run () {
		if(isRunning) {
			timeLeft -= Time.deltaTime;
			if(timeLeft <= 0) {
				timeLeft = 0;
				isRunning = false;
			}
		}
	}
}
