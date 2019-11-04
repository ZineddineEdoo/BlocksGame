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

	private TextMeshProUGUI bonusText;
	private float total;
	private float lastDisplayTime;

	void Awake()
	{
		var scoreManager = GetComponentInParent<MenuUI>().GameManager.GetComponent<ScoreManager>();
		scoreManager.BonusScoreUpdating += ScoreManager_BonusScoreUpdating;
	}

	private void ScoreManager_BonusScoreUpdating(object sender, (float Bonus, Vector2 Position) args)
	{
		var screenPos = Camera.main.WorldToScreenPoint(args.Position);

		if (bonusText == null)
			bonusText = Instantiate(bonusScorePrefab, screenPos, Quaternion.identity, transform);
		
		bonusText.transform.position = screenPos;
		total += args.Bonus;

		if (args.Bonus >= 0f)
		{
			bonusText.SetText($"+{Globals.GetFormattedScoreText(total)}");
			bonusText.color = positiveColor;
		}
		else
		{
			bonusText.SetText($"{Globals.GetFormattedScoreText(total)}");
			bonusText.color = negativeColor;
		}
		
		lastDisplayTime = Time.time;
	}

	void Update()
	{
		if (bonusText != null && Time.time >= lastDisplayTime + BONUS_DISPLAY_DURATION)
		{
			total = 0f;
			Destroy(bonusText.gameObject);
		}
	}
}
