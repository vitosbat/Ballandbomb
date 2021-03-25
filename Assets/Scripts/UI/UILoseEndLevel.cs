using UnityEngine;
using UnityEngine.UI;

public class UILoseEndLevel : MonoBehaviour
{
	GameManager gameManager;

	// Players name and score data
	[SerializeField] private PlayerSO playerData;
	
	private void Start()
	{
		gameObject.SetActive(false);

		// Subscribes to Game State changing
		gameManager = GameManager.Instance;
		gameManager.OnGameStateChanged.AddListener(GameStateChangedHandler);
	}

	// Set Game end panel active in depending on respective Game State
	private void GameStateChangedHandler(GameManager.GameState currentGameState, GameManager.GameState previousGameState)
	{
		gameObject.SetActive(currentGameState == GameManager.GameState.ENDLEVEL_LOSE);
		
		// Creates Game end text with players name and his final score
		if (currentGameState == GameManager.GameState.ENDLEVEL_LOSE)
		{
			string endText = "Good game, " + playerData.PlayerName + "!\n Your score is: " + playerData.PlayerResultScore;
			
			transform.Find("EndGameText").GetComponent<Text>().text = endText;
		}
	}
}
