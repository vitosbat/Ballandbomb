using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// PlayerInfo class used to store players data and making easier to manipulate it
public class PlayerInfo
{
    public string playerName;
    public int playerScore;

    public PlayerInfo (string name, int score)
	{
        this.playerName = name;
        this.playerScore = score;
	}
}


public class Leaderboard : MonoBehaviour
{
    // User data to populate the leaderboard list
    public string userName;
    public int score;

    // List of PlayerInfo objects
    List<PlayerInfo> collectedStats;

	private void Start()
	{
        // [Temporary]
        userName = "Britney Bears";

        collectedStats = new List<PlayerInfo>();

        // TODO: time of leaderbord loading: endgame? LoadLeaderBoard() needs to return data or not?
        LoadLeaderBoard();

        
	}

    void LoadLeaderBoard()
    {
        // Load The String Of The Leaderboard That Was Saved In The "UpdatePlayerPrefsString" Method
        string stats = PlayerPrefs.GetString("LeaderBoards", "");

        // Assign The String To An Array And Split Using The Comma Character
        // This Will Remove The Comma From The String, And Leave Behind The Separated Name And Score
        string[] stats2 = stats.Split(',');

        // Loop Through The Array 2 At A Time Collecting Both The Name And Score
        for (int i = 0; i < stats2.Length - 2; i += 2)
        {
			// Use The Collected Information To Create An Object
			PlayerInfo loadedInfo = new PlayerInfo(stats2[i], int.Parse(stats2[i + 1]));

			// Add The Object To The List
			collectedStats.Add(loadedInfo);

			// Update On Screen LeaderBoard
			// UpdateLeaderBoardVisual();
		}
    }

}
