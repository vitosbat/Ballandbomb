using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObject/PlayerData", fileName = "Player")]
public class PlayerSO : ScriptableObject
{
    [SerializeField] private string playerName;
    public string PlayerName
    {
        get { return playerName; }
        protected set { }
    }

    [SerializeField] private int playerResultScore;
    public int PlayerResultScore
    {
        get { return playerResultScore; }
        protected set { }
    }
}
