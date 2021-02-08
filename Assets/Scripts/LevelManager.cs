using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    // TODO: add the SO array and put there the SO instances in editor
    [SerializeField]
    public Dictionary<string, LevelDataSO> levelsData;
    GameManager gameManager;

    LevelDataSO currentLevelData;

    void Start()
    {

        Debug.Log("Level name = ");
    }
        
}
