using System.Collections.Generic;
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine.Events;

public class BackendManager : Singleton<BackendManager>
{
	// Player data scriptable object
	[SerializeField] PlayerSO playerInfo;

	// Enent invoked when player name changed after login
	public GameEvents.StringParameterEvent OnPlayerNameChanged;

	// Event sent the warning message in login/register process
	public GameEvents.StringParameterEvent OnWarningMessageSent;
	

	public UnityEvent OnRegisterSuccessEvent;

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
		OnWarningMessageSent.Invoke(error.ErrorMessage);
		
		Debug.Log(error.GenerateErrorReport());
	}

	private void OnLoginSuccess(LoginResult result)
	{
		string playerName = null;
		if (result.InfoResultPayload.PlayerProfile != null)
		{
			playerName = result.InfoResultPayload.PlayerProfile.DisplayName;
		}

		if (playerName == "" || playerName == null)
		{
			Debug.Log("Successful login / create account. Nonamer." );
		}
		else
		{
			Debug.Log("Successful login / create account. Display name is: [" + playerName + "]");
			
			playerInfo.PlayerName = playerName;

			OnPlayerNameChanged.Invoke(playerName);
		}
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