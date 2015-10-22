using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TowerHealth : MonoBehaviour
{
	public int numberOfLives = 3;

    int currentLives;
	AudioSource damageAudio;
    Image damageImage;
	bool alive = true;

    void Awake()
	{
        currentLives = numberOfLives;
		damageAudio = GetComponent<AudioSource>();
        damageImage = GameObject.Find("Image").GetComponent<Image>();
    }

	void OnTriggerEnter(Collider other)
	{
		if (other.tag != "Enemy" || !alive)
			return;

		Destroy(other.gameObject);
        currentLives -= 1;
		damageAudio.Play();

		if(currentLives <= 0)
		{
			alive = false;
            if (damageImage)
            {
                Color col = damageImage.color;
                col.a = 1f;
                damageImage.color = col;
            }

			Invoke("Restart", 3f);
		}
	}

	void Restart()
	{
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        for (int i = 0; i < enemies.Length; i++)
            Destroy(enemies[i]);

        currentLives = numberOfLives;
        alive = true;

        if (damageImage)
        {
            Color col = damageImage.color;
            col.a = 0f;
            damageImage.color = col;
        }
    }
}
