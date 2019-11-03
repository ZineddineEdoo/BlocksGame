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
	[Header("Optional")]
	private TextMeshProUGUI valueText = default;

	[SerializeField]
	[Header("Optional")]
	private TextMeshProUGUI dateText = default;

	void Awake()
	{
		var highScoreText = valueText != default ? valueText : GetComponent<TextMeshProUGUI>();
		highScoreText.SetText(Globals.GetFormattedScoreText(SaveManager.CurrentSaveData.HighScore, isFullDisplay));

		if (dateText != default)
		{
			var date = SaveManager.CurrentSaveData.HighScoreDate;
			dateText.SetText(date != default ? SaveManager.CurrentSaveData.HighScoreDate.ToString("g") : "");
		}
	}
}
