using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// PlayerInfo class used to store players data and making easier to manipulate it
public class PlayerResult
{
    public string playerName;
    public int playerScore;

    public PlayerResult (string name, int score)
	{
        this.playerName = name;
        this.playerScore = score;
	}
}

public class Leaderboard : Singleton<Leaderboard>
{    
    public void AddResultToLeaderBoard(string name, int score)
	{
        PlayerResult result = new PlayerResult(name, score);

        List<PlayerResult> leaderBoard = GetLeaderBoard();
        
        leaderBoard.Add(result);

        SortLeaderBoard(leaderBoard);

        SaveLeaderBoard(leaderBoard);
	}

    public List<PlayerResult> GetLeaderBoard()
    {
        List<PlayerResult> leaderboard = new List<PlayerResult>();

        // Load The String Of The Leaderboard That Was Saved In The "UpdatePlayerPrefsString" Method
        string stats = PlayerPrefs.GetString("LeaderBoard");

        // Assign The String To An Array And Split Using The Comma Character
        // This Will Remove The Comma From The String, And Leave Behind The Separated Name And Score
        string[] stats2 = stats.Split(',');

        // Loop Through The Array 2 At A Time Collecting Both The Name And Score
        for (int i = 0; i < stats2.Length - 2; i += 2)
        {
            // Use The Collected Information To Create An Object
            PlayerResult result = new PlayerResult(stats2[i], int.Parse(stats2[i + 1]));
                        
            // Add The Object To The List
            leaderboard.Add(result);
		}

        return leaderboard;
    }

	private void SortLeaderBoard(List<PlayerResult> results)
	{
        // Start At The End Of The List And Compare The Score To The Number Above It
        for (int i = results.Count - 1; i > 0; i--)
        {
            // If The Current Score Is Higher Than The Score Above It , Swap
            if (results[i].playerScore > results[i - 1].playerScore)
            {
                // Temporary variable to hold small score
                PlayerResult tempSmallerResult = results[i - 1];

                // Replace small score with big score
                results[i - 1] = results[i];

                // Set small score closer to the end of the list by placing it at "i" rather than "i-1" 
                results[i] = tempSmallerResult;
            }
        }

        //return results;
	}

	private void SaveLeaderBoard(List<PlayerResult> results)
    {
        // Start With A Blank String
        string strResults = "";

        // Add Each Name And Score From The Collection To The String
        for (int i = 0; i < results.Count; i++)
        {
            // Be Sure To Add A Comma To Both The Name And Score, It Will Be Used To Separate The String Later
            strResults += results[i].playerName + ",";
            strResults += results[i].playerScore + ",";
        }

        // Add The String To The PlayerPrefs, This Allows The Information To Be Saved Even When The Game Is Turned Off
        PlayerPrefs.SetString("LeaderBoard", strResults);
    }
}
