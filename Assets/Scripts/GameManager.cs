using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;

public class GameManager : Singleton<GameManager>
{
	// Game States logics
	public enum GameState { START, GAMEPLAY, PAUSE, ENDLEVEL_WIN, ENDLEVEL_LOSE, FINAL }

	GameState currentGameState = GameState.START;
	public GameState CurrentGameState
	{
		get { return currentGameState; }
		set { currentGameState = value; }
	}

	// Player options
	public PlayerSO playerInfo;

	// The event that will invoke after the game state was changing
	[HideInInspector] public GameEvents.EventGameState OnGameStateChanged;

	// The event that will invoke after the Scene loaded to start LevelData loading in LevelManager, etc.
	[HideInInspector] public GameEvents.StringParameterEvent OnSceneLoaded;

	// Initiate the assets in initial scene 
	public GameObject[] initialPrefabs;
	private List<GameObject> instancedInitialPrefabs;

	// Current level scene instance
	private AsyncOperationHandle<SceneInstance> handle;

	// Start level name for order control (move to settings in future)
	private readonly string firstLevelName = "Level 1";

	// Name of the scene of the current loaded level
	private string currentLevel = string.Empty;
	public string CurrentLevel
	{
		get { return currentLevel; }
		set { }
	}

	#region Game initialize
	private void Start()
	{
		DontDestroyOnLoad(gameObject);

		// Set player name as 'anonym' if not login
		playerInfo.PlayerName = playerInfo.DefaultPlayerName;

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

	#endregion

	#region Scene Management

	// Level scene loading function.
	void LoadLevel(string level)
	{
		Addressables.LoadSceneAsync(level, LoadSceneMode.Additive).Completed += LoadLevelCompleted;
	}

	private void LoadLevelCompleted(AsyncOperationHandle<SceneInstance> obj)
	{
		if (obj.Status == AsyncOperationStatus.Succeeded)
		{
			handle = obj;

			SceneManager.SetActiveScene(obj.Result.Scene);

			currentLevel = obj.Result.Scene.name;

			OnSceneLoaded.Invoke(currentLevel);
		}
	}

	// Level scene unloading function.
	void UnloadLevel(string level)
	{
		Addressables.UnloadSceneAsync(handle, true).Completed += op =>
		{
			if (op.Status == AsyncOperationStatus.Succeeded)
			{
				Debug.Log("Successfully unload " + currentLevel);
			}
		};
	}
	#endregion


	// The main function of the game state updating
	public void UpdateState(GameState state)
	{
		GameState previousGameState = currentGameState;
		currentGameState = state;

		switch (currentGameState)
		{
			case GameState.START:
				Time.timeScale = 1.0f;
				UnloadLevel(currentLevel);
				break;

			case GameState.GAMEPLAY:
				Time.timeScale = 1.0f;
				break;

			case GameState.PAUSE:
				Time.timeScale = 0.0f;
				break;

			case GameState.ENDLEVEL_WIN:
				Time.timeScale = 1.0f;
				break;

			case GameState.ENDLEVEL_LOSE:
				Time.timeScale = 1.0f;
				break;

			case GameState.FINAL:
				Time.timeScale = 1.0f;
				break;

			default:
				break;
		}

		// Event trigger of game state changes
		OnGameStateChanged.Invoke(currentGameState, previousGameState);
	}


	public void StartGame()
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

	public void QuitGame()
	{
		Application.Quit();
	}
}
