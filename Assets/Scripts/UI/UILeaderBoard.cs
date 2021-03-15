using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UILeaderBoard : MonoBehaviour
{
	// LeaderboardManager leaderboardManager;

	BackendManager backendManager;

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

		backendManager = BackendManager.Instance;

		backendManager.OnLeaderboardDataFormed.AddListener(LeaderboardDataGetHandler);
	}

	// Request the actual leaderboard data from server
	private void OnEnable()
	{
		backendManager.GetLeaderboard();
	}

	// Receives actual leaderboard data
	private void LeaderboardDataGetHandler(List<PlayerResult> leaderBoard)
	{
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


	// Create lines with users highscores in leaderboard table using "Line" template
	public void CreateLeaderBoardTable(List<PlayerResult> leaderBoard)
	{
		foreach(PlayerResult res in leaderBoard)
		{
			Debug.Log("Player name: " + res.playerName + "; score: " + res.playerScore);
		}

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

		lineTransform.Find("LineBackground").gameObject.SetActive(place % 2 != 1);

		if (place == 1 || place == 2 || place == 3)
		{
			lineTransform.Find("Place").GetComponent<Text>().fontStyle = FontStyle.Bold;
			lineTransform.Find("Name").GetComponent<Text>().fontStyle = FontStyle.Bold;
			lineTransform.Find("Score").GetComponent<Text>().fontStyle = FontStyle.Bold;
		}

		transformList.Add(lineTransform);

	}

	// States of Leaderboard UI
	public void ShowLeaderBoard()
	{
		gameObject.SetActive(true);
	}

	public void HideLeaderBoard()
	{
		gameObject.SetActive(false);
	}

}
