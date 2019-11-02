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

	[SerializeField]
	private TextMeshProUGUI valueText = default;

	[SerializeField]
	private TextMeshProUGUI dateText = default;

	void Awake()
	{
		valueText.SetText(Globals.GetFormattedScoreText(SaveManager.CurrentSaveData.HighScore, isFullDisplay));
		var date = SaveManager.CurrentSaveData.HighScoreDate;
		
		dateText.SetText(date != default ? SaveManager.CurrentSaveData.HighScoreDate.ToString("g") : "");
	}
}
