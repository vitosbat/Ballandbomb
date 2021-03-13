using UnityEngine;
using UnityEngine.UI;
//using UnityEngine.Events;
//using System;
using TMPro;

public class UIRegisterPanel : MonoBehaviour
{
	GameManager gameManager;
    BackendManager backendManager;

	Transform warningMessage;

	public InputField nameInputField;
    public InputField emailInputField;
    public InputField passwordInputField;

	void Start()
    {
		gameManager = GameManager.Instance;
		backendManager = BackendManager.Instance;

		backendManager.OnWarningMessageSent.AddListener(WarningMessageHandler);
		backendManager.OnRegisterSuccessEvent.AddListener(HideRegisterPanel);

		warningMessage = transform.Find("WarningText");

		HideRegisterPanel();
	}

	private void WarningMessageHandler(string message)
	{
		warningMessage.GetComponent<TextMeshProUGUI>().text = message;
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

		backendManager.Register(playerName, playerEmail, playerPassword);
	}

	public void ShowRegisterPanel()
	{
		warningMessage.GetComponent<TextMeshProUGUI>().text = "";
		gameObject.SetActive(true);
	}

	public void HideRegisterPanel()
	{
		warningMessage.GetComponent<TextMeshProUGUI>().text = "";
		gameObject.SetActive(false);
	}
}
