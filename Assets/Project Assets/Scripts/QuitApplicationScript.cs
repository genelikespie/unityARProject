using UnityEngine;

public class QuitApplicationScript : MonoBehaviour
{
	void Update ()
	{
		if (Input.GetButtonDown("Cancel"))
			Application.Quit();
	}
}
