using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class Timer {

    public float timeSet;
	public float timeLeft;
	
	private bool isRunning = false;

	public Timer(float val) {
        timeLeft = timeSet = val;
	}

	// Starts the timer.
	public void StartTimer() {
		isRunning = true;
	}

	// Stops the timer.
	public void StopTimer() {
		isRunning = false;
	}

	// Resets the timer (Stops timer by default)
	public void ResetTimer(float newVal) {
        timeLeft = newVal;
        StopTimer();
	}
    public void ResetTimer()
    {
        ResetTimer(timeSet);
    }

    public void ToggleTimer()
    {
        if (isRunning)
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
