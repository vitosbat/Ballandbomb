using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UILifeSlider : MonoBehaviour
{
	GameManager gameManager;
	LevelManager levelManager;

	[SerializeField] Slider lifeSlider;
	[SerializeField] GameObject scoreText;


	private void Start()
	{
		gameObject.SetActive(false);
		gameManager = GameManager.Instance;
		levelManager = LevelManager.Instance;
		gameManager.OnGameStateChanged.AddListener(GameStateChangedHandler);
		levelManager.OnScoreChangesEvent.AddListener(ScoreChangesHandler);
	}

	// Show player LifeSlider in-game, pause, and in the game-ending
	private void GameStateChangedHandler(GameManager.GameState currentGameState, GameManager.GameState previousGameState)
	{
		gameObject.SetActive(
			currentGameState == GameManager.GameState.GAMEPLAY ||
			currentGameState == GameManager.GameState.PAUSE ||
			currentGameState == GameManager.GameState.ENDLEVEL_WIN ||
			currentGameState == GameManager.GameState.ENDLEVEL_LOSE
			);
	}

	// Changes Lifeslider fill area and score text after score changed
	public void ScoreChangesHandler(int value)
	{
		lifeSlider.value = value;
		scoreText.GetComponent<TextMeshProUGUI>().text = value + "%";
	}
}
