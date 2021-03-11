using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System;
using TMPro;

public class UIRegisterPanel : MonoBehaviour
{
	GameManager gameManager;
    BackendManager backendManager;

    public InputField nameInputField;
    public InputField emailInputField;
    public InputField passwordInputField;

	public UnityEvent PlayerNameChangedEvent;

	void Start()
    {
		HideRegisterPanel();

		gameManager = GameManager.Instance;
		backendManager = BackendManager.Instance;

		backendManager.OnWarningMessageSent.AddListener(WarningMessageHandler);
	}

	private void WarningMessageHandler(string message)
	{
		gameObject.transform.Find("WarningText").GetComponent<TextMeshProUGUI>().text = message;
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

		PlayerNameChangedEvent.Invoke();
	}

	public void ShowRegisterPanel()
	{
		gameObject.SetActive(true);
	}

	public void HideRegisterPanel()
	{
		gameObject.SetActive(false);
	}
}
