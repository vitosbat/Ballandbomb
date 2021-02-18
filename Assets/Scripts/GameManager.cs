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

	// The event that will invoke after the game state was changing
	public GameEvents.EventGameState OnGameStateChanged;

	public GameEvents.EventSceneChanges OnLevelLoaded;

	// Initiate the assets in initial scene 
	public GameObject[] initialPrefabs;
	private List<GameObject> _instancedInitialPrefabs;
	
	// Level data
	private readonly string firstLevelName = "Level1";
	
	private string currentLevel = string.Empty;
	public string CurrentLevel
	{
		get { return currentLevel; }
		set { }
	}
	

	private void Start()
	{
		DontDestroyOnLoad(gameObject);

		_instancedInitialPrefabs = new List<GameObject>();
		InstantiateInitialPrefabs();

	}

	// Level loading function.
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

		OnLevelLoaded.Invoke(currentLevel);
		// event invoke that the scene had loaded
		// 
	}

	// Level unloading function.
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
				break;
			case GameState.FINAL:
				Debug.Log("Update State: FINAL");
				Time.timeScale = 1.0f;
				break;
			default:
				break;
		}

		// Event trigger about game state changes
		OnGameStateChanged.Invoke(currentGameState, previousGameState);

	}

	private void InstantiateInitialPrefabs()
	{
		GameObject prefabInstance;
		
		for (int i = 0; i < initialPrefabs.Length; i++)
		{
			prefabInstance = Instantiate(initialPrefabs[i]);
			_instancedInitialPrefabs.Add(prefabInstance);
		}
	}

	protected override void OnDestroy()
	{
		base.OnDestroy();

		for (int i = 0; i < _instancedInitialPrefabs.Count; i++)
		{
			Destroy(_instancedInitialPrefabs[i]);
		}

		_instancedInitialPrefabs.Clear();
	}

	public void StartGame ()
	{
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

	public void GoToFinalScreen()
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
