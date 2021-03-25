using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UINextLevelMenu : MonoBehaviour
{
    GameManager gameManager;

    [SerializeField] Text winLevelText;
    
    void Start()
    {
        gameObject.SetActive(false);

        gameManager = GameManager.Instance;
        gameManager.OnGameStateChanged.AddListener(GameStateChangedHandler);
    }

    private void GameStateChangedHandler(GameManager.GameState currentGameState, GameManager.GameState previousGameState)
    {
        gameObject.SetActive(currentGameState == GameManager.GameState.ENDLEVEL_WIN);

        winLevelText.text = "Perfect!\n " + GameManager.Instance.CurrentLevel + " completed.";
    }

    public void NextLevelButtonHandler()
	{
        gameManager.GoToNextLevel();
	}

}
