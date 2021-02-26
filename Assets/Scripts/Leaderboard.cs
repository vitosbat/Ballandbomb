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


public class Leaderboard : Singleton<Leaderboard>
{
    // List of PlayerInfo objects
    List<PlayerInfo> collectedStats;

    public void AddResultToLeaderBoard(string name, int score)
	{
        Debug.Log("Added name: " + name + ", score: " + score);
        
        LoadLeaderBoard();

        collectedStats.Add(new PlayerInfo(name, score));
        
        UpdateLeaderBoard();
        LoadLeaderBoard();

	}

    void LoadLeaderBoard()
    {
        collectedStats = new List<PlayerInfo>();

        // Load The String Of The Leaderboard That Was Saved In The "UpdatePlayerPrefsString" Method
        string stats = PlayerPrefs.GetString("LeaderBoards");

        // Assign The String To An Array And Split Using The Comma Character
        // This Will Remove The Comma From The String, And Leave Behind The Separated Name And Score
        string[] stats2 = stats.Split(',');

        // Loop Through The Array 2 At A Time Collecting Both The Name And Score
        for (int i = 0; i < stats2.Length - 2; i += 2)
        {
			// Use The Collected Information To Create An Object
			PlayerInfo loadedInfo = new PlayerInfo(stats2[i], int.Parse(stats2[i + 1]));

            Debug.Log("Name: " + loadedInfo.playerName + ", score: " + loadedInfo.playerScore + ".\n");

			// Add The Object To The List
			collectedStats.Add(loadedInfo);
		}
    }

    void UpdateLeaderBoard()
    {
        //Start With A Blank String
        string stats = "";

        //Add Each Name And Score From The Collection To The String
        for (int i = 0; i < collectedStats.Count; i++)
        {
            //Be Sure To Add A Comma To Both The Name And Score, It Will Be Used To Separate The String Later
            stats += collectedStats[i].playerName + ",";
            stats += collectedStats[i].playerScore + ",";
        }

        //Add The String To The PlayerPrefs, This Allows The Information To Be Saved Even When The Game Is Turned Off
        PlayerPrefs.SetString("LeaderBoards", stats);
    }
}
