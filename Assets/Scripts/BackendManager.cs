using System.Collections.Generic;
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine.Events;
using System;

public class BackendManager : Singleton<BackendManager>
{
	GameManager gameManager;

	// Player data scriptable object
	[SerializeField] PlayerSO playerInfo;

	// Enent invoked when player name changed after login
	public GameEvents.StringParameterEvent OnPlayerNameChanged;

	// Event sent the warning messages of login/register process
	public GameEvents.StringParameterEvent OnWarningMessageSent;

	// Events invoked after Player has successfully registered/logged in
	public UnityEvent OnRegisterSuccessEvent;
	public UnityEvent OnLoginSuccessEvent;

	// Event 
	public GameEvents.EventLeaderboard OnLeaderboardDataFormed;

	private void Start()
	{
		gameManager = GameManager.Instance;
		gameManager.OnGameStateChanged.AddListener(GameStateChangesHandler);

		// Start user session via deviceID login (anonymous)
		Login();
	}

	// Handle the game state changing
	private void GameStateChangesHandler(GameManager.GameState currentState, GameManager.GameState previousState)
	{
		// Update the leaderboard after game ending
		if (currentState == GameManager.GameState.ENDLEVEL_LOSE || currentState == GameManager.GameState.FINAL)
		{
			UpdateLeaderboard(playerInfo.PlayerResultScore);
		}
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

		OnLoginSuccessEvent.Invoke();
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

	#region Register
	public void Register(string username, string email, string password)
	{
		if (password.Length < 6)
		{
			OnWarningMessageSent.Invoke("Password too short. Minimum 6 characters.");

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
		Debug.Log("Successiful registered and logged in. PlayfabID: " + result.PlayFabId);

		GetPlayerAccount(result.PlayFabId);

		OnRegisterSuccessEvent.Invoke();
	}
	#endregion


	#region Leaderboard management
	public void UpdateLeaderboard(int score)
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
		GetLeaderboard();
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
		List<PlayerResult> leaderboard = new List<PlayerResult>();

		foreach (var item in result.Leaderboard)
		{
			leaderboard.Add(new PlayerResult(item.DisplayName, item.StatValue));
		}

		OnLeaderboardDataFormed.Invoke(leaderboard);
	}

	#endregion

}