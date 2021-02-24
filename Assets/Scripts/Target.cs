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
			TargetDestroy(gameObject); 
			//gameObject.SetActive(false);
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
				TargetDestroy(gameObject); 
				//gameObject.SetActive(false);
				levelManager.ScoreUpdate(-pointValue);
			}

			TargetDestroy(gameObject);
			//gameObject.SetActive(false);
		}
	}

	void TargetDestroy(GameObject gameObject)
	{
		Rigidbody targetRb = gameObject.GetComponent<Rigidbody>();
		
		targetRb.velocity = Vector3.zero;
		targetRb.angularVelocity = Vector3.zero;
		targetRb.rotation = Quaternion.identity;
		//targetRb.inertiaTensorRotation = Quaternion.identity;
		//targetRb.inertiaTensor = Vector3.zero;

		gameObject.SetActive(false);
	}
}
