﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;


public class LevelManager : Singleton<LevelManager>
{
	GameManager gameManager;
	ObjectPooler objectPooler;

	// Scriptable object contained the main data of current level
	public LevelDataSO levelData;

	// Player data
	public PlayerSO playerInfo;

	// List of target object that populate from LevelData SO
	private List<GameObject> targets;

	// Scoring
	int currentScore;
	int maxLevelScore;

	// Event invoked after every score updating
	[HideInInspector] public GameEvents.IntParameterEvent OnScoreChangesEvent;

	// Event invoked after Level Data loaded from scriptable object
	[HideInInspector] public UnityEvent OnLevelDataLoadedEvent;
	

	void Start()
	{
		DontDestroyOnLoad(gameObject);

		gameManager = GameManager.Instance;
		objectPooler = ObjectPooler.Instance;

		gameManager.OnSceneLoaded.AddListener(LevelLoadedHandler);
	}

	// Loading LevelData scriptable object of new level from LevelData scriptable object. Final scene excepted.
	private void LevelLoadedHandler(string levelName)
	{
		if (gameManager.CurrentGameState != GameManager.GameState.FINAL)
		{
			string levelDataAddress = "LevelData/" + levelName;
			Addressables.LoadAssetAsync<LevelDataSO>(levelDataAddress).Completed += OnLoadDone;
		}
	}

	// After LevelData SO was loaded, the method checks the data is not null, and than init level parameters and start spawning.
	private void OnLoadDone(AsyncOperationHandle<LevelDataSO> obj)
	{
		if (obj.Result == null)
		{
			Debug.LogError("[Addressables] Load result is null.");
			return;
		}

		levelData = obj.Result;
		OnLevelDataLoadedEvent.Invoke();

		currentScore = levelData.StartScore;
		maxLevelScore = currentScore;

		OnScoreChangesEvent.Invoke(currentScore);

		targets = levelData.Targets;

		StartCoroutine(SpawnTarget());
	}

	IEnumerator SpawnTarget()
	{
		while (gameManager.CurrentGameState == GameManager.GameState.GAMEPLAY)
		{
			yield return new WaitForSeconds(levelData.SpawnRate);

			int targetIndex = Random.Range(0, targets.Count);

			CreateTarget(targetIndex);
		}
	}

	// The function creates the certain target using geometry and physic limits from LevelData scriptable object
	void CreateTarget(int index)
	{
		Vector3 position = new Vector3(Random.Range(levelData.MinXSpawnPosition, levelData.MaxXSpawnPosition),
									   Random.Range(levelData.MinYSpawnPosition, levelData.MaxYSpawnPosition),
									   0);

		GameObject target = objectPooler.GetObjectFromPool(index, position, transform.rotation);

		if (target != null)
		{
			Rigidbody targetRb = target.GetComponent<Rigidbody>();

			targetRb.AddTorque(RandomTorque(), RandomTorque(), RandomTorque(), ForceMode.Impulse);

			targetRb.AddForce(RandomForce(), ForceMode.Impulse);
		}

	}

	Vector3 RandomForce()
	{
		Vector3 xVector = Vector3.right * Random.Range(levelData.MinXForceValue, levelData.MaxXForceValue);
		Vector3 yVector = Vector3.up * Random.Range(levelData.MinYForceValue, levelData.MaxYForceValue);

		return xVector + yVector;
	}

	float RandomTorque()
	{
		return Random.Range(-levelData.TorqueRange, levelData.TorqueRange);
	}


	void Update()
	{
		// This block includes cheatcodes and helps to test the game. It needs to delete in production.
		// Victory condition - key "w".
		if (gameManager.CurrentGameState == GameManager.GameState.GAMEPLAY)
		{
			if (Input.GetKeyDown(KeyCode.W))
			{
				StopCoroutine(SpawnTarget());

				playerInfo.PlayerResultScore += maxLevelScore;

				if (levelData.NextLevelName == "Final")
				{
					gameManager.UpdateState(GameManager.GameState.FINAL);
					gameManager.StartFinalScreen();
				}
				else
				{
					gameManager.UpdateState(GameManager.GameState.ENDLEVEL_WIN);
				}
			}

			// Defeat condition - key "l".
			if (Input.GetKeyDown(KeyCode.L))
			{
				StopCoroutine(SpawnTarget());

				playerInfo.PlayerResultScore += maxLevelScore;

				gameManager.UpdateState(GameManager.GameState.ENDLEVEL_LOSE);
			}
		}
	}

	// Updates current and max level score, invokes appropriate event, checks win/lose condition after score updating 
	public void ScoreUpdate(int score)
	{
		if (gameManager.CurrentGameState == GameManager.GameState.GAMEPLAY)
		{
			currentScore += score;

			OnScoreChangesEvent.Invoke(currentScore);

			// Set the maximum score on this level
			if (currentScore > maxLevelScore)
			{
				maxLevelScore += currentScore - maxLevelScore;
			}

			// Victory condition
			if (currentScore >= levelData.WinScore)
			{
				// Stop spawning new targets
				StopCoroutine(SpawnTarget());

				playerInfo.PlayerResultScore += levelData.WinScore;

				if (levelData.NextLevelName == "Final")
				{
					// Immediate start Final scene, instead End Game Panel
					gameManager.UpdateState(GameManager.GameState.FINAL);
					gameManager.StartFinalScreen();
				}
				else
				{
					gameManager.UpdateState(GameManager.GameState.ENDLEVEL_WIN);
				}
			}

			// Defeat condition
			if (currentScore <= levelData.LoseScore)
			{
				StopCoroutine(SpawnTarget());

				playerInfo.PlayerResultScore += maxLevelScore;

				gameManager.UpdateState(GameManager.GameState.ENDLEVEL_LOSE);
			}
		}
	}
}
