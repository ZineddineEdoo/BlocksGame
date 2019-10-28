using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class DisplayHighScore : MonoBehaviour
{
	[SerializeField]
	private bool isFullDisplay = default;

	void Awake()
	{
		var textUI = GetComponent<TextMeshProUGUI>();

		textUI.SetText(Globals.GetFormattedScoreText(SaveManager.CurrentSaveData.HighScore, isFullDisplay));
	}
}
