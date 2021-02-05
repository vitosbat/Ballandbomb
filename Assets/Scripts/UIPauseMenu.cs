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
		Debug.Log("UIPause handler");
		gameObject.SetActive(currentGameState == GameManager.GameState.PAUSE);
	}
}
