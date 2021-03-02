using UnityEngine;
using TMPro;

public class UIPlayerAvatar : MonoBehaviour
{
	string playerName;

	[SerializeField] private PlayerSO playerData;

	private void Awake()
	{
		gameObject.GetComponent<TextMeshProUGUI>().text = playerData.DefaultPlayerName;
	}

	public void OnPlayerNameChangedHandler()
	{
		gameObject.GetComponent<TextMeshProUGUI>().text = playerData.PlayerName;
	}
}
