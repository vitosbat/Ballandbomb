using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIRestartButton : MonoBehaviour
{
	GameManager gameManager;

	public void RestartButtonDownHandler()
	{
		gameManager = GameManager.Instance;
		gameManager.UpdateState(GameManager.GameState.START);
	}
}
