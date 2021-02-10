//using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// TODO: 
// Spawn logic
// Destroy and points logic
// Object pooling
// Victory and defeat logic

public class LevelManager : Singleton<LevelManager>
{
    GameManager gameManager;

    public LevelDataSO levelData;

    public List<GameObject> targets;

    float spawnRate = 0.5f;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameManager.Instance;
        
        StartCoroutine(SpawnTarget());
    }

	IEnumerator SpawnTarget()
	{        
        while (gameManager.CurrentGameState == GameManager.GameState.GAMEPLAY)
		{
			yield return new WaitForSeconds(spawnRate);
			int targetIndex = Random.Range(0, targets.Count);
            Instantiate(targets[targetIndex]);
		}
	}

    // Update is called once per frame
    void Update()
    {

        // Victory condition
        if (Input.GetKeyDown(KeyCode.W))
        {
            Debug.Log("You win!");
            gameManager.UpdateState(GameManager.GameState.ENDLEVEL);
        }

        // Defeat condition
        if (Input.GetKeyDown(KeyCode.L))
        {
            Debug.Log("You lost.");
            gameManager.UpdateState(GameManager.GameState.ENDLEVEL);
        }

    }
}
