using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIPauseMenu : MonoBehaviour
{
	GameManager gameManager;

	private void Start()
	{
		gameObject.SetActive(false);
		gameManager = GameManager.Instance;
		gameManager.OnGameStateChanged.AddListener(GameStateChangedHandler);
	}

	private void GameStateChangedHandler(GameManager.GameState currentGameState, GameManager.GameState previousGameState)
	{
		gameObject.SetActive(currentGameState == GameManager.GameState.PAUSE);
	}


	public void ResumeButtonDownHandler()
	{
		gameManager.UpdateState(GameManager.GameState.GAMEPLAY);

	}

	public void RestartButtonDownHandler()
	{
		gameManager.UpdateState(GameManager.GameState.START);
	}

	public void QuitGameButtonDownHandler()
	{
		gameManager.QuitGame();
	}
}
