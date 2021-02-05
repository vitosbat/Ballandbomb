using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIStartMenu : MonoBehaviour
{
	GameManager gameManager;

	private void Start()
	{
		gameManager = GameManager.Instance;
		gameManager.OnGameStateChanged.AddListener(GameStateChangedHandler);
	}

	private void GameStateChangedHandler(GameManager.GameState currentGameState, GameManager.GameState previousGameState)
	{
		gameObject.SetActive(currentGameState == GameManager.GameState.START);
	}


	public void GameStartButtonDownHandler()
	{
		gameManager.StartGame();
		gameManager.UpdateState(GameManager.GameState.GAMEPLAY);

	}

	public void OptionsButtonDownHandler()
	{
		Debug.Log("Options Button down handler");
	}

	public void QuitGameButtonDownHandler()
	{
		gameManager.QuitGame();
		Debug.Log("Game over :(");
		
	}
}
