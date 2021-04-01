using UnityEngine;

public class UIStartMenu : MonoBehaviour
{
	GameManager gameManager;

	private void Start()
	{
		gameManager = GameManager.Instance;
		gameManager.OnGameStateChanged.AddListener(GameStateChangedHandler);
	}

	// Activates the panel after game state set on START
	private void GameStateChangedHandler(GameManager.GameState currentGameState, GameManager.GameState previousGameState)
	{
		gameObject.SetActive(currentGameState == GameManager.GameState.START);
	}

	// Set the game state to START after pushing the Start button
	public void GameStartButtonDownHandler()
	{
		gameManager.StartGame();
		gameManager.UpdateState(GameManager.GameState.GAMEPLAY);
	}
}
