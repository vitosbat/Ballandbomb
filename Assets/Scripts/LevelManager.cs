using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.UI;

// TODO: 
// Object pooling

public class LevelManager : Singleton<LevelManager>
{
	GameManager gameManager;

	public LevelDataSO levelData;

	private List<GameObject> targets;

	int currentScore;

	public GameEvents.EventScoreChanges OnScoreChangesEvent;


	void Start()
	{
		DontDestroyOnLoad(gameObject);

		gameManager = GameManager.Instance;

		gameManager.OnLevelLoaded.AddListener(LevelLoadedHandler);
	}

	private void LevelLoadedHandler(string levelName)
	{
		string currentLevel = gameManager.CurrentLevel;
		string levelDataAddress = "LevelData/" + gameManager.CurrentLevel;

		Debug.Log("Address: " + levelDataAddress);
		Addressables.LoadAssetAsync<LevelDataSO>(levelDataAddress).Completed += OnLoadDone;
	}

	private void OnLoadDone(AsyncOperationHandle<LevelDataSO> obj)
	{
		if (obj.Result == null)
		{
			Debug.LogError("[Addressables] Load result is null.");
			return;
		}

		levelData = obj.Result;
		Debug.Log("AddrLevelData level name: " + levelData.LevelName);

		currentScore = levelData.StartScore;

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

	void Update()
	{
		// Victory condition [cheating]
		if (gameManager.CurrentGameState == GameManager.GameState.GAMEPLAY)
		{
			if (Input.GetKeyDown(KeyCode.W))
			{
				if (levelData.NextLevelName == "Final")
				{
					gameManager.UpdateState(GameManager.GameState.FINAL);
					//gameManager.GoToFinalScreen();
				}
				else
				{
					gameManager.UpdateState(GameManager.GameState.ENDLEVEL_WIN);
				}

			}

			// Defeat condition [cheating]
			if (Input.GetKeyDown(KeyCode.L))
			{
				gameManager.UpdateState(GameManager.GameState.ENDLEVEL_LOSE);
			}
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
		Vector3 xVector = Vector3.right * Random.Range(levelData.MinXForceValue, levelData.MaxXForceValue);
		Vector3 yVector = Vector3.up * Random.Range(levelData.MinYForceValue, levelData.MaxYForceValue);

		return xVector + yVector;
	}

	float RandomTorque()
	{
		return Random.Range(-levelData.TorqueRange, levelData.TorqueRange);
	}

	public void ScoreUpdate(int score)
	{
		if (gameManager.CurrentGameState == GameManager.GameState.GAMEPLAY)
		{
			currentScore += score;
			Debug.Log("Current score: " + currentScore);

			OnScoreChangesEvent.Invoke(currentScore);

			if (currentScore >= levelData.WinScore)
			{
				Debug.Log("You win!");
				if (levelData.NextLevelName == "Final")
				{
					gameManager.UpdateState(GameManager.GameState.FINAL);
					gameManager.GoToFinalScreen();
				}
				else
				{
					gameManager.UpdateState(GameManager.GameState.ENDLEVEL_WIN);
				}
			}

			// Defeat condition
			if (currentScore <= levelData.LoseScore)
			{
				Debug.Log("You lost.");
				gameManager.UpdateState(GameManager.GameState.ENDLEVEL_LOSE);
			}
		}
	}

}
