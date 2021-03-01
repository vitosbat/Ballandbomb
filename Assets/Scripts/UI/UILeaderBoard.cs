using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UILeaderBoard : MonoBehaviour
{
	private Transform leaderboardTable;
	private Transform leaderboardLine;
	private List<PlayerResult> leaderBoard;
	private List<Transform> leaderBoardTransformList;

	private void Start()
	{
		gameObject.SetActive(false);
	}

	private void OnEnable()
	{
		leaderBoard = Leaderboard.Instance.GetLeaderBoard();

		CreateLeaderBoardTable(leaderBoard);
	}

	private void OnDisable()
	{
		DestroyLeaderBoard();
	}

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

	public void CreateLeaderBoardTable(List<PlayerResult> leaderBoard)
	{
		leaderboardTable = transform.Find("Table");
		leaderboardLine = leaderboardTable.Find("Line");

		leaderboardLine.gameObject.SetActive(false);

		leaderBoardTransformList = new List<Transform>();

		foreach (PlayerResult result in leaderBoard.GetRange(0, 10))
		{
			CreateLeaderboardLine(result, leaderboardTable, leaderBoardTransformList);
		}
	}

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

	public void ShowLeaderBoard()
	{
		gameObject.SetActive(true);
	}

	public void HideLeaderBoard()
	{
		gameObject.SetActive(false);
	}

}
