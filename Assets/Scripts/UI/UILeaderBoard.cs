using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UILeaderBoard : MonoBehaviour
{
	LeaderboardManager leaderboardManager;

	// Leaderboard table UI object
	private Transform leaderboardTable;

	// UI template of line in Leaderboard object
	private Transform leaderboardLine;

	// List of player highscores data
	private List<PlayerResult> leaderBoard;

	// List of UI lines in Leaderboard
	private List<Transform> leaderBoardTransformList;

	// Number of lines of player highscores in Leaderboard Table
	private int numberOfHighscores = 10;


	private void Awake()
	{
		gameObject.SetActive(false);

		leaderboardManager = LeaderboardManager.Instance;
	}

	private void OnEnable()
	{
		// Receives actual leaderboard data
		leaderBoard = leaderboardManager.GetLeaderBoard();

		CreateLeaderBoardTable(leaderBoard);
	}

	private void OnDisable()
	{
		DestroyLeaderBoard();
	}


	// Removes lines with not actual data from table after Disable Leaderboard UI
	private void DestroyLeaderBoard()
	{
		if (leaderboardTable != null)
		{
			foreach (Transform child in leaderboardTable.transform)
			{
				if (child.gameObject.activeSelf != false)
				{
					Destroy(child.gameObject);
				}
			}
		}
	}


	// Create {numberOfHighscores} lines in leaderboard table using {"Line"} template
	public void CreateLeaderBoardTable(List<PlayerResult> leaderBoard)
	{
		leaderboardTable = transform.Find("Table");
		leaderboardLine = leaderboardTable.Find("Line");

		// Hides the template 
		leaderboardLine.gameObject.SetActive(false);

		// Create list of the hignscores lines and populate it in the loop
		leaderBoardTransformList = new List<Transform>();

		int numberOfLines = leaderBoard.Count < numberOfHighscores ? leaderBoard.Count : numberOfHighscores;

		foreach (PlayerResult result in leaderBoard.GetRange(0, numberOfLines))
		{
			CreateLeaderboardLine(result, leaderboardTable, leaderBoardTransformList);
		}
	}


	// Instantiates and populates a line of Leaderboard using a PlayerResult value, and adds it to the List of lines
	private void CreateLeaderboardLine(PlayerResult result, Transform table, List<Transform> transformList)
	{
		float templateHeight = 30f;

		Transform lineTransform = Instantiate(leaderboardLine, table);

		RectTransform lineRectTransform = lineTransform.GetComponent<RectTransform>();

		lineRectTransform.anchoredPosition += new Vector2(0, -templateHeight * transformList.Count);
		lineTransform.gameObject.SetActive(true);

		int place = (transformList.Count + 1);
		lineTransform.Find("Place").GetComponent<Text>().text = place.ToString();

		string name = result.playerName;
		lineTransform.Find("Name").GetComponent<Text>().text = name;

		int score = result.playerScore;
		lineTransform.Find("Score").GetComponent<Text>().text = score.ToString();

		transformList.Add(lineTransform);

	}


	// States of Leaderboard UI (using for events)
	public void ShowLeaderBoard()
	{
		gameObject.SetActive(true);
	}

	public void HideLeaderBoard()
	{
		gameObject.SetActive(false);
	}

}
