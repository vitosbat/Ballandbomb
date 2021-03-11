using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;


public class GameManager : Singleton<GameManager>
{
	// Game States logics
	public enum GameState {START, GAMEPLAY, PAUSE, ENDLEVEL_WIN, ENDLEVEL_LOSE, FINAL}

	GameState currentGameState = GameState.START;
	public GameState CurrentGameState
	{
		get { return currentGameState; }
		set { currentGameState = value; }
	}

	// Player options
	public PlayerSO playerInfo;
	
	// The event that will invoke after the game state was changing
	public GameEvents.EventGameState OnGameStateChanged;

	// The event that will invoke after the Scene loaded to start LevelData loading in LevelManager, etc.
	public GameEvents.StringParameterEvent OnSceneLoaded;

	// Initiate the assets in initial scene 
	public GameObject[] initialPrefabs;
	private List<GameObject> instancedInitialPrefabs;
	
	// Level order control
	private readonly string firstLevelName = "Level1";
	
	private string currentLevel = string.Empty;
	public string CurrentLevel
	{
		get { return currentLevel; }
		set { }
	}

	// Backend manager
	BackendManager backendManager;

	// Leaderboard data manager
	LeaderboardManager leaderboard;


	private void Start()
	{
		DontDestroyOnLoad(gameObject);

		playerInfo.PlayerName = playerInfo.DefaultPlayerName;
		
		leaderboard = LeaderboardManager.Instance;
		backendManager = BackendManager.Instance;

		// Instantiates prefabs that will exist all the game session time
		instancedInitialPrefabs = new List<GameObject>();
		
		InstantiateInitialPrefabs();
	}
	
	private void InstantiateInitialPrefabs()
	{
		GameObject prefabInstance;
		
		for (int i = 0; i < initialPrefabs.Length; i++)
		{
			prefabInstance = Instantiate(initialPrefabs[i]);
			instancedInitialPrefabs.Add(prefabInstance);
		}
	}

	protected override void OnDestroy()
	{
		base.OnDestroy();

		for (int i = 0; i < instancedInitialPrefabs.Count; i++)
		{
			Destroy(instancedInitialPrefabs[i]);
		}

		instancedInitialPrefabs.Clear();
	}

	// Level scene loading function.
	void LoadLevel(string level)
	{
		AsyncOperation levelLoading = SceneManager.LoadSceneAsync(level, LoadSceneMode.Additive);
		
		if (levelLoading == null)
		{
			Debug.LogError("[Game Manager] unable to load level " + level);
			return;
		}

		StartCoroutine(WaitForSceneLoad(SceneManager.GetSceneByName(level), level));
	}

	private IEnumerator WaitForSceneLoad(Scene scene, string levelName)
	{
		while (!scene.isLoaded)
		{
			yield return null;
		}
		
		SceneManager.SetActiveScene(scene);
		
		currentLevel = levelName;

		OnSceneLoaded.Invoke(currentLevel);
	}

	// Level scene unloading function.
	void UnloadLevel (string level)
	{
		AsyncOperation levelUnloading = SceneManager.UnloadSceneAsync(level);
		
		if (levelUnloading == null)
		{
			Debug.LogError("[Game Manager] unable to unload level " + level);
			return;
		}
	}
	
	// The main function of the game state updating
	public void UpdateState(GameState state)
	{
		GameState previousGameState = currentGameState;
		currentGameState = state;

		switch (currentGameState)
		{
			case GameState.START:
				Debug.Log("Update State: START");
				Time.timeScale = 1.0f;
				UnloadLevel(currentLevel);
				break;
			case GameState.GAMEPLAY:
				Debug.Log("Update State: GAMEPLAY");
				Time.timeScale = 1.0f;
				break;
			case GameState.PAUSE:
				Debug.Log("Update State: PAUSE");
				Time.timeScale = 0.0f;
				break;
			case GameState.ENDLEVEL_WIN:
				Debug.Log("Update State: ENDLEVEL_WIN");
				Time.timeScale = 1.0f;
				break;
			case GameState.ENDLEVEL_LOSE:
				Debug.Log("Update State: ENDLEVEL_LOSE");
				Time.timeScale = 1.0f;
				
				// Save new result and update Leaderboard
				leaderboard.AddResultToLeaderBoard(playerInfo.PlayerName, playerInfo.PlayerResultScore);
				backendManager.SendLeaderboard(playerInfo.PlayerResultScore);

				break;
			case GameState.FINAL:
				Debug.Log("Update State: FINAL");
				Time.timeScale = 1.0f;
				
				// Save new result and update Leaderboard
				leaderboard.AddResultToLeaderBoard(playerInfo.PlayerName, playerInfo.PlayerResultScore);
				backendManager.SendLeaderboard(playerInfo.PlayerResultScore);

				break;
			default:
				break;
		}

		// Event trigger about game state changes
		OnGameStateChanged.Invoke(currentGameState, previousGameState);

	}

	public void StartGame ()
	{
		playerInfo.PlayerResultScore = playerInfo.DefaultPlayerResultScore;

		LoadLevel(firstLevelName);
	}

	public void GoToNextLevel()
	{
		if (currentGameState == GameState.ENDLEVEL_WIN)
		{
			UnloadLevel(currentLevel);

			LoadLevel(LevelManager.Instance.levelData.NextLevelName);

			UpdateState(GameState.GAMEPLAY);
		}
	}

	public void StartFinalScreen()
	{
		if (currentGameState == GameState.FINAL)
		{
			UnloadLevel(currentLevel);
			LoadLevel(LevelManager.Instance.levelData.NextLevelName);
		}
	}


	public void QuitGame ()
	{
		Application.Quit();
	}
}
