using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour
{
	private GameManager gameManager;
	private LevelManager levelManager;

	[SerializeField] ParticleSystem explosionParticle;

	[SerializeField] int pointValue;

	void Start()
	{
		gameManager = GameManager.Instance;
		levelManager = LevelManager.Instance;
	}


	private void OnMouseDown()
	{
		if (gameManager.CurrentGameState == GameManager.GameState.GAMEPLAY)
		{
			gameObject.SetActive(false);
			Instantiate(explosionParticle, transform.position, explosionParticle.transform.rotation);
		}

		levelManager.ScoreUpdate(pointValue);
	}

	private void Update()
	{
		if (gameObject.transform.position.y <= -5)
		{
			if (!gameObject.CompareTag("Bad Target"))
			{
				gameObject.SetActive(false);
				levelManager.ScoreUpdate(-pointValue);
			}

			gameObject.SetActive(false);
		}
	}
}
