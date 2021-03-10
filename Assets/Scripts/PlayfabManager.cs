using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;
using System;


public class PlayfabManager : Singleton<PlayfabManager>
{
	private void Start()
	{
		Login();
	}

	void Login()
	{
		var request = new LoginWithCustomIDRequest
		{
			CustomId = SystemInfo.deviceUniqueIdentifier,
			CreateAccount = true,
			InfoRequestParameters = new GetPlayerCombinedInfoRequestParams
			{
				GetPlayerProfile = true,
			}
		};

		PlayFabClientAPI.LoginWithCustomID(request, OnLoginSuccess, OnError);
	}

	private void OnError(PlayFabError error)
	{
		Debug.Log(error.ErrorMessage);
		Debug.Log(error.GenerateErrorReport());
	}

	private void OnLoginSuccess(LoginResult result)
	{
		string name = null;
		if (result.InfoResultPayload.PlayerProfile != null)
		{
			name = result.InfoResultPayload.PlayerProfile.DisplayName;
		}
		Debug.Log("Successful login / create account. Display name is " + name);

	}

	public void LoginWithEmail(string email, string password)
	{
		var request = new LoginWithEmailAddressRequest
		{
			Email = email,
			Password = password
		};
		PlayFabClientAPI.LoginWithEmailAddress(request, OnLoginSuccess, OnError);
	}


	public void Register(string username, string email, string password)
	{
		if (password.Length < 6)
		{
			Debug.Log("Password too short. Minimum 6 characters.");
			return;
		}

		var request = new RegisterPlayFabUserRequest
		{
			Email = email,
			Password = password,
			Username = username,
			RequireBothUsernameAndEmail = false
		};
		PlayFabClientAPI.RegisterPlayFabUser(request, OnRegisterSuccess, OnError);
	}

	private void OnRegisterSuccess(RegisterPlayFabUserResult result)
	{
		Debug.Log("Registered and logged in!");
		Debug.Log("Username: " + result.Username);
	}






	#region Leaderboard management
	public void SendLeaderboard(int score)
	{
		var request = new UpdatePlayerStatisticsRequest
		{
			Statistics = new List<StatisticUpdate>
			{
				new StatisticUpdate
				{
					StatisticName = "PlatformScore",
					Value = score
				}
			}
		};

		PlayFabClientAPI.UpdatePlayerStatistics(request, OnLeaderboardUpdate, OnError);
	}

	private void OnLeaderboardUpdate(UpdatePlayerStatisticsResult result)
	{
		Debug.Log("Successful leaderboard sent.");
	}

	public void GetLeaderboard()
	{
		var requst = new GetLeaderboardRequest
		{
			StatisticName = "PlatformScore",
			StartPosition = 0,
			MaxResultsCount = 10
		};
		PlayFabClientAPI.GetLeaderboard(requst, OnLeaderboardGet, OnError);
	}

	private void OnLeaderboardGet(GetLeaderboardResult result)
	{
		foreach (var item in result.Leaderboard)
		{
			Debug.Log(item.Position + " " + item.PlayFabId + " " + item.StatValue);
		}
	}
	#endregion

}