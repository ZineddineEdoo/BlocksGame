using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class Achievement
{
	public const AchievementID SCORE_START = AchievementID.Score1K;
	public const AchievementID BONUS_START = AchievementID.Bonus1K;

	public enum AchievementID
	{
		Score1K = 0,
		Score10K,
		Score100K,
		Score1M,
		Score10M,
		Score100M,
		ScoreN1K,
		ScoreN10K,
		ScoreN100K,
		ScoreN1M,
		Bonus1K = 1000,
		Bonus10K,
		Bonus100K,
		Bonus1M,
		Bonus10M,
		Bonus100M,
		BonusN1K,
		BonusN10K,
		BonusN100K,
		BonusN1M
	}

	public AchievementID ID { get; set; }
	public string Name { get; set; }
	public string Detail { get; set; }
}
