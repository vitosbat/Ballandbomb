using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIQuitGameButton : MonoBehaviour
{
	GameManager gameManager;

	public void QuitButtonDownHandler()
	{
		gameManager = GameManager.Instance;
		gameManager.QuitGame();
	}
}
