using System.Collections;
using System.Collections.Generic;


// PlayerResult class used to store players result data and making easier to manipulate it
public class PlayerResult
{
	public string playerName;
	public int playerScore;

	public PlayerResult(string name, int score)
	{
		this.playerName = name;
		this.playerScore = score;
	}
}
