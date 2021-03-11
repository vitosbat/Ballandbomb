using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class UILoginPanel : MonoBehaviour
{
	GameManager gameManager;

	BackendManager backendManager;

	public InputField emailInputField;
	public InputField passwordInputField;

	public UnityEvent PlayerNameChangedEvent;

	private void Start()
	{
		gameManager = GameManager.Instance;
		backendManager = BackendManager.Instance;

		HideLoginPanel();
	}
	
	public void OnLoginButtonSubmit()
	{
		backendManager.LoginWithEmail(emailInputField.text, passwordInputField.text);

		//TODO: set gameManager.playerInfo.PlayerName and invoke PlayerNameChangedEvent.Invoke()

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
