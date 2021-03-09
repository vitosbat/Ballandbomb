using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class UILoginPanel : MonoBehaviour
{
	GameManager gameManager;

	PlayfabManager playfab;

	public InputField nameInputField;
	public InputField emailInputField;
	public InputField passwordInputField;

	public UnityEvent PlayerNameChangedEvent;

	private void Start()
	{
		gameObject.SetActive(false);
		gameManager = GameManager.Instance;
		playfab = PlayfabManager.Instance;
	}

	public void OnRegisterButtonSubmit()
	{
		string playerName = nameInputField.text;
		string playerEmail = emailInputField.text;
		string playerPassword = passwordInputField.text;

		if (playerName == "")
		{
			playerName = gameManager.playerInfo.DefaultPlayerName;
		}
		else
		{
			gameManager.playerInfo.PlayerName = playerName;
		}

		playfab.Register(playerName, playerEmail, playerPassword);

		PlayerNameChangedEvent.Invoke();
	}

	public void OnLoginButtonSubmit()
	{
		playfab.LoginWithEmail(emailInputField.text, passwordInputField.text);
	}
}
