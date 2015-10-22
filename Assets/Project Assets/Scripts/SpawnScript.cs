using UnityEngine;

public class SpawnScript : MonoBehaviour
{
	public GameObject enemy;
	public float spawnInterval = 8f;

	float spawnVariance;

	void Start () 
	{
		spawnVariance = spawnInterval * .5f;

		Invoke ("Spawn", spawnInterval + Random.Range(-spawnVariance, spawnVariance));
	}

	void Update()
	{
		if (spawnInterval > 1f)
		{
			//every 50 seconds of gameplay reduces the timer by 1 second
			float timeReduction = Time.deltaTime / 50;

			spawnInterval = Mathf.Max(1f, spawnInterval - timeReduction);
			spawnVariance = spawnInterval * .5f;
		}
	}

	void Spawn()
	{
        GameObject enemyObj = Instantiate (enemy, transform.position, transform.rotation) as GameObject;
        enemyObj.transform.parent = transform;

		Invoke("Spawn", spawnInterval + Random.Range(-spawnVariance, spawnVariance));
	}
}
