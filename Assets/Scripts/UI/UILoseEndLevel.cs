using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UILoseEndLevel : MonoBehaviour
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
		gameObject.SetActive(currentGameState == GameManager.GameState.ENDLEVEL_LOSE);
	}

}
