using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObject/PlayerData", fileName = "Player")]
public class PlayerSO : ScriptableObject
{
    [SerializeField] private string defaultPlayerName;
    public string DefaultPlayerName
    {
        get { return defaultPlayerName; }
        set { }
    }

    [SerializeField] private int defaultPlayerResultScore;
    public int DefaultPlayerResultScore
    {
        get { return defaultPlayerResultScore; }
        set { }
    }

    [SerializeField] private string playerName;
    public string PlayerName
    {
        get { return playerName; }
        set { playerName = value; }
    }

    [SerializeField] private int playerResultScore;
    public int PlayerResultScore
    {
        get { return playerResultScore; }
        set { playerResultScore = value; }
    }
}
