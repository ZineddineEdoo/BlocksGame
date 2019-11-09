using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class Globals
{
	public const float MINUTE = 60f;
	public const float BILLION = 1000000000f;
	public const float MILLION = 1000000f;
	public const float THOUSAND = 1000f;

	private static float score;

	public static event EventHandler<float> ScoreUpdated;

	public static float Score 
	{
		get => score;
		set
		{
			score = value;

			ScoreUpdated?.Invoke(null, score);
		}
	}

	/// <summary>
	/// Absolute Value of Score. <code>Mathf.Abs(Score)</code>
	/// </summary>
	public static float ScoreAbs => Mathf.Abs(Score);

	// TODO: Use for Single Game Check (100K -> 0 -> 100K)
	public static float CurrentStartTime { get; set; }

	// Used for High Score Animation on Game Over Scene
	public static float PreviousHighScore { get; set; } = SaveManager.CurrentSaveData.HighScore;

	private static string GetScoreDisplayText(float score, bool isCompactDisplay)
	{
		string disp;

		if (Mathf.Abs(score) >= BILLION)
			disp = isCompactDisplay ? $"{score / BILLION:0}B": $"{score / BILLION:0.00}B";
		else if (Mathf.Abs(score) >= MILLION)
			disp = isCompactDisplay ? $"{score / MILLION:0}M" : $"{score / MILLION:0.00}M";
		else if (Mathf.Abs(score) >= THOUSAND)
			disp = isCompactDisplay ? $"{score / THOUSAND:0}K" : $"{score / THOUSAND:0.00}K";
		else
			disp = $"{score:0}";

		return disp;
	}

	public static string GetFormattedScoreText(float score, bool isFullDisplay = false)
	{
		return isFullDisplay ? $"{score:0}" : GetScoreDisplayText(score, false);
	}

	public static string GetCompactFormattedScoreText(float score, bool isFullDisplay = false)
	{
		return isFullDisplay ? $"{score:0}" : GetScoreDisplayText(score, true);
	}
}
