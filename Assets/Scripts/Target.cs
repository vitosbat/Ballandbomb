using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour
{
	private GameManager gameManager;
	
	public ParticleSystem explosionParticle;
	
	void Start()
	{
		gameManager = GameManager.Instance;
	}


	private void OnMouseDown()
	{
		if (gameManager.CurrentGameState == GameManager.GameState.GAMEPLAY)
		{
			Destroy(gameObject);
			Instantiate(explosionParticle, transform.position, explosionParticle.transform.rotation);
		}
		// gameManager.UpdateScore(pointValue); event?
		// OnTargetDestroy.Invoke(pointValue); ?
	}

	private void OnTriggerEnter(Collider other)
	{
		Destroy(gameObject);
		if (!gameObject.CompareTag("Bad Target"))
		{
			Destroy(gameObject);

			//gameManager.UpdateScore(-pointValue); event?
		}
	}

}
