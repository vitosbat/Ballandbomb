using UnityEngine;
using TMPro;

public class UILoseEndLevel : MonoBehaviour
{
	GameManager gameManager;

	[SerializeField] private PlayerSO playerData;
	
	private void Start()
	{
		gameObject.SetActive(false);
		gameManager = GameManager.Instance;
		gameManager.OnGameStateChanged.AddListener(GameStateChangedHandler);

	}


	private void GameStateChangedHandler(GameManager.GameState currentGameState, GameManager.GameState previousGameState)
	{
		gameObject.SetActive(currentGameState == GameManager.GameState.ENDLEVEL_LOSE);
		if (currentGameState == GameManager.GameState.ENDLEVEL_LOSE)
		{
			string endText = "Good job, " + playerData.PlayerName + "!\n Your score is: " + playerData.PlayerResultScore;
			
			transform.Find("EndText").GetComponent<TextMeshProUGUI>().text = endText;
		}
	}

}
