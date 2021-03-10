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

	public void OnLoginButtonSubmit()
	{
		playfab.LoginWithEmail(emailInputField.text, passwordInputField.text);

		//TODO: set gameManager.playerInfo.PlayerName and invoke PlayerNameChangedEvent.Invoke()
	}
}
