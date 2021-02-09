using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;


public class GameManager : Singleton<GameManager>
{
	// Game States logics
	public enum GameState {START, GAMEPLAY, PAUSE, ENDLEVEL}

	GameState currentGameState = GameState.START;
	public GameState CurrentGameState
	{
		get { return currentGameState; }
		set { currentGameState = value; }
	}

	// The event that will invoke after the game state was changing
	public GameEvents.EventGameState OnGameStateChanged;


	// Scenes Loading logic
	// Initiate the assets in initial scene 
	public GameObject[] initialPrefabs;
	private List<GameObject> _instancedInitialPrefabs;

	// Current level
	private string currentLevel = string.Empty;
	private LevelDataSO curreneLevelData;


	private void Start()
	{
		DontDestroyOnLoad(gameObject);

		_instancedInitialPrefabs = new List<GameObject>();
		InstantiateInitialPrefabs();

	}

	// Level loading function. Asyncronous operation uses for tracking loading state.
	void LoadLevel(string level)
	{
		AsyncOperation levelLoading = SceneManager.LoadSceneAsync(level, LoadSceneMode.Additive);
		
		if (levelLoading == null)
		{
			Debug.LogError("[Game Manager] unable to load level " + level);
			return;
		}

		currentLevel = level;
	}

	// Level unloading function. Asyncronous operation uses for tracking unloading state.
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
			case GameState.ENDLEVEL:
				Debug.Log("Update State: ENDLEVEL");
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
		LoadLevel("Level1");
	}

	public void GoToNextLevel()
	{
		// Unload currentLevel
		// Load next level (maybe put it here as a func string parameter)
		if (currentGameState == GameState.ENDLEVEL)
		{
			// TODO
			UnloadLevel(currentLevel);
			
			//string nextLevel = curreneLevelData.
			LoadLevel("Level2");
			
			UpdateState(GameState.GAMEPLAY);
			Debug.Log("GoToNextLevel function.");
		}
	}

	public void QuitGame ()
	{
		Application.Quit();
	}
}


//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using TMPro;				
//using UnityEngine.SceneManagement;
//using UnityEngine.UI;

//public class GameManager : MonoBehaviour
//{
//	private float spawnRate = 1.0f;
//	private int score;

//	public TextMeshProUGUI gameOverText;
//	public GameObject titleScreen;
//	public Slider lifeSlider;
//	public GameObject sensor;
//	public List<GameObject> targets;
//	public bool isGameActive;
//	public Button restartButton;


//	public void StartGame(int difficulty)
//	{
//		isGameActive = true;
//		titleScreen.SetActive(false);
//		spawnRate /= difficulty;
//		StartCoroutine(SpawnTarget());
//		score = 0;
//		UpdateScore(0);
//		lifeSlider.gameObject.SetActive(true);
//		lifeSlider.value = score;
//	}

//	public void UpdateScore(int scoreToAdd)
//	{
//		score += scoreToAdd;
//		lifeSlider.value = score;
//		if (score >= 200)
//		{
//			GameOver(true);
//		}
//		else if (score <= -100)
//		{
//			GameOver(false);
//		}
//	}

//	public void GameOver(bool isWin)
//	{
//		isGameActive = false;
//		sensor.gameObject.SetActive(false);

//		gameOverText.gameObject.SetActive(true);

//		if (isWin)
//		{
//			gameOverText.SetText("Congratulations!\n You win!");
//		}
//		else
//		{
//			gameOverText.SetText("Lost\n Try again...");
//		}

//		restartButton.gameObject.SetActive(true);
//	}

//	public void ReloadGame()
//	{
//		SceneManager.LoadScene(SceneManager.GetActiveScene().name);
//	}

//	IEnumerator SpawnTarget()
//	{
//		while (isGameActive)
//		{
//			yield return new WaitForSeconds(spawnRate);
//			int targetIndex = Random.Range(0, targets.Count);
//			Instantiate(targets[targetIndex]);
//		}
//	}
//}
