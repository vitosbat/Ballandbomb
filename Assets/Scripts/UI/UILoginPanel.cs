using UnityEngine;
using UnityEngine.UI;
//using UnityEngine.Events;
using System;
using TMPro;

public class UILoginPanel : MonoBehaviour
{
	GameManager gameManager;

	BackendManager backendManager;

	public InputField emailInputField;
	public InputField passwordInputField;

	private void Start()
	{
		HideLoginPanel();

		gameManager = GameManager.Instance;
		backendManager = BackendManager.Instance;
		
		backendManager.OnLoginSuccessEvent.AddListener(HideLoginPanel);
		backendManager.OnWarningMessageSent.AddListener(WarningMessageHandler);
	}

	private void WarningMessageHandler(string message)
	{
		gameObject.transform.Find("WarningText").GetComponent<TextMeshProUGUI>().text = message;
	}

	public void OnLoginButtonSubmit()
	{
		backendManager.LoginWithEmail(emailInputField.text, passwordInputField.text);

		//gameObject.SetActive(false);
	}

	public void ShowLoginPanel()
	{
		gameObject.SetActive(true);
	}

	public void HideLoginPanel()
	{
		gameObject.SetActive(false);
	}
}
