using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : Singleton<UIManager>
{
	GameManager gameManager;

	[SerializeField] Texture2D targetCursor;

	private void Start()
	{
		gameManager = GameManager.Instance;
		gameManager.OnGameStateChanged.AddListener(GameStateChangedHandler);
	}

	private void GameStateChangedHandler(GameManager.GameState currentGameState, GameManager.GameState previousGameState)
	{
		if (currentGameState == GameManager.GameState.GAMEPLAY)
		{
			Cursor.SetCursor(targetCursor, new Vector2(64f, 64f), CursorMode.Auto);
		}
		else
		{
			Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
		}
	}
}
