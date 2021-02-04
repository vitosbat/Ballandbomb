using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
	//public Button startGameButton;

	private void Start()
	{
		GameManager.Instance.OnGameStateChanged.AddListener(GameStateChangedHandler);
	}

	private void GameStateChangedHandler(GameManager.GameState currentGameState, GameManager.GameState previousGameState)
	{
		Debug.Log("Previous: " + previousGameState);
		Debug.Log("Current: " + currentGameState);
		gameObject.SetActive(currentGameState == GameManager.GameState.START);
		//throw new NotImplementedException();
	}

	public void GameStartButtonDownHandler()
	{
		Debug.Log("Start Button down handler");
		GameManager.Instance.UpdateState(GameManager.GameState.GAMEPLAY);

	}

	public void OptionsButtonDownHandler()
	{
		Debug.Log("Options Button down handler");
	}

}
