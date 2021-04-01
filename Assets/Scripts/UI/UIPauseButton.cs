using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIPauseButton : MonoBehaviour
{
	GameManager gameManager;

	private void Start()
	{
		gameObject.SetActive(false);
		gameManager = GameManager.Instance;
		gameManager.OnGameStateChanged.AddListener(GameStateChangedHandler);
	}

	// Show Pause button in GAMEPLAY state
	private void GameStateChangedHandler(GameManager.GameState currentGameState, GameManager.GameState previousGameState)
	{
		gameObject.SetActive(currentGameState == GameManager.GameState.GAMEPLAY);
	}

	// Set the game state to PAUSE after pushing the Pause button
	public void PauseButtonDownHandler()
	{
		gameManager.UpdateState(GameManager.GameState.PAUSE);
	}
}
