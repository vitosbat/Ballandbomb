using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : Singleton<LevelManager>
{
    GameManager gameManager;

    public LevelDataSO levelData;

    
    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameManager.Instance;
        
        Debug.Log("Level name is " + levelData.LevelName + " | Next level name is " + levelData.NextLevelName);
        
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
