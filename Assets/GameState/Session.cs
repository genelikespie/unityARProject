using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;

// TODO: Create NetworkSession and LocalSession

public class Session {
	// Keep track of game information
	public List<Player> playerDevices;
	
	// These variables are initialized in SharedModeMenu
	public int numOfBombs;
	public Timer plantTimer;
	public Timer defuseTimer;
	public Timer passTimer;
	
	public bool bombPlanted = false;
	public bool playerOneWins;

	public void updateTimers() {
		plantTimer.Run();
		defuseTimer.Run();
		passTimer.Run();
	}

	public Session() {
		playerDevices = new List<Player>();
		plantTimer = new Timer(42);
		defuseTimer = new Timer(42);
		passTimer = new Timer(42);
	}
}