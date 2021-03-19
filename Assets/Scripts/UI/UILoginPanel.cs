using UnityEngine;
using UnityEngine.UI;
//using UnityEngine.Events;
using System.Collections.Generic;
using System;
using TMPro;

public class UILoginPanel : MonoBehaviour
{
	BackendManager backendManager;

	Transform warningMessage;

	public InputField emailInputField;
	public InputField passwordInputField;

	// Declare the list of input fields that will use the TAB key navigation
	private List<InputField> tabNavigateList;
	private int tabNavigateListIndex;

	private void Start()
	{
		backendManager = BackendManager.Instance;
		
		backendManager.OnLoginSuccessEvent.AddListener(HideLoginPanel);
		backendManager.OnWarningMessageSent.AddListener(WarningMessageHandler);

		warningMessage = transform.Find("WarningText");

		// Initiates list of input fields that will use the TAB key navigation
		tabNavigateList = new List<InputField> { emailInputField, passwordInputField };

		// Set cursor on first input field
		emailInputField.Select();
		tabNavigateListIndex = 0;

		HideLoginPanel();
	}

	private void Update()
	{
		// Implements TAB key navigation between input fields of Login form
		if(Input.GetKeyDown(KeyCode.Tab))
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

	public void OnLoginButtonSubmit()
	{
		backendManager.LoginWithEmail(emailInputField.text, passwordInputField.text);
	}

	public void ShowLoginPanel()
	{
		gameObject.SetActive(true);

		// Set cursor to the first input field
		emailInputField.Select();
		tabNavigateListIndex = 0;
	}

	public void HideLoginPanel()
	{
		// Clear the warning messages
		warningMessage.GetComponent<TextMeshProUGUI>().text = "";

		gameObject.SetActive(false);
	}
}
