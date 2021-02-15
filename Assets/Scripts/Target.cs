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
			Destroy(gameObject);
			Instantiate(explosionParticle, transform.position, explosionParticle.transform.rotation);
		}

		levelManager.ScoreUpdate(pointValue);
		
	}

	private void OnTriggerEnter(Collider other)
	{
		Destroy(gameObject);
		if (!gameObject.CompareTag("Bad Target"))
		{
			Destroy(gameObject);

			levelManager.ScoreUpdate(-pointValue);
		}
	}

}
