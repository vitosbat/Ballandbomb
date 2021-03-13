using System.Collections.Generic;
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine.Events;
using System;

public class BackendManager : Singleton<BackendManager>
{
	// Player data scriptable object
	[SerializeField] PlayerSO playerInfo;

	// Enent invoked when player name changed after login
	public GameEvents.StringParameterEvent OnPlayerNameChanged;

	// Event sent the warning message in login/register process
	public GameEvents.StringParameterEvent OnWarningMessageSent;

	// Event invokes when Player have registered
	public UnityEvent OnRegisterSuccessEvent;

	private void Start()
	{
		Login();
	}

	// Start anonymous user session using deviceID
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

	// Login via registered email address
	public void LoginWithEmail(string email, string password)
	{
		var request = new LoginWithEmailAddressRequest
		{
			Email = email,
			Password = password
		};
		PlayFabClientAPI.LoginWithEmailAddress(request, OnLoginSuccess, OnError);
	}

	private void OnError(PlayFabError error)
	{
		OnWarningMessageSent.Invoke(error.ErrorMessage);

		Debug.Log(error.GenerateErrorReport());
	}

	private void OnLoginSuccess(LoginResult result)
	{
		Debug.Log("OnLoginSuccess entry. PlayfabID: " + result.PlayFabId);

		GetPlayerAccount(result.PlayFabId);
	}

	public void GetPlayerAccount(string playfabId)
	{
		var request = new GetAccountInfoRequest
		{
			PlayFabId = playfabId
		};
		PlayFabClientAPI.GetAccountInfo(request, OnGetAccountInfoSuccess, OnError);
	}

	private void OnGetAccountInfoSuccess(GetAccountInfoResult accountInfo)
	{
		string displayName = null;

		if (accountInfo.AccountInfo.TitleInfo != null)
		{
			Debug.Log("Display name: " + displayName);
			displayName = accountInfo.AccountInfo.TitleInfo.DisplayName;
		}

		if (displayName == "" || displayName == null)
		{
			Debug.Log("Successful anonymous login.");
		}
		else
		{
			Debug.Log("Successful login account. Display name is: [" + displayName + "]");

			playerInfo.PlayerName = displayName;

			OnPlayerNameChanged.Invoke(displayName);
		}
	}


	public void Register(string username, string email, string password)
	{
		if (password.Length < 6)
		{
			OnWarningMessageSent.Invoke("Password too short. Minimum 6 characters.");
			Debug.Log("Password too short. Minimum 6 characters.");

			return;
		}

		var request = new RegisterPlayFabUserRequest
		{
			Email = email,
			Password = password,
			DisplayName = username,
			RequireBothUsernameAndEmail = false
		};
		PlayFabClientAPI.RegisterPlayFabUser(request, OnRegisterSuccess, OnError);
	}

	private void OnRegisterSuccess(RegisterPlayFabUserResult result)
	{
		OnRegisterSuccessEvent.Invoke();

		Debug.Log("Successiful registered and logged in.");
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