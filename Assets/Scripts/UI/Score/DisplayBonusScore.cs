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
	private TextMeshProUGUI bonusScorePrefab = default;
	
	[SerializeField]
	private Color positiveColor = default;
	
	[SerializeField]
	private Color negativeColor = default;

	void Awake()
	{
		var scoreManager = GetComponentInParent<MenuUI>().GameManager.GetComponent<ScoreManager>();
		scoreManager.BonusScoreUpdating += ScoreManager_BonusScoreUpdating;
	}

	private void ScoreManager_BonusScoreUpdating(object sender, (float Bonus, Vector2 Position) args)
	{
		var screenPos = Camera.main.WorldToScreenPoint(args.Position);
		
		var bonusScore = Instantiate(bonusScorePrefab, screenPos, Quaternion.identity, transform);
		if (args.Bonus >= 0f)
		{
			bonusScore.SetText($"+{args.Bonus:0}");
			bonusScore.color = positiveColor;
		}
		else
		{
			bonusScore.SetText($"-{args.Bonus:0}");
			bonusScore.color = negativeColor;
		}

		bonusScore.CrossFadeAlpha(0f, BONUS_DISPLAY_DURATION, false);
		Destroy(bonusScore.gameObject, BONUS_DISPLAY_DURATION + 0.1f);
	}
}
