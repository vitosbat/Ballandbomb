using UnityEngine;
using UnityEngine.UI;
//using UnityEngine.Events;
using System;
using TMPro;

public class UILoginPanel : MonoBehaviour
{
	BackendManager backendManager;

	Transform warningMessage;

	public InputField emailInputField;
	public InputField passwordInputField;

	private void Start()
	{
		backendManager = BackendManager.Instance;
		
		backendManager.OnLoginSuccessEvent.AddListener(HideLoginPanel);
		backendManager.OnWarningMessageSent.AddListener(WarningMessageHandler);

		warningMessage = transform.Find("WarningText");

		HideLoginPanel();
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
	}

	public void HideLoginPanel()
	{
		warningMessage.GetComponent<TextMeshProUGUI>().text = "";
		gameObject.SetActive(false);
	}
}
