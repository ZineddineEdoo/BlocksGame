using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class DisplayBonusScore : MonoBehaviour
{
	private const float BONUS_DISPLAY_DURATION = 0.5f;
	
	[SerializeField]
	private Color positiveColor = default;
	
	[SerializeField]
	private Color negativeColor = default;

	private TextMeshProUGUI bonusText;
	private float total;

	void Awake()
	{
		bonusText = GetComponent<TextMeshProUGUI>();

		var scoreManager = GetComponentInParent<Player>().GameManager.GetComponent<ScoreManager>();
		scoreManager.BonusScoreUpdating += ScoreManager_BonusScoreUpdating;
	}

	private void ScoreManager_BonusScoreUpdating(object sender, float bonus)
	{
		total += bonus;

		if (total >= 0f)
		{
			bonusText.SetText($"+{Globals.GetFormattedScoreText(total)}");
			bonusText.color = positiveColor;
		}
		else
		{
			bonusText.SetText($"{Globals.GetFormattedScoreText(total)}");
			bonusText.color = negativeColor;
		}

		this.RestartCoroutine(ClearText(), nameof(ClearText));
	}

	private IEnumerator ClearText()
	{
		yield return new WaitForSecondsRealtime(BONUS_DISPLAY_DURATION);

		total = 0f;
		bonusText.SetText("");

		this.RemoveCoroutine(nameof(ClearText));
	}
}
