using UnityEngine;
using UnityEngine.UI;

public class UILifeSlider : MonoBehaviour
{
	GameManager gameManager;
	LevelManager levelManager;

	[SerializeField] Slider lifeSlider;


	private void Start()
	{
		gameObject.SetActive(false);
		gameManager = GameManager.Instance;
		levelManager = LevelManager.Instance;
		gameManager.OnGameStateChanged.AddListener(GameStateChangedHandler);
		levelManager.OnScoreChangesEvent.AddListener(ScoreChangesHandler);
	}

	private void GameStateChangedHandler(GameManager.GameState currentGameState, GameManager.GameState previousGameState)
	{
		gameObject.SetActive(
			currentGameState == GameManager.GameState.GAMEPLAY ||
			currentGameState == GameManager.GameState.PAUSE ||
			currentGameState == GameManager.GameState.ENDLEVEL_WIN ||
			currentGameState == GameManager.GameState.ENDLEVEL_LOSE
			);
	}

	public void ScoreChangesHandler(int value)
	{
		lifeSlider.value = value;
	}
}
