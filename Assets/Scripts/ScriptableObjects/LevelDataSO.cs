using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObject/LevelData", fileName ="Level_")]
public class LevelDataSO : ScriptableObject
{
    [Header("Level Hierarchy Data")]
    [Tooltip("The name of this level")]
    [SerializeField] private string levelName;
    public string LevelName
	{
        get { return levelName; }
        protected set { }
	}

    [Tooltip("The name of the next level")]
    [SerializeField] private string nextLevelName;
    public string NextLevelName
    {
        get { return nextLevelName; }
        protected set { }
    }

    [Header("Spawn Data")]
    [Tooltip("The list of objects that will be spawn in the current level")]
    [SerializeField] private List<GameObject> targets;
    public List<GameObject> Targets
	{
        get { return targets; }
        protected set { }
	}

    [Space(5)]
    [Tooltip("The number of seconds between objects spawn")]
    [SerializeField] private float spawnRate;
    public float SpawnRate
    {
        get { return spawnRate; }
        protected set { }
    }
        
    [Space(5)]
    [Tooltip("The min X-coordinate of the object spawning")]
    [SerializeField] private float minXSpawnPosition;
    public float MinXSpawnPosition
    {
        get { return minXSpawnPosition; }
        protected set { }
    }

    [Tooltip("The max X-coordinate of the object spawning")]
    [SerializeField] private float maxXSpawnPosition;
    public float MaxXSpawnPosition
    {
        get { return maxXSpawnPosition; }
        protected set { }
    }

    [Tooltip("The min Y-coordinate of the object spawning")]
    [SerializeField] private float minYSpawnPosition;
    public float MinYSpawnPosition
    {
        get { return minYSpawnPosition; }
        protected set { }
    }

    [Tooltip("The max Y-coordinate of the object spawning")]
    [SerializeField] private float maxYSpawnPosition;
    public float MaxYSpawnPosition
    {
        get { return maxYSpawnPosition; }
        protected set { }
    }


    [Space(5)]
    [Tooltip("The torque rate limit")]
    [SerializeField] private float torqueRange;
    public float TorqueRange
    {
        get { return torqueRange; }
        protected set { }
    }

    
}
