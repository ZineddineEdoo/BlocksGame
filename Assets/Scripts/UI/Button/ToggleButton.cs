using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ToggleButton : MonoBehaviour
{
	private MenuUI menuUI;

	void Awake()
	{
		menuUI = FindObjectOfType<MenuUI>();
		if (menuUI != null)
			SetDisplay(menuUI.IsBonusActive);

		GetComponent<Button>().onClick.AddListener(() => 
		{
			menuUI.ToggleBonusDisplay();

			SetDisplay(menuUI.IsBonusActive);
		});
	}

	private void SetDisplay(bool isOn)
	{
		if (isOn)
		{
			GetComponentInChildren<TextMeshProUGUI>().SetText("Bonus Toggle On");
			GetComponent<Image>().color = Color.white;
		}
		else
		{
			GetComponentInChildren<TextMeshProUGUI>().SetText("Bonus Toggle Off");
			GetComponent<Image>().color = Color.black;
		}
	}
}
