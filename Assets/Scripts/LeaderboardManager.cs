using System.Collections.Generic;
using UnityEngine;

// PlayerResult class used to store players result data and making easier to manipulate it
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


// Leaderboard data management class
public class LeaderboardManager : Singleton<LeaderboardManager>
{
    // Receives List of objects contained full data of player results
    public List<PlayerResult> GetLeaderBoard()
    {
        List<PlayerResult> leaderboard = new List<PlayerResult>();

        // Load the string of the Leaderboard that was saved in the "AddResultToLeaderBoard" method
        string stats = PlayerPrefs.GetString("LeaderBoard");

        // Assign the string to an array and split using the comma character
        // This will remove the comma from the string
        string[] stats2 = stats.Split(',');

        // Loop through the array 2 at a time collecting both the name and score
        for (int i = 0; i < stats2.Length - 2; i += 2)
        {
            // Use the collected information to create an object
            PlayerResult result = new PlayerResult(stats2[i], int.Parse(stats2[i + 1]));
                        
            // Add the object to the list
            leaderboard.Add(result);
		}

        return leaderboard;
    }
   
    // Addes result to the current leaderboard, sort the leaderboard and save it.
    public void AddResultToLeaderBoard(string name, int score)
	{
        PlayerResult result = new PlayerResult(name, score);

        List<PlayerResult> leaderBoard = GetLeaderBoard();
        
        leaderBoard.Add(result);

        SortLeaderBoard(leaderBoard);

        SaveLeaderBoard(leaderBoard);
	}

    // Convert List of PlayerResult object to comma-separated string, and save it to PlayerPrefs 
    private void SaveLeaderBoard(List<PlayerResult> results)
    {
        // Start with a blank string
        string strResults = "";

        // Add each name and score from the collection to the string
        for (int i = 0; i < results.Count; i++)
        {
            strResults += results[i].playerName + ",";
            strResults += results[i].playerScore + ",";
        }

        // Add the string to the PlayerPrefs
        PlayerPrefs.SetString("LeaderBoard", strResults);
    }

    // Put just added result - last member of List - to the convenient place in leaderboard List
	private void SortLeaderBoard(List<PlayerResult> results)
	{
        // Start at the end of the list and compare the score to the number above it
        for (int i = results.Count - 1; i > 0; i--)
        {
            if (results[i].playerScore > results[i - 1].playerScore)
            {
                // Temporary variable to hold smaller score
                PlayerResult tempSmallerResult = results[i - 1];

                // Replace smaller score with bigger score
                results[i - 1] = results[i];

                // Set smaller score closer to the end of the list by placing it at "i" rather than "i-1" 
                results[i] = tempSmallerResult;
            }
        }
	}
}
