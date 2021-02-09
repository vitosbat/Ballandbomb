using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// TO DOCS:
// Every  level will have it's own Level Manager Class instead the one singleton Level Manager
// It is more modular system (independent scenes) and avoiding the "God Object" antipatter
public class Level1Manager : MonoBehaviour
{
    [SerializeField] LevelDataSO levelData;
    GameManager gameManager;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameManager.Instance;

        Debug.Log("Level name is " + levelData.LevelName);
        Debug.Log("Next level name is " + levelData.NextLevelName);
        
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
