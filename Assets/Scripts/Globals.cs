using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class Globals
{
	public const float BILLION = 1000000000f;
	public const float MILLION = 1000000f;
	public const float THOUSAND = 1000f;

	public static float Score { get; set; }

	public static float PreviousHighScore { get; set; } = SaveManager.CurrentSaveData.HighScore;

	private static string GetFormattedScoreText(float score)
	{
		string disp;

		if (Mathf.Abs(score) >= BILLION)
			disp = $"{score / BILLION:0.00}B";
		else if (Mathf.Abs(score) >= MILLION)
			disp = $"{score / MILLION:0.00}M";
		else if (Mathf.Abs(score) >= THOUSAND)
			disp = $"{score / THOUSAND:0.00}K";
		else
			disp = $"{score:0}";

		return disp;
	}

	public static string GetFormattedScoreText(float score, bool isFullDisplay = false)
	{
		return isFullDisplay ? $"{score:0}" : GetFormattedScoreText(score);
	}
}
