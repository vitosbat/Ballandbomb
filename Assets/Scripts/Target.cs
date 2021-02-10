using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour
{
	private Rigidbody targetRb;
	private GameManager gameManager;
	private float minSpeed = 11.0f;
	private float maxSpeed = 14.0f;

	private float maxTorque = 10.0f;

	private float xSpawnRange = 6.0f;

	private float ySpawnPos = -2.0f;

	public int pointValue;
	public ParticleSystem explosionParticle;

	// Start is called before the first frame update
	void Start()
	{
		
		targetRb = GetComponent<Rigidbody>();

		transform.position = RandomSpawnPos();

		targetRb.AddTorque(RandomTorque(), RandomTorque(), RandomTorque(), ForceMode.Impulse);

		targetRb.AddForce(RandomForce(), ForceMode.Impulse);

	}


	private void OnMouseDown()
	{
		// TODO: add the checking for GameState is GAMEPLAY.
		Destroy(gameObject);
		Instantiate(explosionParticle, transform.position, explosionParticle.transform.rotation);
		//gameManager.UpdateScore(pointValue);

	}

	private void OnTriggerEnter(Collider other)
	{
		Destroy(gameObject);
		if (!gameObject.CompareTag("Bad Target"))
		{
			Destroy(gameObject);
			//gameManager.UpdateScore(-pointValue);
		}
	}

	Vector3 RandomForce()
	{
		return Vector3.up * Random.Range(minSpeed, maxSpeed);
	}

	float RandomTorque()
	{
		return Random.Range(-maxTorque, maxTorque);
	}

	Vector3 RandomSpawnPos()
	{
		return new Vector3(Random.Range(-xSpawnRange, xSpawnRange), ySpawnPos);
	}
}
