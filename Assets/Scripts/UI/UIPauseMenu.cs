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

	// Activates the panel after game state set on PAUSE
	private void GameStateChangedHandler(GameManager.GameState currentGameState, GameManager.GameState previousGameState)
	{
		gameObject.SetActive(currentGameState == GameManager.GameState.PAUSE);
	}

	// Back to GAMEPLAY state
	public void ResumeButtonDownHandler()
	{
		gameManager.UpdateState(GameManager.GameState.GAMEPLAY);
	}
}
