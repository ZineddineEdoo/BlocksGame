using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class DisplayTotalBonus : MonoBehaviour
{
	[SerializeField]
	private bool isFullDisplay = default;

	private TextMeshProUGUI textUI;

	void Awake()
	{
		textUI = GetComponent<TextMeshProUGUI>();
		Globals.BonusUpdated += Globals_BonusUpdated;
	}

	private void Globals_BonusUpdated(object sender, float bonus)
	{
		var formattedText = Globals.GetFormattedScoreText(Globals.Bonus, isFullDisplay);

		textUI.SetText(formattedText);
	}
}
