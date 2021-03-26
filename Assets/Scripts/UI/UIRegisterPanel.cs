using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class UIRegisterPanel : MonoBehaviour
{
	GameManager gameManager;
    BackendManager backendManager;

	Transform warningMessage;

	public InputField nameInputField;
    public InputField emailInputField;
    public InputField passwordInputField;

	// Declare the list of input fields that will use the TAB key navigation
	private List<InputField> tabNavigateList;
	private int tabNavigateListIndex;

	void Start()
    {
		gameManager = GameManager.Instance;
		backendManager = BackendManager.Instance;

		backendManager.OnWarningMessageSent.AddListener(WarningMessageHandler);
		backendManager.OnRegisterSuccessEvent.AddListener(HideRegisterPanel);

		warningMessage = transform.Find("WarningText");

		// Initiates list of input fields that will use the TAB key navigation
		tabNavigateList = new List<InputField> { nameInputField, emailInputField, passwordInputField };

		// Set cursor on first input field
		nameInputField.Select();
		tabNavigateListIndex = 0;

		HideRegisterPanel();
	}

	private void Update()
	{
		// Implements TAB key navigation between input fields of Login form
		if (Input.GetKeyDown(KeyCode.Tab))
		{
			if (tabNavigateListIndex + 1 >= tabNavigateList.Count)
			{
				tabNavigateListIndex = -1;
			}
			tabNavigateListIndex++;
			tabNavigateList[tabNavigateListIndex].Select();
		}
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
		gameObject.SetActive(true);

		// Clear the warning messages
		warningMessage.GetComponent<TextMeshProUGUI>().text = "";

		// Set cursor to the first input field
		nameInputField.Select();
		tabNavigateListIndex = 0;
	}

	public void HideRegisterPanel()
	{
		// Clear the warning messages
		warningMessage.GetComponent<TextMeshProUGUI>().text = "";

		gameObject.SetActive(false);
	}
}
