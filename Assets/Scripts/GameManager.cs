using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
	private float spawnRate = 1.0f;
	private int score;

	public TextMeshProUGUI gameOverText;
	public GameObject titleScreen;
	public Slider lifeSlider;
	public GameObject sensor;
	public List<GameObject> targets;
	public bool isGameActive;
	public Button restartButton;


	public void StartGame(int difficulty)
	{
		isGameActive = true;
		titleScreen.SetActive(false);
		spawnRate /= difficulty;
		StartCoroutine(SpawnTarget());
		score = 0;
		UpdateScore(0);
		lifeSlider.gameObject.SetActive(true);
		lifeSlider.value = score;
	}

	public void UpdateScore(int scoreToAdd)
	{
		score += scoreToAdd;
		lifeSlider.value = score;
		if (score >= 200)
		{
			GameOver(true);
		}
		else if (score <= -100)
		{
			GameOver(false);
		}
	}

	public void GameOver(bool isWin)
	{
		isGameActive = false;
		sensor.gameObject.SetActive(false);

		gameOverText.gameObject.SetActive(true);

		if (isWin)
		{
			gameOverText.SetText("Congratulations!\n You win!");
		}
		else
		{
			gameOverText.SetText("Lost\n Try again...");
		}

		restartButton.gameObject.SetActive(true);
	}

	public void ReloadGame()
	{
		SceneManager.LoadScene(SceneManager.GetActiveScene().name);
	}

	IEnumerator SpawnTarget()
	{
		while (isGameActive)
		{
			yield return new WaitForSeconds(spawnRate);
			int targetIndex = Random.Range(0, targets.Count);
			Instantiate(targets[targetIndex]);
		}
	}
}
