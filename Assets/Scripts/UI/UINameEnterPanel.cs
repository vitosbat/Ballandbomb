using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class UINameEnterPanel : MonoBehaviour
{
	GameManager gameManager;

	public InputField inputField;

	public UnityEvent PlayerNameChangedEvent;

	private void Start()
	{
		gameManager = GameManager.Instance;
	}

	public void OnButtonSubmit()
	{
		string playerName = inputField.text;

		if (playerName == "")
		{
			playerName = gameManager.playerInfo.DefaultPlayerName;
		}
		else
		{
			gameManager.playerInfo.PlayerName = playerName;
		}

		PlayerNameChangedEvent.Invoke();
		
	}
}
