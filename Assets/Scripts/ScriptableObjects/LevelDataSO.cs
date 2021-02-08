﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObject/LevelData", fileName ="Level_")]
public class LevelDataSO : ScriptableObject
{
    [Tooltip("The name of the level")]
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
}
