using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class DisplayBonusUpdateText : MonoBehaviour
{
	[SerializeField]
	private Color positiveColor = default;

	[SerializeField]
	private Color negativeColor = default;

	void Awake()
	{
		var scoreManager = GetComponentInParent<MenuUI>().GameManager.GetComponent<ScoreManager>();
		scoreManager.BonusScoreOneTimeUpdating += ScoreManager_BonusScoreOneTimeUpdating;
	}

	private void ScoreManager_BonusScoreOneTimeUpdating(object sender, float bonus)
	{
		var text = GetComponent<TextMeshProUGUI>();

		if (bonus >= 0f)
		{
			text.SetText($"+{Globals.GetFormattedScoreText(bonus)}");
			text.color = positiveColor;
		}
		else
		{
			text.SetText($"{Globals.GetFormattedScoreText(bonus)}");
			text.color = negativeColor;
		}

		// Play From Beginning
		GetComponentInParent<Animator>().Play("Bonus Score Animation", -1, 0f);
	}
}
