//using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// TODO: 
// Spawn logic
// Destroy and points logic
// Object pooling
// Victory and defeat logic

public class LevelManager : Singleton<LevelManager>
{
	GameManager gameManager;

	public LevelDataSO levelData;

	private List<GameObject> targets;

	private float maxTorque = 10.0f;
	private float minSpeed = 11.0f;
	private float maxSpeed = 14.0f;


	void Start()
	{
		gameManager = GameManager.Instance;

		targets = levelData.Targets;

		StartCoroutine(SpawnTarget());
	}

	IEnumerator SpawnTarget()
	{
		while (gameManager.CurrentGameState == GameManager.GameState.GAMEPLAY)
		{
			yield return new WaitForSeconds(levelData.SpawnRate);
			
			int targetIndex = Random.Range(0, targets.Count);
			CreateTarget(targets[targetIndex]);
		}
	}

	void CreateTarget(GameObject targetObject)
	{
		Vector3 position = new Vector3(Random.Range(levelData.MinXSpawnPosition, levelData.MaxXSpawnPosition),
									   Random.Range(levelData.MinYSpawnPosition, levelData.MaxYSpawnPosition),
									   0);

		GameObject target = Instantiate(targetObject, position, transform.rotation);
		
		Rigidbody targetRb = target.GetComponent<Rigidbody>();

		targetRb.AddTorque(RandomTorque(), RandomTorque(), RandomTorque(), ForceMode.Impulse);

		targetRb.AddForce(RandomForce(), ForceMode.Impulse);

	}

	Vector3 RandomForce()
	{
		return Vector3.up * Random.Range(minSpeed, maxSpeed);
	}

	float RandomTorque()
	{
		return Random.Range(-maxTorque, maxTorque);
	}

	void Update()
	{

		// Victory condition
		if (Input.GetKeyDown(KeyCode.W))
		{
			Debug.Log("You win!");
			gameManager.UpdateState(GameManager.GameState.ENDLEVEL);
		}

		// Defeat condition
		if (Input.GetKeyDown(KeyCode.L))
		{
			Debug.Log("You lost.");
			gameManager.UpdateState(GameManager.GameState.ENDLEVEL);
		}

	}
}
