using UnityEngine;
using TMPro;

public class UIPlayerAvatar : MonoBehaviour
{
	string playerName;

	[SerializeField] private PlayerSO playerData;

	BackendManager backendManager;

	private void Awake()
	{
		gameObject.GetComponent<TextMeshProUGUI>().text = playerData.DefaultPlayerName;

		backendManager = BackendManager.Instance;
		backendManager.OnPlayerNameChanged.AddListener(OnPlayerNameChangedHandler);
	}

	public void OnPlayerNameChangedHandler(string displayName)
	{
		gameObject.GetComponent<TextMeshProUGUI>().text = displayName;
	}
}
