using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UINameEnterPanel : MonoBehaviour
{
	GameManager gameManager;

	public InputField inputField;

	private void Start()
	{
		gameManager = GameManager.Instance;
	}

	public void OnButtonSubmit()
	{
		gameManager.playerName = inputField.text;
	}
}
