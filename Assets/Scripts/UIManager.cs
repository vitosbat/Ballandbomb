using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
	GameManager gameManager;

	private void Start()
	{
		gameManager = GameManager.Instance;
		GameManager.Instance.OnGameStateChanged.AddListener(GameStateChangedHandler);
	}

	private void GameStateChangedHandler(GameManager.GameState currentGameState, GameManager.GameState previousGameState)
	{
		//It must be change! SetActive not on Manager, but on a Menu
		Debug.Log("Previous: " + previousGameState);
		Debug.Log("Current: " + currentGameState);
		gameObject.SetActive(currentGameState == GameManager.GameState.START);
	}

	public void GameStartButtonDownHandler()
	{
		Debug.Log("Start Button down handler");
		
		GameManager.Instance.StartGame();
		GameManager.Instance.UpdateState(GameManager.GameState.GAMEPLAY);

	}

	public void OptionsButtonDownHandler()
	{
		Debug.Log("Options Button down handler");
	}

}
