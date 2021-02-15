using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UILifeSlider : MonoBehaviour
{
	[SerializeField] Slider lifeSlider;



	private void Start()
	{
		
	}

	public void ScoreChangesHandler(int value)
	{
		lifeSlider.value = value;
	}
}
