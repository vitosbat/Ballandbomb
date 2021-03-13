using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

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
		// TODO: implement warnings of login process
		// backendManager.OnWarningMessageSent.AddListener(WarningMessageHandler);
	}
	
	public void OnLoginButtonSubmit()
	{
		backendManager.LoginWithEmail(emailInputField.text, passwordInputField.text);

		gameObject.SetActive(false);
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
