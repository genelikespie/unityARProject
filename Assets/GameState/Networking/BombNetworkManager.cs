using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class BombNetworkManager : NetworkManager {

	GameManager gameManager;

	void Start() {
		gameManager = FindObjectOfType<GameManager>();
	}

	// In the menu where names were entered, a Player object was created.
	// This function pulls data from that object when creating a NetworkPlayer.
	public override void OnServerAddPlayer(NetworkConnection conn, short playerControllerId) {
		GameObject playerObj = ((GameObject)GameObject.Instantiate(playerPrefab, Vector3.zero, Quaternion.identity));
		NetworkPlayer netPlayer = playerObj.GetComponent<NetworkPlayer>();

		netPlayer.InitPlayer(gameManager.localPlayer.planterName,
		                     gameManager.localPlayer.defuserName,
		                     gameManager.localPlayer.numLocalBombs);
		NetworkServer.AddPlayerForConnection(conn, playerObj, playerControllerId);

	}
}